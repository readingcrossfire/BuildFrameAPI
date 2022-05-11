using System.Text;
using System.Text.Json;
using BLL.BLL_DrawlsFolder.Interface;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Distributed;
using ML.APIResult;
using ML.Entity;

namespace BLL
{
    public partial class BLL_Drawls
    {
        private async Task<APIListObjectResult<DrawlsEntity>> CrawlDataCodeMaze()
        {
            try
            {
                string urlBase = @"https://code-maze.com/latest-posts-on-code-maze/";
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlDocument htmlDocument = await htmlWeb.LoadFromWebAsync(urlBase);
                HtmlNode rootList = htmlDocument.GetElementbyId("et-boc");
                IEnumerable<HtmlNode> listPostNode = rootList.Descendants("article");
                List<DrawlsEntity> listPost = new List<DrawlsEntity>();
                listPostNode.ToList().ForEach((post) =>
                {
                    HtmlNode? titleNode = post.Descendants("h2").Where(x => x.Attributes["class"].Value == "entry-title").FirstOrDefault();
                    string title = titleNode?.InnerText ?? "";
                    HtmlNode? urlNode = titleNode?.ChildNodes.FirstOrDefault();
                    string? url = urlNode?.GetAttributeValue("href", "").ToString();
                    HtmlNode? timeNodeParent = post.Descendants("p").Where(x => x.Attributes["class"].Value == "post-meta").FirstOrDefault();
                    HtmlNode? timeNode = timeNodeParent?.Descendants("span").Where(z => z.Attributes["class"].Value == "published").FirstOrDefault();
                    string? time = timeNode?.InnerText;
                    HtmlNode? quickContentNode = post.Descendants("div").Where(x => x.Attributes["class"].Value == "post-content").FirstOrDefault();
                    string? quickContent = quickContentNode?.InnerText;
                    listPost.Add(new DrawlsEntity { Title = title, PostUrl = url, PostDate = time, QuickContent = quickContent });
                });
                return new APIListObjectResult<DrawlsEntity> { IsError = false, Message = "Lấy thành công", ListObject = listPost };
            }
            catch (Exception ex)
            {
                return new APIListObjectResult<DrawlsEntity> { IsError = true, Message = ex.Message, ListObject = null };
            }
        }

        public async Task<APIListObjectResult<DrawlsEntity>> CrawlDataCodeMaze(bool useCache = false)
        {
            try
            {
                if (useCache)
                {
                    CancellationTokenSource cancel = new CancellationTokenSource();
                    string keyCache = "CACHE_CRAWLDATACODEMAZE";
                    byte[] cachedData = await this._cache.GetAsync(keyCache, cancel.Token);
                    if (cachedData != null)
                    {
                        List<DrawlsEntity> lstDrawlsEntity = new();
                        var cachedDataString = Encoding.UTF8.GetString(cachedData);
                        lstDrawlsEntity = JsonSerializer.Deserialize<List<DrawlsEntity>>(cachedDataString) ?? new();
                        return new APIListObjectResult<DrawlsEntity>
                        {
                            IsError = false,
                            Message = "Lấy thành công",
                            ListObject = lstDrawlsEntity
                        };
                    }
                    else
                    {
                        if (cancel.IsCancellationRequested)
                        {
                            return new APIListObjectResult<DrawlsEntity>
                            {
                                IsError = true,
                                Message = "Có lỗi xảy ra"
                            };
                        }
                        var result = await this.CrawlDataCodeMaze();
                        if (result.IsError)
                        {
                            return new APIListObjectResult<DrawlsEntity>
                            {
                                IsError = true,
                                Message = "Có lỗi xảy ra"
                            };
                        }

                        string cachedDataString = JsonSerializer.Serialize(result.ListObject);
                        var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                        // Add the data into the cache
                        await _cache.SetAsync(keyCache, dataToCache, options);
                        return result;
                    }
                }
                else
                {
                    string keyCache = "CACHE_CRAWLDATACODEMAZE";
                    var result = await this.CrawlDataCodeMaze();
                    if (result.IsError)
                    {
                        return new APIListObjectResult<DrawlsEntity>
                        {
                            IsError = true,
                            Message = "Có lỗi xảy ra"
                        };
                    }

                    string cachedDataString = JsonSerializer.Serialize(result.ListObject);
                    var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Add the data into the cache
                    await _cache.SetAsync(keyCache, dataToCache, options);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIListObjectResult<DrawlsEntity>
                {
                    IsError = true,
                    Message = ex.Message
                };
            }
        }
    }
}
using System.Text;
using System.Text.Json;
using BLL.BLL_Drawls;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Distributed;
using ML;
using ML.APIResult;
using ML.Drawls;

namespace BLL.BLL_Drawls
{
    public partial class BLL_Drawls: IDrawlsService
    {
        private async Task<APIResult<List<Drawls>>>CrawlDataCodeMaze()
        {
            try
            {
                string urlBase = @"https://code-maze.com/latest-posts-on-code-maze/";
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlDocument htmlDocument = await htmlWeb.LoadFromWebAsync(urlBase);
                HtmlNode rootList = htmlDocument.GetElementbyId("et-boc");
                IEnumerable<HtmlNode> listPostNode = rootList.Descendants("article");
                List<Drawls> listPost = new List<Drawls>();
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
                    listPost.Add(new Drawls { Title = title, PostUrl = url, PostDate = time, QuickContent = quickContent });
                });
                return new APIResult<List<Drawls>> { IsError = false, Message = "Lấy thành công", ResultObject = listPost };
            }
            catch (Exception ex)
            {
                return new APIResult<List<Drawls>> { IsError = true, Message = ex.Message, ResultObject = null };
            }
        }

        public async Task<APIResult<List<Drawls>>> CrawlDataCodeMaze(bool useCache = false)
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
                        List<Drawls> lstDrawlsEntity = new();
                        var cachedDataString = Encoding.UTF8.GetString(cachedData);
                        lstDrawlsEntity = JsonSerializer.Deserialize<List<Drawls>>(cachedDataString) ?? new();
                        return new APIResult<List<Drawls>>
                        {
                            IsError = false,
                            Message = "Lấy thành công",
                            ResultObject = lstDrawlsEntity
                        };
                    }
                    else
                    {
                        if (cancel.IsCancellationRequested)
                        {
                            return new APIResult<List<Drawls>>
                            {
                                IsError = true,
                                Message = "Có lỗi xảy ra"
                            };
                        }
                        var result = await this.CrawlDataCodeMaze();
                        if (result.IsError)
                        {
                            return new APIResult<List<Drawls>>
                            {
                                IsError = true,
                                Message = "Có lỗi xảy ra"
                            };
                        }

                        string cachedDataString = JsonSerializer.Serialize(result.ResultObject);
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
                        return new APIResult<List<Drawls>>
                        {
                            IsError = true,
                            Message = "Có lỗi xảy ra"
                        };
                    }

                    string cachedDataString = JsonSerializer.Serialize(result.ResultObject);
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
                return new APIResult<List<Drawls>>
                {
                    IsError = true,
                    Message = ex.Message
                };
            }
        }
    }
}
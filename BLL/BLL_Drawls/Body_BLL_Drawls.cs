using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Distributed;
using ML.APIResult;
using ML.Drawls;

namespace BLL.BLL_Drawls
{
    public partial class BLL_Drawls : IDrawlsService
    {
        private async Task<APIResult<List<Drawls>>> CrawlDataCodeMaze()
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
                    listPost.Add(new Drawls { MenuTypes = new() { Type = "C#", Name = "CodeMaze" }, Title = title, PostUrl = url, PostDate = time, QuickContent = quickContent });
                });
                return new APIResult<List<Drawls>> { IsError = false, Message = "Lấy thành công", ResultObject = listPost };
            }
            catch (Exception ex)
            {
                return new APIResult<List<Drawls>> { IsError = true, Message = ex.Message, ResultObject = null };
            }
        }
        private async Task<APIResult<List<EndjinCategory>>> CrawlDataCategoryEndjin()
        {
            try
            {
                string urlBase = @"https://endjin.com/what-we-think/editions/";
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlDocument htmlDocument = await htmlWeb.LoadFromWebAsync(urlBase);
                HtmlNode rootList = htmlDocument.GetElementbyId("main-content");
                HtmlNode lstChildNode = rootList.Descendants("ol").Where(x => x.Attributes["class"].Value == "cat-nav-list").FirstOrDefault();
                List<EndjinCategory> lstResult = new List<EndjinCategory>();
                IEnumerable<HtmlNode> lstMenuItem = lstChildNode.Descendants("li").Where(x => x.Attributes["class"].Value == "cat-nav-list__item");
                int index = 1;
                foreach (HtmlNode menuItem in lstMenuItem)
                {
                    HtmlNode htmlNodeMenuItem = menuItem.SelectSingleNode($"//*[@id='main-content']/article/nav/ol/li[{index}]/a");
                    string strTitle = htmlNodeMenuItem.InnerText;
                    string strURLAttr = htmlNodeMenuItem.GetAttributeValue("href", "");
                    string strPath = strURLAttr.Replace("#", "");
                    string categoryID = strURLAttr.Remove(0, 1);
                    HtmlNode categoryNode = htmlDocument.GetElementbyId(categoryID);
                    HtmlNode countNode = categoryNode.SelectSingleNode($"//*[@id='{categoryID}']/header/a");
                    char[] count = countNode.InnerText.ToCharArray().Where(x => Char.IsDigit(x)).ToArray();
                    lstResult.Add(new()
                    {
                        Title = strTitle,
                        Path = strPath,
                        Count = Convert.ToInt32(new string(count))
                    });
                    index++;
                }
                return new APIResult<List<EndjinCategory>>
                {
                    IsError = false,
                    Message = "Lấy danh sách thành công",
                    ResultObject = lstResult
                };
            }
            catch (Exception objEx)
            {
                return new APIResult<List<EndjinCategory>>
                {
                    IsError = true,
                    Message = objEx.Message,
                    ResultObject = null
                };
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
        public Task<APIResult<List<Drawls>>> CrawlDataEndjin(string path, bool useCache = false)
        {
            throw new NotImplementedException();
        }
        public async Task<APIResult<List<EndjinCategory>>> CrawlDataCategoryEndjin(bool useCache = false)
        {
            try
            {
                if (useCache)
                {
                    CancellationTokenSource cancel = new CancellationTokenSource();
                    string keyCache = "CACHE_ENDJINCATEGORY";
                    byte[] cachedData = await this._cache.GetAsync(keyCache, cancel.Token);
                    if (cachedData != null)
                    {
                        List<EndjinCategory> lstEndjinCategory = new();
                        var cachedDataString = Encoding.UTF8.GetString(cachedData);
                        lstEndjinCategory = JsonSerializer.Deserialize<List<EndjinCategory>>(cachedDataString) ?? new();

                        return new APIResult<List<EndjinCategory>>
                        {
                            IsError = false,
                            Message = "Lấy thành công",
                            ResultObject = lstEndjinCategory
                        };
                    }
                    else
                    {
                        if (cancel.IsCancellationRequested)
                        {
                            return new APIResult<List<EndjinCategory>>
                            {
                                IsError = true,
                                Message = "Có lỗi xảy ra"
                            };
                        }
                        var result = await this.CrawlDataCategoryEndjin();
                        if (result.IsError)
                        {
                            return new APIResult<List<EndjinCategory>>
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
                    string keyCache = "CACHE_ENDJINCATEGORY";
                    var result = await this.CrawlDataCategoryEndjin();
                    if (result.IsError)
                    {
                        return new APIResult<List<EndjinCategory>>
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
                return new APIResult<List<EndjinCategory>> { IsError = true, Message = $"Lấy thất bại || {ex.Message}", ResultObject = null };
            }
        }
    }
}
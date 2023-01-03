using System.Text;
using System.Text.Json;
using CACHE;
using FIREBASE.SendNotification;
using FIREBASE.SendNotification.Interface;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Distributed;
using ML.APIResult;
using ML.Drawls;
using ML.Paging;

namespace BLL.BLL_Drawls
{
    public class BLL_Drawls : IDrawlsService
    {
        //private readonly ISendNotification _sendNotification;
        //public BLL_Drawls(ISendNotification sendNotification)
        //{
        //    this._sendNotification = sendNotification;
        //}
        private async Task<APIResult<List<DrawlsItem>>> CrawlDataCodeMaze()
        {
            try
            {
                //await _sendNotification.Send("BG9l327lk8pl_OmxAwSQkmBhJNnU3WadtjQZJedAmbopKFV3kqeLBCfcNv1rv-pTaonG0K7OQ4YFnQfLy6XGxWU", "TestData", "OK");
                string urlBase = @"https://code-maze.com/latest-posts-on-code-maze/";
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlDocument htmlDocument = await htmlWeb.LoadFromWebAsync(urlBase);
                HtmlNode rootList = htmlDocument.GetElementbyId("et-boc");
                IEnumerable<HtmlNode> listPostNode = rootList.Descendants("article");
                List<DrawlsItem> listPost = new List<DrawlsItem>();
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
                    listPost.Add(new DrawlsItem { MenuTypes = new() { Key = "CodeMaze", Type = "C#", Name = "Code Maze" }, Title = title, PostUrl = url ?? "", PostDate = time ?? "", QuickContent = quickContent ?? "" });
                });

                return new APIResult<List<DrawlsItem>> { IsError = false, Message = "Lấy thành công", ResultObject = listPost };
            }
            catch (Exception ex)
            {
                return new APIResult<List<DrawlsItem>> { IsError = true, Message = ex.Message, ResultObject = null };
            }
        }

        private async Task<APIResult<List<EndjinCategoryItem>>> CrawlDataCategoryEndjin()
        {
            try
            {
                string urlBase = @"https://endjin.com/what-we-think/editions/";
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlDocument htmlDocument = await htmlWeb.LoadFromWebAsync(urlBase);
                HtmlNode rootList = htmlDocument.GetElementbyId("main-content");
                HtmlNode lstChildNode = rootList.Descendants("ol").Where(x => x.Attributes["class"].Value == "cat-nav-list").FirstOrDefault();
                List<EndjinCategoryItem> lstResult = new List<EndjinCategoryItem>();
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
                    lstResult.Add(new EndjinCategoryItem()
                    {
                        Title = strTitle,
                        Path = strPath,
                        Count = Convert.ToInt32(new string(count)),
                        MenuTypes = new ML.MenuTypes.MenuTypesItem
                        {
                            Key = "Endjin",
                        }
                    });
                    index++;
                }
                return new APIResult<List<EndjinCategoryItem>>
                {
                    IsError = false,
                    Message = "Lấy danh sách thành công",
                    ResultObject = lstResult
                };
            }
            catch (Exception objEx)
            {
                return new APIResult<List<EndjinCategoryItem>>
                {
                    IsError = true,
                    Message = objEx.Message,
                    ResultObject = null
                };
            }
        }

        public async Task<APIResult<List<DrawlsItem>>> CrawlDataCodeMaze(Paging paging, bool useCache = false)
        {
            try
            {

                if (useCache)
                {
                    string keyCache = "CACHE_CRAWLDATACODEMAZE";

                    MessageResultBase objCacheResult = await Cache.Instance.Get<byte[]>(keyCache);
                    byte[] arrBeCachedData = null;
                    if (!objCacheResult.IsError)
                    {
                        arrBeCachedData = (objCacheResult as MessageResult<byte[]>).ResultObject;
                    }

                    if (arrBeCachedData != null)
                    {
                        List<DrawlsItem> lstDrawlsEntity = new();
                        var cachedDataString = Encoding.UTF8.GetString(arrBeCachedData);
                        lstDrawlsEntity = JsonSerializer.Deserialize<List<DrawlsItem>>(cachedDataString) ?? new();
                        int intTotalCount = lstDrawlsEntity.Count();
                        if (paging.PageSize == -1)
                        {
                            lstDrawlsEntity = lstDrawlsEntity.Select(x =>
                            {
                                x.Paging = new PagingItem
                                {
                                    PageIndex = paging.PageIndex,
                                    PageSize = paging.PageSize,
                                    PageTotal = intTotalCount
                                };
                                return x;
                            }).ToList();
                            return new APIResult<List<DrawlsItem>>
                            {
                                IsError = false,
                                Message = "Lấy thành công",
                                ResultObject = lstDrawlsEntity
                            };
                        }

                        lstDrawlsEntity = lstDrawlsEntity.Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize).ToList();
                        lstDrawlsEntity = lstDrawlsEntity.Select(x =>
                        {
                            x.Paging = new PagingItem
                            {
                                PageIndex = paging.PageIndex,
                                PageSize = paging.PageSize,
                                PageTotal = intTotalCount
                            };
                            return x;
                        }).ToList();
                        return new APIResult<List<DrawlsItem>>
                        {
                            IsError = false,
                            Message = "Lấy thành công",
                            ResultObject = lstDrawlsEntity
                        };
                    }
                    else
                    {
                        var result = await this.CrawlDataCodeMaze();
                        int intTotalCount = result.ResultObject.Count();

                        if (result.IsError)
                        {
                            return new APIResult<List<DrawlsItem>>
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
                        await Cache.Instance.Set(keyCache, dataToCache, DateTimeOffset.Now.AddHours(1));
                        if (paging.PageSize == -1)
                        {
                            result.ResultObject = result.ResultObject.Select(x =>
                            {
                                x.Paging = new PagingItem
                                {
                                    PageIndex = paging.PageIndex,
                                    PageSize = paging.PageSize,
                                    PageTotal = intTotalCount
                                };
                                return x;
                            }).ToList();
                            return result;
                        }

                        result.ResultObject = result.ResultObject.Select(x =>
                        {
                            x.Paging = new PagingItem
                            {
                                PageIndex = paging.PageIndex,
                                PageSize = paging.PageSize,
                                PageTotal = intTotalCount
                            };
                            return x;
                        }).ToList();
                        result.ResultObject = result.ResultObject.Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize).ToList();
                        return result;
                    }
                }
                else
                {
                    string keyCache = "CACHE_CRAWLDATACODEMAZE";
                    var result = await this.CrawlDataCodeMaze();
                    int intTotalCount = result.ResultObject.Count();
                    if (result.IsError)
                    {
                        return new APIResult<List<DrawlsItem>>
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
                    await Cache.Instance.Set(keyCache, dataToCache, DateTimeOffset.Now.AddHours(1));

                    if (paging.PageSize == -1)
                    {
                        result.ResultObject = result.ResultObject.Select(x =>
                        {
                            x.Paging = new PagingItem
                            {
                                PageIndex = paging.PageIndex,
                                PageSize = paging.PageSize,
                                PageTotal = intTotalCount
                            };
                            return x;
                        }).ToList();
                        return result;
                    }

                    result.ResultObject = result.ResultObject.Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize).ToList();
                    result.ResultObject = result.ResultObject.Select(x =>
                    {
                        x.Paging = new PagingItem
                        {
                            PageIndex = paging.PageIndex,
                            PageSize = paging.PageSize,
                            PageTotal = intTotalCount
                        };
                        return x;
                    }).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new APIResult<List<DrawlsItem>>
                {
                    IsError = true,
                    Message = ex.Message
                };
            }
        }

        //public async Task<APIResult<List<EndjinCategoryItem>>> CrawlDataCategoryEndjin(bool useCache = false)
        //{
        //    try
        //    {
        //        if (useCache)
        //        {
        //            CancellationTokenSource cancel = new CancellationTokenSource();
        //            string keyCache = "CACHE_ENDJINCATEGORY";
        //            byte[] cachedData = await this._cache.GetAsync(keyCache, cancel.Token);
        //            if (cachedData != null)
        //            {
        //                List<EndjinCategoryItem> lstEndjinCategory = new();
        //                var cachedDataString = Encoding.UTF8.GetString(cachedData);
        //                lstEndjinCategory = JsonSerializer.Deserialize<List<EndjinCategoryItem>>(cachedDataString) ?? new();

        //                return new APIResult<List<EndjinCategoryItem>>
        //                {
        //                    IsError = false,
        //                    Message = "Lấy thành công",
        //                    ResultObject = lstEndjinCategory,
        //                };
        //            }
        //            else
        //            {
        //                if (cancel.IsCancellationRequested)
        //                {
        //                    return new APIResult<List<EndjinCategoryItem>>
        //                    {
        //                        IsError = true,
        //                        Message = "Có lỗi xảy ra"
        //                    };
        //                }
        //                var result = await this.CrawlDataCategoryEndjin();
        //                if (result.IsError)
        //                {
        //                    return new APIResult<List<EndjinCategoryItem>>
        //                    {
        //                        IsError = true,
        //                        Message = "Có lỗi xảy ra"
        //                    };
        //                }

        //                string cachedDataString = JsonSerializer.Serialize(result.ResultObject);
        //                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
        //                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
        //                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
        //                .SetSlidingExpiration(TimeSpan.FromMinutes(3));

        //                // Add the data into the cache
        //                await _cache.SetAsync(keyCache, dataToCache, options);
        //                return result;
        //            }
        //        }
        //        else
        //        {
        //            string keyCache = "CACHE_ENDJINCATEGORY";
        //            var result = await this.CrawlDataCategoryEndjin();
        //            if (result.IsError)
        //            {
        //                return new APIResult<List<EndjinCategoryItem>>
        //                {
        //                    IsError = true,
        //                    Message = "Có lỗi xảy ra"
        //                };
        //            }

        //            string cachedDataString = JsonSerializer.Serialize(result.ResultObject);
        //            var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
        //            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
        //            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(3));

        //            // Add the data into the cache
        //            await _cache.SetAsync(keyCache, dataToCache, options);
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResult<List<EndjinCategoryItem>> { IsError = true, Message = $"Lấy thất bại || {ex.Message}", ResultObject = null };
        //    }
        //}
    }
}
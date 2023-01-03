using ML.APIResult;
using ML.Drawls;
using ML.Paging;

namespace BLL.BLL_Drawls
{
    public interface IDrawlsService
    {
        public Task<APIResult<List<DrawlsItem>>> CrawlDataCodeMaze(Paging paging, bool useCache = false);

        //public Task<APIResult<List<DrawlsItem>>> CrawlDataEndjin(string path, Paging paging, bool useCache = false);

        //public Task<APIResult<List<EndjinCategoryItem>>> CrawlDataCategoryEndjin(bool useCache = false);
    }
}
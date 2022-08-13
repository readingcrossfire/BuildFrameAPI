using ML;
using ML.APIResult;
using ML.Drawls;

namespace BLL.BLL_Drawls
{
    public interface IDrawlsService
    {
        public Task<APIResult<List<Drawls>>> CrawlDataCodeMaze(bool useCache = false);
    }
}
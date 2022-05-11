using ML.APIResult;
using ML.Entity;

namespace BLL.BLL_DrawlsFolder.Interface
{
    public interface IDrawlsService
    {
        public Task<APIListObjectResult<DrawlsEntity>> CrawlDataCodeMaze(bool useCache = false);
    }
}
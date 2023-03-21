using DAL.DAL_Logs;
using ML.APIResult;
using ML.Logs;
using ML.Paging;

namespace BLL.BLL_Logs
{
    public class BLL_Logs : ILogsService
    {
        private readonly DAL_Logs _dalLogs;

        public BLL_Logs(DAL_Logs dal_Logs)
        {
            this._dalLogs = dal_Logs;
        }

        public async Task<APIResult<List<LogsItem>>> LogsGet(Paging paging)
        {
            try
            {
                List<LogsItem> lstLogsAll = await _dalLogs.LogsGet(paging) as List<LogsItem>;

                if (lstLogsAll != null && lstLogsAll.Count > 0)
                {
                    return new APIResult<List<LogsItem>>
                    {
                        IsError = false,
                        Message = "Lấy danh sách logs thành công",
                        ResultObject = lstLogsAll
                    };
                }

                return new APIResult<List<LogsItem>>
                {
                    IsError = false,
                    Message = "Lấy danh sách logs thất bại",
                    ResultObject = null
                };
            }
            catch (Exception objEx)
            {
                return new APIResult<List<LogsItem>>
                {
                    IsError = true,
                    Message = objEx.ToString(),
                };
            }
        }

        public async Task<APIResult<List<LogsItem>>> LogsGetAll()
        {
            try
            {
                List<LogsItem> lstLogsAll = await _dalLogs.LogsGetAll() as List<LogsItem>;

                if (lstLogsAll != null && lstLogsAll.Count > 0)
                {
                    return new APIResult<List<LogsItem>>
                    {
                        IsError = false,
                        Message = "Lấy danh sách logs thành công",
                        ResultObject = lstLogsAll
                    };
                }

                return new APIResult<List<LogsItem>>
                {
                    IsError = false,
                    Message = "Lấy danh sách logs thất bại",
                    ResultObject = null
                };
            }
            catch (Exception objEx)
            {
                return new APIResult<List<LogsItem>>
                {
                    IsError = true,
                    Message = objEx.ToString(),
                };
            }
        }
    }
}
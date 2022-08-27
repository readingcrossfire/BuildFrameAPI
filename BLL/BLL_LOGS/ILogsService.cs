﻿using ML.APIResult;
using ML.Logs;

namespace BLL.BLL_Logs
{
    public interface ILogsService
    {
        public Task<APIResult<List<Logs>>> LogsGetAll(bool useCache = false);
    }
}
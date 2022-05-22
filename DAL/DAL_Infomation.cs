
using CONNECTION.DataAccess;
using Dapper;
using Microsoft.Extensions.Configuration;
using ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data;
public class DAL_Infomation : IDAL_Infomation
{
    private readonly ISqlDataAccess _db;
    private readonly IConfiguration _config;

    public DAL_Infomation(ISqlDataAccess db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public Task<IEnumerable<Infomation>> GetInfoALL()
    {
        return _db.LoadDataAsync<Infomation, dynamic>("dbo.Info_GetAll", parameters: new { });
    }
    public IEnumerable<Infomation> GetInfoALL2()
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }
        var a =connection.Query<Infomation>("dbo.Info_GetAll", new { }, commandType: CommandType.StoredProcedure);
        Infomation list = new Infomation();
        list = (Infomation)a;
        return a;
    }

    public List<Infomation> GetInfoALLDatable(ref ResultMessage objResultMessage)
    {
        List<Infomation> list = new List<Infomation>();
        try
        {
            list = _db.LoadData<Infomation, dynamic>("dbo.Info_GetAll", parameters: new { }).ToList();
        }
        catch (Exception ex)
        {
            objResultMessage = new ResultMessage(true,
                ResultMessage.ErrorTypes.GetData,
                "Lỗi lấy thông tin danh sách info", ex.ToString());
          
            return new List<Infomation>();
        }

        return list;

    }

    public async Task<Infomation> GetInfomationByID(int id)
    {
        var result = await _db.LoadDataAsync<Infomation, dynamic>("dbo.Info_GetInfoByID", parameters: new { id = id });
        return result.FirstOrDefault();
    }
    public Task InsertInfo(Infomation infomation) =>
        _db.SaveDataAsync("dbo.Info_Add", new { name = infomation.name, description = infomation.description });
    public Task UpdateInfo(Infomation infomation) =>
        _db.SaveDataAsync("dbo.Info_UPD", infomation);
    public Task DeleteInfo(int id) =>
        _db.SaveDataAsync("dbo.Info_DEL", new Infomation { id = id });
}

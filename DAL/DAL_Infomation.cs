
using CONNECTION.DataAccess;
using Dapper;
using Microsoft.Extensions.Configuration;
using ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data;
public class DAL_Infomation 
{
    public DataTable GetInfoALL(ref ResultMessage objResultMessage)
    {
        SQLHelper sQLHelper = new SQLHelper();
        DataTable table = new DataTable();
        try
        {
            table = sQLHelper.ExecProcedureDataAsDataTable("dbo.Info_GetAll", new {});

        }
        catch (Exception ex)
        {
            objResultMessage = new ResultMessage(true,
              ResultMessage.ErrorTypes.GetData,
              "Lỗi lấy thông tin danh sách info", ex.ToString());

        }
        return table;
    }
    public Infomation GetInfoByID(int intID, ref ResultMessage objResultMessage)
    {
        SQLHelper sQLHelper = new SQLHelper();
        Infomation objInfomation = new Infomation();
        try
        {
            objInfomation = sQLHelper.ExecProcedureDataFirstOrDefaultAsync<Infomation>("dbo.Info_GetInfoByID", new { ID = intID });

        }
        catch (Exception ex)
        {
            objResultMessage = new ResultMessage(true,
              ResultMessage.ErrorTypes.GetData,
              "Lỗi lấy thông tin danh sách info", ex.ToString());

        }
        return objInfomation;
    }
}

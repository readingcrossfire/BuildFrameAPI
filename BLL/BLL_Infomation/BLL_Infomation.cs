
using DAL.Data;
using ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;
public class BLL_Infomation : IBLL_Infomation
{

    public APIResult<object> GetListInfo()
    {

        List<Infomation> lstInfomation = new List<Infomation>();
        ResultMessage objResultMessage = new ResultMessage();
        DAL_Infomation objDAL_Infomation = new DAL_Infomation();
        DataTable dtInfomation = objDAL_Infomation.GetInfoALL(ref objResultMessage);
        APIResult<object> objAPIResult = new APIResult<object>();

        if (objResultMessage.IsError)
            return new APIResult<object>(objResultMessage);
        foreach (DataRow objRow in dtInfomation.Rows)
        {
            lstInfomation.Add(new Infomation(objRow));
        }
        objAPIResult.Message = "Lấy danh sách nhân viên thành công";
        objAPIResult.ResultObject = lstInfomation;
        return objAPIResult;

    }
    public APIResult<object> GetListInfoByID(int intID)
    {
        Infomation objInfomation = new Infomation();
        ResultMessage objResultMessage = new ResultMessage();
        DAL_Infomation objDAL_Infomation = new DAL_Infomation();
        APIResult<object> objAPIResult = new APIResult<object>();
        objInfomation = objDAL_Infomation.GetInfoByID(intID, ref objResultMessage);
        if (objResultMessage.IsError)
            return new APIResult<object>(objResultMessage);

        objAPIResult.Message = "Lấy danh sách nhân viên thành công";
        objAPIResult.ResultObject = objInfomation;
        return objAPIResult;

    }

}

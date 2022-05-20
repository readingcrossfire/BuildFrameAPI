
using DAL.Data;
using ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;
public class BLL_Infomation : IBLL_Infomation
{
    private readonly IDAL_Infomation _db;

    public BLL_Infomation(IDAL_Infomation db)
    {
        _db = db;
    }
    public APIResult GetListInfo()
    {

        List<Infomation> lstInfomation = new List<Infomation>();
        ResultMessage objResultMessage = new ResultMessage();
        //DAL_Infomation dAL_Infomation = new DAL_Infomation();
        //DataTable dtInfomation = objDAL_CoordinatorGroup_Member.GetByCoordinatorGroupUserName(strMemberUserName, ref objResultMessage);
        APIResult objAPIResult = new APIResult();

        lstInfomation = _db.GetInfoALLDatable(ref objResultMessage);

        if (objResultMessage.IsError)
            return new APIResult(objResultMessage);
        //foreach (DataRow objRow in dtCoordinatorGroup_MemberInfo.Rows)
        //{
        //    lstCoordinatorGroup_MemberList.Add(new CoordinatorGroup_Member(objRow));
        //}
        objAPIResult.Message = "Lấy danh sách nhân viên thành công";
        objAPIResult.ResultObject = lstInfomation;
        return objAPIResult;

    }

}

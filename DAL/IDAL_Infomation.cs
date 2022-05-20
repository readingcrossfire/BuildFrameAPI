using ML;

namespace DAL.Data;

public interface IDAL_Infomation
{
    Task DeleteInfo(int id);
    Task<IEnumerable<Infomation>> GetInfoALL();
    List<Infomation> GetInfoALLDatable(ref ResultMessage objResultMessage);
    Task<Infomation> GetInfomationByID(int id);
    Task InsertInfo(Infomation infomation);
    Task UpdateInfo(Infomation infomation);
}
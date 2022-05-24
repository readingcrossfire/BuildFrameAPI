using System.Data;
using SHARED.DbDataCheck;

namespace ML
{
    public class Logs
    {
        public string Id { get; set; }
        public string ApiName { get; set; }
        public string MethodName { get; set; }
        public string Ip { get; set; }
        public DateTime CreatedDate { get; set; }


        public Logs(DataRow objDataRow)
        {
            if (!objDataRow.IsNullColumn("ID"))
            {
                this.Id = Convert.ToString(objDataRow["ID"]);
            }

            if (!objDataRow.IsNullColumn("APINAME"))
            {
                this.ApiName = Convert.ToString(objDataRow["APINAME"]);
            }

            if (!objDataRow.IsNullColumn("METHODNAME"))
            {
                this.MethodName = Convert.ToString(objDataRow["METHODNAME"]);
            }

            if (!objDataRow.IsNullColumn("IP"))
            {
                this.Ip = Convert.ToString(objDataRow["IP"]);
            }

            if (!objDataRow.IsNullColumn("CREATEDDATE"))
            {
                this.CreatedDate = Convert.ToDateTime(objDataRow["CREATEDDATE"]);
            }
        }
    }
}
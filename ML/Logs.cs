using System.Data;

namespace ML
{
    public class Logs
    {
        public string Id { get; set; } = "";
        public string ApiName { get; set; } = "";
        public string MethodName { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public Logs(DataRow objDataRow)
        {
            //if (!objDataRow.IsNullColumn("ID"))
            //{
            //    this.Id = Convert.ToString(objDataRow["ID"]);
            //}

            //if (!objDataRow.IsNullColumn("APINAME"))
            //{
            //    this.ApiName = Convert.ToString(objDataRow["APINAME"]);
            //}

            //if (!objDataRow.IsNullColumn("METHODNAME"))
            //{
            //    this.MethodName = Convert.ToString(objDataRow["METHODNAME"]);
            //}

            //if (!objDataRow.IsNullColumn("DESCRIPTION"))
            //{
            //    this.Description = Convert.ToString(objDataRow["DESCRIPTION"]);
            //}

            //if (!objDataRow.IsNullColumn("CREATEDDATE"))
            //{
            //    this.CreatedDate = Convert.ToDateTime(objDataRow["CREATEDDATE"]);
            //}
        }
    }
}
using System.Data;
using ML.APIResult;

namespace ML
{
    public class Infomation
    {
        private int intID = 0;
        private string strName = "";
        private string strDescription = "";
        public int Id { get => intID; set => intID = value; } 
        public string Name { get => strName; set => strName = value; }
        public string Description { get => strDescription; set => strDescription = value; }
        public Infomation()
        {
        }
        public Infomation(DataRow objRow)
        {
            if (!ConvertData.IsNullColumn(objRow, "ID")) this.intID = Convert.ToInt32(objRow["ID"]);
            if (!ConvertData.IsNullColumn(objRow, "NAME")) this.strName = Convert.ToString(objRow["NAME"]).Trim();
            if (!ConvertData.IsNullColumn(objRow, "DESCRIPTION")) this.strDescription = Convert.ToString(objRow["DESCRIPTION"]).Trim();

        }
    }
}
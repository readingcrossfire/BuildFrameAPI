using System.Data;

namespace SHARED.DbDataCheck
{
    public static class DbDataCheck
    {
        public static bool IsNullColumn(this DataRow objDataRow, string columnName)
        {
            if (!objDataRow.Table.Columns.Contains(columnName))
            {
                return true;
            }

            return Convert.IsDBNull(objDataRow[columnName]);
        }
    }
}
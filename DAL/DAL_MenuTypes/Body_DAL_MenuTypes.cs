using System.Data;
using CONNECTION.DapperConnection;
using ML.MenuTypes;

namespace DAL.DAL_MenuTypes
{
    public partial class DAL_MenuTypes
    {
        public async Task<IEnumerable<MenuTypes>> MenuTypesGetAll()
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();

            try
            {
                dapperConnection.OpenConnect();
                dapperConnection.CreateNewStoredProcedure("MENUTYPES_GETALL");
                var result = await dapperConnection.QueryAsync<MenuTypes>();
                return result;
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }

        public async Task<DataTable> MenuTypesGetAllToDataTable()
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();
            try
            {
                dapperConnection.OpenConnect();
                dapperConnection.CreateNewStoredProcedure("MENUTYPES_GETALL");
                var result = await dapperConnection.QueryToDataTable();
                return result;
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }
    }
}
using System.Data;

namespace CONNECTION.DapperConnection
{
    public interface IDapperConnection
    {
        public static IDapperConnection CreateConnection(string connectionString = "") => throw new NotImplementedException();
        public void BeginTransaction();
        public void Commit();
        public void RollBack();
        public void Dispose();
        public void OpenConnect();
        public void CloseConnect();
        public void CreateNewStoredProcedure(string nameStore);
        public void AddParameter(string field, object value);
        public Task<int> ExecuteAsync();
        public Task<IEnumerable<T>> QueryAsync<T>();
        public Task<IEnumerable<dynamic>> QueryAsync();
        public Task<DataTable> QueryToDataTable();
    }
}
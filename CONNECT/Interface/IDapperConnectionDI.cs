namespace CONNECTION.Interface
{
    public interface IDapperConnectionDI
    {
        public IDapperConnectionDI CreateConnection(string connectionString = "");

        public void OpenConnect();

        public void CloseConnect();

        public void CreateNewStoredProcedure(string nameStore);

        public void AddParameter(string field, object value);

        public Task<int> ExecuteAsync();

        public Task<IEnumerable<T>> QueryAsync<T>();

        public Task<IEnumerable<dynamic>> QueryAsync();
    }
}
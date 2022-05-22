namespace CONNECTION.DataAccess
{
    public interface ISqlDataAccess
    {
        IEnumerable<T> LoadData<T, U>(string storeProcedure, U parameters, string connectionID = "Default");
        Task<IEnumerable<T>> LoadDataAsync<T, U>(string storeProcedure, U parameters, string connectionID = "Default");
        int SaveData<T>(string storeProcedure, T parameters, string connectionID = "Default");
        Task SaveDataAsync<T>(string storeProcedure, T parameters, string connectionID = "Default");
    }
}
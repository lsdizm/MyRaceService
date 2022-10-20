namespace my.race.connect {
    public interface IDataBases 
    {
        MySql.Data.MySqlClient.MySqlConnection Connect();
        Task<int> ExecuteAsync(string sql, MySql.Data.MySqlClient.MySqlConnection connection);

        Task<List<string>> SelectAllSqls();
        Task<List<T>> SelectAsync<T>(string sqlId, object jsonParameters);

        Task<int> SaveLog(MySql.Data.MySqlClient.MySqlConnection connection, string title, DateTime dateTime, string logContent);
    }
}

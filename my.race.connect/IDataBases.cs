using my.race.model;

namespace my.race.connect {
    public interface IDataBases 
    {
        MySql.Data.MySqlClient.MySqlConnection Connect();
        Task<int> ExecuteAsync(string sql, MySql.Data.MySqlClient.MySqlConnection connection);

        Task<List<T>> SelectConfiguration<T>(string tableName);
        Task<List<string>> SelectAllSqls();
         Task<ApiInformation> SelectApiInformation(string id);
        Task<List<T>> SelectAsync<T>(string sqlId, object jsonParameters);


        Task<List<RaceResult>> UpdateRaceResult(List<RaceResult> raceResult);


        Task<int> SaveLog(MySql.Data.MySqlClient.MySqlConnection connection, string title, DateTime dateTime, string logContent);
    }
}

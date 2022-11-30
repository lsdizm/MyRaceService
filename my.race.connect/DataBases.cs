using System;
using System.Dynamic;
using my.race.model;
using Newtonsoft.Json;

namespace my.race.connect {
    public class DataBases : IDataBases
    {
        public DataBases()
        {
        }

        // TO-DO connection 정보 관리 방안
        // TO-DO Search 공통화, 파라미터 바인딩 방안.
        public MySql.Data.MySqlClient.MySqlConnection Connect()
        {
            var _connectionString = "host=152.70.232.248;port=3306;user id=mj;password=!Dhfkzmffkdnem1;database=mj;";
            return new MySql.Data.MySqlClient.MySqlConnection(_connectionString);
        }

        public async Task<int> ExecuteAsync(string sql, MySql.Data.MySqlClient.MySqlConnection connection)
        {
            var command = new MySql.Data.MySqlClient.MySqlCommand(sql, connection);
            var result = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<List<T>> SelectConfiguration<T>(string tableName) 
        {
            using (var connection = Connect()) 
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var sqlContent = $"select * from " + tableName;
                var sql = await Dapper.SqlMapper.QueryAsync<T>(connection, sqlContent).ConfigureAwait(false);
                return sql.ToList();
            }   
        }        

        public async Task<List<string>> SelectAllSqls() 
        {
            using (var connection = Connect()) 
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var sqlContent = $"select distinct Id from SQL_STORAGE";
                var sql = await Dapper.SqlMapper.QueryAsync<string>(connection, sqlContent).ConfigureAwait(false);
                return sql.ToList();
            }   
        }

        public async Task<ApiInformation> SelectApiInformation(string id) 
        {
            using (var connection = Connect()) 
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var sqlContent = $"select Id, UrlAddress, Parameters from ApiInformation where Id = @Id";
                var sql = await Dapper.SqlMapper.QueryFirstOrDefaultAsync<ApiInformation>(connection, sqlContent, new { Id = id }).ConfigureAwait(false);
                return sql;
            }   
        }

        public async Task<List<T>> SelectAsync<T>(string sqlId, object jsonParameters)
        {
            var convert = JsonConvert.SerializeObject(jsonParameters);            
            var parameters = JsonConvert.DeserializeObject<object>(convert);            

            using (var connection = Connect()) 
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var sqlContent = $"select SQL_CONTENT from SQL_STORAGE where id = '{sqlId}'";
                var sql = await Dapper.SqlMapper.QueryFirstAsync<string>(connection, sqlContent).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(sql)) {
                    var result = await Dapper.SqlMapper.QueryAsync<T>(connection, sql, parameters).ConfigureAwait(false);
                    return result.ToList();
                }
                else 
                {
                    return new List<T>();
                }
            }   
        }

        public async Task<int> SaveLog(MySql.Data.MySqlClient.MySqlConnection connection, string title, DateTime dateTime, string logContent)
        {
            var sql = $"insert into API_LOG (ID, TITLE, DATETIME, LOG_CONTENTS) values ('{Guid.NewGuid().ToString()}', '{title}', '{dateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{logContent}');";                    
            var command = new MySql.Data.MySqlClient.MySqlCommand(sql, connection);
            var result = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            return result;            
        }
    }
}


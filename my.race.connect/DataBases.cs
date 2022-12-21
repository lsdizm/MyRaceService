using System;
using System.Dynamic;
using Dapper;
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

        public async Task<List<T>> SelectAsync<T>(string sqlId, Dictionary<string, string> parameters)
        {
            //var convert = JsonConvert.SerializeObject(jsonParameters);            
            //var parameters = JsonConvert.DeserializeObject<ExpandoObject>(convert);            

            using (var connection = Connect()) 
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var sqlContent = $"select SQL_CONTENT from SQL_STORAGE where id = '{sqlId}'";
                var sql = await Dapper.SqlMapper.QueryFirstAsync<string>(connection, sqlContent).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(sql)) 
                {
                    var dynamicParameters = new DynamicParameters();
                    foreach(var item in parameters)
                    {
                        dynamicParameters.Add("@" + item.Key.Replace("@", ""), item.Value);
                    }

                    var result = await Dapper.SqlMapper.QueryAsync<T>(connection, sql, dynamicParameters).ConfigureAwait(false);
                    return result.ToList();
                }
                else 
                {
                    return new List<T>();
                }
            }   
        }
        
        public async Task<List<RaceResult>> UpdateRaceResult(List<RaceResult> raceResult)
        {
            if (raceResult != null && raceResult.Any())
            {
                var dateList = raceResult.Select(s => s.rcDate).Distinct();
                var queryParameter = " in ('" + string.Join("','", dateList) + "')";
                var sql = "delete from RaceResult where rcDate " + queryParameter + ";";
                sql += "insert into RaceResult (chaksun, diffTot, divide, hrName, hrno, jkName, jkNo, meet, noracefl, prow, prowName, prtr, prtrName, rankKind, rc10dusu, rcAge, rcBudam, rcChul, rcCode, rcDate, rcDiff2, rcDiff3, rcDiff4, rcDiff5, rcDist, rcFrflag, rcGrade, rcHrfor, rcHrnew, rcNo, rcNrace, rcOrd, rcP1Odd, rcP2Odd, rcP3Odd, rcP4Odd, rcP5Odd, rcP6Odd, rcP8Odd, rcPlansu, rcRank, rcSex, rcSpcbu, rcTime, rcVtdusu, rundayth, track, weath, wgHr) values ";

                sql += string.Join(",", raceResult.Select(item => "('" + string.Join("','", new object[]
                    {
                        item.chaksun,
                        item.diffTot,
                        item.divide,
                        item.hrName,
                        item.hrno,
                        item.jkName,
                        item.jkNo,
                        item.meet,
                        item.noracefl,
                        item.prow,
                        item.prowName,
                        item.prtr,
                        item.prtrName,
                        item.rankKind,
                        item.rc10dusu,
                        item.rcAge,
                        item.rcBudam,
                        item.rcChul,
                        item.rcCode,
                        item.rcDate,
                        item.rcDiff2,
                        item.rcDiff3,
                        item.rcDiff4,
                        item.rcDiff5,
                        item.rcDist,
                        item.rcFrflag,
                        item.rcGrade,
                        item.rcHrfor,
                        item.rcHrnew,
                        item.rcNo,
                        item.rcNrace,
                        item.rcOrd,
                        item.rcP1Odd,
                        item.rcP2Odd,
                        item.rcP3Odd,
                        item.rcP4Odd,
                        item.rcP5Odd,
                        item.rcP6Odd,
                        item.rcP8Odd,
                        item.rcPlansu,
                        item.rcRank,
                        item.rcSex,
                        item.rcSpcbu,
                        item.rcTime,
                        item.rcVtdusu,
                        item.rundayth,
                        item.track,
                        item.weath,
                        item.wgHr
                    }) + "')"));
                
                
                using (var connection = Connect())
                {
                    var command = new MySql.Data.MySqlClient.MySqlCommand(sql, connection);
                    await connection.OpenAsync().ConfigureAwait(false);
                    var result = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    await connection.CloseAsync().ConfigureAwait(false);
                }
            }
            // 기존 내역 삭제후 새로 입력

            //var sql = $"insert into API_LOG (ID, TITLE, DATETIME, LOG_CONTENTS) values ('{Guid.NewGuid().ToString()}', '{title}', '{dateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{logContent}');";
            //var command = new MySql.Data.MySqlClient.MySqlCommand(sql, connection);
            //var result = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            //return result;
            return raceResult;
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


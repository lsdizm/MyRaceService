using System.Web;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using my.race.model;
using Microsoft.Extensions.Logging;

namespace my.race.connect 
{
    public class Configurations : IConfigurations
    {
        private readonly List<MyRaceEnvironment> _myRaceEnvironment;
        private readonly ILogger<DataAPI>? _logger;

        public Configurations()
        {
            _myRaceEnvironment = new List<MyRaceEnvironment>();

            using (var connection = connect()) 
            {
                connection.Open();
                var sqlContent = $"select * from MyRaceEnvironment";
                _myRaceEnvironment = Dapper.SqlMapper.Query<MyRaceEnvironment>(connection, sqlContent).ToList();
            }
        }

        public Configurations(ILogger<DataAPI> logger) : this()
        {
            _logger = logger;
        }

        public List<MyRaceEnvironment> GetMyRaceEnvironment(string itemCode)
        {
            return _myRaceEnvironment.Where(w => string.IsNullOrEmpty(itemCode) || itemCode == w.ItemCode).ToList();
        }

        public async Task<MyRaceEnvironment> GetEnvironment()
        {
            using (var connection = connect()) 
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var sqlContent = $"select * from MyRaceEnvironment";
                var result = await Dapper.SqlMapper.QueryFirstOrDefaultAsync<MyRaceEnvironment>(connection, sqlContent).ConfigureAwait(false);
                return result;
            }   
        }

        private MySql.Data.MySqlClient.MySqlConnection connect()
        {
            var _connectionString = "host=152.70.232.248;port=3306;user id=mj;password=!Dhfkzmffkdnem1;database=mj;";
            return new MySql.Data.MySqlClient.MySqlConnection(_connectionString);
        }
    }
}
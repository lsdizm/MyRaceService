using System.Web;
using System.Dynamic;
using System.Net.Http.Headers;
//using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using my.race.model;
using Microsoft.Extensions.Logging;

namespace my.race.connect 
{
    public class DataAPI : IDataAPI
    {
        private readonly ILogger<DataAPI>? _logger;
        private const string serviceKey = "gKTNtNTmRwLKq8JD1zkpfaggw28u5FJ%2F%2BCZ3PpQxX15sOjBrSoWWMf2oSe3dG%2BJqsIcXim5EW5xlTx1jxGqKgA%3D%3D";
        private const string dataUrlAddress = "https://apis.data.go.kr/";

        public DataAPI()
        {
        }
        
        public DataAPI(ILogger<DataAPI> logger) : this()
        {
            _logger = logger;
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(dataUrlAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public async Task<List<T>> GetFromAPI<T>(string path, Dictionary<string, string> parameters) where T : class
        {
            var aTypeResult = new List<dynamic>();
            var pageNo = 1;
            var numOfRows = 500;

            parameters.TryAdd("pageNo", pageNo.ToString());
            parameters.TryAdd("numOfRows", numOfRows.ToString());
            parameters.TryAdd("serviceKey", serviceKey);
            parameters.TryAdd("_type", "json");

            using (var client = GetHttpClient())
            {
                while (true) 
                {
                    try
                    {
                        parameters["pageNo"] = pageNo.ToString();

                        var queryString = string.Join("&", parameters.Select(s => string.Format("{0}={1}", s.Key, s.Value)));
                        var url = $"{path}?{queryString}";
                        var response = await client.GetAsync(url).ConfigureAwait(false);
                        response.EnsureSuccessStatusCode();
                        var responseString = await response.Content.ReadAsStringAsync();                
                        var apiResult = JsonConvert.DeserializeObject<ApiResult>(responseString);
                        
                        if (apiResult == null || apiResult.response  == null || apiResult.response.body == null ||
                            apiResult.response.body.totalCount.HasValue == false)
                        {
                            break;
                        }
                        else if (apiResult.response.body.items != null && 
                            apiResult.response.body.items.item != null &&
                            apiResult.response.body.items.item.Any())
                        {
                            aTypeResult.AddRange(apiResult.response.body.items.item);
                        }

                        if (apiResult.response.body.totalCount < (pageNo * numOfRows))
                        {
                            break;
                        }

                        if (_logger != null) 
                        { 
                            _logger.LogInformation(queryString);
                            _logger.LogInformation(aTypeResult.Count.ToString());
                        }

                        pageNo = pageNo + 1;
                    }
                    catch (Exception ex)
                    {
                        if (_logger != null)
                        {
                            _logger.LogInformation(ex.ToString());
                        }
                        break;
                    }
                }
            }

            if (aTypeResult != null && aTypeResult.Any())
            {
                var result = new List<T>();
                foreach(var item in aTypeResult)
                {
                    var jsonResult = JsonConvert.SerializeObject(item);

                    var converted = JsonConvert.DeserializeObject<T>(jsonResult);
                    if (converted != null)
                    {
                        result.Add(converted);
                    }
                }
                return result;
            }
            else
            {
                return new List<T>();
            }

        }
    
        public async Task<List<HorseApi>> GetHorceResult(string meet, string rank)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("meet", meet);
            parameters.Add("rank", rank);
            var result = await GetFromAPI<HorseApi>("B551015/racehorselist/getracehorselist", parameters).ConfigureAwait(false);
            return result;
        }
    }
}
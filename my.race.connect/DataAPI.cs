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
        private readonly IConfigurations _configurations;
        private readonly string serviceKey;
        private const string dataUrlAddress = "https://apis.data.go.kr/";

        public DataAPI(ILogger<DataAPI>? logger, 
            IConfigurations configurations)
        {
            _logger = logger;
            _configurations = configurations;
            serviceKey = _configurations.GetMyRaceEnvironment("API-SERVICE-KEY")?.FirstOrDefault()?.ItemValue ?? string.Empty;
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
                        
                        if (apiResult == null || 
                            apiResult.response  == null ||                            
                            apiResult.response.body == null ||                            
                            apiResult.response.body.totalCount.HasValue == false)
                        {
                            var errorMessage = string.Empty;
                            switch (apiResult?.response?.header?.resultCode)
                            {
                                case "1": errorMessage = "APPLICATION ERROR(1) :"; break;
                                case "4": errorMessage = "HTTP_ERROR(4) :"; break;
                                case "12": errorMessage = "NO_OPENAPI_SERVICE_ERROR(12) :"; break;
                                case "20": errorMessage = "SERVICE_ACCESS_DENIED_ERROR(20) :"; break;
                                case "22": errorMessage = "LIMITED_NUMBER_OF_SERVICE_REQUESTS_EXCEEDS_ERROR(22) :"; break;
                                case "30":errorMessage = "SERVICE_KEY_IS_NOT_REGISTERED_ERROR(30) :"; break;
                                case "31": errorMessage = "DEADLINE_HAS_EXPIRED_ERROR(31) :"; break;
                                case "32": errorMessage = "UNREGISTERED_IP_ERROR(32) :"; break;
                                case "99": errorMessage = "UNKNOWN_ERROR(99) :"; break;
                                default:
                                    break;
                            }

                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                throw new Exception(errorMessage + " [\"" + url + "\"] " + apiResult?.response?.header?.resultMsg);
                            }

                            //break;
                        }
                        else if (apiResult.response.body.items != null && 
                            apiResult.response.body.items.item != null &&
                            apiResult.response.body.items.item.Any())
                        {
                            aTypeResult.AddRange(apiResult.response.body.items.item);
                        }

                        if (apiResult?.response?.body?.totalCount < (pageNo * numOfRows))
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
                        await _configurations.SendMessage(ex.Message);
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
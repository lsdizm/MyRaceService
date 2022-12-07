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
        private readonly string serviceKey = "gKTNtNTmRwLKq8JD1zkpfaggw28u5FJ%2F%2BCZ3PpQxX15sOjBrSoWWMf2oSe3dG%2BJqsIcXim5EW5xlTx1jxGqKgA%3D%3D";

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
            client.BaseAddress = new Uri("https://apis.data.go.kr/");
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
                        //var apiResult = JsonSerializer.Deserialize<ApiResult>(responseString);
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
                try
                {
                    foreach(var item in aTypeResult)
                    {
                        var sss = JsonConvert.SerializeObject(item);

                        var converted = JsonConvert.DeserializeObject<T>(sss);
                        if (converted != null)
                        {
                            result.Add(converted);
                        }
                    }
                }
                catch
                {

                }
                return result;
            }
            else
            {
                return new List<T>();
            }

        }

        public async Task<List<RaceResultApi>> GetRaceResult(DateTime fromDate, DateTime toDate)
        {
            return null;
        }

        public async Task<dynamic> GetDataTest(string url)
        {
            var result = await GetFromAPI<dynamic>(url, new Dictionary<string, string>()).ConfigureAwait(false);
            return result;
        }

        public async Task<List<RaceResult>> GetRaceResultDetail(DateTime fromDate, DateTime toDate)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("rc_date_fr", fromDate.ToString("yyyyMMdd"));
            parameters.Add("rc_date_to", toDate.ToString("yyyyMMdd"));
            var result = await GetFromAPI<RaceResult>("B551015/API186/SeoulRace", parameters).ConfigureAwait(false);
            return result;
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
/*




            var rc_date_fr = ;
            var  = ;

            using (var client = GetHttpClient())
            {
                while (true) 
                {
                    try
                    {
                        var response = await client.GetAsync($"{path}?pageNo={pageNo}&numOfRows={numOfRows}&rc_date_fr={rc_date_fr}&rc_date_to={rc_date_to}&serviceKey={serviceKey}").ConfigureAwait(false);
                        response.EnsureSuccessStatusCode();
                        var responseString = await response.Content.ReadAsStringAsync();                
                        var apiResult = JsonSerializer.Deserialize<ApiResult>(responseString);

                        if (apiResult == null || apiResult.response  == null || apiResult.response.body == null ||
                            apiResult.response.body.totalCount.HasValue == false)
                        {
                            break;
                        }
                        else 
                        {
                            if (apiResult.response.body.items != null && apiResult.response.body.items.item != null){
                                // result.AddRange(apiResult.response.body.items.item);
                            }
                        }
                        pageNo = pageNo + 1;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                return result;
            }





            return resulta;
            var path = "B551015/racehorselist/getracehorselist";
            var result = new List<HorseResult>();
            var pageNo = 1;
            var numOfRows = 10;

            using (var client = GetHttpClient())
            {
                while (true) 
                {
                    try
                    {
                        var url = $"{path}?pageNo={pageNo}&numOfRows={numOfRows}&meet={meet}&rank={rank}&serviceKey={serviceKey}";                                                
                        var response = await client.GetAsync(url).ConfigureAwait(false);
                        response.EnsureSuccessStatusCode();
                        var responseString = await response.Content.ReadAsStringAsync();                
                        var apiResult = JsonSerializer.Deserialize<ApiResult>(responseString);

                        if (apiResult == null || apiResult.response  == null || apiResult.response.body == null ||
                            apiResult.response.body.totalCount.HasValue == false)
                        {
                            throw new Exception("API ERROR ");                            
                        }
                        else if (apiResult.response.body.items.GetType() == typeof(System.Text.Json.JsonElement))
                        {                            
                            var items = (apiResult.response.body.items as System.Text.Json.JsonElement?);
                            var itemsJsonString = JsonSerializer.Serialize(items);
                            if (!string.IsNullOrWhiteSpace(itemsJsonString) && itemsJsonString != "\"\"") 
                            {
                                var modelResult = JsonSerializer.Deserialize<HorseResultApiItems>(itemsJsonString);
                                if (modelResult != null && modelResult.item.Any())
                                {
                                    result.AddRange(modelResult.item);
                                }
                            }
                        }

                        if (apiResult.response.body.totalCount < (pageNo * numOfRows))
                        {
                            break;
                        }

                        pageNo = pageNo + 1;

                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(result.Count.ToString());
                        _logger.LogInformation(ex.ToString());
                        break;
                    }
                }

                return result;*/


/*
 
 //var items = apiResult.response.body.items.item;
                            //var itemsJsonString = JsonSerializer.Serialize(items);
                            //if (!string.IsNullOrWhiteSpace(itemsJsonString) && itemsJsonString != "\"\"")
                            //{
                            //    var modelResult = JsonSerializer.Deserialize<JsonElement>(itemsJsonString);
                            //    modelResult.TryGetProperty("item", out var modelResultProperty);
                            //    var modelResultPropertyValue = modelResultProperty.Deserialize(typeof(List<T>));
                            //    if (modelResultPropertyValue != null)
                            //    {
                            //        result.AddRange((modelResultPropertyValue as List<T>));
                            //    }
                            //}
 //else if (apiResult.response.body.items.GetType() == typeof(System.Text.Json.JsonElement))
                        //{                            
                        //    var items = (apiResult.response.body.items as System.Text.Json.JsonElement?);
                        //    var itemsJsonString = JsonSerializer.Serialize(items);
                        //    if (!string.IsNullOrWhiteSpace(itemsJsonString) && itemsJsonString != "\"\"") 
                        //    {
                        //        var modelResult = JsonSerializer.Deserialize<JsonElement>(itemsJsonString);
                        //        modelResult.TryGetProperty("item", out var modelResultProperty);                                        
                        //        var modelResultPropertyValue = modelResultProperty.Deserialize(typeof(List<T>));
                        //        if (modelResultPropertyValue != null) 
                        //        {
                        //            result.AddRange((modelResultPropertyValue as List<T>));
                        //        }
                        //    }
                        //}
 */
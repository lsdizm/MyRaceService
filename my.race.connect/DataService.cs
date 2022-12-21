using my.race.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.race.connect
{
    public class DataService : IDataService
    {
        private readonly IDataAPI _api;
        private readonly IDataBases _bases;
        private readonly IConfigurations _configurations;

        public DataService(IDataAPI api, 
            IDataBases bases, 
            IConfigurations configurations)
        {
            _api = api;
            _bases = bases;
            _configurations = configurations;
        }

        public async Task<List<RaceResult>> GetRaceResultDetail(DateTime fromDate, DateTime toDate, bool overwrite)
        {
            var result = new List<RaceResult>();
            var parameters = new Dictionary<string, string>();
            var apiInformation = await _bases.SelectApiInformation("RaceResult").ConfigureAwait(false);

            if (apiInformation != null) 
            { 
                var date = fromDate.Date;
                parameters.Add("rc_date_fr", date.ToString("yyyyMMdd"));
                parameters.Add("rc_date_to", date.ToString("yyyyMMdd"));

                while (date <= toDate)
                {
                    parameters["rc_date_fr"] = date.ToString("yyyyMMdd");
                    parameters["rc_date_to"] = date.ToString("yyyyMMdd");

                    // check exists 
                    if (!overwrite)
                    {
                        var extist = await _bases.SelectAsync<RaceResult>(
                            "race-result-by-date", 
                            new Dictionary<string, string>()
                            {
                                { "rcdate", date.ToString("yyyyMMdd")}
                            }).ConfigureAwait(false);

                        if (extist != null && extist.Any())
                        {
                            date = date.AddDays(1);
                            continue;
                        }
                    }                    

                    var apiResult = await _api.GetFromAPI<RaceResult>(apiInformation.UrlAddress, parameters).ConfigureAwait(false);

                    if (apiResult != null && apiResult.Any())
                    {
                        var dbUpdateResult = await _bases.UpdateRaceResult(apiResult).ConfigureAwait(false);
                        await _configurations.SendMessage("결과 업데이트 : " + date.ToString("yyyyMMdd")).ConfigureAwait(false);
                        result.AddRange(apiResult);
                    }
                    
                    date = date.AddDays(1);
                }
            }
            return result;
        }

    }
}

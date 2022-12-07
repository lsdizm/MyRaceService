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
        private IDataAPI _api;
        private IDataBases _bases;

        public DataService(IDataAPI api, IDataBases bases)
        {
            _api = api;
            _bases = bases;
        }

        public async Task<List<RaceResult>> GetRaceResultDetail(DateTime fromDate, DateTime toDate)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("rc_date_fr", fromDate.ToString("yyyyMMdd"));
            parameters.Add("rc_date_to", toDate.ToString("yyyyMMdd"));
            var apiInformation = await _bases.SelectApiInformation("RaceResult").ConfigureAwait(false);

            if (apiInformation != null) 
            { 
                var apiResult = await _api.GetFromAPI<RaceResult>(apiInformation.UrlAddress, parameters).ConfigureAwait(false);

                if (apiResult != null && apiResult.Any())
                {
                    var result = await _bases.UpdateRaceResult(apiResult).ConfigureAwait(false);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }

    }
}

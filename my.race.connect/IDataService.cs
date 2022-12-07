using my.race.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.race.connect
{
    public interface IDataService
    {
        Task<List<RaceResult>> GetRaceResultDetail(DateTime fromDate, DateTime toDate);
    }
}

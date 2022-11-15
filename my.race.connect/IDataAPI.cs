using my.race.model;

namespace my.race.connect {
    public interface IDataAPI
    {

         Task<dynamic> GetDataTest(string url);
        Task<List<RaceResultApi>> GetRaceResult(DateTime fromDate, DateTime toDate);
        Task<List<RaceApi>> GetRaceResultDetail(DateTime fromDate, DateTime toDate);
        Task<List<HorseApi>> GetHorceResult(string meet, string rank);
    }
}

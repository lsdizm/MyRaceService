using my.race.model;

namespace my.race.connect {
    public interface IDataAPI
    {
        Task<List<T>> GetFromAPI<T>(string path, Dictionary<string, string> parameters) where T : class;
        Task<dynamic> GetDataTest(string url);
        Task<List<RaceResultApi>> GetRaceResult(DateTime fromDate, DateTime toDate);
        Task<List<RaceApi>> GetRaceResultDetail(DateTime fromDate, DateTime toDate);
        Task<List<HorseApi>> GetHorceResult(string meet, string rank);
    }
}

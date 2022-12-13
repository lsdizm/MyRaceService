using my.race.model;

namespace my.race.connect {
    public interface IDataAPI
    {
        Task<List<T>> GetFromAPI<T>(string path, Dictionary<string, string> parameters) where T : class;
        Task<List<HorseApi>> GetHorceResult(string meet, string rank);
    }
}

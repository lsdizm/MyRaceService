using my.race.model;

namespace my.race.connect 
{
    public interface IConfigurations
    {
        List<MyRaceEnvironment> GetMyRaceEnvironment(string itemCode);
        Task<MyRaceEnvironment> GetEnvironment();
        Task<string> SendMessage(string message);
    }
}

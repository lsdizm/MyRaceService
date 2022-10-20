namespace my.race.model {
    public class ApiLog
    {
        public string Id { get; set; } = string.Empty;
        public string Title {get; set;} = string.Empty;
        public DateTime DateTime {get; set;}
        public string LogContents {get; set;} = string.Empty;
    }
}
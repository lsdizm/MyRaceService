namespace my.race.model {
    public class RaceResult
    {
        public string Location {get; set;} = string.Empty;
        public DateTime RaceDate { get; set; }
        public int RaceNumber {get; set;}
        public int TrackNumber {get ;set;}
        public string HorseNumber {get; set;} = string.Empty;
        public string RiderNumber {get; set;} = string.Empty;
        public int Rank {get; set;}
    }
}
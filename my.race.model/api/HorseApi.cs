using System.Text.Json.Serialization;

namespace my.race.model {
    public class HorseApi
    {
        public int? age { get; set; }
        public int? chulYn { get; set; }
        public string? hrName { get; set; }
        public string? hrNo { get; set; }
        public string? jun { get; set; }
        public int? meet { get; set; }
        public int? minRcDate { get; set; }
        public string? nameSp { get; set; }
        public string? prdCty { get; set; }
        public int? prizeYear { get; set; }        
        public dynamic? prizetsum { get; set; }
        public string? rank { get; set; }
        public int? rating1 { get; set; }
        public int? rating2 { get; set; }
        public int? rating3 { get; set; }
        public int? rating4 { get; set; }
        public string? sex { get; set; }
        public string? spTerm { get; set; }
        public int? stDate { get; set; }
    }
}
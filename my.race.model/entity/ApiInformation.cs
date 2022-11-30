namespace my.race.model {
    public class ApiInformation
    {
        public string ID { get; set; } = string.Empty;
        public string UrlAddress {get; set;} = string.Empty;
        public string Parameters {get; set;} = string.Empty;
        public List<string> ParameterList 
        {
            get 
            {
                if (!string.IsNullOrEmpty(Parameters)) 
                {
                    return Parameters.Split(",").ToList();
                }
                else 
                {
                    return new List<string>();
                }
            }
        }
        public Dictionary<string, string> MakeParameters(List<string> values)
        {        
            var queryStringList = Parameters?.Split(",").ToList() ?? new List<string>();
            if (values != null && values.Count == queryStringList.Count){
                var result = queryStringList.Select((k, i) => new {k, v = values[i]}).ToDictionary(d => d.k, d => d.v);
                return result;
                // var queryString = string.Join("&", queryStringList.Select(s => string.Format("{0}={1}", s, s)));
                // var url = $"{UrlAddress}?{queryString}";
                // return url;
            }
            else 
            {
                return new Dictionary<string, string>();
            }

            
        }
    }
}
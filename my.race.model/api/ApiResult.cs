namespace my.race.model {
    public class ApiResult
    {
        public Response response { get; set; } = new Response();
    }

    public class Response 
    {
        public Header header { get; set; } = new Header();
        public Body body { get; set; } = new Body();
    }

    public class Header
    {
        public string? resultCode { get; set; }
        public string? resultMsg { get; set; }
    }

    public class Body
    {
        public Items? items { get; set; }
        public int? numOfRows { get; set; }
        public int? pageNo { get; set; }
        public int? totalCount { get; set; }
    }

    public class Items
    {
        public List<object>? item { get; set; }
    }
}
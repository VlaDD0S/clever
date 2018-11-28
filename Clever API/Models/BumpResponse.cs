using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class BumpResponse : ApiResponse
    {
        public new Response response { get; set; }
        public new class Response
        {
            // public List<?> Users {get; set;}
            [JsonProperty(PropertyName = "bonus")]
            public int Bonus { get; set; }
        }
    }
}

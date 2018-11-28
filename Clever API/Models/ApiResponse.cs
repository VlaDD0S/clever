using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class ApiResponse
    {
        [JsonProperty(PropertyName = "response")]
        public Response response { get; set; }
        public class Response { }

        [JsonProperty(PropertyName = "error")]
        public Error error { get; set; }
        public class Error
        {
            [JsonProperty(PropertyName = "error_code")]
            public int Code { get; set; }

            [JsonProperty(PropertyName = "error_msg")]
            public string Message { get; set; }
        }
    }
}

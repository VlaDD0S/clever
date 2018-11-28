using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class GetLongPollResponse : ApiResponse
    {
        public new Response response { get; set; }
        public new class Response
        {
            [JsonProperty(PropertyName = "url")]
            public string Url { get; set; }
        }
    }
}

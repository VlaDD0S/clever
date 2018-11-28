using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class ActionResponse : ApiResponse
    {
        public new Response response { get; set; }
        public new class Response
        {
            [JsonProperty(PropertyName = "success")]
            public bool Success { get; set; }

            [JsonProperty(PropertyName = "old_balance")]
            public int OldBalance { get; set; }

            [JsonProperty(PropertyName = "new_balance")]
            public int NewBalance { get; set; }

            [JsonProperty(PropertyName = "prize")]
            public int Prize { get; set; }
        }
    }
}

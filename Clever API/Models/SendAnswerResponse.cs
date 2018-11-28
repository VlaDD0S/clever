using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class SendAnswerResponse : ApiResponse
    {
        [JsonProperty(PropertyName = "response")]
        public int Result { get; set; }
    }

    public class CreateCommentResponse : SendAnswerResponse { }
}

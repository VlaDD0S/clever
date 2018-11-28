using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Clever_API.Models.LPResponseModels
{
    public class Question : ResponseModel
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "time")]
        public int? Time { get; set; }

        [JsonProperty(PropertyName = "answers")]
        public List<AnswerInfo> Answers { get; set; }
        
        public class AnswerInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
        }
    }
}

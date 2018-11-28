using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class GiftsDataResponse : ApiResponse
    {
        public new Response response { get; set; }
        public new class Response
        {
            [JsonProperty(PropertyName = "gifts")]
            public List<GiftsInfo> Gifts { get; set; }

            [JsonProperty(PropertyName = "rules")]
            public RulesInfo Rules { get; set; }

            public class GiftsInfo
            {
                public const int EXTRA_LIFE_ID = 1;
                public const int FRIEND_ANSWERS_ID = 50;

                [JsonProperty(PropertyName = "id")]
                public int Id { get; set; }

                [JsonProperty(PropertyName = "name")]
                public string Name { get; set; }

                [JsonProperty(PropertyName = "description")]
                public string Description { get; set; }

                [JsonProperty(PropertyName = "type")]
                public string Type { get; set; }

                [JsonProperty(PropertyName = "cost")]
                public int Cost { get; set; }
            }

            public class RulesInfo
            {
                [JsonProperty(PropertyName = "id")]
                public int Id { get; set; }

                [JsonProperty(PropertyName = "list_title")]
                public int Name { get; set; }

                [JsonProperty(PropertyName = "value")]
                public int? Value { get; set; }

                [JsonProperty(PropertyName = "prize")]
                public int Prize { get; set; }
            }
        }
    }
}

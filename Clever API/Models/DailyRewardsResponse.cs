using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class DailyRewardsResponse : ApiResponse
    {
        public new Response response { get; set; }
        public new class Response
        {
            [JsonProperty(PropertyName = "daily_rewards")]
            public List<Rewards> DailyRewards { get; set; }

            [JsonProperty(PropertyName = "day_number")]
            public int NumberDays { get; set; }

            [JsonProperty(PropertyName = "day_tracked")]
            public bool DayTracked { get; set; }

            public class Rewards
            {
                [JsonProperty(PropertyName = "day_number")]
                public int NumberDay { get; set; }

                [JsonProperty(PropertyName = "reward")]
                public Reward _Reward { get; set; }

                public class Reward
                {
                    [JsonProperty(PropertyName = "coins")]
                    public int? Coins { get; set; }

                    [JsonProperty(PropertyName = "lifes")]
                    public int? Lifes { get; set; }
                }

            }
        }
    }
}

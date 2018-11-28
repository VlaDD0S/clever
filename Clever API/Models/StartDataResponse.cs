using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class StartDataResponse : ApiResponse
    {
        public new Response response { get; set; }
        public new class Response
        {
            [JsonProperty(PropertyName = "game_info")]
            public GameInfo Gameinfo { get; set; }

            [JsonProperty(PropertyName = "leaderboard")]
            public Leaderboard _Leaderboard { get; set; }

            public class GameInfo
            {

                [JsonProperty(PropertyName = "game")]
                public Game _Game { get; set; }

                [JsonProperty(PropertyName = "user")]
                public User _User { get; set; }


                public class Game
                {
                    [JsonProperty(PropertyName = "game_id")]
                    public int Id { get; set; }

                    [JsonProperty(PropertyName = "status")]
                    public string Status { get; set; }

                    [JsonProperty(PropertyName = "start_time")]
                    public int StartTime { get; set; }

                    [JsonProperty(PropertyName = "prize")]
                    public int Prize { get; set; }

                    [JsonProperty(PropertyName = "video_owner_id")]
                    public int? OwnerId { get; set; }

                    [JsonProperty(PropertyName = "video_id")]
                    public int? VideoId { get; set; }
                }

                public class User
                {
                    [JsonProperty(PropertyName = "extra_lives")]
                    public int ExtraLives { get; set; }

                    [JsonProperty(PropertyName = "balance")]
                    public int Balance { get; set; }

                    [JsonProperty(PropertyName = "coins")]
                    public int Coins { get; set; }


                }
            }

            public class Leaderboard
            {
                [JsonProperty(PropertyName = "all_time_leaderboard")]
                public List<LeaderboardUser> AllTimeUsers { get; set; }

                [JsonProperty(PropertyName = "week_leaderboard")]
                public List<LeaderboardUser> WeekUsers { get; set; }

                public class LeaderboardUser
                {
                    [JsonProperty(PropertyName = "name")]
                    public string Name { get; set; }

                    [JsonProperty(PropertyName = "photo_url")]
                    public string PhotoUrl { get; set; }

                    [JsonProperty(PropertyName = "value")]
                    public int Money { get; set; }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Clever_API.Models;
using Clever_API.Enums;

namespace Clever_API
{
    public class Api
    {
        private static HttpClient http = new HttpClient();
        private Dictionary<string, string> baseUserData;
        private readonly string apiUrl;
        public string device_id { get; } 

        public int userId { get; }

        public Api(string Token, int UserId) : 
            this(Token, UserId, "https://api.vk.com/method", "5.73") { }
        
        public Api(string Token, int UserId, string UrlApi) : 
            this(Token, UserId, UrlApi, "5.73") { }

        public Api(string Token, int UserId, string UrlApi, string Version)
        {
            apiUrl = UrlApi;
            userId = UserId;
            http.DefaultRequestHeaders
                .AcceptEncoding
                .Add(new StringWithQualityHeaderValue("UTF-8"));

            http.DefaultRequestHeaders
                .Add("User-Agent", "%D0%9A%D0%BB%D0%B5%D0%B2%D0%B5%D1%80%2F2.3.3+%28" +
                "Redmi+Note+5%3B+Android+28%3B+VK+SDK+1.6.8%3B+com.vk.quiz%29");

            baseUserData = new Dictionary<string, string>
            {
                {"access_token", Token },
                {"v", Version }
            };
            device_id = Guid.NewGuid().ToString("N").Substring(16);
        }
       
        public async Task<bool> PingVkAsync()
        {
            
            var response = await http.GetAsync(apiUrl);
            return response.IsSuccessStatusCode;
        }

        internal string PrepareUrlToSend(string MethodName)
        {
            return PrepareUrlToSend(MethodName, new Dictionary<string, string>());
        }

        internal string PrepareUrlToSend(string MethodName, Dictionary<string, string> data)
        {
            var DataUnion = baseUserData.Concat(data).ToDictionary(x => x.Key, y => y.Value);
            var Parameters = string.Join("&", DataUnion.Select(kvp =>
            {
                var str = string.Format("{0}={1}", kvp.Key, kvp.Value);
                return str;
            }));
            var Url = string.Format("{0}/{1}?{2}", apiUrl, MethodName, Parameters); 
           
            return Url; 
        }

        private async Task<bool> SendDataAsync(string MethodName, Dictionary<string, string> data)
        {
            string Url = PrepareUrlToSend(MethodName, data);
            var result = await http.GetAsync(Url);

            return result.IsSuccessStatusCode;
        }

        private bool IsResultValid(ApiResponse result) => 
            !(result == null || result.error != null);

        private string GetHash(int UserId)
        {
            return GetHash(UserId, new List<string>());
        }

        public string GetHash(int UserId, List<string> Parameters)
        {
            var md5 = MD5.Create("md5");
            UserId ^= 0x31718;
            byte[] UserBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(UserId.ToString()));
            string UserHash = BitConverter.ToString(UserBytes).Replace("-", string.Empty); 

            string DeviceIdHash = device_id + "0MgLscD6R3";
            byte[] DeviceHash = md5.ComputeHash(Encoding.UTF8.GetBytes(DeviceIdHash));
            DeviceIdHash = BitConverter.ToString(DeviceHash).Replace("-", String.Empty);

            string ParametersHash = string.Join("", Parameters.Select(k => { return k; }))
                + "3aUFMZGRCJ";
            byte[] ParametersByte = md5.ComputeHash(Encoding.UTF8.GetBytes(ParametersHash));
            ParametersHash = BitConverter.ToString(ParametersByte).Replace("-", string.Empty);

            string FinalHash = string.Format("{0}#{1}#{2}", ParametersHash, UserHash, DeviceIdHash);

            return FinalHash.ToLower(); 
        }

        private async Task<T> GetDataAsync<T>(string MethodName, Dictionary<string, string> data)
           where T: ApiResponse
        {
            string Url = (data == null) ? PrepareUrlToSend(MethodName) : 
                                            PrepareUrlToSend(MethodName, data); 

            var result = await http.GetStringAsync(Url);
            
            if (result != null)
            {
                T res = JsonConvert.DeserializeObject<T>(result);
                return res;
            }
            else
                return null;
        }

        public async Task<StartDataResponse.Response.GameInfo.User> GetUserDataAsync()
        {
            var result = await GetDataAsync<StartDataResponse>("execute.getStartData", null);
            return result.response.Gameinfo._User;
        }

        public async Task<StartDataResponse.Response.GameInfo.Game> GetGameDataAsync()
        {
            var result = await GetDataAsync<StartDataResponse>("execute.getStartData", null);
            return result.response.Gameinfo._Game;
        }

        public async Task<List<StartDataResponse.Response.Leaderboard.LeaderboardUser>>
            GetLeaderboardAsync(TYPE_LEADERBOARD t)
        {
            var result = await GetDataAsync<StartDataResponse>("execute.getStartData", null);
            if (t == TYPE_LEADERBOARD.WEEK)
                return result.response._Leaderboard.WeekUsers;
            else
                return result.response._Leaderboard.AllTimeUsers;
        } 

        public async Task<List<GiftsDataResponse.Response.GiftsInfo>> GetGiftsInfosAsync()
        {
            var result = await GetDataAsync<GiftsDataResponse>("execute.getGifts", null);
            if (IsResultValid(result))
                return result.response.Gifts;
            else 
                return null;
        }

        public int Bump(double lat, double lon)
        {

            var result = GetDataAsync<BumpResponse>("execute.Bump", 
                new Dictionary<string, string>
                {
                    { "lat", lat.ToString() },
                    { "lon", lon.ToString() },
                    { "func_v", "1" },
                    { "prod", "1" }
                }).Result;

            if (IsResultValid(result))
                return result.response.Bonus;
            else
                return -1;
        }

        public async Task<ActionResponse.Response> SendActionAsync(ACTION_IDS Id)
        {
            int id = (int)Id;
            string SecureHash = GetHash(this.userId, new List<string> { id.ToString() });
            var result = await GetDataAsync<ActionResponse>("streamQuiz.trackAction", 
            new Dictionary<string, string>
            {
                { "action_id", id.ToString() },
                { "hash", HttpUtility.UrlEncode(SecureHash) }
            });
            return result.response;
        }

        // ????
        public async Task<SendAnswerResponse> SendAnswerAsync(bool AnswerForCoins, int GameId, 
                                                    int AnswerId, int QuestionId)
        {
            string Hash = GetHash(this.userId,
                                    new List<string>
                                    {
                                        GameId.ToString(),
                                        QuestionId.ToString()
                                    });
            
            var result = await GetDataAsync<SendAnswerResponse>("streamQuiz.sendAnswer",
                new Dictionary<string, string>
                {
                    { "answer_id", AnswerId.ToString() },
                    { "question_id", QuestionId.ToString() },
                    { "hash", HttpUtility.UrlEncode(Hash) },
                    { "coin_answer", AnswerForCoins.ToString() }
                });
            return result;
        }

        public bool PurchaseGift(int GiftId)
        {
            var result = GetDataAsync<ApiResponse>("streamQuiz.purchaseGift",
                new Dictionary<string, string> { { "gift_id", GiftId.ToString() } }).Result;

            return result.error.Message != null;
        }

        public async Task<DailyRewardsResponse.Response> GetDailyRewardsAsync()
        {
            var result = await GetDataAsync<DailyRewardsResponse>("streamQuiz." +
                                                    "getDailyRewardsData", null);
            return result.response;
        }

        public async Task<List<TrainQuestionsResponse.Question>> GetTrainQuestionsAsync()
        {
            var result = await GetDataAsync<TrainQuestionsResponse>("streamQuiz." +
                "getTrainQuestions", null);

            return result.Questions;
        }

        public async Task UseExtraLife() =>
            await SendDataAsync("streamQuiz.useExtraLife", null);
        
        public async Task<CreateCommentResponse> CreateComment(string Message, int OwnerId, int VideoId)
        {
            var data = new Dictionary<string, string>
            {
                { "owner_id", OwnerId.ToString() },
                { "video_id", VideoId.ToString() },
                { "message", Message }
            };
            var result = await GetDataAsync<CreateCommentResponse>("execute.createComment", 
                                                                        data);
            return result;
        }

        public async Task<string> GetLongPollServer(int VideoId, int OwnerId)
        {
            var result = await GetDataAsync<GetLongPollResponse>("video.getLongPollServer", new Dictionary<string, string>
            {
                {"video_id", VideoId.ToString() },
                {"owner_id", OwnerId.ToString() }
            });

            return result.response.Url;
        }
    }
}

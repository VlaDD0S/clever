using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;
using Clever_API.Exceptions.LongPollExceptions;
using Clever_API.Models; 
using Clever_API.Models.LPResponseModels; 

namespace Clever_API
{
    public class LongPollApi
    {
        private readonly Api _Api;
        private Dictionary<string, string> _LPData;
        private readonly HttpClient _client;
        private string LPUrl;
        private List<HandlerModel> _handlerModels;

        public int GameId { get; }
        public int OwnerId { get; }
        public int UserId { get; }
        public int VideoId { get; }

        public LongPollApi(Api Api, int _GameId, int _VideoId, int _OwnerId)
        {
            GameId = _GameId;
            OwnerId = _OwnerId;
            UserId = Api.userId;
            VideoId = _VideoId; 
            _Api = Api;
            _client = new HttpClient();
            _handlerModels = new List<HandlerModel>();
            _LPData = new Dictionary<string, string>();
        }

        public void StartLongPolling()
        {
            if (!IsGameStarted())
                throw new GameNotStarted();

            LPUrl = _Api.GetLongPollServer(VideoId, OwnerId).Result;
            ParseUriDataToDict(LPUrl); 
            var LPH = new Thread(LongPollHandler);
            LPH.Start();
        }
        
        public void AddHandler(HandlerModel handler) =>
            _handlerModels.Add(handler);

        public void RemoveHandler(HandlerModel handler) =>
            _handlerModels.Remove(handler);

        private bool IsGameStarted()
        {
            var GameData = _Api.GetGameDataAsync().Result;

            return GameData.Status == "started"; 
        }

        private void LongPollHandler()
        {
            string FinalUrl;
            while (true)
            {
                FinalUrl = PrepareUrlToSend(LPUrl, _LPData);
                var result = GetDataAsync<LongPollResponse>(FinalUrl).Result;
                _LPData["ts"] = result.Ts;
                if (result.Events != null)
                    AnalyzeEvents(result.Events);
                Thread.Sleep(500);
            }
        }

        private void ParseUriDataToDict(string LpUrl)
        {
            int index = LpUrl.IndexOf("?") + 1;
            LpUrl = LpUrl.Substring(index, LpUrl.Length - index);
            var query = HttpUtility.ParseQueryString(LpUrl);
            
            foreach (var i in query)
            {
                _LPData.Add(i.ToString(), query[i.ToString()]);
            }
        }

        private string PrepareUrlToSend(string Url, Dictionary<string, string> data)
        {
            int index = Url.IndexOf("?");
            Url = Url.Substring(0, index+1);
            string Parameters = String.Join("&", data.Select(kvp =>
            {
                return kvp.Key + "=" + kvp.Value;
            }));

            return Url + Parameters;
        }

        private async Task<T> GetDataAsync<T>(string Url)
            where T: ApiResponse
        {
            var result = await _client.GetStringAsync(Url);
            if (result != null)
            {
                result = Regex.Unescape(result);
                // Remove bad symbols.
                var builder = new StringBuilder(result);
                builder.Replace("<!>0", ""); 
                builder.Replace("\"{", "{");
                builder.Replace("}\"", "}");
                T res = JsonConvert.DeserializeObject<T>(builder.ToString());
                return res;
            }
            else
                return null;
            
            
        }

        private void AnalyzeEvents(List<LongPollResponse.EventsInfo> Events)
        {
            foreach (var e in Events)
            {
                var d = SearchHandler(e.Type);
                if (d != null)
                    NotifyHandler(d, e); 
            }
        }

        private Action<ResponseModel> SearchHandler(string Type)
        {
            foreach (var h in _handlerModels)
            {
                if (h.HandlerType == Type)
                    return h.Func;
            }
            return null;
        }

        private void NotifyHandler(Action<ResponseModel> d, LongPollResponse.EventsInfo e)
        {
            switch (e.Type)
            {
                case "sq_ed_game":
                    {
                        d.Invoke(new ResponseModel());
                        break; 
                    }
                case "sq_question":
                    {
                        var q = new Question
                        {
                            Id = e.Question.Id, 
                            Text = e.Question.Text,
                            Time = e.UnixQuestionTime,
                            Answers = e.Question.Answers_q
                        };
                        d.Invoke(q);
                        break;
                    }
                case "sq_game_winners":
                    {
                        var gw = new GameWinners
                        {
                            Users = e.Users,
                            Prize = e.Prize,
                            CountWinners = e.CountWinners
                        };
                        d.Invoke(gw);
                        break; 
                    }
                case "sq_friend_answer":
                    {
                        var fa = new FriendAnswer
                        {
                            AnswerFriendId = e.AnswerFriendId,
                            PhotoFriendUrl = e.PhotoFriendUrl,
                            IsLiveEnabled = e.IsLiveEnabled
                        };
                        d.Invoke(fa);
                        break; 
                    }
                case "sq_question_answers_right":
                    {
                        var ra = new RightAnswer
                        {
                            Id = e.Question.Id,
                            Text = e.Question.Text,
                            Time = e.UnixQuestionTime,
                            Answers = e.Question.Answers,
                            RightAnswerId = e.Question.RightAnswerId,
                            UnixSentTime = e.Question.UnixSentTime
                        };
                        d.Invoke(ra);
                        break; 
                    }
                case "video_comment_new":
                    {
                        var vcn = new VideoComment
                        {
                            Comment = e.Comment, 
                            User = e.User
                        };
                        d.Invoke(vcn);
                        break; 
                    }
            }
        }
    }
}

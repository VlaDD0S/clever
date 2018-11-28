using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class LongPollResponse : ApiResponse
    {
      
        [JsonProperty(PropertyName = "ts")]
        public string Ts { get; set; }

        [JsonProperty(PropertyName ="events")]
        public List<EventsInfo> Events { get; set; }
        
        public class EventsInfo
        {
            [JsonProperty(PropertyName ="type")]
            public string Type { get; set; }
           
            // video_comment_new
            [JsonProperty(PropertyName ="comment")]
            public CommentInfo Comment { get; set; }

            [JsonProperty(PropertyName ="user")]
            public UserInfo User { get; set; }
            
            public class CommentInfo
            {
                [JsonProperty(PropertyName ="from_id")]
                public int SenderId { get; set; }

                [JsonProperty(PropertyName ="date")]
                public int UnixDate { get; set; }

                [JsonProperty(PropertyName ="text")]
                public string Text { get; set; }
            }

            public class UserInfo
            {
                [JsonProperty(PropertyName ="id")]
                public int Id { get; set; }

                [JsonProperty(PropertyName ="photo_100")]
                public string Photo { get; set; }

                [JsonProperty(PropertyName ="first_name")]
                public string FirstName { get; set; }

                [JsonProperty(PropertyName ="last_name")]
                public string LastName { get; set; }

                [JsonProperty(PropertyName ="sex")]
                public int Sex { get; set; }
            }
            // End of video_comment_new
            
            // sq_question_answers_right
            [JsonProperty(PropertyName ="question")]
            public QuestionInfo Question { get; set; }

            [JsonProperty(PropertyName ="question_time")]
            public int UnixQuestionTime { get; set; }

            public class QuestionInfo : LPResponseModels.RightAnswer { }
            // end of sq_question_answers_right

            // sq_fiend_answer
            [JsonProperty(PropertyName ="answer_id")]
            public int AnswerFriendId { get; set; }

            [JsonProperty(PropertyName ="photo_url")]
            public string PhotoFriendUrl { get; set; }

            [JsonProperty(PropertyName ="is_live_enabled")]
            public bool IsLiveEnabled { get; set; }
            // End of sq_friend_asnwer

            // sq_game_winners
            [JsonProperty(PropertyName ="users")]
            public List<UsersInfo> Users { get; set; }

            public class UsersInfo
            {
                [JsonProperty(PropertyName ="name")]
                public string Name { get; set; }

                [JsonProperty(PropertyName ="photo_url")]
                public string PhotoUrl { get; set; }
            }
            
            [JsonProperty(PropertyName ="prize")]
            public int Prize { get; set; }

            [JsonProperty(PropertyName ="winners_num")]
            public int CountWinners { get; set; }
            // End of sq_game_winners
        }
    }
}

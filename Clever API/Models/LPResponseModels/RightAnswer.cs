using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Clever_API.Models.LPResponseModels
{
    public class RightAnswer : Question
    {
        [JsonProperty(PropertyName = "right_answer_id")]
        public int RightAnswerId { get; set; }

        [JsonProperty(PropertyName = "sent_time")]
        public int UnixSentTime { get; set; }

        [JsonProperty(PropertyName = "answers")]
        public new List<RightAnswerInfo> Answers { get; set; }
        public class RightAnswerInfo : AnswerInfo
        {
            [JsonProperty(PropertyName = "users_answered")]
            public int UsersAnswered { get; set; }
        }

        public List<AnswerInfo> Answers_q { get; set; }
    }
}

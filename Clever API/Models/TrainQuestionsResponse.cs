using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Clever_API.Models
{
    public class TrainQuestionsResponse : ApiResponse
    {
        [JsonProperty(PropertyName = "response")]
        public List<Question> Questions { get; set; }

        public class Question
        {
            [JsonProperty(PropertyName = "question_id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }

            [JsonProperty(PropertyName = "right_answer_id")]
            public int RightAnswerId { get; set; }

            [JsonProperty(PropertyName = "answers")]
            public List<Answer> Answers { get; set; }

            public class Answer
            {
                [JsonProperty(PropertyName = "id")]
                public int Id { get; set; }

                [JsonProperty(PropertyName = "text")]
                public string Text { get; set; }
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Text;

namespace Clever_API.Models.LPResponseModels
{
    public class VideoComment : ResponseModel
    {
        public LongPollResponse.EventsInfo.CommentInfo Comment { get; set; }

        public LongPollResponse.EventsInfo.UserInfo User { get; set; }
    }
}

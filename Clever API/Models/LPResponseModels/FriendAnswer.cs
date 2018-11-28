using System;
using System.Collections.Generic;
using System.Text;

namespace Clever_API.Models.LPResponseModels
{
    public class FriendAnswer : ResponseModel
    {
        
        public int AnswerFriendId { get; set; }

        
        public string PhotoFriendUrl { get; set; }

        
        public bool IsLiveEnabled { get; set; }
    }
}

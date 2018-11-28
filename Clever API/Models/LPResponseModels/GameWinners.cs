using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Clever_API.Models.LPResponseModels
{
    public class GameWinners : ResponseModel
    { 
        public List<LongPollResponse.EventsInfo.UsersInfo> Users { get; set; }
        public int Prize { get; set; }
        public int CountWinners { get; set; }
    }
}

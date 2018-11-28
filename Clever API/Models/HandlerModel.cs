using System;
using System.Collections.Generic;
using System.Text;
using Clever_API.Models.LPResponseModels;

namespace Clever_API.Models
{
    public class HandlerModel
    {
        public string HandlerType { get; set; }
        public Action<ResponseModel> Func { get; set; }
    }
} 

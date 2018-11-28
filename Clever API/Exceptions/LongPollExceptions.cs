using System;
using System.Collections.Generic;
using System.Text;

namespace Clever_API.Exceptions.LongPollExceptions
{
    public class GameNotStarted : Exception
    {
        public override string Message => base.Message;
    }
}

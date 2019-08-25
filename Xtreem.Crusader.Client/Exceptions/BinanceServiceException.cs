using System;

namespace Xtreem.Crusader.Client.Exceptions
{
    internal class BinanceServiceException : Exception
    {
        public BinanceServiceException()
        {
        }

        public BinanceServiceException(string message)
            : base(message)
        {
        }

        public BinanceServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

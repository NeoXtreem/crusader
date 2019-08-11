using System;

namespace Xtreem.CryptoPrediction.Client.Exceptions
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

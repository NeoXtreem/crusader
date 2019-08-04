using System;

namespace Xtreem.CryptoPrediction.Service.Exceptions
{
    public class BinanceServiceException : Exception
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

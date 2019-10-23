using System;
using Xtreem.Crusader.Client.Models;

namespace Xtreem.Crusader.Client.Exceptions
{
    internal class HistoricalDataException : Exception
    {
        public HistoricalDataException()
        {
        }

        public HistoricalDataException(string message)
            : base(message)
        {
        }

        public HistoricalDataException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public HistoricalDataResponse Response { get; set; }
    }
}

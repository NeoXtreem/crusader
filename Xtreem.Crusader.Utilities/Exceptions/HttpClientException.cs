using System;
using System.Net.Http;

namespace Xtreem.Crusader.Utilities.Exceptions
{
    public class HttpClientException : Exception
    {
        public HttpClientException()
        {
        }

        public HttpClientException(string message)
            : base(message)
        {
        }

        public HttpClientException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public HttpResponseMessage Response { get; set; }
    }
}

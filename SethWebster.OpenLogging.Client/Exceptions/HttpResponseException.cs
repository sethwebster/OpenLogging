using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SethWebster.OpenLogging.Client.Exceptions
{
    [Serializable]
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }
        public string ReasonPhrase { get; protected set; }
        public HttpResponseException() { }
        public HttpResponseException(HttpStatusCode statusCode, string reasonPhrase)
            : base(reasonPhrase)
        {
            ReasonPhrase = reasonPhrase;
            StatusCode = statusCode;
        }
        public HttpResponseException(HttpStatusCode statusCode, string reasonPhrase, Exception inner)
            : base(reasonPhrase, inner)
        {
            ReasonPhrase = reasonPhrase;
            StatusCode = statusCode;
        }
        protected HttpResponseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authentication
{
    [Serializable]
    public class AuthorizeRequestClientException : AuthorizeRequestValidationException
    {
        public Uri RedirectUri { get; set; }
        public string Error { get; set; }
        public string ResponseType { get; set; }
        public string State { get; set; }

        public AuthorizeRequestClientException(string message, Uri redirectUri, string error, string responseType, string state = null)
            : base(message)
        {
            RedirectUri = redirectUri;
            Error = error;
            ResponseType = responseType;
            State = state;
        }
    }
}

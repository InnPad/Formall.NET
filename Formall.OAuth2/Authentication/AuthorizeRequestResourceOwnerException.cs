using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authentication
{
    [Serializable]
    public class AuthorizeRequestResourceOwnerException : AuthorizeRequestValidationException
    {
        public AuthorizeRequestResourceOwnerException(string message)
            : base(message)
        { }
    }
}

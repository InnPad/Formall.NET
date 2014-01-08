using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authentication
{
    [Serializable]
    public class AuthorizeRequestValidationException : Exception
    {
        public AuthorizeRequestValidationException(string message)
            : base(message)
        { }
    }
}

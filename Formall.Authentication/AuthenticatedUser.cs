using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Authentication
{
    public class AuthenticatedUser : IUserIdentity
    {
        public string Username
        {
            get { throw new NotImplementedException(); }
            set { }
        }

        public IEnumerable<string> Claims
        {
            get { throw new NotImplementedException(); }
            set { }
        }
    }
}
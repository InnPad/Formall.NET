using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Authentication
{
    public class UserIdentity : IUserIdentity
    {
        public string Username
        {
            get;
            set;
        }

        public IEnumerable<string> Claims
        {
            get { throw new NotImplementedException(); }
            set { }
        }
    }
}
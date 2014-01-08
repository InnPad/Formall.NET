using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Authorization
{
    public class Member : IMember
    {
        /// <summary>
        /// User Identifier
        /// </summary>
        public Guid Identifier
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DisplayName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Profile
        {
            get;
            set;
        }

        Profile IMember.Profile
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
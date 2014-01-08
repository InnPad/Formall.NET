using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom.Authorization
{
    public class Client
    {
        public string Code
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public List<Member> Members
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authorization
{
    public class CodeToken
    {
        public string Code { get; set; }

        public int ClientId { get; set; }

        public string UserName { get; set; }

        public string Scope { get; set; }

        public CodeTokenTypes Type { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}

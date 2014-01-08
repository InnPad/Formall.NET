using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authorization
{
    public class Member
    {
        public Guid Client { get; set; }

        public string Email { get; set; }

        public string Fullname { get; set; }

        public Guid Id { get; set; }

        public Guid Profile { get; set; }

        public Guid User { get; set; }

        public string Username { get; set; }
    }
}

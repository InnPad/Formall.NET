using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authorization
{
    public class Client
    {
        /// <summary>
        /// List of domains that can make request
        /// </summary>
        public string[] Allow
        {
            get;
            set;
        }

        /// <summary>
        /// Domain pattern
        /// </summary>
        public string Domain
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Secret
        {
            get;
            set;
        }

        public Text Summary
        {
            get;
            set;
        }
    }
}

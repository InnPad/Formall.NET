using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    using Formall.Navigation;
    using Formall.Persistence;
    using Formall.Reflection;

    public class Error
    {
        public Text Message
        {
            get;
            set;
        }
    }
}

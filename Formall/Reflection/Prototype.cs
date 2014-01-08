using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Dynamic;
using System.Linq.Expressions;

namespace Formall.Reflection
{
    using Formall.Navigation;
    using Formall.Persistence;
    using Formall.Reflection;

    public abstract class Prototype
    {
        public Guid Id
        {
            get;
            set;
        }

        public Text Summary
        {
            get;
            set;
        }

        public Text Title
        {
            get;
            set;
        }
    }
}

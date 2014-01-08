using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Reflection
{
    using Formall.Navigation;

    public class Value : Prototype
    {
        private IDictionary<string, Rule> _rules;

        public string Name
        {
            get;
            set;
        }

        public IDictionary<string, Rule> Rules
        {
            get { return _rules ?? (_rules = new Dictionary<string, Rule>()); }
            set { _rules = value; }
        }
    }
}

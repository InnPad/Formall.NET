using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    using Formall.Reflection;

    public class Argument
    {
        public string Name { get; set; }

        public Prototype Type { get; set; }

        public object Value { get; set; }
    }
}

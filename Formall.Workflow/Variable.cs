using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    using Formall.Reflection;

    public class Variable
    {
        public string Name { get; set; }

        /// <summary>
        /// If null global to area
        /// </summary>
        public Program Program { get; set; }

        public bool Readonly { get; set; }

        public Prototype Type { get; set; }

        public object Value { get; set; }
    }
}

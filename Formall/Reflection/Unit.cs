using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Formall.Reflection
{
    using Formall.Navigation;
    using Formall.Persistence;

    public class Unit : Item
    {
        public double Factor
        {
            get;
            set;
        }
    }
}

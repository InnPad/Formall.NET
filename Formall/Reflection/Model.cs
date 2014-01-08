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

    public class Model : Prototype
    {
        private IDictionary<string, Action> _actions;
        private IDictionary<string, Field> _fields;

        public string BaseType
        {
            get;
            set;
        }

        public bool Embeddable
        {
            get;
            set;
        }

        public bool Final
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public IDictionary<string, Action> Actions
        {
            get { return _actions ?? (_actions = new Dictionary<string, Action>()); }
            set { _actions = value; }
        }

        public IDictionary<string, Field> Fields
        {
            get { return _fields ?? (_fields = new Dictionary<string, Field>()); }
            set { _fields = value; }
        }

        public string Store
        {
            get;
            set;
        }
    }
}

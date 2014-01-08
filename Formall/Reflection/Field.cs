using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Formall.Reflection
{
    public class Field
    {
        /// <summary>
        /// Allow atutomatic transformations on field value depending on constraint
        /// </summary>
        public bool Auto
        {
            get;
            set;
        }

        /// <summary>
        /// Value constraint field name.
        /// If the type of the field referenced by Contraint is:
        /// * boolean: the value of this field is only valid if the value of the constraint field it true.
        /// * Unit: The value will be in terms this unit type, and if Auto is true, automatic conversion will be perform upon unit changes.
        public string Constraint
        {
            get;
            set;
        }

        public bool HasMany
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Value constraint type name
        /// </summary>
        public string Type
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    public class Primitive : IEquatable<Primitive>
    {
        public static implicit operator string(Primitive type)
        {
            return type != null ? type._name : null;
        }

        public static readonly Primitive None = new Primitive("None");

        public static readonly Primitive Binary = new Primitive("Binary");

        public static readonly Primitive Boolean = new Primitive("Boolean");

        public static readonly Primitive Byte = new Primitive("Byte");

        public static readonly Primitive Collection = new Primitive("Collection");

        public static readonly Primitive Comment = new Primitive("Comment");

        public static readonly Primitive Constructor = new Primitive("Constructor");

        public static readonly Primitive Date = new Primitive("Date");

        public static readonly Primitive Decimal = new Primitive("Decimal");

        public static readonly Primitive Double = new Primitive("Double");

        public static readonly Primitive Function = new Primitive("Function");

        public static readonly Primitive Guid = new Primitive("Guid");

        public static readonly Primitive Int16 = new Primitive("Int16");

        public static readonly Primitive Int32 = new Primitive("Int32");

        public static readonly Primitive Int64 = new Primitive("Int64");

        public static readonly Primitive Integer = new Primitive("Integer");

        public static readonly Primitive List = new Primitive("List");

        public static readonly Primitive Money = new Primitive("Money");

        public static readonly Primitive Null = new Primitive("Null");

        public static readonly Primitive Number = new Primitive("Number");

        public static readonly Primitive Object = new Primitive("Object");

        public static readonly Primitive Property = new Primitive("Property");

        public static readonly Primitive Raw = new Primitive("Raw");

        public static readonly Primitive RegEx = new Primitive("RegEx");

        public static readonly Primitive SByte = new Primitive("SByte");

        public static readonly Primitive Single = new Primitive("Single");

        public static readonly Primitive String = new Primitive("String");

        public static readonly Primitive TimeSpan = new Primitive("TimeSpan");

        public static readonly Primitive Undefined = new Primitive("Undefined");

        public static readonly Primitive UInt16 = new Primitive("UInt16");
        
        public static readonly Primitive UInt32 = new Primitive("UInt32");

        public static readonly Primitive UInt64 = new Primitive("UInt64");

        public static readonly Primitive Unit = new Primitive("Unit");

        public static readonly Primitive Uri = new Primitive("Uri");

        private string _name;

        private Primitive(string name)
        {
            _name = name;
        }
    
        public override string ToString()
        {
            return _name;
        }

        bool IEquatable<Primitive>.Equals(Primitive other)
        {
            return (other != null && (object.Equals(this, other) || this._name == other._name));
        }
    }
}

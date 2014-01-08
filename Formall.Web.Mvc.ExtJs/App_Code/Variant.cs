using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Helpers
{
    public class Variant
    {
        private readonly string _value;

        private Variant(string value)
        {
            _value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as Variant;

            if (other != null)
            {
                return _value == other._value;
            }

            return _value == obj.ToString();
        }

        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(_value) ? 0 : _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value;
        }

        public static bool operator ==(Variant var1, Variant var2)
        {
            return (var1._value == var2._value);
        }

        public static bool operator != (Variant var1, Variant var2)
        {
            return !(var1 == var2);
        }

        public static Variant operator +(Variant var1, Variant var2)
        {
            return new Variant(((int)var1 + (int)var2).ToString());
        }

        public static implicit operator Variant(string value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(int value)
        {
            return new Variant(value.ToString());
        }

        public static implicit operator string(Variant variant)
        {
            return (variant != null) ? variant._value : null;
        }

        public static implicit operator int(Variant variant)
        {
            if (variant == null || string.IsNullOrEmpty(variant._value))
                return default(int);
            int value;
            int.TryParse(variant._value, out value);
            return value;
        }

        public static implicit operator float(Variant variant)
        {
            if (variant == null || string.IsNullOrEmpty(variant._value))
                return default(float);
            float value;
            float.TryParse(variant._value, out value);
            return value;
        }
    }
}
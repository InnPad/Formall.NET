using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Helpers
{
    public class Amount
    {
        private float _value;
        public Unit _unit;

        public Amount(float value, Unit unit)
        {
            _value = value;
            _unit = unit;
        }

        public override string ToString()
        {
            int number = (int)Math.Ceiling(_value);
            return _value == number ? string.Concat(number, _unit) : string.Concat(_value, _unit);
        }

        public static Amount Parse(string value)
        {
            return null;
        }

        public static implicit operator string(Amount amount)
        {
            return amount != null ? amount.ToString() : null; 
        }

        public static implicit operator Amount(string value)
        {
            return value != null ? Parse(value) : null;
        }
    }
}

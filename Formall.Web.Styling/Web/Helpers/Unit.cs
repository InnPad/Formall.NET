using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Helpers
{
    public class Unit
    {
        public static Unit Degree = new Unit("deg");

        public static Unit Percent = new Unit("%");

        public static Unit Pixel = new Unit("px");

        private readonly string _symbol;

        private Unit(string symbol)
        {
            _symbol = symbol;
        }

        public override string ToString()
        {
            return _symbol;
        }
    }
}

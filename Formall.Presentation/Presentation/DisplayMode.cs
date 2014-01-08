using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    public class DisplayMode
    {
        public static DisplayMode Always = new DisplayMode("Always");

        public static DisplayMode Never = new DisplayMode("Never");

        public static DisplayMode IfPresent = new DisplayMode("IfPresent");

        private DisplayMode(string name)
        {
        }
    }
}

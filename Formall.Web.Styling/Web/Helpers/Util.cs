using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System.Web.Helpers
{
    public static class Util
    {
        public static readonly string[] POSITIONS = new[] { "top", "bottom", "left", "right", "center" };

        public static bool IsPosition(string position)
        {
            /*
             *  def is_position(position)
             *    Sass::Script::Bool.new(position.is_a?(Sass::Script::String) && !!(position.value =~ POSITIONS))
             * end
             */

            return position.IndexOf('|') < 0 && POSITIONS.Contains(position);
        }

        public static bool IsPositionList(string position_list)
        {
            /*
             * def is_position_list(position_list)
             *   Sass::Script::Bool.new(position_list.is_a?(Sass::Script::List) && position_list.value.all?{|p| is_position(p).to_bool})
             * end
             */

            var positions = position_list.Split('|');

            return positions.Length > 1 && positions.All(o => POSITIONS.Contains(o));
        }

        public static string OppositePosition(string position)
        {
            if (position == null)
            {
                return null;
            }

            if (IsPosition(position))
            {
                switch (position)
                {
                    case "top":
                        return "bottom";

                    case "bottom":
                        return "top";

                    case "left":
                        return "right";

                    case "right":
                        return "left";

                    case "center":
                        return "center";

                    default:
                        Trace.TraceWarning("Cannot determine the opposite position of: " + position);
                        return position;
                }
            }
            else if (position.EndsWith("%") && position.Take(position.Length - 1).All(c => -1 < "0123456789".IndexOf(c)))
            {
                return string.Concat(100 - int.Parse(position.Substring(position.Length - 1)), '%');
            }
            else
            {
                var positions = position.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (positions.All(pos => IsPosition(pos)))
                {
                    return string.Join(" ", positions.Select(pos => OppositePosition(pos)));
                }
            }
            
            Trace.TraceWarning("Cannot determine the opposite position of: " + position);

            return position;
        }

        public static bool IsUrl(string value)
        {
            return Regex.IsMatch(value, @"^url\(.*\)$", RegexOptions.ECMAScript);
        }
    }
}

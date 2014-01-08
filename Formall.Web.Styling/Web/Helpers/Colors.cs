using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Helpers
{
    public static class Colors
    {
        /// <summary>
        /// A genericized version of lighten/darken so that negative values can be used.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string AdjustLightness(Color color, Amount amount)
        {
            /*
             * def adjust_lightness(color, amount)
             *     assert_type color, :Color
             *     assert_type amount, :Number
             *     color.with(:lightness => Compass::Util.restrict(color.lightness + amount.value, 0..100))
             * end
             */
            return Color.Transparent;
        }

        /// <summary>
        /// Scales a color's lightness by some percentage.
        /// If the amount is negative, the color is scaled darker, if positive, it is scaled lighter.
        /// This will never return a pure light or dark color unless the amount is 100%.
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Color ScaleLightness(Color color, Amount amount)
        {
            /*
             * def scale_lightness(color, amount)
             *     assert_type color, :Color
             *     assert_type amount, :Number
             *     color.with(:lightness => scale_color_value(color.lightness, amount.value))
             * end
             */
            return Color.Transparent;
        }

        /// <summary>
        /// A genericized version of saturation/desaturate so that negative values can be used.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Color AdjustSaturation(Color color, Amount amount)
        {
            /*
             * def adjust_saturation(color, amount)
             *     assert_type color, :Color
             *     assert_type amount, :Number
             *    color.with(:saturation => Compass::Util.restrict(color.saturation + amount.value, 0..100))
             * end
             */
            return Color.Transparent;
        }

        /// <summary>
        /// Scales a color's saturation by some percentage.
        /// If the amount is negative, the color is desaturated, if positive, it is saturated.
        /// This will never return a pure saturated or desaturated color unless the amount is 100%.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Color ScaleSaturation(Color color, Amount amount)
        {
            /* def scale_saturation(color, amount)
             *     assert_type color, :Color
             *     assert_type amount, :Number
             *     color.with(:saturation => scale_color_value(color.saturation, amount.value))
             * end
             */
            return Color.Transparent;
        }

        public static Color Shade(Color color, float percentage)
        {
            /*
             * def shade(color, percentage)
             *     assert_type color, :Color
             *     assert_type percentage, :Number
             *     black = Sass::Script::Color.new([0, 0, 0])
             *     mix(black, color, percentage)
             * end
             */
            return Color.Transparent;
        }

        public static Color Tint(Color color, float percentage)
        {
            /*
             * def tint(color, percentage)
             *     assert_type color, :Color
             *     assert_type percentage, :Number
             *     white = Sass::Script::Color.new([255, 255, 255])
             *     mix(white, color, percentage)
             * end
             */
            return Color.Transparent;
        }

        /// <summary>
        /// Returns an IE hex string for a color with an alpha channel
        /// suitable for passing to IE filters.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string IeHexStr(Color color)
        {
            /*
             * def ie_hex_str(color)
             *   assert_type color, :Color
             *   alpha = (color.alpha * 255).round
             *   alphastr = alpha.to_s(16).rjust(2, '0')
             *   Sass::Script::String.new("##{alphastr}#{color.send(:hex_str)[1..-1]}".upcase)
             * end
             */
            return Color.Transparent;
        }

        private static byte ScaleColorValue(byte value, float amount)
        {
            /*
             * def scale_color_value(value, amount)
             *   if amount > 0
             *     value += (100 - value) * (amount / 100.0)
             *   elsif amount < 0
             *     value += value * amount / 100.0
             *   end
             *   value
             * end
             */

            if (amount > 0)
            {
                return (byte)Math.Max(0, Math.Min(255, (int)Math.Round(value + (100 - value) * (amount / 100))));
            }

            if (amount < 0)
            {
                return (byte)Math.Max(0, Math.Min(255, (int)Math.Round(value + value * amount / 100)));
            }

            return value;
        }
    }
}

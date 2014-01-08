using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Helpers
{
    public class Color
    {
        public static Color Transparent = new Color(0, 0, 0, 0, "transparent");

        private byte _alpha;
        private byte _red;
        private byte _green;
        private byte _blue;
        private string _value;

        public Color(byte alpha, byte red, byte green, byte blue, string value = null)
        {
            _alpha = alpha;
            _red = red;
            _green = green;
            _blue = blue;
            _value = value;
        }

        public byte Alpha
        {
            get { return _alpha; }
        }

        public byte Red
        {
            get { return _red; }
        }

        public byte Green
        {
            get { return _green; }
        }

        public byte Blue
        {
            get { return _blue; }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_value))
            {
                return _value;
            }

            if (_alpha != 0)
            {
                if ((_alpha & 0x0F) == 0 && (_red & 0x0F) == 0 && (_green & 0x0F) == 0 && (_blue & 0x0F) == 0)
                {
                    return string.Format("#{0:X1}{1:X1}{2:X1}{3:X1}", _alpha >> 4, _red >> 4, _green >> 4, _blue >> 4);
                }

                return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", _alpha, _red, _green, _blue);
            }
            else if (_red == _green && _green == _blue)
            {
                if ((_green & 0x0F) == 0)
                {
                    return string.Format("#{0:X1}", _green >> 4);
                }

                return string.Format("#{0:X2}", _green);
            }

            if ((_red & 0x0F) == 0 && (_green & 0x0F) == 0 && (_blue & 0x0F) == 0)
            {
                return string.Format("#{0:X1}{1:X1}{2:X1}", _red >> 4, _green >> 4, _blue >> 4);
            }

            return string.Format("#{0:X2}{1:X2}{2:X2}", _red, _green, _blue);
        }

        public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
        {
            return new Color(alpha, red, green, blue);
        }

        public static Color FromRgb(byte red, byte green, byte blue)
        {
            return new Color(0, red, green, blue);
        }

        public static Color Parse(string value)
        {
            Color color;
            TryParse(value, out color);
            return color;
        }

        public static bool TryParse(string value, out Color color)
        {
            byte luminance, alpha, red, green, blue;

            if (string.IsNullOrEmpty(value))
            {
                color = Transparent;
                return false;
            }

            value = value.ToLowerInvariant();

            if (value == (string)Transparent)
            {
                color = Transparent;
                return true;
            }

            var hex = value.TrimStart('#');

            if (hex.ToCharArray().Any(o => "0123456789abcdef".IndexOf(o) < 0))
            {
                color = Transparent;
                return false;
            }

            switch (hex.Length)
            {
                case 1:
                    luminance = byte.Parse(new string(hex[0], 2), NumberStyles.HexNumber);
                    color = Color.FromRgb(luminance, luminance, luminance);
                    return true;

                case 2:
                    luminance = byte.Parse(hex, NumberStyles.HexNumber);
                    color = Color.FromRgb(luminance, luminance, luminance);
                    return true;

                case 3:
                    red = byte.Parse(new string(hex[0], 2), NumberStyles.HexNumber);
                    green = byte.Parse(new string(hex[1], 2), NumberStyles.HexNumber);
                    blue = byte.Parse(new string(hex[2], 2), NumberStyles.HexNumber);
                    color = Color.FromRgb(red, green, blue);
                    return true;

                case 4:
                    alpha = byte.Parse(new string(hex[0], 2), NumberStyles.HexNumber);
                    red = byte.Parse(new string(hex[1], 2), NumberStyles.HexNumber);
                    green = byte.Parse(new string(hex[2], 2), NumberStyles.HexNumber);
                    blue = byte.Parse(new string(hex[3], 2), NumberStyles.HexNumber);
                    color = Color.FromArgb(alpha, red, green, blue);
                    return true;

                case 5:
                    alpha = byte.Parse(hex.Substring(0, 2));
                    red = byte.Parse(new string(hex[2], 2), NumberStyles.HexNumber);
                    green = byte.Parse(new string(hex[3], 2), NumberStyles.HexNumber);
                    blue = byte.Parse(new string(hex[4], 2), NumberStyles.HexNumber);
                    color = Color.FromArgb(alpha, red, green, blue);
                    return true;

                case 6:
                    red = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                    green = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                    blue = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                    color = Color.FromRgb(red, green, blue);
                    return true;

                case 7:
                    alpha = byte.Parse(new string(hex[0], 2), NumberStyles.HexNumber);
                    red = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
                    green = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
                    blue = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
                    color = Color.FromArgb(alpha, red, green, blue);
                    return true;

                case 8:
                    alpha = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                    red = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                    green = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                    blue = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                    color = Color.FromArgb(alpha, red, green, blue);
                    return true;
            }

            color = Transparent;
            return false;
        }

        public static implicit operator string(Color color)
        {
            return color != null ? color.ToString() : null;
        }

        public static implicit operator Color(string value)
        {
            return value != null ? Parse(value) : null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Imaging
{
    //[Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(SizeConverter)), ValueSerializer(typeof(SizeValueSerializer))]
    public struct Size //: IFormattable
    {
        public static implicit operator System.Windows.Size(Size size)
        {
            return new System.Windows.Size(size.Width, size.Height);
        }

        internal double _width;
        internal double _height;
        private static readonly Size s_empty;
        public static bool operator ==(Size size1, Size size2)
        {
            return ((size1.Width == size2.Width) && (size1.Height == size2.Height));
        }

        public static bool operator !=(Size size1, Size size2)
        {
            return !(size1 == size2);
        }

        public static bool Equals(Size size1, Size size2)
        {
            if (size1.IsEmpty)
            {
                return size2.IsEmpty;
            }
            return (size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height));
        }

        public override bool Equals(object o)
        {
            if ((o == null) || !(o is Size))
            {
                return false;
            }
            Size size = (Size)o;
            return Equals(this, size);
        }

        public bool Equals(Size value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (this.Width.GetHashCode() ^ this.Height.GetHashCode());
        }

        /*public static Size Parse(string source)
        {
            Size empty;
            IFormatProvider invariantEnglishUS = TypeConverterHelper.InvariantEnglishUS;
            TokenizerHelper helper = new TokenizerHelper(source, invariantEnglishUS);
            string str = helper.NextTokenRequired();
            if (str == "Empty")
            {
                empty = Empty;
            }
            else
            {
                empty = new Size(Convert.ToDouble(str, invariantEnglishUS), Convert.ToDouble(helper.NextTokenRequired(), invariantEnglishUS));
            }
            helper.LastTokenRequired();
            return empty;
        }*/

        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        /*public override string ToString()
        {
            return this.ConvertToString(null, null);
        }*/

        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        /*public string ToString(IFormatProvider provider)
        {
            return this.ConvertToString(null, provider);
        }*/

        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        /*string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return this.ConvertToString(format, provider);
        }*/

        /*internal string ConvertToString(string format, IFormatProvider provider)
        {
            if (this.IsEmpty)
            {
                return "Empty";
            }
            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[] { numericListSeparator, this._width, this._height });
        }*/

        public Size(double width, double height)
        {
            if ((width < 0.0) || (height < 0.0))
            {
                throw new ArgumentException(/*MS.Internal.WindowsBase.SR.Get("Size_WidthAndHeightCannotBeNegative")*/);
            }
            this._width = width;
            this._height = height;
        }

        public static Size Empty
        {
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return s_empty;
            }
        }
        public bool IsEmpty
        {
            get
            {
                return (this._width < 0.0);
            }
        }
        public double Width
        {
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._width;
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Size_CannotModifyEmptySize")*/);
                }
                if (value < 0.0)
                {
                    throw new ArgumentException(/*MS.Internal.WindowsBase.SR.Get("Size_WidthCannotBeNegative")*/);
                }
                this._width = value;
            }
        }
        public double Height
        {
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._height;
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Size_CannotModifyEmptySize")*/);
                }
                if (value < 0.0)
                {
                    throw new ArgumentException(/*MS.Internal.WindowsBase.SR.Get("Size_HeightCannotBeNegative")*/);
                }
                this._height = value;
            }
        }
        public static explicit operator Vector(Size size)
        {
            return new Vector(size._width, size._height);
        }

        public static explicit operator Point(Size size)
        {
            return new Point(size._width, size._height);
        }

        private static Size CreateEmptySize()
        {
            return new Size { _width = double.NegativeInfinity, _height = double.NegativeInfinity };
        }

        static Size()
        {
            s_empty = CreateEmptySize();
        }
    }
}

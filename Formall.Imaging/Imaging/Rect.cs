using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Imaging
{
    //[Serializable, StructLayout(LayoutKind.Sequential), ValueSerializer(typeof(RectValueSerializer)), TypeConverter(typeof(RectConverter))]
    public struct Rect //: IFormattable
    {
        public static implicit operator System.Windows.Rect(Rect rect)
        {
            return new System.Windows.Rect(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        internal double _x;
        internal double _y;
        internal double _width;
        internal double _height;
        private static readonly Rect s_empty;
        public static bool operator ==(Rect rect1, Rect rect2)
        {
            return ((((rect1.X == rect2.X) && (rect1.Y == rect2.Y)) && (rect1.Width == rect2.Width)) && (rect1.Height == rect2.Height));
        }

        public static bool operator !=(Rect rect1, Rect rect2)
        {
            return !(rect1 == rect2);
        }

        public static bool Equals(Rect rect1, Rect rect2)
        {
            if (rect1.IsEmpty)
            {
                return rect2.IsEmpty;
            }
            return (((rect1.X.Equals(rect2.X) && rect1.Y.Equals(rect2.Y)) && rect1.Width.Equals(rect2.Width)) && rect1.Height.Equals(rect2.Height));
        }

        public override bool Equals(object o)
        {
            if ((o == null) || !(o is Rect))
            {
                return false;
            }
            Rect rect = (Rect)o;
            return Equals(this, rect);
        }

        public bool Equals(Rect value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (((this.X.GetHashCode() ^ this.Y.GetHashCode()) ^ this.Width.GetHashCode()) ^ this.Height.GetHashCode());
        }

        /*public static Rect Parse(string source)
        {
            Rect empty;
            IFormatProvider invariantEnglishUS = TypeConverterHelper.InvariantEnglishUS;
            TokenizerHelper helper = new TokenizerHelper(source, invariantEnglishUS);
            string str = helper.NextTokenRequired();
            if (str == "Empty")
            {
                empty = Empty;
            }
            else
            {
                empty = new Rect(Convert.ToDouble(str, invariantEnglishUS), Convert.ToDouble(helper.NextTokenRequired(), invariantEnglishUS), Convert.ToDouble(helper.NextTokenRequired(), invariantEnglishUS), Convert.ToDouble(helper.NextTokenRequired(), invariantEnglishUS));
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
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}", new object[] { numericListSeparator, this._x, this._y, this._width, this._height });
        }*/

        public Rect(Point location, Size size)
        {
            if (size.IsEmpty)
            {
                this = s_empty;
            }
            else
            {
                this._x = location._x;
                this._y = location._y;
                this._width = size._width;
                this._height = size._height;
            }
        }

        public Rect(double x, double y, double width, double height)
        {
            if ((width < 0.0) || (height < 0.0))
            {
                throw new ArgumentException(/*MS.Internal.WindowsBase.SR.Get("Size_WidthAndHeightCannotBeNegative")*/);
            }
            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
        }

        public Rect(Point point1, Point point2)
        {
            this._x = Math.Min(point1._x, point2._x);
            this._y = Math.Min(point1._y, point2._y);
            this._width = Math.Max((double)(Math.Max(point1._x, point2._x) - this._x), (double)0.0);
            this._height = Math.Max((double)(Math.Max(point1._y, point2._y) - this._y), (double)0.0);
        }

        public Rect(Point point, Vector vector)
            : this(point, point + vector)
        {
        }

        public Rect(System.Windows.Size size)
        {
            if (size.IsEmpty)
            {
                this = s_empty;
            }
            else
            {
                this._x = this._y = 0.0;
                this._width = size.Width;
                this._height = size.Height;
            }
        }

        public static Rect Empty
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
        public Point Location
        {
            get
            {
                return new Point(this._x, this._y);
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotModifyEmptyRect")*/);
                }
                this._x = value._x;
                this._y = value._y;
            }
        }
        public Size Size
        {
            get
            {
                if (this.IsEmpty)
                {
                    return Size.Empty;
                }
                return new Size(this._width, this._height);
            }
            set
            {
                if (value.IsEmpty)
                {
                    this = s_empty;
                }
                else
                {
                    if (this.IsEmpty)
                    {
                        throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotModifyEmptyRect")*/);
                    }
                    this._width = value._width;
                    this._height = value._height;
                }
            }
        }
        public double X
        {
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._x;
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotModifyEmptyRect")*/);
                }
                this._x = value;
            }
        }
        public double Y
        {
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._y;
            }
            set
            {
                if (this.IsEmpty)
                {
                    throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotModifyEmptyRect")*/);
                }
                this._y = value;
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
                    throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotModifyEmptyRect")*/);
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
                    throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotModifyEmptyRect")*/);
                }
                if (value < 0.0)
                {
                    throw new ArgumentException(/*MS.Internal.WindowsBase.SR.Get("Size_HeightCannotBeNegative")*/);
                }
                this._height = value;
            }
        }
        public double Left
        {
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._x;
            }
        }
        public double Top
        {
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._y;
            }
        }
        public double Right
        {
            get
            {
                if (this.IsEmpty)
                {
                    return double.NegativeInfinity;
                }
                return (this._x + this._width);
            }
        }
        public double Bottom
        {
            get
            {
                if (this.IsEmpty)
                {
                    return double.NegativeInfinity;
                }
                return (this._y + this._height);
            }
        }
        public Point TopLeft
        {
            get
            {
                return new Point(this.Left, this.Top);
            }
        }
        public Point TopRight
        {
            get
            {
                return new Point(this.Right, this.Top);
            }
        }
        public Point BottomLeft
        {
            get
            {
                return new Point(this.Left, this.Bottom);
            }
        }
        public Point BottomRight
        {
            get
            {
                return new Point(this.Right, this.Bottom);
            }
        }
        public bool Contains(Point point)
        {
            return this.Contains(point._x, point._y);
        }

        public bool Contains(double x, double y)
        {
            if (this.IsEmpty)
            {
                return false;
            }
            return this.ContainsInternal(x, y);
        }

        public bool Contains(Rect rect)
        {
            if (this.IsEmpty || rect.IsEmpty)
            {
                return false;
            }
            return ((((this._x <= rect._x) && (this._y <= rect._y)) && ((this._x + this._width) >= (rect._x + rect._width))) && ((this._y + this._height) >= (rect._y + rect._height)));
        }

        public bool IntersectsWith(Rect rect)
        {
            if (this.IsEmpty || rect.IsEmpty)
            {
                return false;
            }
            return ((((rect.Left <= this.Right) && (rect.Right >= this.Left)) && (rect.Top <= this.Bottom)) && (rect.Bottom >= this.Top));
        }

        public void Intersect(Rect rect)
        {
            if (!this.IntersectsWith(rect))
            {
                this = Empty;
            }
            else
            {
                double num2 = Math.Max(this.Left, rect.Left);
                double num = Math.Max(this.Top, rect.Top);
                this._width = Math.Max((double)(Math.Min(this.Right, rect.Right) - num2), (double)0.0);
                this._height = Math.Max((double)(Math.Min(this.Bottom, rect.Bottom) - num), (double)0.0);
                this._x = num2;
                this._y = num;
            }
        }

        public static Rect Intersect(Rect rect1, Rect rect2)
        {
            rect1.Intersect(rect2);
            return rect1;
        }

        public void Union(Rect rect)
        {
            if (this.IsEmpty)
            {
                this = rect;
            }
            else if (!rect.IsEmpty)
            {
                double num2 = Math.Min(this.Left, rect.Left);
                double num = Math.Min(this.Top, rect.Top);
                if ((rect.Width == double.PositiveInfinity) || (this.Width == double.PositiveInfinity))
                {
                    this._width = double.PositiveInfinity;
                }
                else
                {
                    double num4 = Math.Max(this.Right, rect.Right);
                    this._width = Math.Max((double)(num4 - num2), (double)0.0);
                }
                if ((rect.Height == double.PositiveInfinity) || (this.Height == double.PositiveInfinity))
                {
                    this._height = double.PositiveInfinity;
                }
                else
                {
                    double num3 = Math.Max(this.Bottom, rect.Bottom);
                    this._height = Math.Max((double)(num3 - num), (double)0.0);
                }
                this._x = num2;
                this._y = num;
            }
        }

        public static Rect Union(Rect rect1, Rect rect2)
        {
            rect1.Union(rect2);
            return rect1;
        }

        public void Union(Point point)
        {
            this.Union(new Rect(point, point));
        }

        public static Rect Union(Rect rect, Point point)
        {
            rect.Union(new Rect(point, point));
            return rect;
        }

        public void Offset(Vector offsetVector)
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotCallMethod")*/);
            }
            this._x += offsetVector._x;
            this._y += offsetVector._y;
        }

        public void Offset(double offsetX, double offsetY)
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotCallMethod")*/);
            }
            this._x += offsetX;
            this._y += offsetY;
        }

        public static Rect Offset(Rect rect, Vector offsetVector)
        {
            rect.Offset(offsetVector.X, offsetVector.Y);
            return rect;
        }

        public static Rect Offset(Rect rect, double offsetX, double offsetY)
        {
            rect.Offset(offsetX, offsetY);
            return rect;
        }

        public void Inflate(Size size)
        {
            this.Inflate(size._width, size._height);
        }

        public void Inflate(double width, double height)
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException(/*MS.Internal.WindowsBase.SR.Get("Rect_CannotCallMethod")*/);
            }
            this._x -= width;
            this._y -= height;
            this._width += width;
            this._width += width;
            this._height += height;
            this._height += height;
            if ((this._width < 0.0) || (this._height < 0.0))
            {
                this = s_empty;
            }
        }

        public static Rect Inflate(Rect rect, Size size)
        {
            rect.Inflate(size._width, size._height);
            return rect;
        }

        public static Rect Inflate(Rect rect, double width, double height)
        {
            rect.Inflate(width, height);
            return rect;
        }

        public static Rect Transform(Rect rect, Matrix matrix)
        {
            MatrixUtil.TransformRect(ref rect, ref matrix);
            return rect;
        }

        public void Transform(Matrix matrix)
        {
            MatrixUtil.TransformRect(ref this, ref matrix);
        }

        public void Scale(double scaleX, double scaleY)
        {
            if (!this.IsEmpty)
            {
                this._x *= scaleX;
                this._y *= scaleY;
                this._width *= scaleX;
                this._height *= scaleY;
                if (scaleX < 0.0)
                {
                    this._x += this._width;
                    this._width *= -1.0;
                }
                if (scaleY < 0.0)
                {
                    this._y += this._height;
                    this._height *= -1.0;
                }
            }
        }

        private bool ContainsInternal(double x, double y)
        {
            return ((((x >= this._x) && ((x - this._width) <= this._x)) && (y >= this._y)) && ((y - this._height) <= this._y));
        }

        private static Rect CreateEmptyRect()
        {
            return new Rect { _x = double.PositiveInfinity, _y = double.PositiveInfinity, _width = double.NegativeInfinity, _height = double.NegativeInfinity };
        }

        static Rect()
        {
            s_empty = CreateEmptyRect();
        }
    }
}

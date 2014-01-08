using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Imaging
{
    //[Serializable, StructLayout(LayoutKind.Sequential), ValueSerializer(typeof(PointValueSerializer)), TypeConverter(typeof(PointConverter))]
    public struct Point //: IFormattable
    {
        public static implicit operator System.Windows.Point(Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }

        internal double _x;
        internal double _y;
        public static bool operator ==(Point point1, Point point2)
        {
            return ((point1.X == point2.X) && (point1.Y == point2.Y));
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(Point point1, Point point2)
        {
            return (point1.X.Equals(point2.X) && point1.Y.Equals(point2.Y));
        }

        public override bool Equals(object o)
        {
            if ((o == null) || !(o is Point))
            {
                return false;
            }
            Point point = (Point)o;
            return Equals(this, point);
        }

        public bool Equals(Point value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() ^ this.Y.GetHashCode());
        }

        /*public static Point Parse(string source)
        {
            IFormatProvider invariantEnglishUS = TypeConverterHelper.InvariantEnglishUS;
            TokenizerHelper helper = new TokenizerHelper(source, invariantEnglishUS);
            string str = helper.NextTokenRequired();
            Point point = new Point(Convert.ToDouble(str, invariantEnglishUS), Convert.ToDouble(helper.NextTokenRequired(), invariantEnglishUS));
            helper.LastTokenRequired();
            return point;
        }*/

        public double X
        {
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this._x;
            }
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
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
            //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            set
            {
                this._y = value;
            }
        }

        /*[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public override string ToString()
        {
            return this.ConvertToString(null, null);
        }*/

        /*[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public string ToString(IFormatProvider provider)
        {
            return this.ConvertToString(null, provider);
        }*/

        /*[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return this.ConvertToString(format, provider);
        }*/

        /*internal string ConvertToString(string format, IFormatProvider provider)
        {
            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[] { numericListSeparator, this._x, this._y });
        }*/

        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Point(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public void Offset(double offsetX, double offsetY)
        {
            this._x += offsetX;
            this._y += offsetY;
        }

        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point._x + vector._x, point._y + vector._y);
        }

        public static Point Add(Point point, Vector vector)
        {
            return new Point(point._x + vector._x, point._y + vector._y);
        }

        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point._x - vector._x, point._y - vector._y);
        }

        public static Point Subtract(Point point, Vector vector)
        {
            return new Point(point._x - vector._x, point._y - vector._y);
        }

        public static Vector operator -(Point point1, Point point2)
        {
            return new Vector(point1._x - point2._x, point1._y - point2._y);
        }

        public static Vector Subtract(Point point1, Point point2)
        {
            return new Vector(point1._x - point2._x, point1._y - point2._y);
        }

        public static Point operator *(Point point, Matrix matrix)
        {
            return matrix.Transform(point);
        }

        public static Point Multiply(Point point, Matrix matrix)
        {
            return matrix.Transform(point);
        }

        public static explicit operator Size(Point point)
        {
            return new Size(Math.Abs(point._x), Math.Abs(point._y));
        }

        public static explicit operator Vector(Point point)
        {
            return new Vector(point._x, point._y);
        }
    }
}

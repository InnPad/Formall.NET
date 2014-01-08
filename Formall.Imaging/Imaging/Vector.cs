using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Imaging
{
    //[Serializable, StructLayout(LayoutKind.Sequential), ValueSerializer(typeof(VectorValueSerializer)), TypeConverter(typeof(VectorConverter))]
    public struct Vector //: IFormattable
    {
        internal double _x;
        internal double _y;
        public static bool operator ==(Vector vector1, Vector vector2)
        {
            return ((vector1.X == vector2.X) && (vector1.Y == vector2.Y));
        }

        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector vector1, Vector vector2)
        {
            return (vector1.X.Equals(vector2.X) && vector1.Y.Equals(vector2.Y));
        }

        public override bool Equals(object o)
        {
            if ((o == null) || !(o is Vector))
            {
                return false;
            }
            Vector vector = (Vector)o;
            return Equals(this, vector);
        }

        public bool Equals(Vector value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() ^ this.Y.GetHashCode());
        }

        /*public static Vector Parse(string source)
        {
            IFormatProvider invariantEnglishUS = TypeConverterHelper.InvariantEnglishUS;
            TokenizerHelper helper = new TokenizerHelper(source, invariantEnglishUS);
            string str = helper.NextTokenRequired();
            Vector vector = new Vector(Convert.ToDouble(str, invariantEnglishUS), Convert.ToDouble(helper.NextTokenRequired(), invariantEnglishUS));
            helper.LastTokenRequired();
            return vector;
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
            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[] { numericListSeparator, this._x, this._y });
        }*/

        //[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Vector(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public double Length
        {
            get
            {
                return Math.Sqrt((this._x * this._x) + (this._y * this._y));
            }
        }
        public double LengthSquared
        {
            get
            {
                return ((this._x * this._x) + (this._y * this._y));
            }
        }
        public void Normalize()
        {
            this = (Vector)(this / Math.Max(Math.Abs(this._x), Math.Abs(this._y)));
            this = (Vector)(this / this.Length);
        }

        public static double CrossProduct(Vector vector1, Vector vector2)
        {
            return ((vector1._x * vector2._y) - (vector1._y * vector2._x));
        }

        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            double y = (vector1._x * vector2._y) - (vector2._x * vector1._y);
            double x = (vector1._x * vector2._x) + (vector1._y * vector2._y);
            return (Math.Atan2(y, x) * 57.295779513082323);
        }

        public static Vector operator -(Vector vector)
        {
            return new Vector(-vector._x, -vector._y);
        }

        public void Negate()
        {
            this._x = -this._x;
            this._y = -this._y;
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x - vector2._x, vector1._y - vector2._y);
        }

        public static Vector Subtract(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x - vector2._x, vector1._y - vector2._y);
        }

        public static Point operator +(Vector vector, Point point)
        {
            return new Point(point._x + vector._x, point._y + vector._y);
        }

        public static Point Add(Vector vector, Point point)
        {
            return new Point(point._x + vector._x, point._y + vector._y);
        }

        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector._x * scalar, vector._y * scalar);
        }

        public static Vector Multiply(Vector vector, double scalar)
        {
            return new Vector(vector._x * scalar, vector._y * scalar);
        }

        public static Vector operator *(double scalar, Vector vector)
        {
            return new Vector(vector._x * scalar, vector._y * scalar);
        }

        public static Vector Multiply(double scalar, Vector vector)
        {
            return new Vector(vector._x * scalar, vector._y * scalar);
        }

        public static Vector operator /(Vector vector, double scalar)
        {
            return (Vector)(vector * (1.0 / scalar));
        }

        public static Vector Divide(Vector vector, double scalar)
        {
            return (Vector)(vector * (1.0 / scalar));
        }

        public static Vector operator *(Vector vector, Matrix matrix)
        {
            return matrix.Transform(vector);
        }

        public static Vector Multiply(Vector vector, Matrix matrix)
        {
            return matrix.Transform(vector);
        }

        public static double operator *(Vector vector1, Vector vector2)
        {
            return ((vector1._x * vector2._x) + (vector1._y * vector2._y));
        }

        public static double Multiply(Vector vector1, Vector vector2)
        {
            return ((vector1._x * vector2._x) + (vector1._y * vector2._y));
        }

        public static double Determinant(Vector vector1, Vector vector2)
        {
            return ((vector1._x * vector2._y) - (vector1._y * vector2._x));
        }

        public static explicit operator Size(Vector vector)
        {
            return new Size(Math.Abs(vector._x), Math.Abs(vector._y));
        }

        public static explicit operator Point(Vector vector)
        {
            return new Point(vector._x, vector._y);
        }
    }
}

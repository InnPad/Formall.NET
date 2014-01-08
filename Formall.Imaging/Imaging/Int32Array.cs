using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Imaging
{
    public struct Int32Array
    {
        private readonly int[] _array;

        private Int32Array(int[] array)
        {
            _array = array;
        }

        public int this[int index]
        {
            get { return index >= 0 && index < _array.Length ? _array[index] : 0; }
        }

        public int Bottom
        {
            get { return _array.Length > 3 ? _array[3] : 0; }
        }

        public int Height
        {
            get { return _array.Length > 3 ? _array[3] : 0; }
        }

        public int Left
        {
            get { return _array.Length.Equals(0) ? 0 : this[0]; }
        }

        public int Length
        {
            get { return _array.Length; }
        }

        public int Right
        {
            get { return _array.Length > 2 ? _array[2] : 0; }
        }

        public int Top
        {
            get { return _array.Length.Equals(0) ? 0 : _array.Length > 1 ? _array[1] : _array[0]; }
        }

        public int Width
        {
            get { return _array.Length > 2 ? _array[2] : 0; }
        }

        public int X
        {
            get { return _array.Length.Equals(0) ? 0 : this[0]; }
        }

        public int Y
        {
            get { return _array.Length.Equals(0) ? 0 : _array.Length > 1 ? _array[1] : _array[0]; }
        }

        public int Z
        {
            get { return _array.Length.Equals(0) ? 0 : _array.Length > 2 ? _array[2] : _array[_array.Length - 1]; }
        }

        public static bool operator ==(Int32Array array1, Int32Array array2)
        {
            return ((array1.X == array2.X) && (array1.Y == array2.Y));
        }

        public static bool operator !=(Int32Array array1, Int32Array array2)
        {
            return !(array1 == array2);
        }

        public static bool Equals(Int32Array array1, Int32Array array2)
        {
            return (array1.X.Equals(array2.X) && array1.Y.Equals(array2.Y));
        }

        public override bool Equals(object o)
        {
            if ((o == null) || !(o is Int32Array))
            {
                return false;
            }
            Int32Array array = (Int32Array)o;
            return Equals(this, array);
        }

        public bool Equals(Int32Array value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() ^ this.Y.GetHashCode());
        }

        public override string ToString()
        {
            return string.Concat('[', string.Join(",", _array), ']');
        }

        public static Int32Array Parse(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new Int32Array(new int[] { });
            }

            var tokens = source
                .TrimStart('[', '{', '(', ' ')
                .TrimEnd(']', '}', ')', ' ')
                .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var values = new List<int>();

            for (var i = 0; i < tokens.Length; i++)
            {
                int value;
                if (!int.TryParse(tokens[i], out value))
                {
                    break;
                }
                values.Add(value);
            }

            return new Int32Array(values.ToArray());
        }

        public static explicit operator Point(Int32Array array)
        {
            return new Point(array.X, array.Y);
        }

        public static explicit operator Rect(Int32Array array)
        {
            return new Rect(array.X, array.Y, Math.Abs(array.Width), Math.Abs(array.Height));
        }

        public static explicit operator Size(Int32Array array)
        {
            return new Size(Math.Abs(array.X), Math.Abs(array.Y));
        }

        public static explicit operator Vector(Int32Array array)
        {
            return new Vector(array.X, array.Y);
        }

        public static implicit operator System.Windows.Point(Int32Array array)
        {
            return new System.Windows.Point(array.X, array.Y);
        }

        public static explicit operator System.Windows.Rect(Int32Array array)
        {
            return new System.Windows.Rect(array.X, array.Y, Math.Abs(array.Width), Math.Abs(array.Height));
        }

        public static explicit operator System.Windows.Size(Int32Array array)
        {
            return new System.Windows.Size(Math.Abs(array.X), Math.Abs(array.Y));
        }

        public static explicit operator System.Windows.Vector(Int32Array array)
        {
            return new System.Windows.Vector(array.X, array.Y);
        }
    }
}

using System;

namespace Custom.Algebra
{
    public class SquareBitMatrix : BitMatrix
    {
        private readonly bool[,] m_publicArray;

        private readonly int m_Width;

        public SquareBitMatrix(int width)
        {
            m_publicArray = new bool[width, width];
            m_Width = width;
        }

        public SquareBitMatrix(bool[,] publicArray)
        {
            m_publicArray = publicArray;
            int width = publicArray.GetLength(0);
            m_Width = width;
        }

        public static bool CreateSquareBitMatrix(bool[,] publicArray, out SquareBitMatrix triStateMatrix)
        {
            triStateMatrix = null;
            if (publicArray == null)
                return false;

            if (publicArray.GetLength(0) == publicArray.GetLength(1))
            {
                triStateMatrix = new SquareBitMatrix(publicArray);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Return value will be public array itself. Not deep/shallow copy. 
        /// </summary>
        public override bool[,] InternalArray
        {
            get 
            {
                bool[,] deepCopyArray = new bool[m_Width, m_Width];
                for (int x = 0; x < m_Width; x++)
                    for (int y = 0; y < m_Width; y++)
                        deepCopyArray[x, y] = m_publicArray[x, y];
                return deepCopyArray;
            }
        }

        

        public override bool this[int i, int j]
        {
            get
            {
                return m_publicArray[i, j];
            }
            set
            {
                m_publicArray[i, j] = value;
            }
        }
       
         public override int Height
        {
            get { return Width; }
        }

        public override int Width
        {
            get { return m_Width; }
        }
    }
}

using System;

namespace Custom.Algebra
{
    public class TriStateMatrix : BitMatrix
    {
        private readonly StateMatrix m_stateMatrix;
        
        private readonly bool[,] m_publicArray;

        private readonly int m_Width;

        public TriStateMatrix(int width)
        {
            m_stateMatrix = new StateMatrix(width);
            m_publicArray = new bool[width, width];
            m_Width = width;
        }

        public static bool CreateTriStateMatrix(bool[,] publicArray, out TriStateMatrix triStateMatrix)
        {
            triStateMatrix = null;
            if (publicArray == null)
                return false;

            if (publicArray.GetLength(0) == publicArray.GetLength(1))
            {
                triStateMatrix = new TriStateMatrix(publicArray);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Return value will be deep copy of array. 
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

        public TriStateMatrix(bool[,] publicArray)
        {
            m_publicArray = publicArray;
            int width = publicArray.GetLength(0);
            m_stateMatrix = new StateMatrix(width);
            m_Width = width;
        }

        public override bool this[int i, int j]
        {
            get
            {
                return m_publicArray[i, j];
            }
            set
            {
            	if (MStatus(i, j) == MatrixStatus.None || MStatus(i, j) == MatrixStatus.NoMask)
            	{
            		throw new InvalidOperationException(string.Format("The value of cell [{0},{1}] is not set or is Stencil.", i, j));
            	}
                m_publicArray[i, j] = value;
            }
        }
        
        public bool this[int i, int j, MatrixStatus mstatus]
        {
        	set
        	{
        		m_stateMatrix[i, j] = mstatus;
        		m_publicArray[i, j] = value;
        	}
        }

        public MatrixStatus MStatus(int i, int j)
        {
            return m_stateMatrix[i, j];
        }

        public MatrixStatus MStatus(MatrixPoint point)
        {
            return MStatus(point.X, point.Y);
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

using System;

namespace Custom.Algebra
{
    public sealed class ByteMatrix
    {
        private readonly sbyte[,] m_Bytes;
        
        public sbyte this[int x, int y]
        {
            get { return m_Bytes[y, x]; }
            set { m_Bytes[y, x] = value; }
        }
        
        public int Width
        {
            get { return m_Bytes.GetLength(1); }
        }

        public int Height
        {
            get { return m_Bytes.GetLength(0); }
        }

        public ByteMatrix(int width, int height)
        {
            m_Bytes = new sbyte[height, width];
        }

        public void Clear(sbyte value)
        {
            this.ForAll((x, y, matrix) => { matrix[x, y] = value; });
        }

        public void ForAll(Action<int, int, ByteMatrix> action)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    action.Invoke(x, y, this);
                }
            }
        }
    }
}
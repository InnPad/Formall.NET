namespace Custom.Algebra
{
    public static class ByteMatrixExtensions
    {
        public static TriStateMatrix ToBitMatrix(this ByteMatrix byteMatrix) 
        {
            TriStateMatrix matrix = new TriStateMatrix(byteMatrix.Width);
            for (int i = 0; i < byteMatrix.Width; i++)
            {
                for (int j = 0; j < byteMatrix.Height; j++)
                {
                    if (byteMatrix[j, i] != -1)
                    {
                        matrix[i, j, MatrixStatus.NoMask] = byteMatrix[j, i] != 0;
                    }
                }
            }
            return matrix;
        }
        
        public static TriStateMatrix ToPatternBitMatrix(this ByteMatrix byteMatrix) 
        {
            TriStateMatrix matrix = new TriStateMatrix(byteMatrix.Width);
            for (int i = 0; i < byteMatrix.Width; i++)
            {
                for (int j = 0; j < byteMatrix.Height; j++)
                {
                    if (byteMatrix[j, i] != -1)
                    {
                        matrix[i, j, MatrixStatus.Data] = byteMatrix[j, i] != 0;
                    }
                }
            }
            return matrix;
        }
    }
}

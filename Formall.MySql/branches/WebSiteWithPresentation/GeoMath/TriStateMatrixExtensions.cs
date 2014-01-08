using System.Collections.Generic;

namespace Custom.Algebra
{
    public static class TriStateMatrixExtensions
    {
        public static TriStateMatrix Embed(this TriStateMatrix matrix, BitMatrix stencil, MatrixPoint location)
        {
            stencil.CopyTo(matrix, location, MatrixStatus.NoMask);
            return matrix;
        }

        public static TriStateMatrix Embed(this TriStateMatrix matrix, BitMatrix stencil, IEnumerable<MatrixPoint> locations)
        {
            foreach (MatrixPoint location in locations)
            {
                Embed(matrix, stencil, location);
            }
            return matrix;
        }
    }
}

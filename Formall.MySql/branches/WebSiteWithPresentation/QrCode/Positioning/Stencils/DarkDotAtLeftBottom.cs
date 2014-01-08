using System;

namespace Custom.Algebra.QrCode.Encoding.Positioning.Stencils
{
    internal class DarkDotAtLeftBottom : PatternStencilBase
    {
        public DarkDotAtLeftBottom(int version) : base(version)
        {
        }

        public override bool[,] Stencil
        {
            get { throw new NotImplementedException(); }
        }

        public override void ApplyTo(TriStateMatrix matrix)
        {
            matrix[8, matrix.Width - 8, MatrixStatus.NoMask] = true;
        }
    }
}

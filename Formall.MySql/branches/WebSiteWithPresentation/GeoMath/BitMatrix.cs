namespace Custom.Algebra
{
    public abstract class BitMatrix
    {
        public abstract bool this[int i, int j] { get; set; }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract bool[,] InternalArray { get; }

        public MatrixSize Size
        {
            get { return new MatrixSize(Width, Height); }
        }

        public bool this[MatrixPoint point]
        {
            get { return this[point.X, point.Y]; }
            set { this[point.X, point.Y] = value; }
        }

        public void CopyTo(TriStateMatrix target, MatrixRectangle sourceArea, MatrixPoint targetPoint, MatrixStatus mstatus)
        {
            for (int j = 0; j < sourceArea.Size.Height; j++)
            {
                for (int i = 0; i < sourceArea.Size.Width; i++)
                {
                    bool value = this[sourceArea.Location.X + i, sourceArea.Location.Y + j];
                    target[targetPoint.X + i, targetPoint.Y + j, mstatus] = value;
                }
            }
        }

        public void CopyTo(TriStateMatrix target, MatrixPoint targetPoint, MatrixStatus mstatus)
        {
            CopyTo(target, new MatrixRectangle(new MatrixPoint(0,0), new MatrixSize(Width, Height)), targetPoint, mstatus);
        }
    }
}

namespace Custom.Algebra
{
    public struct MatrixPoint
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public MatrixPoint(int x, int y) 
            : this()
        {   
            X = x;
            Y = y;
        }

        public MatrixPoint Offset(MatrixPoint offset)
        {
            return new MatrixPoint(offset.X + this.X, offset.Y + this.Y);
        }

        public MatrixPoint Offset(int offsetX, int offsetY)
        {
            return Offset(new MatrixPoint(offsetX, offsetY));
        }

        public override string ToString()
        {
            return string.Format("Point({0};{1})", X, Y);
        }
    }
}

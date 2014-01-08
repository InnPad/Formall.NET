namespace Custom.Algebra
{
    public struct MatrixSize
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public MatrixSize(int width, int height) 
            : this()
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return string.Format("Size({0};{1})", Width, Height);
        }
    }
}

namespace Formall.Imaging
{
    public abstract class BitMatrix
    {
        public abstract bool this[int i, int j] { get; }
        public abstract int Width { get; }
        public abstract int Height { get; }
    }
}

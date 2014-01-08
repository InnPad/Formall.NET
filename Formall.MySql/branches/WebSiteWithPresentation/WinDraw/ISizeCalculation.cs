using System;

namespace Custom.Presentation
{
    public interface ISizeCalculation
    {
        DrawingSize GetSize(int matrixWidth);
    }
}

using System;

namespace Formall.Imaging
{
    public interface ISizeCalculation
    {
        DrawingSize GetSize(int matrixWidth);
    }
}

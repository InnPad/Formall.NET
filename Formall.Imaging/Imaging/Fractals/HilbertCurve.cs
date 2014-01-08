using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Formall.Imaging.Fractals
{
    /// <summary>
    /// Hilbert curve construction, whose limiting space-filling
    /// curve was devised by mathematician David Hilbert.
    /// http://en.wikipedia.org/wiki/Hilbert_curve
    /// </summary>
    public class HilbertCurve : AbstractFractal
    {
        public HilbertCurve(byte iterations)
            : base(iterations)
        {
        }

        public override bool Closed { get { return false; } }

        protected override void Render()
        {
            Dx(Append(new Point(0, 0)), Iterations - 1, 1, 1);
        }

        private Point Dx(Point cursor, int level, double dX, double dY)
        {
            if (level != 0)
            {
                cursor = Dy(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X + dX, cursor.Y));

            if (level != 0)
            {
                cursor = Dx(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y + dY));

            if (level != 0)
            {
                cursor = Dx(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X - dX, cursor.Y));

            if (level != 0)
            {
                cursor = Dy(cursor, level - 1, -dX, -dY);
            }

            return cursor;
        }

        private Point Dy(Point cursor, int level, double dX, double dY)
        {
            if (level != 0)
            {
                cursor = Dx(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y + dY));

            if (level != 0)
            {
                cursor = Dy(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X + dX, cursor.Y));

            if (level != 0)
            {
                cursor = Dy(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y - dY));

            if (level != 0)
            {
                cursor = Dx(cursor, level - 1, -dX, -dY);
            }

            return cursor;
        }
    }
}

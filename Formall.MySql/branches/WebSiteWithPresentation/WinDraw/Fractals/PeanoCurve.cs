using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Custom.Presentation.Fractals
{
    public class PeanoCurve : AbstractFractal
    {
        public PeanoCurve(byte iterations)
            : base(iterations)
        {
        }

        public override bool Closed { get { return true; } }

        protected override void Render()
        {
            Iterate(Append(new Point(0, 0)), Iterations - 1, 1, 1);
        }

        private Point Iterate(Point cursor, int level, double dX, double dY)
        {
            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y + dY));

            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, -dX, dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y + dY));

            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X + dX, cursor.Y));

            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, dX, -dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y - dY));

            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, -dX, -dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y - dY));

            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, dX, -dY);
            }

            cursor = Append(new Point(cursor.X + dX, cursor.Y));

            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, dX, dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y + dY));

            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, -dX, dY);
            }

            cursor = Append(new Point(cursor.X, cursor.Y + dY));

            if (level != 0)
            {
                cursor = Iterate(cursor, level - 1, dX, dY);
            }

            return cursor;
        }
    }
}

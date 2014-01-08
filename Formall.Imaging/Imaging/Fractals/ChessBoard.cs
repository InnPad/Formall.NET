using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Formall.Imaging.Fractals
{
    public class ChessBoard : AbstractFractal
    {
        public ChessBoard(byte iterations)
            : base(iterations)
        {
        }

        public override bool Closed { get { return true; } }

        protected override void Render()
        {
            DecY(DecX(IncY(IncX(Append(new Point(0, 0)), Iterations), Iterations), Iterations), Iterations);
        }

        #region - Recursive functions -

        private Point DecX(Point cursor, int iteration)
        {
            if (iteration != 1)
            {
                cursor = DecX(cursor, iteration - 1);
                cursor = DecY(cursor, iteration - 1);
            }

            cursor = Append(new Point(cursor.X - 1, cursor.Y));

            if (iteration != 1)
            {
                cursor = IncY(cursor, iteration - 1);
                cursor = DecX(cursor, iteration - 1);
            }

            return cursor;
        }

        private Point DecY(Point cursor, int iteration)
        {
            if (iteration != 1)
            {
                cursor = DecY(cursor, iteration - 1);
                cursor = IncX(cursor, iteration - 1);
            }

            cursor = Append(new Point(cursor.X, cursor.Y - 1));

            if (iteration != 1)
            {
                cursor = DecX(cursor, iteration - 1);
                cursor = DecY(cursor, iteration - 1);
            }

            return cursor;
        }

        private Point IncX(Point cursor, int iteration)
        {
            if (iteration != 1)
            {
                cursor = IncX(cursor, iteration - 1);
                cursor = IncY(cursor, iteration - 1);
            }

            cursor = Append(new Point(cursor.X + 1, cursor.Y));

            if (iteration != 1)
            {
                cursor = DecY(cursor, iteration - 1);
                cursor = IncX(cursor, iteration - 1);
            }

            return cursor;
        }

        private Point IncY(Point cursor, int iteration)
        {
            if (iteration != 1)
            {
                cursor = IncY(cursor, iteration - 1);
                cursor = DecX(cursor, iteration - 1);
            }

            cursor = Append(new Point(cursor.X, cursor.Y + 1));

            if (iteration != 1)
            {
                cursor = IncX(cursor, iteration - 1);
                cursor = IncY(cursor, iteration - 1);
            }

            return cursor;
        }

        #endregion  - Recursive functions -
    }
}

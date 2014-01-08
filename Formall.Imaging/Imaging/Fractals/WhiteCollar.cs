using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Formall.Imaging.Fractals
{
    public class WhiteCollar : AbstractFractal
    {
        public WhiteCollar(byte iterations)
            : base(iterations)
        {
        }

        protected override bool PrepareBackground(DrawingContext context, Point start, double dx, double dy, params Brush[] brushes)
        {
            var nullPen = new Pen(Brushes.Transparent, 0);

            for (var i = 0; i <= Iterations; i++)
            {
                var factor = Math.Pow(2, Iterations - i);
                var size = new Size(2 * factor * dx, 2 * factor * dy);
                var sx = 4 * factor * dx;
                var sy = 4 * factor * dy;
                var mx = (factor - 1) * dx;
                var my = (factor - 1) * dy;

                var len = Math.Pow(2, i);
                for (var y = 0; y < len; y++)
                    for (var x = 0; x < len; x++)
                    {
                        context.DrawRectangle(brushes.Skip(1 + i).FirstOrDefault() ?? Brushes.Cyan, nullPen, new Rect(new Point(x * sx + mx, y * sy + my), size));
                    }
            }

            return true;
        }

        protected override void Render()
        {
            DecY(DecX(IncY(IncX(Append(new Point(0, 0)),
                Iterations, true, true),
                Iterations, true, true),
                Iterations, true, true),
                Iterations, true, true);
        }

        #region - Recursive functions -

        private Point DecX(Point cursor, int iteration, bool pre, bool post)
        {
            if (iteration != 1)
            {
                cursor = DecX(cursor, iteration - 1, pre, true);
                cursor = DecY(cursor, iteration - 1, true, false);
            }
            else if (pre)
            {
                cursor = Append(new Point(cursor.X - 2, cursor.Y));
            }

            cursor = Append(new Point(cursor.X, cursor.Y - 1));
            cursor = Append(new Point(cursor.X - 2, cursor.Y));
            cursor = Append(new Point(cursor.X, cursor.Y + 1));

            if (iteration != 1)
            {
                cursor = IncY(cursor, iteration - 1, false, true);
                cursor = DecX(cursor, iteration - 1, true, post);
            }
            else if (post)
            {
                cursor = Append(new Point(cursor.X - 2, cursor.Y));
            }

            return cursor;
        }

        private Point DecY(Point cursor, int iteration, bool pre, bool post)
        {
            if (iteration != 1)
            {
                cursor = DecY(cursor, iteration - 1, pre, true);
                cursor = IncX(cursor, iteration - 1, true, false);
            }
            else if (pre)
            {
                cursor = Append(new Point(cursor.X, cursor.Y - 2));
            }

            cursor = Append(new Point(cursor.X + 1, cursor.Y));
            cursor = Append(new Point(cursor.X, cursor.Y - 2));
            cursor = Append(new Point(cursor.X - 1, cursor.Y));

            if (iteration != 1)
            {
                cursor = DecX(cursor, iteration - 1, false, true);
                cursor = DecY(cursor, iteration - 1, true, post);
            }
            else if (post)
            {
                cursor = Append(new Point(cursor.X, cursor.Y - 2));
            }

            return cursor;
        }

        private Point IncX(Point cursor, int iteration, bool pre, bool post)
        {
            if (iteration != 1)
            {
                cursor = IncX(cursor, iteration - 1, pre, true);
                cursor = IncY(cursor, iteration - 1, true, false);
            }
            else if (pre)
            {
                cursor = Append(new Point(cursor.X + 2, cursor.Y));
            }

            cursor = Append(new Point(cursor.X, cursor.Y + 1));
            cursor = Append(new Point(cursor.X + 2, cursor.Y));
            cursor = Append(new Point(cursor.X, cursor.Y - 1));

            if (iteration != 1)
            {
                cursor = DecY(cursor, iteration - 1, false, true);
                cursor = IncX(cursor, iteration - 1, true, post);
            }
            else if (post)
            {
                cursor = Append(new Point(cursor.X + 2, cursor.Y));
            }

            return cursor;
        }

        private Point IncY(Point cursor, int iteration, bool pre, bool post)
        {
            if (iteration != 1)
            {
                cursor = IncY(cursor, iteration - 1, pre, true);
                cursor = DecX(cursor, iteration - 1, true, false);
            }
            else if (pre)
            {
                cursor = Append(new Point(cursor.X, cursor.Y + 2));
            }

            cursor = Append(new Point(cursor.X - 1, cursor.Y));
            cursor = Append(new Point(cursor.X, cursor.Y + 2));
            cursor = Append(new Point(cursor.X + 1, cursor.Y));

            if (iteration != 1)
            {
                cursor = IncX(cursor, iteration - 1, false, true);
                cursor = IncY(cursor, iteration - 1, true, post);
            }
            else if (post)
            {
                cursor = Append(new Point(cursor.X, cursor.Y + 2));
            }

            return cursor;
        }

        #endregion  - Recursive functions -
    }
}

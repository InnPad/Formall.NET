using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Custom.Presentation.Fractals
{
    public abstract class AbstractFractal
    {
        private readonly byte _iterations;
        private readonly List<Point> _points;
        private Rect _bounds;

        public AbstractFractal(byte iterations)
        {
            _iterations = iterations;
            _points = new List<Point>();
            _bounds = new Rect(0, 0, 0, 0);

            Render();
        }

        public Rect Bounds
        {
            get { return _bounds; }
        }

        public virtual bool Closed
        {
            get { return _points.Count > 1 && _points.First().Equals(_points.Last()); }
        }

        public virtual bool IsEmpty
        {
            get { return _points.Count == 0; }
        }

        public byte Iterations
        {
            get { return _iterations; }
        }

        public Point[] Raw
        {
            get { return _points.ToArray(); }
        }

        public virtual Point Start
        {
            get { return _points.FirstOrDefault(); }
        }

        protected Point Append(Point point)
        {
            _points.Add(point);

            if (double.IsNaN(_bounds.Left) || point.X < _bounds.Left)
                _bounds = new Rect(new Point(point.X, _bounds.Top), _bounds.BottomRight);

            if (double.IsNaN(_bounds.Right) || point.X > _bounds.Right)
                _bounds = new Rect(_bounds.TopLeft, new Point(point.X, _bounds.Bottom));

            if (double.IsNaN(_bounds.Top) || point.Y < _bounds.Top)
                _bounds = new Rect(new Point(_bounds.Left, point.Y), _bounds.BottomRight);
            
            if (double.IsNaN(_bounds.Bottom) || point.Y > _bounds.Bottom)
                _bounds = new Rect(_bounds.TopLeft, new Point(_bounds.Right, point.Y));

            return point;
        }

        public PathFigure CreatePathFigure(Point start, Size size)
        {
            if (_points.Count == 0)
                return null;

            var first = _points[0];
            var dx = size.Width / _bounds.Width;
            var dy = size.Height / _bounds.Height;
            var figure = new PathFigure(
                new Point(start.X - _bounds.X - first.X * size.Width, start.Y - _bounds.Y - first.Y * size.Height), 
                _points.Skip(1).Select(o => new LineSegment(new Point(o.X * dx, o.Y * dy), true)), 
                Closed);

            return figure;
        }

        public DrawingVisual CreateDrawingVisual(Point start, Size size, double thickness, params Brush[] brushes)
        {
            var pen = new Pen(brushes.FirstOrDefault() ?? Brushes.Black, thickness);

            var visual = new DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            var context = visual.RenderOpen();

            start.Offset(-_bounds.X, -_bounds.Y);
            
            var dx = size.Width / _bounds.Width;
            var dy = size.Height / _bounds.Height;

            PrepareBackground(context, start, dx, dy, brushes);

            if (_points.Count > 0)
            {
                var first = _points[0];
                first = new Point(start.X + first.X * dx, start.Y + first.Y * dy);

                // Draw fractal in the DrawingContext.
                for (var i = 1; i < _points.Count; i++)
                {
                    var last = _points[i];
                    context.DrawLine(pen, first, (last = new Point(start.X + last.X * dx, start.Y + last.Y * dy)));

                    first = last;
                }
            }

            // Persist the drawing content.
            context.Close();

            return visual;
        }

        protected virtual bool PrepareBackground(DrawingContext context, Point start, double dx, double dy, params Brush[] brushes)
        {
            return false;
        }

        protected abstract void Render();
    }
}

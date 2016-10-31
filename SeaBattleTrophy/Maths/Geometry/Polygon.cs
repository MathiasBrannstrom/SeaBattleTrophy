using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths.Geometry
{
    public struct Polygon : IReadOnlyList<Point2D>
    {
        public IReadOnlyList<Point2D> Points { get; }

        public Polygon(IReadOnlyList<Point2D> points)
        {
            Points = points;
        }

        public Point2D this[int index]
        {
            get
            {
                return Points[index];
            }
        }

        public int Count { get { return Points.Count; } }

        public IEnumerator<Point2D> GetEnumerator()
        {
            return Points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Points.GetEnumerator();
        }
    }
}

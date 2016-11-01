using System;
using System.Collections;
using System.Collections.Generic;

namespace Maths.Geometry
{
    public struct Polygon2D : IReadOnlyList<Point2D>, IEquatable<Polygon2D>
    {
        public IReadOnlyList<Point2D> Points { get; }

        public Polygon2D(IReadOnlyList<Point2D> points)
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

        public bool Equals(Polygon2D other)
        {
            if (Count != other.Count)
                return false;

            for (int i = 0; i < Count; i++)
                if (!this[i].Equals(other[i]))
                    return false;

            return true;
        }
    }
}

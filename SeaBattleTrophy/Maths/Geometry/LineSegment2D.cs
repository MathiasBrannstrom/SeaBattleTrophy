using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths.Geometry
{
    public struct LineSegment2D
    {
        public Point2D PointA { get; }

        public Point2D PointB { get; }

        public LineSegment2D(Point2D pointA, Point2D pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }

        public float SquaredLength()
        {
            return (PointB - PointA).SquaredLength();
        }

        public float Length()
        {
            return (float)Math.Sqrt(SquaredLength());
        }
    }
}

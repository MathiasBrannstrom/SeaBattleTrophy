using System;

namespace Maths.Geometry
{
    public struct LineSegment2D : IEquatable<LineSegment2D>
    {
        public Point2D PointA { get; }

        public Point2D PointB { get; }

        public LineSegment2D(Point2D pointA, Point2D pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }

        public double SquaredLength()
        {
            return (PointB - PointA).SquaredLength();
        }

        public double Length()
        {
            return Math.Sqrt(SquaredLength());
        }

        public bool Equals(LineSegment2D other)
        {
            return PointA.Equals(other.PointA) && PointB.Equals(other.PointB);
        }
    }
}

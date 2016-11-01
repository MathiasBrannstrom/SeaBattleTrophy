using System;

namespace Maths.Geometry
{
    /// <summary>
    /// Line in 2D defined by PointA + t*Direction, where t: [-inf, inf] and Direction is (PointB - PointA)
    /// </summary>
    public struct Line2D : IEquatable<Line2D>
    {
        public Point2D PointA { get; }

        public Point2D PointB { get; }
        
        public Line2D(Point2D pointA, Point2D pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }

        public Line2D(LineSegment2D lineSegment)
        {
            PointA = lineSegment.PointA;
            PointB = lineSegment.PointB;
        }

        public Vector2D Direction { get { return PointB - PointA; } }

        public bool Equals(Line2D other)
        {
            return PointA.Equals(other.PointA) && PointB.Equals(other.PointB);
        }
    }
}

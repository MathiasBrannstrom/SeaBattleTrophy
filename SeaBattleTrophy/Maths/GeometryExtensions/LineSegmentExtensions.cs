using System;

namespace Maths.Geometry
{
    public static class LineSegmentExtensions
    {
        public static double ShortestSquaredDistanceToPoint(this LineSegment lineSegment, Point2D point)
        {
            Point2D pointOnSegment;
            return ShortestSquaredDistanceToPoint(lineSegment, point, out pointOnSegment);
        }

        public static double ShortestSquaredDistanceToPoint(this LineSegment lineSegment, Point2D point, out Point2D pointOnSegment)
        {
            var lineSegmentSquaredLength = lineSegment.SquaredLength();
            var pA = lineSegment.PointA;
            var pB = lineSegment.PointB;

            if (lineSegmentSquaredLength == 0)
            {
                pointOnSegment = pA;
                return (point - pA).SquaredLength();
            }

            var t = ((point.X - pA.X) * (pB.X - pA.X) + (point.Y - pA.Y) * (pB.Y - pA.Y)) / lineSegmentSquaredLength;

            t = Math.Max(0, Math.Min(1, t));

            pointOnSegment = new Point2D(pA.X + t * (pB.X - pA.X), pA.Y + t * (pB.Y - pA.Y));

            return (pointOnSegment - point).SquaredLength();
        }
    }
}

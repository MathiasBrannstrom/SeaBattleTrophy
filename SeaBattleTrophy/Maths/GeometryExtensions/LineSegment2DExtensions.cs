using System;

namespace Maths.Geometry
{
    public static class LineSegment2DExtensions
    {
        public static double ShortestSquaredDistanceToPoint(this LineSegment2D lineSegment, Point2D point)
        {
            Point2D pointOnSegment;
            return ShortestSquaredDistanceToPoint(lineSegment, point, out pointOnSegment);
        }

        public static double ShortestSquaredDistanceToPoint(this LineSegment2D lineSegment, Point2D point, out Point2D pointOnSegment)
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

        public static bool IntersectsOtherLineSegment(this LineSegment2D lineSegment, LineSegment2D otherLineSegment)
        {
            Point2D? intersectionPoint;
            return IntersectsOtherLineSegment(lineSegment, otherLineSegment, out intersectionPoint);
        }

        public static bool IntersectsOtherLineSegment(this LineSegment2D lineSegment, LineSegment2D otherLineSegment, out Point2D? intersectionPoint)
        {
            var line0 = new Line2D(lineSegment);
            var line1 = new Line2D(otherLineSegment);
            
            if(line0.IntersectsOtherLine(line1, out intersectionPoint))
            {
                var intersectionPointRatio0 = (intersectionPoint.Value - line0.PointA).SquaredLength() / lineSegment.SquaredLength();

                if(intersectionPointRatio0 < 0 || intersectionPointRatio0 > 1)
                {
                    intersectionPoint = null;
                    return false;
                }
                var intersectionPointRatio1 = (intersectionPoint.Value - line1.PointA).SquaredLength() / otherLineSegment.SquaredLength();
                if (intersectionPointRatio1 < 0 || intersectionPointRatio1 > 1)
                {
                    intersectionPoint = null;
                    return false;
                }

                return true;
            }

            return false;
        }

        public static double ShortestDistanceToOtherLineSegment(this LineSegment2D lineSegment, LineSegment2D otherLineSegment)
        {
            return Math.Sqrt(ShortestSquaredDistanceToOtherLineSegment(lineSegment, otherLineSegment));
        }

        /// <summary>
        /// Finds the shortest distance between two line segments.
        /// </summary>
        /// <param name="lineSegment"></param>
        /// <param name="otherLineSegment"></param>
        /// <param name="shortestDistanceSegment">Outputs a line segment for the shortest found distance.
        /// PointA is on the first input line segment, PointB is on the second input line segment. </param>
        /// <returns></returns>
        public static double ShortestDistanceToOtherLineSegment(this LineSegment2D lineSegment, LineSegment2D otherLineSegment, out LineSegment2D shortestDistanceSegment)
        {
            return Math.Sqrt(ShortestSquaredDistanceToOtherLineSegment(lineSegment, otherLineSegment, out shortestDistanceSegment));
        }

        public static double ShortestSquaredDistanceToOtherLineSegment(this LineSegment2D lineSegment, LineSegment2D otherLineSegment)
        {
            LineSegment2D shortestDistanceSegment;
            return ShortestSquaredDistanceToOtherLineSegment(lineSegment, otherLineSegment, out shortestDistanceSegment);
        }

        /// <summary>
        /// Finds the shortest distance between two line segments.
        /// </summary>
        /// <param name="lineSegment"></param>
        /// <param name="otherLineSegment"></param>
        /// <param name="shortestDistanceSegment">Outputs a line segment for the shortest found distance.
        /// PointA is on the first input line segment, PointB is on the second input line segment. </param>
        /// <returns></returns>
        public static double ShortestSquaredDistanceToOtherLineSegment(this LineSegment2D lineSegment, LineSegment2D otherLineSegment, out LineSegment2D shortestDistanceSegment)
        {
            Point2D? intersectionPoint;
            if(lineSegment.IntersectsOtherLineSegment(otherLineSegment, out intersectionPoint))
            {
                shortestDistanceSegment = new LineSegment2D(intersectionPoint.Value, intersectionPoint.Value);
                return 0;
            }

            Point2D pointA, pointB;
            double shortestSquaredDistance;

            // Check first end point on first segment to second line segment.
            pointA = lineSegment.PointA;
            shortestSquaredDistance = otherLineSegment.ShortestSquaredDistanceToPoint(lineSegment.PointA, out pointB);

            Point2D pointCandidate;
            double distanceCandidate;
            
            // Check second end point on first segment to second line segment.
            distanceCandidate = otherLineSegment.ShortestSquaredDistanceToPoint(lineSegment.PointB, out pointCandidate);
            if(distanceCandidate < shortestSquaredDistance)
            {
                shortestSquaredDistance = distanceCandidate;
                pointA = lineSegment.PointB;
                pointB = pointCandidate;
            }

            // Check first end point on second line segment to first line segment.
            distanceCandidate = lineSegment.ShortestSquaredDistanceToPoint(otherLineSegment.PointA, out pointCandidate);
            if (distanceCandidate < shortestSquaredDistance)
            {
                shortestSquaredDistance = distanceCandidate;
                pointA = pointCandidate;
                pointB = otherLineSegment.PointA;
            }

            // Check second end point on second line segment to first line segment.
            distanceCandidate = lineSegment.ShortestSquaredDistanceToPoint(otherLineSegment.PointB, out pointCandidate);
            if (distanceCandidate < shortestSquaredDistance)
            {
                shortestSquaredDistance = distanceCandidate;
                pointA = pointCandidate;
                pointB = otherLineSegment.PointB;
            }

            shortestDistanceSegment = new LineSegment2D(pointA, pointB);
            return shortestSquaredDistance;
        }
    }
}

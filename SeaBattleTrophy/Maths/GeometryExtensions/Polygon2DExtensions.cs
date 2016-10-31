using System;
using System.Collections.Generic;

namespace Maths.Geometry
{
    public static class Polygon2DExtensions
    {
        public static IEnumerable<LineSegment2D> GetLineSegments(this Polygon2D polygon)
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                var p0 = polygon[i];
                var p1 = polygon[(i + 1) % polygon.Count];

                yield return new LineSegment2D(p0, p1);
            }
        }

        public static double ShortestDistanceToPoint(this Polygon2D polygon, Point2D point)
        {
            Point2D pointOnPolygon;
            return Math.Sqrt(ShortestSquaredDistanceToPoint(polygon, point, out pointOnPolygon));
        }

        public static double ShortestDistanceToPoint(this Polygon2D polygon, Point2D point, out Point2D pointOnPolygon)
        {
            return Math.Sqrt(ShortestSquaredDistanceToPoint(polygon, point, out pointOnPolygon));
        }

        public static double ShortestSquaredDistanceToPoint(this Polygon2D polygon, Point2D point)
        {
            Point2D pointOnPolygon;
            return ShortestSquaredDistanceToPoint(polygon, point, out pointOnPolygon);
        }

        public static double ShortestSquaredDistanceToPoint(this Polygon2D polygon, Point2D point, out Point2D pointOnPolygon)
        {
            var minSquaredDistance = double.MaxValue;
            pointOnPolygon = new Point2D();
            foreach (var lineSegment in polygon.GetLineSegments())
            {
                Point2D candidateForPointOnPolygon;
                var squaredDis = lineSegment.ShortestSquaredDistanceToPoint(point, out candidateForPointOnPolygon);

                if (squaredDis < minSquaredDistance)
                {
                    minSquaredDistance = squaredDis;
                    pointOnPolygon = candidateForPointOnPolygon;
                }
            }

            return minSquaredDistance;
        }
    }
}

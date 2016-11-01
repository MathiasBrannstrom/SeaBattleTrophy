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

        public static double ShortestDistanceToOtherPolygon(this Polygon2D polygon, Polygon2D otherPolygon)
        {
            return Math.Sqrt(ShortestSquaredDistanceToOtherPolygon(polygon, otherPolygon));
        }

        public static double ShortestDistanceToOtherPolygon(this Polygon2D polygon, Polygon2D otherPolygon, out LineSegment2D shortestDistanceSegment)
        {
            return Math.Sqrt(ShortestSquaredDistanceToOtherPolygon(polygon, otherPolygon, out shortestDistanceSegment));
        }

        public static double ShortestSquaredDistanceToOtherPolygon(this Polygon2D polygon, Polygon2D otherPolygon)
        {
            LineSegment2D shortestDistanceSegment;
            return ShortestSquaredDistanceToOtherPolygon(polygon, otherPolygon, out shortestDistanceSegment);
        }

        public static double ShortestSquaredDistanceToOtherPolygon(this Polygon2D polygon, Polygon2D otherPolygon, out LineSegment2D shortestDistanceSegment)
        {
            var shortestSquaredDistance = double.MaxValue;
            shortestDistanceSegment = new LineSegment2D();

            LineSegment2D candidateSegment;
            double candidateSquaredDistance;
            foreach(var lineSegment0 in polygon.GetLineSegments())
                foreach(var lineSegment1 in otherPolygon.GetLineSegments())
                {
                    candidateSquaredDistance = lineSegment0.ShortestSquaredDistanceToOtherLineSegment(lineSegment1, out candidateSegment);

                    if(candidateSquaredDistance < shortestSquaredDistance)
                    {
                        shortestSquaredDistance = candidateSquaredDistance;
                        shortestDistanceSegment = candidateSegment;
                    }
                }

            return shortestSquaredDistance;
        }
    }
}

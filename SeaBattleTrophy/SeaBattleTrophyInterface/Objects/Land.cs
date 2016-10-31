using System;
using System.Collections.Generic;
using Maths.Geometry;

namespace SeaBattleTrophyGame
{
    public class LandMass
    {
        public IEnumerable<Point2D> CornerCoordinates { get { return _cornerCoordinates; } }
        private List<Point2D> _cornerCoordinates;

        public LandMass(IEnumerable<Point2D> cornerCoordinates)
        {
            _cornerCoordinates = new List<Point2D>(cornerCoordinates);
        }

        public double DistanceToPoint(Point2D point)
        {
            var minSquaredDistance = double.MaxValue;
            for(int i = 0; i < _cornerCoordinates.Count; i++)
            {
                var p0 = _cornerCoordinates[i];
                var p1 = _cornerCoordinates[(i + 1) % _cornerCoordinates.Count];

                var lineSegment = new LineSegment(p0, p1);

                var squaredDis = lineSegment.ShortestSquaredDistanceToPoint(point);
            
                if (squaredDis < minSquaredDistance)
                        minSquaredDistance = squaredDis;
            }

            return Math.Sqrt(minSquaredDistance);
        }

    }
}

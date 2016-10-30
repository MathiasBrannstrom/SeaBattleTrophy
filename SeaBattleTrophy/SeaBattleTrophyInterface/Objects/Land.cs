using System;
using System.Collections.Generic;
using Utilities;

namespace SeaBattleTrophyGame
{
    public class Land
    {
        public IEnumerable<Point2D> CornerCoordinates { get { return _cornerCoordinates; } }
        private List<Point2D> _cornerCoordinates;

        public Land(IEnumerable<Point2D> cornerCoordinates)
        {
            _cornerCoordinates = new List<Point2D>(cornerCoordinates);
        }

        public double DistanceToPoint(Point2D point)
        {
            var minSquaredDistance = float.MaxValue;
            for(int i = 0; i < _cornerCoordinates.Count; i++)
            {
                var p0 = _cornerCoordinates[i];
                var p1 = _cornerCoordinates[(i + 1) % _cornerCoordinates.Count];

                var lineSegmentSquaredLength = (p1 - p0).SquaredLength();

                if (lineSegmentSquaredLength == 0) return (point - p1).SquaredLength();

                var t = ((point.X - p0.X) * (p1.X - p0.X) + (point.Y - p0.Y) * (p1.Y - p0.Y)) / lineSegmentSquaredLength;

                t = Math.Max(0, Math.Min(1, t));

                var closestPointOnSegment = new Point2D(p0.X + t * (p1.X - p0.X), p0.Y + t * (p1.Y - p0.Y));

                var squaredDis = (closestPointOnSegment - point).SquaredLength();
            
                if (squaredDis < minSquaredDistance)
                        minSquaredDistance = squaredDis;
            }

            return Math.Sqrt(minSquaredDistance);
        }

    }
}

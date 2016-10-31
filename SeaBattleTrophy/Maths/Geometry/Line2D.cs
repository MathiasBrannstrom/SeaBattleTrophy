﻿namespace Maths.Geometry
{
    /// <summary>
    /// Line in 2D defined by PointA + t*Direction, where t: [-inf, inf] and Direction is (PointB - PointA)
    /// </summary>
    public struct Line2D
    {
        public Point2D PointA { get; }

        public Point2D PointB { get; }
        
        public Line2D(Point2D pointA, Point2D pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }

        public Vector2D Direction { get { return PointB - PointA; } }
    }
}
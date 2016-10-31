﻿namespace Maths.Geometry
{
    public struct Point2D
    {
        public float X { get; }
        public float Y { get; }

        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Point2D operator *(Point2D point, float val)
        {
            return new Point2D(point.X * val, point.Y * val);
        }

        public static Point2D operator /(Point2D point, float val)
        {
            return new Point2D (point.X / val, point.Y / val);
        }

        public static Point2D operator +(Point2D point, Vector2D vector)
        {
            return new Point2D(point.X + vector.X, point.Y + vector.Y);
        }

        public static Vector2D operator -(Point2D point0, Point2D point1)
        {
            return new Vector2D(point0.X - point1.X, point0.Y - point1.Y);
        }
    }
}
using System;

namespace Maths.Geometry
{
    public struct Point2D : IEquatable<Point2D>
    {
        public double X { get; }
        public double Y { get; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point2D operator *(Point2D point, double val)
        {
            return new Point2D(point.X * val, point.Y * val);
        }

        public static Point2D operator /(Point2D point, double val)
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

        public bool Equals(Point2D other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public Point2D Transform(Matrix2x2 matrix)
        {
            return new Point2D(matrix.V00 * X + matrix.V01 * Y, matrix.V10 * X + matrix.V11 * Y);
        }

        public override string ToString()
        {
            return string.Format("X: {0:0.00}; Y: {1:0.00}", X, Y);
        }
    }
}

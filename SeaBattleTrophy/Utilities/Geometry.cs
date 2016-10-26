using System;

namespace Utilities
{
    public struct Point2D
    {
        public float X;
        public float Y;

        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Point2D operator *(Point2D point, float val)
        {
            return new Point2D { X = point.X * val, Y = point.Y * val };
        }

        public static Point2D operator /(Point2D point, float val)
        {
            return new Point2D { X = point.X / val, Y = point.Y / val };
        }

        public static Point2D operator +(Point2D point, Vector2D vector)
        {
            return new Point2D { X = point.X + vector.X, Y = point.Y + vector.Y };
        }
    }

    public struct Vector2D
    {
        public float X;
        public float Y;

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator *(Vector2D vector, float val)
        {
            return new Vector2D { X = vector.X * val, Y = vector.Y * val };
        }

        public static Vector2D operator /(Vector2D vector, float val)
        {
            return new Vector2D { X = vector.X / val, Y = vector.Y / val };
        }

        public static Vector2D operator +(Vector2D vector0, Vector2D vector1)
        {
            return new Vector2D { X = vector0.X + vector1.X, Y = vector0.Y + vector1.Y };
        }

        public Vector2D Rotate(float degrees)
        {
            var radian = degrees / 180 * Math.PI;
            var x = this.X * Math.Cos(radian) - this.Y * Math.Sin(radian);
            var y = this.X * Math.Sin(radian) + this.Y * Math.Cos(radian);

            return new Vector2D((float)x, (float)y);
        }
    }

    public struct Size
    {
        public float Width;
        public float Length;
    }
}

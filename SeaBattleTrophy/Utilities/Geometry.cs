namespace Utilities
{
    public struct Point2D
    {
        public float X;
        public float Y;

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
    }

    public struct Size
    {
        public float Width;
        public float Length;
    }
}

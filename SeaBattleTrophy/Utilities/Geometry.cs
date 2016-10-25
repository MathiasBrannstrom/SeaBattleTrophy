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
    }

    public struct Vector2D
    {
        public float X;
        public float Y;
    }

    public struct Size
    {
        public float Width;
        public float Length;
    }
}

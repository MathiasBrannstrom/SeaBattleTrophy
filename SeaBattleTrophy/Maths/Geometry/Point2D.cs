using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths.Geometry
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

        public static Vector2D operator -(Point2D point0, Point2D point1)
        {
            return new Vector2D { X = point0.X - point1.X, Y = point0.Y - point1.Y };
        }
    }
}

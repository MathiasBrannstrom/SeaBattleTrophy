using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths.Geometry
{
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

        public float Dot(Vector2D vector)
        {
            return this.X * vector.X + this.Y * vector.Y;
        }

        public float SquaredLength()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        public float Length()
        {
            return (float)Math.Sqrt(SquaredLength());
        }

        public Vector2D Rotate(float degrees)
        {
            var radian = degrees / 180 * Math.PI;
            var x = this.X * Math.Cos(radian) - this.Y * Math.Sin(radian);
            var y = this.X * Math.Sin(radian) + this.Y * Math.Cos(radian);

            return new Vector2D((float)x, (float)y);
        }
    }
}

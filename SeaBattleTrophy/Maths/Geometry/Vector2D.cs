﻿using System;

namespace Maths.Geometry
{
    public struct Vector2D : IEquatable<Vector2D>
    {
        public double X { get; }
        public double Y { get; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator *(Vector2D vector, double val)
        {
            return new Vector2D(vector.X * val, vector.Y * val);
        }

        public static Vector2D operator /(Vector2D vector, double val)
        {
            return new Vector2D(vector.X / val, vector.Y / val);
        }

        public static Vector2D operator +(Vector2D vector0, Vector2D vector1)
        {
            return new Vector2D(vector0.X + vector1.X, vector0.Y + vector1.Y);
        }

        public double Dot(Vector2D vector)
        {
            return X * vector.X + Y * vector.Y;
        }

        public double SquaredLength()
        {
            return X * X + Y * Y;
        }

        public double Length()
        {
            return Math.Sqrt(SquaredLength());
        }

        public Vector2D Rotate(double degrees)
        {
            var radian = degrees / 180 * Math.PI;
            var x = X * Math.Cos(radian) - Y * Math.Sin(radian);
            var y = X * Math.Sin(radian) + Y * Math.Cos(radian);

            return new Vector2D(x, y);
        }

        public bool Equals(Vector2D other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public static Vector2D DirectionFromAngle(double angle)
        {
            return new Vector2D(-Math.Sin(angle * Math.PI / 180.0), Math.Cos(angle * Math.PI / 180.0));
        }
    }
}

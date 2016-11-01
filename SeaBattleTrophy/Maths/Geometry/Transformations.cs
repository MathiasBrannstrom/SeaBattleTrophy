using System;

namespace Maths.Geometry
{
    public static class Transformations
    {
        /// <summary>
        /// Get a rotation transform from an angle. The angle is specified in degrees and is in CCW rotation.
        /// </summary>
        public static Matrix2x2 Rotation2D(float angle)
        {
            var cos = (float)Math.Cos(angle * Math.PI / 180);
            var sin = (float)Math.Sin(angle * Math.PI / 180);
            return new Matrix2x2(cos, -sin, sin, cos);
        }
    }
}

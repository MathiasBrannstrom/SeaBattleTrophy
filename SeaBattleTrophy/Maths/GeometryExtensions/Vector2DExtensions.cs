using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths.Geometry
{
    public static class Vector2DExtensions
    {
        public static Vector2D Normalize(this Vector2D v)
        {
            return v / v.Length();
        }

        public static bool IsParallelWith(this Vector2D v0, Vector2D v1)
        {
            return Math.Abs(v0.Normalize().Dot(v1.Normalize())).NearEquals(1);
        }
    }
}

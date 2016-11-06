using System;

namespace Maths
{
    public static class DoubleExtensions
    {
        public static bool NearEquals(this double val, double otherVal, double acceptanceLevel = double.Epsilon)
        {
            return Math.Abs(val - otherVal) < acceptanceLevel;
        }

        public static double Modulo(this double val, double mod)
        {
            return val - mod * Math.Floor(val / mod);
        }
    }
}

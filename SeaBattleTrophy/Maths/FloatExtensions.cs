using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths
{
    public static class doubleExtensions
    {
        public static bool NearEquals(this double val, double otherVal, double acceptanceLevel = double.Epsilon)
        {
            return Math.Abs(val - otherVal) < acceptanceLevel;
        }
    }
}

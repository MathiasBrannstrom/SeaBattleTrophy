using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class FloatExtensions
    {
        public static bool NearEquals(this float val, float otherVal, float acceptanceLevel = float.Epsilon)
        {
            return Math.Abs(val - otherVal) < acceptanceLevel;
        }
    }
}

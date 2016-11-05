using System;

namespace Utilities
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan DivideBy(this TimeSpan timeSpan, long val)
        {
            return new TimeSpan(timeSpan.Ticks / val);
        }
    }
}

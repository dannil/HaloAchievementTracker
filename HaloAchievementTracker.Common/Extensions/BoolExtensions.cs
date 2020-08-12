using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Extensions
{
    public static class BoolExtensions
    {
        public static string ToMarks(this bool b)
        {
            return (b ? "X" : "-");
        }
    }
}

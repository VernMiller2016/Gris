using System;

namespace GRis.Core.Extensions
{
    public static class DateExtensions
    {
        public static string GetTimeStamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff");
        }
    }
}
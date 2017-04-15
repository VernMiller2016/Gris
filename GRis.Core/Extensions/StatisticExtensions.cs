using System;
using System.Collections.Generic;
using System.Linq;

namespace GRis.Core.Extensions
{
    public static class StatisticExtensions
    {
        public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            return source.Aggregate(TimeSpan.Zero, (sumSoFar, nextMyObject) => sumSoFar + selector(nextMyObject));
        }
    }
}
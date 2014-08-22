using System;
using System.Collections.Generic;
using System.Linq;

namespace InputSite.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Random<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(x => Guid.NewGuid());
        }

        public static void Map<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }
    }
}
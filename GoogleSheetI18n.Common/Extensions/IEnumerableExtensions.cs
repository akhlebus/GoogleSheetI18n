using System;
using System.Collections.Generic;

namespace GoogleSheetI18n.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int FindIndex<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            var index = 0;
            var isFound = false;

            foreach (var item in items)
            {
                if (predicate(item))
                {
                    isFound = true;
                    break;
                }

                index++;
            }

            return isFound ? index : -1;
        }
    }
}
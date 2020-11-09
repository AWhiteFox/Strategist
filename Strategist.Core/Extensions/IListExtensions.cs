using System;
using System.Collections.Generic;

namespace Strategist.Core.Extensions
{
    public static class IListExtensions
    {
        public static void Fill<T>(this IList<T> list, Func<int, T> func)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = func.Invoke(i);
            }
        }

        public static int FindIndex<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate.Invoke(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

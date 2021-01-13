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

        public static IEnumerable<List<T>> Combinations<T>(this IList<T> list)
        {
            int count = (int)Math.Pow(2, list.Count);
            for (int i = count - 1; i > 0; i--)
            {
                string str = Convert.ToString(i, 2).PadLeft(list.Count, '0');
                var result = new List<T>(list.Count);
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '1')
                    {
                        result.Add(list[j]);
                    }
                }
                yield return result;
            }
        }
    }
}

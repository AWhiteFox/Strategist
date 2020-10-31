using System;

namespace Strategist.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static void Fill<T>(this T[] array, Func<int, T> func)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = func.Invoke(i);
            }
        }

        public static int FindIndex<T>(this T[] array, Func<T, bool> predicate)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (predicate.Invoke(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

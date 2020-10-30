using System;

namespace Strategist.UI
{
    internal static class ArrayExtensions
    {
        public static void Fill<T>(this T[] array, Func<int, T> func)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = func.Invoke(i);
            }
        }
    }
}

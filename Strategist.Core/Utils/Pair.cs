using System;

namespace Strategist.Core.Utils
{
    internal class Pair<T>
    {
        public T First { get; set; }
        public T Second { get; set; }

        public T this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return First;
                    case 1:
                        return Second;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        First = value;
                        return;
                    case 1:
                        Second = value;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    internal static class Pair
    {
        public static Pair<T> FromFunc<T>(Func<int, T> func) => new Pair<T> { First = func.Invoke(0), Second = func.Invoke(1) };
    }
}

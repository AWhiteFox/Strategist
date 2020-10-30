using System;

namespace Strategist.UI  // TODO: Move to different project
{
    public class Matrix
    {
        public int Columns { get; }

        public int Rows { get; }
        
        public string[] ColumnHeaders { get; }

        public string[] RowHeaders { get; }

        public float[,] Values { get; }

        public Matrix(int ccount, int rcount)
        {
            Columns = ccount;
            Rows = rcount;
            ColumnHeaders = new string[ccount];
            RowHeaders = new string[rcount];
            Values = new float[ccount, rcount];
        }

        public static Matrix Random(int ccount, int rcount)
        {
            var m = new Matrix(ccount, rcount);
            var rnd = new Random();

            for (int i = 0; i < ccount; i++)
            {
                m.ColumnHeaders[i] = $"Угроза {i}";
            }
            for (int i = 0; i < rcount; i++)
            {
                m.RowHeaders[i] = $"Средство защиты {i}";
            }

            for (int j = 0; j < rcount; j++)
            {
                for (int i = 0; i < ccount; i++)
                {
                    m.Values[i, j] = (float)Math.Round(rnd.NextDouble(), 2);
                }
            }

            return m;
        }
    }
}

using System;

namespace Strategist.UI  // TODO: Move to different project
{
    public class Matrix
    {
        public MatrixColumnRowData[] Columns { get; }

        public MatrixColumnRowData[] Rows { get; }

        public float[,] Values { get; }

        public Matrix(int ccount, int rcount)
        {
            Columns = new MatrixColumnRowData[ccount];   
            Rows = new MatrixColumnRowData[rcount];
            
            Values = new float[ccount, rcount];
        }

        public static Matrix Random(int ccount, int rcount)
        {
            var m = new Matrix(ccount, rcount);
            var rnd = new Random();

            m.Columns.Fill((i) => new MatrixColumnRowData 
            { 
                Header = $"Угроза {i}",
                Enabled = true 
            });
            m.Rows.Fill((i) => new MatrixColumnRowData
            {
                Header = $"Средство защиты {i}",
                Enabled = true
            });

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

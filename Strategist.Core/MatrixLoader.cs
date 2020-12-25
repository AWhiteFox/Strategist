using Strategist.Core.Extensions;
using System;

namespace Strategist.Core
{
    public static class MatrixLoader
    {
        // TEMOPORARY IMPLEMENTATION
        public static Matrix LoadRandom()
        {
            int col_count = 6;
            int row_count = 3;

            var m = new Matrix();

            for (int i = 0; i < col_count; i++)
            {
                m.AddColumn(new[] { $"Угроза {i}" });
            }

            string[] rows = new string[row_count];
            rows.Fill(i => $"Средство {i}");
            foreach (var row in rows.Combinations())
            {
                m.AddRow(row.ToArray());
            }

            var rnd = new Random();
            for (int j = 0; j < m.Rows.Count; j++)
            {
                for (int i = 0; i < m.Columns.Count; i++)
                {
                    m[i, j] = Math.Round(rnd.NextDouble(), 2);
                }
            }

            return m;
        }
    }
}

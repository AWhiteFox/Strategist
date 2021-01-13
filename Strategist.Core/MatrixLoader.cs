using Strategist.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var rnd = new Random();

            for (int i = 0; i < col_count; i++)
            {
                m.AddColumn(new[] { $"Угроза {i}" });
            }

            var rows = new Dictionary<string, double[]>();
            for (int i = 0; i < row_count; i++)
            {
                var arr = new double[col_count];
                arr.Fill(_ => Math.Round(rnd.NextDouble(), 2));
                rows[$"Средство {i}"] = arr;
            }
            
            foreach (var row in rows.Keys.ToArray().Combinations())
            {
                m.AddRow(row.ToArray());
                int i = m.Rows.Count - 1;
                for (int j = 0; j < col_count; j++)
                {
                    double val = 0.0;
                    foreach (var tag in row)
                    {
                        val = Math.Max(val, rows[tag][j]);
                    }
                    m[j, i] = val;
                }
            }

            return m;
        }
    }
}

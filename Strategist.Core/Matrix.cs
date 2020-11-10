using Strategist.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Strategist.Core
{
    public class Matrix
    {
        public MatrixHeader[] ColumnHeaders { get; }

        public MatrixHeader[] RowHeaders { get; }

        public MatrixRow[] Rows { get; }

        public double[,] Values { get; }

        public Matrix(string[] columnHeaders, string[] rowHeaders)
        {
            int rcount = (int)Math.Pow(2, rowHeaders.Length) - 1;

            ColumnHeaders = columnHeaders.Select(x => new MatrixHeader(x)).ToArray();
            RowHeaders = rowHeaders.Select(x => new MatrixHeader(x)).ToArray();
            
            Rows = new MatrixRow[rcount];
            Values = new double[columnHeaders.Length, rcount];
        }

        public void SetRowHeaders(int i, IEnumerable<string> headerTitles)
        {
            if (i < 0 || i >= Rows.Length)
                throw new ArgumentOutOfRangeException(nameof(i));
            
            MatrixHeader[] headers = headerTitles.Select(x => Array.Find(RowHeaders, y => y.Title == x)).ToArray();
            Rows[i] = new MatrixRow(this, i, headers);
        }

        public static Matrix GetRandom()
        { 
            string[] columnHeaders = new string[6];
            columnHeaders.Fill(i => $"Угроза {i}");
            string[] rowHeaders = new string[3];
            rowHeaders.Fill(i => $"Средство {i}");

            var rnd = new Random();
            var m = new Matrix(columnHeaders, rowHeaders);

            List<List<string>> combinations = rowHeaders.Combinations().ToList();
            for (int i = 0; i < combinations.Count; i++)
            {
                m.SetRowHeaders(i, combinations[i]);
            }

            for (int j = 0; j < m.Rows.Length; j++)
            {
                for (int i = 0; i < m.ColumnHeaders.Length; i++)
                {
                    m.Values[i, j] = Math.Round(rnd.NextDouble(), 2);
                }
            }

            return m;
        }
    }
}

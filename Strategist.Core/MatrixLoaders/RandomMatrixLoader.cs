using Strategist.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Strategist.Core.MatrixLoaders
{
    public class RandomMatrixLoader : MatrixLoader
    {
        private readonly int width;
        private readonly int height;

        public RandomMatrixLoader(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override Matrix Load()
        {
            var m = new Matrix();
            var rnd = new Random();

            for (int i = 0; i < width; i++)
            {
                m.AddColumn(new[] { $"Контрстратегия {i}" });
            }

            var rows = new Dictionary<string, double[]>();
            for (int i = 0; i < height; i++)
            {
                var arr = new double[width];
                arr.Fill(_ => Math.Round(rnd.NextDouble(), 2));
                rows[$"Стратегия {i}"] = arr;
            }

            foreach (var row in rows.Keys.ToArray().Combinations())
            {
                m.AddRow(row.ToArray());
                int i = m.Height - 1;
                for (int j = 0; j < width; j++)
                {
                    double val = row.Select(tag => rows[tag][j]).Max();
                    m[j, i] = val;
                }
            }

            return m;
        }
    }
}

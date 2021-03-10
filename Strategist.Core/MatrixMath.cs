using System;
using System.Collections.Generic;

namespace Strategist.Core
{
    public static class MatrixMath
    {
        public static int FindBestRow(Matrix matrix, IList<double> thresholds)
        {
            int best = -1;
            for (int i = 0; i < matrix.Height; i++)
            {
                if (!matrix.RowsEnabled[i])
                    continue;

                bool flag = true;
                for (int j = 0; j < matrix.Width; j++)
                {
                    if (!matrix.ColumnsEnabled[j] || matrix[j, i] >= thresholds[j]) 
                        continue;
                    flag = false;
                    break;
                }
                if (flag && (best == -1 || matrix.RowHeaders[i].Count < matrix.RowHeaders[best].Count))
                {
                    best = i;
                }
            }
            return best;
        }

        public static double[] GetColumnMaximums(Matrix matrix)
        {
            var maximums = new double[matrix.Width];
            for (int i = 0; i < matrix.Width; i++)
            {
                if (matrix.ColumnsEnabled[i])
                {
                    double max = 0;
                    for (int j = 0; j < matrix.Height; j++)
                    {
                        if (matrix.RowsEnabled[j])
                        {
                            max = Math.Max(max, matrix[i, j]);
                        }
                    }
                    maximums[i] = max;
                }
                else
                {
                    maximums[i] = -1;
                }
            }
            return maximums;
        }

        public static double[] GetColumnMedians(Matrix matrix)
        {
            var medians = new double[matrix.Width];
            var values = new List<double>();
            for (int i = 0; i < matrix.Width; i++)
            {
                if (matrix.ColumnsEnabled[i])
                {
                    values.Clear();
                    for (int j = 0; j < matrix.Height; j++)
                    {  
                        if (matrix.RowsEnabled[j])
                        {
                            values.Add(matrix[i, j]);
                        }
                    }
                    values.Sort();
                    if (values.Count % 2 == 0)
                    {
                        medians[i] = (values[values.Count / 2 - 1] + values[values.Count / 2]) / 2;
                    }
                    else
                    {
                        medians[i] = values[values.Count / 2];
                    }
                }
                else
                {
                    medians[i] = -1;
                }
            }
            return medians;
        }
    }
}

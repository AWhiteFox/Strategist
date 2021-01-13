using System;
using System.Collections.Generic;
using System.Linq;

namespace Strategist.Core
{
    public static class MatrixMath
    {
        public static int FindBestRow(Matrix matrix, double[] thresholds)
        {
            int best_i = -1;
            for (int i = 0; i < matrix.Rows.Count; i++)
            {
                if (!matrix.Rows[i].IsEnabled)
                    continue;

                bool flag = true;
                for (int j = 0; j < matrix.Columns.Count; j++)
                {
                    if (!matrix.Columns[j].IsEnabled)
                        continue;

                    if (matrix[j, i] < thresholds[j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag && (best_i == -1 || matrix.Columns[i].Tags.Count < matrix.Columns[best_i].Tags.Count))
                {
                    best_i = i;
                }
            }
            return best_i;
        }

        public static int FindBestRow(Matrix matrix, double threshold)
        {
            return FindBestRow(matrix, Enumerable.Repeat(threshold, matrix.Columns.Count).ToArray());
        }

        public static double[] GetColumnMaximums(Matrix matrix)
        {
            double[] maximums = new double[matrix.Columns.Count];
            for (int i = 0; i < matrix.Columns.Count; i++)
            {
                if (matrix.Columns[i].IsEnabled)
                {
                    double max = 0;
                    for (int j = 0; j < matrix.Rows.Count; j++)
                    {
                        if (matrix.Rows[j].IsEnabled)
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
            double[] medians = new double[matrix.Columns.Count];
            List<double> values = new List<double>();
            for (int i = 0; i < matrix.Columns.Count; i++)
            {
                if (matrix.Columns[i].IsEnabled)
                {
                    values.Clear();
                    for (int j = 0; j < matrix.Rows.Count; j++)
                    {  
                        if (matrix.Rows[j].IsEnabled)
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

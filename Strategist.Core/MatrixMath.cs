using System;
using System.Collections.Generic;
using System.Linq;

namespace Strategist.Core
{
    public static class MatrixMath
    {
        private const string MatrixMustHaveRowCombinationsMessage = "Матрица должна содержать комбинации строк.";
        private const string MatrixDoesNotHaveEnoughData = "Матрица не содержит достаточное количество данных.";
        
        public static int FindBestRow(Matrix matrix, IList<double> thresholds)
        {
            if (!matrix.HasCombinedRowHeaders)
                throw new ArgumentException(MatrixMustHaveRowCombinationsMessage);
            return matrix.HasCombinedColumnHeaders ? FindBestRowByColumn(matrix, thresholds) : FindBestRowByComparison(matrix, thresholds);
        }

        public static int ImproveRow(Matrix matrix, IList<double> thresholds)
        {
            if (!matrix.HasCombinedRowHeaders)
                throw new ArgumentException(MatrixMustHaveRowCombinationsMessage);
            return matrix.HasCombinedColumnHeaders ? ImproveRowByColumn(matrix, thresholds) : ImproveRowByComparison(matrix, thresholds);
        }

        public static int AnalyzeRow(Matrix matrix, IList<double> thresholds, int row)
        {
            if (!matrix.HasCombinedRowHeaders)
                throw new ArgumentException(MatrixMustHaveRowCombinationsMessage);
            return matrix.HasCombinedColumnHeaders ? AnalyzeRowByColumn(matrix, thresholds, row) : AnalyzeRowByComparison(matrix, thresholds, row);
        }
        
        public static double[] GetColumnMaximums(Matrix matrix, bool ignoreDisabledRows = false)
        {
            var maximums = new double[matrix.Width];
            for (int i = 0; i < matrix.Width; i++)
            {
                if (matrix.ColumnsEnabled[i])
                {
                    double max = 0;
                    for (int j = 0; j < matrix.Height; j++)
                    {
                        if (ignoreDisabledRows || matrix.RowsEnabled[j])
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

        public static double[] GetColumnMedians(Matrix matrix, bool ignoreDisabledRows = false)
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
                        if (ignoreDisabledRows || matrix.RowsEnabled[j])
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

        private static int FindBestRowByComparison(Matrix matrix, IList<double> thresholds)
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

        private static int FindBestRowByColumn(Matrix matrix, IList<double> thresholds)
        {
            int col = GetFullColumn(matrix);
            int best = -1;
            for (int j = 0; j < matrix.Height; j++)
            {
                if (!matrix.RowsEnabled[j] || matrix[col, j] < thresholds[col])
                    continue;
                if (best == -1 || matrix.RowHeaders[j].Count < matrix.RowHeaders[best].Count)
                    best = j;
            }
            return best;
        }

        private static int ImproveRowByComparison(Matrix matrix, IList<double> thresholds)
        {
            int row = GetFullRow(matrix);
            int best = row;
            for (int j = 0; j < matrix.Height; j++)
            {
                if (!matrix.RowHeaders[row].All(x => matrix.RowHeaders[j].Contains(x)))
                    continue;

                bool isBetter = false;
                bool isWorse = false;
                for (int i = 0; i < matrix.Width; i++)
                {
                    if (!matrix.ColumnsEnabled[i]) 
                        continue;
                    
                    if (matrix[i, j] < thresholds[i] && matrix[i, best] >= thresholds[i])
                    {
                        isWorse = true;
                        break;
                    }
                    if (matrix[i, j] >= thresholds[i] && matrix[i, best] < thresholds[i])
                    {
                        isBetter = true;
                    }
                }
                if (!isWorse && (isBetter || matrix.RowHeaders[j].Count < matrix.RowHeaders[best].Count))
                    best = j;
            }
            return best;
        }

        private static int ImproveRowByColumn(Matrix matrix, IList<double> thresholds)
        {
            int col = GetFullColumn(matrix);
            int row = GetFullRow(matrix);
            int best = row;
            for (int j = 0; j < matrix.Height; j++)
            {
                if (!matrix.RowHeaders[row].All(x => matrix.RowHeaders[j].Contains(x)))
                    continue;

                bool isBetter = matrix[col, j] >= thresholds[col] && matrix[col, best] < thresholds[col];
                bool isWorse = matrix[col, j] < thresholds[col] && matrix[col, best] >= thresholds[col];

                if (!isWorse && (isBetter || matrix.RowHeaders[j].Count < matrix.RowHeaders[best].Count))
                    best = j;
            }
            return best;
        }
        
        private static int AnalyzeRowByComparison(Matrix matrix, IList<double> thresholds, int row)
        {
            throw new NotImplementedException();
        }
        
        private static int AnalyzeRowByColumn(Matrix matrix, IList<double> thresholds, int row)
        {
            throw new NotImplementedException();
        }

        private static int GetFullColumn(Matrix matrix)
        {
            int value = matrix.GetColumnIndex(matrix.ColumnTags.Keys.Where(x => matrix.ColumnTags[x]));
            if (value == -1)
                throw new ArgumentException(MatrixDoesNotHaveEnoughData);
            return value;
        }

        private static int GetFullRow(Matrix matrix)
        {
            int value = matrix.GetRowIndex(matrix.RowTags.Keys.Where(x => matrix.RowTags[x]));
            if (value == -1)
                throw new ArgumentException(MatrixDoesNotHaveEnoughData);
            return value;
        }
    }
}

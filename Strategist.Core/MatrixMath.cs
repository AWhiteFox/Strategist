using System.Collections.Generic;

namespace Strategist.Core
{
    public static class MatrixMath
    {
        /// <summary>
        /// Finds minimal set of best strategies for enabled rows and columns
        /// </summary>
        /// <param name="m">A matrix</param>
        /// <returns>List of strategies</returns>
        public static List<int> FindBest(Matrix m, bool skipDisabledColumns = true)
        {
            var vals = m.Values;
            var best = new int[vals.GetLength(1)];
            var max = new double[vals.GetLength(1)];

            for (int i = 0; i < vals.GetLength(0); i++)
            {
                if (skipDisabledColumns && !m.Columns[i].Enabled)
                    continue;

                for (int j = 0; j < vals.GetLength(1); j++)
                {
                    if (m.Rows[j].Enabled && vals[i, j] > max[j])
                    {
                        max[j] = vals[i, j];
                        best[j] = i;
                    }
                }
            }

            var results = new List<int>();
            for (int i = 0; i < vals.GetLength(1); i++)
            {
                if (m.Rows[i].Enabled && !results.Contains(best[i]))
                {
                    results.Add(best[i]);
                }
            }
            return results;
        }

        /// <summary>
        /// Tries to find minimal set of strategies that protect with equal or higher probability than provided value
        /// </summary>
        /// <param name="m">A matrix</param>
        /// <param name="value">Minimal probability of protection</param>
        /// <param name="results">Best strategies</param>
        /// <returns>True if solution exists</returns>
        public static bool TryFindByProbability(Matrix m, double value, out List<int> results)
        {
            var vals = m.Values;
            var solvable = true;
            results = new List<int>();

            var check = new bool[m.Rows.Length];
            int count = check.Length;
            for (int i = 0; i < check.Length; i++)
            {
                check[i] = !m.Rows[i].Enabled;
                if (check[i])
                {
                    count--;
                }
            }

            while (count > 0)
            {
                int max_val = 0;
                int max_i = -1;
                for (int i = 0; i < m.Columns.Length; i++)
                {
                    if (results.Contains(i))
                        continue;

                    int p = 0;
                    for (int j = 0; j < m.Rows.Length; j++)
                    {
                        if (!check[j] && vals[i, j] >= value)
                        {
                            p++;
                        }
                    }
                    if (p > max_val)
                    {
                        max_val = p;
                        max_i = i;
                    }
                }

                if (max_i == -1)
                {
                    solvable = false;
                    break;
                }

                for (int j = 0; j < m.Rows.Length; j++)
                {
                    if (!check[j] && vals[max_i, j] >= value)
                    {
                        check[j] = true;
                        count--;
                    }
                }

                results.Add(max_i);
            }

            return solvable;
        }
    }
}

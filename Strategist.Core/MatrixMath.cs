namespace Strategist.Core
{
    public static class MatrixMath
    {
        /// <summary>
        /// Finds best row by average value
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static MatrixRow FindBestByAverage(Matrix m)
        {
            MatrixRow result = null;
            double best = 0;

            for (int j = 0; j < m.Values.GetLength(1); j++)
            {
                if (!m.Rows[j].Enabled)
                    continue;

                double sum = 0;
                for (int i = 0; i < m.Values.GetLength(0); i++)
                {
                    if (m.ColumnHeaders[i].Enabled)
                    {
                        sum += m.Values[i, j];
                    }
                }

                if (sum > best)
                {
                    best = sum;
                    result = m.Rows[j];
                }
            }

            return result;
        }

        /// <summary>
        /// Finds best strategy by minimal value
        /// </summary>
        /// <param name="m"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MatrixRow FindByProbability(Matrix m, double value)
        {
            MatrixRow result = null;
            int best = int.MaxValue;

            for (int j = 0; j < m.Values.GetLength(1); j++)
            {
                if (!m.Rows[j].Enabled)
                    continue;

                bool flag = true;
                for (int i = 0; i < m.Values.GetLength(0); i++)
                {
                    if (m.ColumnHeaders[i].Enabled && m.Values[i, j] < value)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag && m.Rows[j].Headers.Length < best)
                {
                    best = m.Rows[j].Headers.Length;
                    result = m.Rows[j];
                }
            }

            return result;
        }
    }
}

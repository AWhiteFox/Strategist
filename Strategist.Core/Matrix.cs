namespace Strategist.Core
{
    public class Matrix
    {
        public MatrixColumnRowData[] Columns { get; }

        public MatrixColumnRowData[] Rows { get; }

        public double[,] Values { get; }

        public Matrix(int ccount, int rcount)
        {
            Columns = new MatrixColumnRowData[ccount];
            Rows = new MatrixColumnRowData[rcount];
            Values = new double[ccount, rcount];
        }
    }
}

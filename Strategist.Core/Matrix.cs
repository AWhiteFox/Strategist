namespace Strategist.Core  // TODO: Move to different project
{
    public class Matrix<T>
    {
        public MatrixColumnRowData[] Columns { get; }

        public MatrixColumnRowData[] Rows { get; }

        public T[,] Values { get; }

        public Matrix(int ccount, int rcount)
        {
            Columns = new MatrixColumnRowData[ccount];
            Rows = new MatrixColumnRowData[rcount];
            Values = new T[ccount, rcount];
        }
    }
}

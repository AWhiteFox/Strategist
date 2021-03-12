using Strategist.Core;
using Strategist.Core.Extensions;
using System.Linq;

namespace Strategist.UI.ViewModels
{
    public class MatrixViewModel
    {
        public Matrix Matrix { get; }
        public MatrixColumnViewModel[] Columns { get; }
        public MatrixRowViewModel[] Rows { get; }
        public MatrixColumnTagViewModel[] ColumnTags { get; }
        public MatrixRowTagViewModel[] RowTags { get; }
        public int ColumnTagsEnabled => ColumnTags.Count(x => x.IsEnabled);
        public int RowTagsEnabled => RowTags.Count(x => x.IsEnabled);

        public double this[int i, int j]
        {
            get => Matrix[i, j];
            set => Matrix[i, j] = value;
        }

        public MatrixViewModel(Matrix matrix)
        {
            Matrix = matrix;
            ColumnTags = Matrix.ColumnTags.Keys.Select(x => new MatrixColumnTagViewModel(x, Matrix)).ToArray();
            RowTags = Matrix.RowTags.Keys.Select(x => new MatrixRowTagViewModel(x, Matrix)).ToArray();
            Columns = new MatrixColumnViewModel[Matrix.Width];
            Columns.Fill(i => new MatrixColumnViewModel(Matrix, i, ColumnTags));
            Rows = new MatrixRowViewModel[Matrix.Height];
            Rows.Fill(i => new MatrixRowViewModel(Matrix, i, RowTags));
        }

        public void SwitchAllColumnTags()
        {
            bool value = !ColumnTags[0].IsEnabled;
            foreach (var t in ColumnTags)
            {
                t.IsEnabled = value;
            }
        }

        public void SwitchAllRowTags()
        {
            bool value = !RowTags[0].IsEnabled;
            foreach (var t in RowTags)
            {
                t.IsEnabled = value;
            }
        }
    }
}

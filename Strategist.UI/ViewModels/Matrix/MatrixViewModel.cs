using Strategist.Core;
using Strategist.Core.Extensions;
using System.Linq;

namespace Strategist.UI.ViewModels
{
    public class MatrixViewModel
    {
        // Public Properties //
        public Matrix Matrix { get; }
        public MatrixColumnViewModel[] Columns { get; }
        public MatrixRowViewModel[] Rows { get; }
        public MatrixColumnTagViewModel[] ColumnTags { get; }
        public MatrixRowTagViewModel[] RowTags { get; }
        public int ColumnTagsEnabled => ColumnTags.Count(x => x.IsEnabled);
        public int RowTagsEnabled => RowTags.Count(x => x.IsEnabled);

        // Indexer //

        public double this[int i, int j]
        {
            get => Matrix[i, j];
            set => Matrix[i, j] = value;
        }

        // Constructor //

        public MatrixViewModel()
        {
            Matrix = MatrixLoader.LoadRandom();
            ColumnTags = Matrix.ColumnTags.Keys.Select(x => new MatrixColumnTagViewModel(x, Matrix)).ToArray();
            RowTags = Matrix.RowTags.Keys.Select(x => new MatrixRowTagViewModel(x, Matrix)).ToArray();
            Columns = new MatrixColumnViewModel[Matrix.ColumnHeaders.Count];
            Columns.Fill(i => new MatrixColumnViewModel(Matrix, i, ColumnTags));
            Rows = new MatrixRowViewModel[Matrix.RowHeaders.Count];
            Rows.Fill(i => new MatrixRowViewModel(Matrix, i, RowTags));
        }

        // Public Methods //

        public void SwitchAllColumnTags()
        {
            bool value = !ColumnTags[0].IsEnabled;
            for (int i = 0; i < ColumnTags.Length; i++)
            {
                ColumnTags[i].IsEnabled = value;
            }
        }

        public void SwitchAllRowTags()
        {
            bool value = !RowTags[0].IsEnabled;
            for (int i = 0; i < RowTags.Length; i++)
            {
                RowTags[i].IsEnabled = value;
            }
        }
    }
}

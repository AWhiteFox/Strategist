using Strategist.Core;
using Strategist.Core.Extensions;
using System.Linq;

namespace Strategist.UI.ViewModels
{
    public class MatrixViewModel
    {
        private readonly Matrix matrix;

        // Public Properties

        public MatrixColumnViewModel[] Columns { get; }
        public MatrixRowViewModel[] Rows { get; }
        public MatrixColumnTagViewModel[] ColumnTags { get; }
        public MatrixRowTagViewModel[] RowTags { get; }
        public int ColumnTagsEnabled => ColumnTags.Count(x => x.IsEnabled);
        public int RowTagsEnabled => RowTags.Count(x => x.IsEnabled);

        // Indexer

        public double this[int i, int j]
        {
            get => matrix[i, j];
            set => matrix[i, j] = value;
        }

        // Public Commands

        public RelayCommand SwitchAllColumnTagsCommand { get; }
        public RelayCommand SwitchAllRowTagsCommand { get; }

        // Constructor

        public MatrixViewModel()
        {
            matrix = MatrixLoader.LoadRandom();
            ColumnTags = matrix.ColumnTags.Keys.Select(x => new MatrixColumnTagViewModel(x, matrix)).ToArray();
            RowTags = matrix.RowTags.Keys.Select(x => new MatrixRowTagViewModel(x, matrix)).ToArray();
            Columns = new MatrixColumnViewModel[matrix.Columns.Count];
            Columns.Fill(i => new MatrixColumnViewModel(matrix, i, ColumnTags));
            Rows = new MatrixRowViewModel[matrix.Rows.Count];
            Rows.Fill(i => new MatrixRowViewModel(matrix, i, RowTags));

            SwitchAllColumnTagsCommand = new RelayCommand(SwitchAllColumnTags);
            SwitchAllRowTagsCommand = new RelayCommand(SwitchAllRowTags);
        }

        // Command Realizations

        private void SwitchAllColumnTags(object parameter)
        {
            bool value = !ColumnTags[0].IsEnabled;
            for (int i = 0; i < ColumnTags.Length; i++)
            {
                ColumnTags[i].IsEnabled = value;
            }
        }

        private void SwitchAllRowTags(object parameter)
        {
            bool value = !RowTags[0].IsEnabled;
            for (int i = 0; i < RowTags.Length; i++)
            {
                RowTags[i].IsEnabled = value;
            }
        }
    }
}

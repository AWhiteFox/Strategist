using System.Linq;
using Strategist.Core;

namespace Strategist.UI.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly MainWindow window;

        public MatrixViewModel Matrix { get; }
        public RelayCommand SwitchAllColumnTagsCommand { get; }
        public RelayCommand SwitchAllRowTagsCommand { get; }
        public RelayCommand FindBestRowCommand { get; }

        public MainWindowViewModel(MainWindow window)
        {
            this.window = window;
            Matrix = new MatrixViewModel();

            SwitchAllColumnTagsCommand = new RelayCommand(_ => Matrix.SwitchAllColumnTags());
            SwitchAllRowTagsCommand = new RelayCommand(_ => Matrix.SwitchAllRowTags());
            FindBestRowCommand = new RelayCommand(_ => OnFindBestRowCommand());
        }

        private void OnFindBestRowCommand()
        {
            var matrix = Matrix.Matrix;
            int result;
            if (window.MaxThresholdSelected)
                result = MatrixMath.FindBestRow(matrix, MatrixMath.GetColumnMaximums(matrix));
            else if (window.MedianThresholdSelected)
                result = MatrixMath.FindBestRow(matrix, MatrixMath.GetColumnMedians(matrix));
            else if (window.CustomThresholdSelected && window.TryGetCustomThreshold(out double val))
                result = MatrixMath.FindBestRow(matrix, Enumerable.Repeat(val, matrix.Width).ToArray());
            else
                return;
            window.ShowMessage("Результат", result == -1 ? "Нет решения" : Matrix.Rows[result].Header);
        }
    }
}

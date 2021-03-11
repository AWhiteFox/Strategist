using System.Collections.Generic;
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
        public RelayCommand ImproveRowCommand { get; }

        public MainWindowViewModel(MainWindow window)
        {
            this.window = window;
            Matrix = new MatrixViewModel();

            SwitchAllColumnTagsCommand = new RelayCommand(_ => Matrix.SwitchAllColumnTags());
            SwitchAllRowTagsCommand = new RelayCommand(_ => Matrix.SwitchAllRowTags());
            FindBestRowCommand = new RelayCommand(_ => OnFindBestRowCommand());
            ImproveRowCommand = new RelayCommand(_ => OnImproveRowCommand());
        }

        private IList<double> GetThresholds(bool ignoreDisabledRows = false)
        {
            var matrix = Matrix.Matrix;
            if (window.MaxThresholdSelected)
                return MatrixMath.GetColumnMaximums(matrix, ignoreDisabledRows);
            if (window.MedianThresholdSelected)
                return MatrixMath.GetColumnMedians(matrix, ignoreDisabledRows);
            if (window.CustomThresholdSelected && window.TryGetCustomThreshold(out double val))
                return Enumerable.Repeat(val, matrix.Width).ToArray();
            return null;
        }

        private void OnFindBestRowCommand()
        {
            int result = MatrixMath.FindBestRow(Matrix.Matrix, GetThresholds());
            window.ShowMessage("Результат", result == -1 ? "Нет решения" : Matrix.Rows[result].Header);
        }

        private void OnImproveRowCommand()
        {
            int result = MatrixMath.ImproveRow(Matrix.Matrix, GetThresholds(true));
            window.ShowMessage("Результат", result == -1 ? "Нет решения" : Matrix.Rows[result].Header);
            if (result == -1)
                return;
            foreach (string tag in Matrix.Rows[result].Tags)
            {
                Matrix.RowTags.First(x => x.Title == tag).IsEnabled = true;
            }
        }
    }
}

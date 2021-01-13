using Strategist.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategist.UI.ViewModels
{
    public class MainWindowViewModel
    {
        private MainWindow window;

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
                result = MatrixMath.FindBestRow(matrix, val);
            else
                return;
            window.ShowMessage("Результат", result == -1 ? "Нет решения" : Matrix.Rows[result].Header);
        }
    }
}

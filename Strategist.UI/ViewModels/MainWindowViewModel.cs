using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Strategist.Core;

namespace Strategist.UI.ViewModels
{
    public class MainWindowViewModel
    {
        private string customThresholdText;
        private double customThresholdValue;
        
        public MatrixViewModel Matrix { get; }

        public bool MaxThresholdSelected { get; set; }
        public bool MedianThresholdSelected { get; set; }
        public bool CustomThresholdSelected { get; set; }

        public string CustomThresholdText
        {
            get => customThresholdText;
            set
            {
                if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                    return;
                val = Math.Min(val, 1.0);
                val = Math.Max(val, 0.0);
                customThresholdText = val.ToString(CultureInfo.InvariantCulture);
                customThresholdValue = val;
            }
        }

        public RelayCommand SwitchAllColumnTagsCommand { get; }
        public RelayCommand SwitchAllRowTagsCommand { get; }
        public RelayCommand FindBestRowCommand { get; }
        public RelayCommand ImproveRowCommand { get; }
        public RelayCommand AnalyzeRowCommand { get; }

        public MainWindowViewModel(Matrix matrix)
        {
            Matrix = new MatrixViewModel(matrix);

            MaxThresholdSelected = true;
            CustomThresholdText = "0.5";
            
            SwitchAllColumnTagsCommand = new RelayCommand(_ => Matrix.SwitchAllColumnTags());
            SwitchAllRowTagsCommand = new RelayCommand(_ => Matrix.SwitchAllRowTags());
            FindBestRowCommand = new RelayCommand(_ => OnFindBestRowCommand());
            ImproveRowCommand = new RelayCommand(_ => OnImproveRowCommand());
            AnalyzeRowCommand = new RelayCommand(_ => OnAnalyzeRowCommand());
        }

        private bool TryGetThresholds(out IList<double> thresholds, bool ignoreDisabledRows = false)
        {
            thresholds = null;
            var matrix = Matrix.Matrix;
            if (MaxThresholdSelected)
                thresholds = MatrixMath.GetColumnMaximums(matrix, ignoreDisabledRows);
            else if (MedianThresholdSelected)
                thresholds = MatrixMath.GetColumnMedians(matrix, ignoreDisabledRows);
            else if (CustomThresholdSelected)
                thresholds = Enumerable.Repeat(customThresholdValue, matrix.Width).ToArray();
            return !(thresholds is null);
        }

        private void OnFindBestRowCommand()
        {
            if (!TryGetThresholds(out var thresholds))
                return;
            int result = MatrixMath.FindBestRow(Matrix.Matrix, thresholds);
            MessageBoxHelper.Info("Результат", result == -1 ? "Нет решения" : "Оптимальный набор:\n" +  Matrix.Rows[result].Header);
        }

        private void OnImproveRowCommand()
        {
            if (!TryGetThresholds(out var thresholds, true))
                return;
            int result = MatrixMath.ImproveRow(Matrix.Matrix, thresholds);
            MessageBoxHelper.Info("Результат", result == -1 ? "Нет решения" : "Улучшенный набор:\n" + Matrix.Rows[result].Header);
            if (result == -1)
                return;
            foreach (string tag in Matrix.Rows[result].Tags)
                Matrix.RowTags.First(x => x.Title == tag).IsEnabled = true;
        }

        private void OnAnalyzeRowCommand()
        {
            if (!TryGetThresholds(out var thresholds, true))
                return;
            var results = MatrixMath.AnalyzeRow(Matrix.Matrix, thresholds);
            var message = "Выбранный набор стратегий лучше всего защищает от:\n\n" + string.Join("\n", results.Select(x => Matrix.Columns[x].Header));
            MessageBoxHelper.Info("Результат", results.Count == 0 ? "Выбранный набор стратегий полностью не соответствует заданным критериям" : message);
        }
    }
}

using Strategist.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Strategist.Core;

namespace Strategist.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool MaxThresholdSelected => RbMax.IsChecked.HasValue && RbMax.IsChecked.Value;
        public bool MedianThresholdSelected => RbMedian.IsChecked.HasValue && RbMedian.IsChecked.Value;
        public bool CustomThresholdSelected => RbCustom.IsChecked.HasValue && RbCustom.IsChecked.Value;

        public MainWindow(Matrix matrix)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this, matrix);
            InitializeDataTable();
        }

        public bool TryGetCustomThreshold(out double value, bool suppressErrorMessage = false)
        {
            if (double.TryParse(TbCustomThreshold.Text.Replace('.', ','), out value) && 0.0 <= value && value <= 1.0) 
                return true;
            if (!suppressErrorMessage)
                MessageBoxHelper.Error("Неверный формат данных для критерия оценки");
            return false;
        }

        private void InitializeDataTable()
        {
            var matrix = ((MainWindowViewModel)DataContext).Matrix;
            for (int i = 0; i < matrix.Columns.Length; i++)
            {
                var col = new DataGridTextColumn
                {
                    Header = matrix.Columns[i].Header,
                    Binding = new Binding($"[{i}]")
                };
                BindingOperations.SetBinding(col, DataGridColumn.VisibilityProperty, new Binding
                {
                    Path = new PropertyPath(nameof(MatrixColumnViewModel.IsEnabled)),
                    Source = matrix.Columns[i],
                    Converter = (IValueConverter)Resources[nameof(BooleanToVisibilityConverter)],
                });
                DataGrid.Columns.Add(col);
            }
        }
    }
}

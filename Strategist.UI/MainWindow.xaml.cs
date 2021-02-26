using Strategist.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Strategist.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool MaxThresholdSelected => RbMax.IsChecked.HasValue && RbMax.IsChecked.Value;
        public bool MedianThresholdSelected => RbMedian.IsChecked.HasValue && RbMedian.IsChecked.Value;
        public bool CustomThresholdSelected => RbCustom.IsChecked.HasValue && RbCustom.IsChecked.Value;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
            InitializeDataTable();
        }

        public bool TryGetCustomThreshold(out double value, bool suppressErrorMessage = false)
        {
            if (double.TryParse(TbCustomThreshold.Text.Replace('.', ','), out value) && 0.0 <= value && value <= 1.0) 
                return true;
            if (!suppressErrorMessage)
                ShowError("Неверный формат данных для критерия оценки");
            return false;
        }

        public void ShowMessage(string header, string content)
        {
            MessageBox.Show(content, header, MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void ShowError(string content) => MessageBox.Show(content, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

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
        // Public Properties //
        
        public bool MaxThresholdSelected => rb_max.IsChecked.Value;
        public bool MedianThresholdSelected => rb_median.IsChecked.Value;
        public bool CustomThresholdSelected => rb_custom.IsChecked.Value;

        // Constructor //

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
            InitializeDataTable();
        }

        // Public Methods // 

        public bool TryGetCustomThreshold(out double value, bool suppressErrorMessage = false)
        {
            if (!double.TryParse(tb_customThreshold.Text.Replace('.', ','), out value) || value < 0.0 || value > 1.0)
            {
                if (!suppressErrorMessage)
                {
                    ShowError("Неверный формат данных для критерия оценки");
                }
                return false;
            }
            return true;
        }

        public void ShowMessage(string header, string content)
        {
            MessageBox.Show(content, header, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Private Methods //

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
                BindingOperations.SetBinding(col, DataGridTextColumn.VisibilityProperty, new Binding
                {
                    Path = new PropertyPath(nameof(MatrixColumnViewModel.IsEnabled)),
                    Source = matrix.Columns[i],
                    Converter = (IValueConverter)Resources[nameof(BooleanToVisibilityConverter)],
                });
                dataGrid.Columns.Add(col);
            }
        }

        private void ShowError(string content) => MessageBox.Show(content, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

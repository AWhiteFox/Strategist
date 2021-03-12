using Strategist.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Strategist.Core;

namespace Strategist.UI
{
    public partial class MainWindow
    {
        public MainWindow(Matrix matrix)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(matrix);
            InitializeDataTable();
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

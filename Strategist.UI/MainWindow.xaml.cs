using Strategist.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Matrix = Strategist.Core.Matrix;

namespace Strategist.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MatrixViewModel Matrix { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Matrix = new MatrixViewModel();
            CreateColumns();
        }

        // Private Methods //

        private void CreateColumns()
        {
            for (int i = 0; i < Matrix.Columns.Length; i++)
            {
                var col = new DataGridTextColumn
                {
                    Header = Matrix.Columns[i].Header,
                    Binding = new Binding($"[{i}]")
                };
                BindingOperations.SetBinding(col, DataGridTextColumn.VisibilityProperty, new Binding
                {
                    Path = new PropertyPath(nameof(MatrixColumnViewModel.IsEnabled)),
                    Source = Matrix.Columns[i],
                    Converter = (IValueConverter)Resources[nameof(BooleanToVisibilityConverter)],
                });
                dataGrid.Columns.Add(col);
            }
        }
    }
}

using Strategist.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public Matrix Matrix { get; private set; }
        public DataTable MatrixAsDataTable { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Matrix = Matrix.GetRandom();
            PopulateDataGrid();
        }

        // Private Methods //

        private void PopulateDataGrid()
        {
            for (int i = 0; i < Matrix.ColumnHeaders.Length; i++)
            {
                var col = new DataGridTextColumn
                {
                    Header = Matrix.ColumnHeaders[i].Title,
                    Binding = new Binding($"[{i}]")
                };
                BindingOperations.SetBinding(col, DataGridTextColumn.VisibilityProperty, new Binding
                {
                    Path = new PropertyPath($"Enabled"),
                    Source = Matrix.ColumnHeaders[i],
                    Converter = (IValueConverter)this.Resources["BooleanToVisibilityConverter"],
                });
                dataGrid.Columns.Add(col);
            }
        }

        private bool CheckSelection()
        {
            if (Matrix.ColumnHeaders.Count(x => x.Enabled) == 0)
            {
                ShowError("Выберите хотя бы одно средство защиты");
                return false;
            }
            if (MatrixAsDataTable.Rows.Count == 0)
            {
                ShowError("Выберите хотя бы одну угрозу");
                return false;
            }
            return true;
        }

        private void ShowSolutions(List<int> solutions)
        {
            throw new NotImplementedException();
            //for (int i = 0; i < Matrix.Columns.Length; i++)
            //{
            //    var col = Matrix.Columns[i];
            //    bool b = solutions.Contains(i);

            //    col.Enabled = b;
            //    SetColumnVisibility(col.Header, b);
            //}
        }

        private void ShowError(string content)
        {
            MessageBox.Show(content, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // UI Events //

        private void ButtonProbability_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(thresholdTextBox.Text.Replace('.', ','), out double val) || val < 0f || val > 1f)
            {
                ShowError("Неверный формат данных для минимальной вероятности защиты");
                return;
            }

            if (!CheckSelection())
                return;

            if (MatrixMath.TryFindByProbability(Matrix, val, out List<int> results))
                ShowSolutions(results);
            else
                ShowError("Решение невозможно");
        }

        private void ButtonBest_Click(object sender, RoutedEventArgs e)
        {
            if (CheckSelection())
                ShowSolutions(MatrixMath.FindBest(Matrix));
        }

        private void ButtonAllColumns_Click(object sender, RoutedEventArgs e)
        {
            var value = !Matrix.ColumnHeaders[0].Enabled;
            for (int i = Matrix.ColumnHeaders.Length - 1; i >= 0; i--)
            {
                Matrix.ColumnHeaders[i].Enabled = value;
            }
        }

        private void ButtonAllRows_Click(object sender, RoutedEventArgs e)
        {
            var value = !Matrix.RowHeaders[0].Enabled;
            for (int i = Matrix.RowHeaders.Length - 1; i >= 0; i--)
            {
                Matrix.RowHeaders[i].Enabled = value;
            }
        }
    }
}

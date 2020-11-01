using Strategist.Core;
using Strategist.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Matrix = Strategist.Core.Matrix;

namespace Strategist.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int columnsSelected;

        public Matrix Matrix { get; private set; }
        public DataTable MatrixAsDataTable { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            GenerateRandomMatrix();
            LoadDataTable();
        }

        // Private Methods //

        private void GenerateRandomMatrix()
        {
            Matrix = new Matrix(4, 4);
            var rnd = new Random();

            Matrix.Columns.Fill((i) => new MatrixColumnRowData
            {
                Header = $"Средство {i}",
                Enabled = true
            });
            Matrix.Rows.Fill((i) => new MatrixColumnRowData
            {
                Header = $"Угроза {i}",
                Enabled = true
            });

            for (int j = 0; j < Matrix.Rows.Length; j++)
            {
                for (int i = 0; i < Matrix.Columns.Length; i++)
                {
                    Matrix.Values[i, j] = (float)Math.Round(rnd.NextDouble(), 2);
                }
            }
        }

        private void LoadDataTable()
        {
            MatrixAsDataTable = new DataTable();
            MatrixAsDataTable.Columns.Add("Угроза", typeof(string));
            for (int i = 0; i < Matrix.Columns.Length; i++)
            {
                MatrixAsDataTable.Columns.Add(Matrix.Columns[i].Header, typeof(float));
            }
            for (int j = 0; j < Matrix.Rows.Length; j++)
            {
                LoadRow(j);
            }
            columnsSelected = Matrix.Columns.Length;
        }

        private void LoadRow(int rowIndex)
        {
            DataRow row = MatrixAsDataTable.NewRow();
            row[0] = Matrix.Rows[rowIndex].Header;
            for (int i = 0; i < Matrix.Columns.Length; i++)
            {
                row[i + 1] = Matrix.Values[i, rowIndex];
            }
            MatrixAsDataTable.Rows.Add(row);
        }

        private bool CheckSelection()
        {
            if (columnsSelected == 0)
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

        private void SetColumnVisibility(string header, bool visible)
        {
            int i = Matrix.Columns.FindIndex(x => x.Header == header) + 1;
            if (visible)
            {
                if (dataGrid.Columns[i].Visibility != Visibility.Visible)
                {
                    dataGrid.Columns[i].Visibility = Visibility.Visible;
                    columnsSelected++;
                }
            }
            else
            {
                if (dataGrid.Columns[i].Visibility == Visibility.Visible)
                {
                    dataGrid.Columns[i].Visibility = Visibility.Collapsed;
                    columnsSelected--;
                }
            }
        }

        private void SetRowVisibility(string header, bool visible)
        {
            for (int j = 0; j < dataGrid.Items.Count; j++)
            {
                if ((string)MatrixAsDataTable.Rows[j][0] == header)
                {
                    if (!visible)
                    {
                        MatrixAsDataTable.Rows.RemoveAt(j);
                    }
                    return;
                }
            }
            LoadRow(Matrix.Rows.FindIndex(x => x.Header == header));
        }

        private void ShowSolutions(List<int> solutions)
        {
            for (int i = 0; i < Matrix.Columns.Length; i++)
            {
                var col = Matrix.Columns[i];
                bool b = solutions.Contains(i);

                col.Enabled = b;
                SetColumnVisibility(col.Header, b);
            }
        }

        private void ShowError(string content)
        {
            MessageBox.Show(content, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // UI Events //

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.Columns[0].Width = DataGridLength.Auto;
            MatrixAsDataTable.DefaultView.Sort = MatrixAsDataTable.Columns[0].ColumnName;
        }

        private void ColumnCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox)sender;
            SetColumnVisibility((string)cb.Content, cb.IsChecked.Value);
        }

        private void RowCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox)sender;
            SetRowVisibility((string)cb.Content, cb.IsChecked.Value);
        }

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
    }
}

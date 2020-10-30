using Strategist.Core;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Strategist.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int columnsSelected;
        private Brush thresoldTextBoxDefaultBorderBrush;

        public Matrix<float> Matrix { get; private set; }
        public DataTable MatrixAsDataTable { get; private set; }
        public float? Threshold { get; set; } = 0.9f;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            GenerateRandomMatrix();
            LoadDataTable();
        }

        /// <summary>
        /// Fills <see cref="Matrix"/> with random values
        /// </summary>
        private void GenerateRandomMatrix()
        {
            Matrix = new Matrix<float>(8, 8);
            var rnd = new Random();

            Matrix.Columns.Fill((i) => new MatrixColumnRowData
            {
                Header = $"Угроза {i}",
                Enabled = true
            });
            Matrix.Rows.Fill((i) => new MatrixColumnRowData
            {
                Header = $"Средство защиты {i}",
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

        /// <summary>
        /// Loads matrix into <see cref="MatrixAsDataTable"/>
        /// </summary>
        private void LoadDataTable()
        {
            MatrixAsDataTable = new DataTable();
            MatrixAsDataTable.Columns.Add("Средство защиты", typeof(string));
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

        /// <summary>
        /// Reads row with index <paramref name="rowIndex"/> from matrix and loads it into <see cref="MatrixAsDataTable"/>
        /// </summary>
        /// <param name="rowIndex"></param>
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

        /// <summary>
        /// Called when this window is loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.Columns[0].Width = DataGridLength.Auto;
            MatrixAsDataTable.DefaultView.Sort = MatrixAsDataTable.Columns[0].ColumnName;
        }

        /// <summary>
        /// Shows or collapses columns when CheckBox is clicked
        /// </summary>
        private void ColumnCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox)sender;
            var header = (string)cb.Content;
            int i = Matrix.Columns.FindIndex(x => x.Header == header) + 1;
            if (cb.IsChecked.Value)
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

        /// <summary>
        /// Shows or collapses rows when CheckBox is cliked
        /// </summary>
        private void RowCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox)sender;
            var header = (string)cb.Content;

            if (cb.IsChecked.Value)
            {
                LoadRow(Matrix.Rows.FindIndex(x => x.Header == header));
            }
            else
            {
                for (int j = 0; j < dataGrid.Items.Count; j++)
                {
                    if ((string)MatrixAsDataTable.Rows[j][0] == header)
                    {
                        MatrixAsDataTable.Rows.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Called when Button is cliked
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Threshold is null)
            {
                MessageBox.Show("Неверный формат данных для минимальной вероятности защиты",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            if (columnsSelected == 0)
            {
                MessageBox.Show("Выберите хотя бы одну угрозу",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            if (MatrixAsDataTable.Rows.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одно средство защиты",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("ok"); // TODO: Write actual code
        }

        /// <summary>
        /// Checks if text in ThresholdTextBox can be pasrsed into float and if it is in the required range
        /// </summary>
        private void ThresholdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // TODO: Consider creating deparate control for this
            var tb = (TextBox)sender;
            if (thresoldTextBoxDefaultBorderBrush is null)
            {
                thresoldTextBoxDefaultBorderBrush = tb.BorderBrush;
            }
            if (float.TryParse(tb.Text.Replace('.', ','), out float val) && 0f <= val && val <= 1f)
            {
                tb.BorderBrush = thresoldTextBoxDefaultBorderBrush;
                Threshold = val;
            }
            else
            {
                tb.BorderBrush = Brushes.Red;
                Threshold = null;
            }
        }
    }
}

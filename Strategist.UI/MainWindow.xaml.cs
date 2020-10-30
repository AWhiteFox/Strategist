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
        private readonly Matrix matrix = Matrix.Random(8, 8);
        private int columnsSelected;
        private Brush thresoldTextBoxDefaultBorderBrush;

        public float? Threshold { get; set; } = 0.9f;
        public DataTable MatrixAsDataTable { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            LoadDataTable();
            GenerateCheckBoxes();
        }

        /// <summary>
        /// Generates <see cref="CheckBox"/>es for enabling and didabling columns and rows
        /// </summary>
        private void GenerateCheckBoxes()
        {
            for (int i = 0; i < matrix.Columns.Length; i++)
            {
                var cb = new CheckBox
                {
                    Content = matrix.Columns[i].Header,
                    Tag = i,
                    IsChecked = true
                };
                cb.Click += ColumnCheckBox_Click;
                columnStackPanel.Children.Add(cb);
            }
            for (int i = 0; i < matrix.Rows.Length; i++)
            {
                var cb = new CheckBox
                {
                    Content = matrix.Rows[i].Header,
                    Tag = i,
                    IsChecked = true
                };
                cb.Click += RowCheckBox_Click;
                rowStackPanel.Children.Add(cb);
            }
        }

        /// <summary>
        /// Loads matrix into <see cref="MatrixAsDataTable"/>
        /// </summary>
        private void LoadDataTable()
        {
            MatrixAsDataTable = new DataTable();
            MatrixAsDataTable.Columns.Add("Средство защиты", typeof(string));
            for (int i = 0; i < matrix.Columns.Length; i++)
            {
                MatrixAsDataTable.Columns.Add(matrix.Columns[i].Header, typeof(float));
            }
            for (int j = 0; j < matrix.Rows.Length; j++)
            {
                LoadRow(j);
            }
            columnsSelected = matrix.Columns.Length;
        }

        /// <summary>
        /// Reads row with index <paramref name="rowIndex"/> from matrix and loads it into <see cref="MatrixAsDataTable"/>
        /// </summary>
        /// <param name="rowIndex"></param>
        private void LoadRow(int rowIndex)
        {
            DataRow row = MatrixAsDataTable.NewRow();
            row[0] = matrix.Rows[rowIndex].Header;
            for (int i = 0; i < matrix.Columns.Length; i++)
            {
                row[i + 1] = matrix.Values[i, rowIndex];
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
            int i = (int)cb.Tag + 1;
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
            int i = (int)cb.Tag;

            if (cb.IsChecked.Value)
            {
                LoadRow(i);
            }
            else
            {
                for (int j = 0; j < dataGrid.Items.Count; j++)
                {
                    if ((string)MatrixAsDataTable.Rows[j][0] == matrix.Rows[i].Header)
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

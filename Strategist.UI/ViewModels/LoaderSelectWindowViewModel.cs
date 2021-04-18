using System;
using System.Windows;
using Strategist.Core;
using Strategist.Loaders;

namespace Strategist.UI.ViewModels
{
    public class LoaderSelectWindowViewModel
    {
        public bool RandomLoaderSelected { get; set; }
        public bool MongoDbLoaderSelected { get; set; }
        public bool JsonLoaderSelected { get; set; }
        public bool CsvLoaderSelected { get; set; }

        public string RandomStrategyCount { get; set; }
        public string RandomCounterStrategyCount { get; set; }
        public string MongoDbConnectionString { get; set; }
        public string MongoDbDatabaseName { get; set; }
        public string CsvSeparator { get; set; }

        public RelayCommand LoadMatrixCommand { get; }
        
        public LoaderSelectWindowViewModel()
        {
            RandomLoaderSelected = true;
            RandomStrategyCount = "4";
            RandomCounterStrategyCount = "6";
            LoadMatrixCommand = new RelayCommand(_ => OnLoadMatrixCommand());
        }

        private void OnLoadMatrixCommand()
        {
            MatrixLoader loader;
            if (RandomLoaderSelected)
                loader = GetRandomMatrixLoader();
            else if (MongoDbLoaderSelected)
                loader = GetMongoDbMatrixLoader();
            else if (JsonLoaderSelected)
                loader = GetJsonMatrixLoader();
            else if (CsvLoaderSelected)
                loader = GetCsvMatrixLoader();
            else
                return;
            if (loader is null)
                return;

            Matrix matrix;
            try
            {
                matrix = loader.Load();
            }
            catch (Exception e)
            {
                MessageBoxHelper.Error(e.Message);
                return;
            }
            
            var current = Application.Current.MainWindow;
            Application.Current.MainWindow = new MainWindow(matrix);
            current?.Close();
            Application.Current.MainWindow.Show();
        }

        private RandomMatrixLoader GetRandomMatrixLoader()
        {
            if (int.TryParse(RandomStrategyCount, out int height) && int.TryParse(RandomCounterStrategyCount, out int width))
            {
                return new RandomMatrixLoader(width, height);
            }

            MessageBoxHelper.Error("Не удалось преобразовать введенные значения в числа.");
            return null;
        }

        private MongoDBMatrixLoader GetMongoDbMatrixLoader()
        {
            return new MongoDBMatrixLoader(MongoDbConnectionString, MongoDbDatabaseName);
        }

        private JsonMatrixLoader GetJsonMatrixLoader()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".json", 
                Filter = "JSON Files|*.json"
            };
            bool? result = dlg.ShowDialog();
            return result.HasValue && result.Value ? new JsonMatrixLoader(dlg.FileName) : null;
        }
        
        private CsvMatrixLoader GetCsvMatrixLoader()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".csv", 
                Filter = "CSV Files|*.csv"
            };
            bool? result = dlg.ShowDialog();
            return result.HasValue && result.Value ? new CsvMatrixLoader(dlg.FileName, CsvSeparator) : null;
        }
    }
}
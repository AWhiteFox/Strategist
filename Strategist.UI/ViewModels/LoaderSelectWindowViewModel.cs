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

        public string RandomStrategyCount { get; set; }
        public string RandomCounterStrategyCount { get; set; }
        public string MongoDbConnectionString { get; set; }
        public string MongoDbDatabaseName { get; set; }

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

        private MatrixLoader GetRandomMatrixLoader()
        {
            if (int.TryParse(RandomStrategyCount, out int height) && int.TryParse(RandomCounterStrategyCount, out int width))
            {
                return new RandomMatrixLoader(width, height);
            }

            MessageBoxHelper.Error("Не удалось преобразовать введенные значения в числа.");
            return null;
        }

        private MatrixLoader GetMongoDbMatrixLoader()
        {
            return new MongoDBMatrixLoader(MongoDbConnectionString, MongoDbDatabaseName);
        }
    }
}
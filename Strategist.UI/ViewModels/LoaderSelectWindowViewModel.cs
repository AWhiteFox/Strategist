using System;

namespace Strategist.UI.ViewModels
{
    public class LoaderSelectWindowViewModel
    {
        public bool RandomLoaderSelected { get; set; } = true;
        public bool MongoDbLoaderSelected { get; set; }
        public bool JsonLoaderSelected { get; set; }
        public bool CsvLoaderSelected { get; set; }

        public string RandomStrategyCount { get; set; }
        public string RandomCounterStrategyCount { get; set; }
        public string MongoDbConnectionString { get; set; }
        public string MongoDbDatabaseName { get; set; }
        public string JsonFilePath { get; set; }
        public string CsvFilePath { get; set; }

        public RelayCommand LoadMatrixCommand { get; }
        
        public LoaderSelectWindowViewModel()
        {
            LoadMatrixCommand = new RelayCommand(_ => OnLoadMatrixCommand());
        }

        private void OnLoadMatrixCommand()
        {
            throw new NotImplementedException();
        }
    }
}
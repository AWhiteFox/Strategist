using Strategist.Core;

namespace Strategist.Loaders
{
    public class CsvMatrixLoader : MatrixLoader
    {
        private string pathToFile;
        private string separator;
        
        public CsvMatrixLoader(string pathToFile, string separator)
        {
            this.pathToFile = pathToFile;
            this.separator = separator;
        }
        
        public override Matrix Load()
        {
            throw new System.NotImplementedException();
        }
    }
}
using Strategist.Core;

namespace Strategist.Loaders
{
    public class JsonMatrixLoader : MatrixLoader
    {
        private string pathToFile;

        public JsonMatrixLoader(string pathToFile)
        {
            this.pathToFile = pathToFile;
        }
        
        public override Matrix Load()
        {
            throw new System.NotImplementedException();
        }
    }
}
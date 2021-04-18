using System.Globalization;
using System.IO;
using Strategist.Core;

namespace Strategist.Loaders
{
    public class CsvMatrixLoader : MatrixLoader
    {
        private readonly string pathToFile;
        private readonly char valueSeparator;
        private readonly char headerSeparator;
        
        public CsvMatrixLoader(string pathToFile, char valueSeparator, char headerSeparator)
        {
            this.pathToFile = pathToFile;
            this.valueSeparator = valueSeparator;
            this.headerSeparator = headerSeparator;
        }
        
        public override Matrix Load()
        {
            var matrix = new Matrix();
            
            string[] lines = File.ReadAllLines(pathToFile);
            foreach (string columnHeader in lines[0].Split(valueSeparator))
            {
                matrix.AddColumn(columnHeader.Split(headerSeparator));
            }

            for (var i = 1; i < lines.Length; i++)
            {
                string row = lines[i];
                string[] columns = row.Split(valueSeparator);
                matrix.AddRow(columns[0].Split(headerSeparator));
                for (int j = 1; j < columns.Length; j++)
                {
                    matrix[j - 1, i - 1] = double.Parse(columns[j], NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            }

            return matrix;
        }
    }
}
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Strategist.Core;
using Strategist.Core.Extensions;

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
            var document = JsonConvert.DeserializeObject<Document>(File.ReadAllText(pathToFile));
            if (document is null)
                throw new ArgumentNullException(nameof(pathToFile));

            var matrix = new Matrix();
            
            foreach (var strategies in document.Strategies.Combinations())
                matrix.AddRow(strategies);
            foreach (var counterStrategies in document.CounterStrategies.Combinations())
                matrix.AddColumn(counterStrategies);

            foreach (var test in document.Tests)
            {
                var rows = test.Strategies.Select(x => document.Strategies[x]);
                var columns = test.CounterStrategies.Select(x => document.CounterStrategies[x]);
                matrix[columns, rows] = test.SuccessProbability;
            }

            return matrix;
        }

        private class Document
        {
            [JsonProperty("strategies")]
            public string[] Strategies { get; set; }
            
            [JsonProperty("counter_strategies")]
            public string[] CounterStrategies { get; set; }
            
            [JsonProperty("tests")]
            public StrategyTest[] Tests { get; set; }
        }

        private class StrategyTest
        {
            [JsonProperty("strategies")]
            public int[] Strategies { get; set; }
            
            [JsonProperty("counter_strategies")]
            public int[] CounterStrategies { get; set; }
            
            [JsonProperty("success_probability")]
            public double SuccessProbability { get; set; }
        }
    }
}
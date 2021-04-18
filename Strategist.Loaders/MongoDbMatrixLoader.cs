using System.Collections.Generic;
using Strategist.Core.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Strategist.Core;

namespace Strategist.Loaders
{
    public class MongoDbMatrixLoader : MatrixLoader
    {
        private string connectionString;
        private string databaseName;
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<StrategyModel> counterStrategiesCollection;
        private IMongoCollection<StrategyModel> strategiesCollection;
        private IMongoCollection<ProbabilityModel> probabilitiesCollection;
        private readonly List<StrategyModel> counterStrategies = new List<StrategyModel>();
        private readonly List<StrategyModel> strategies = new List<StrategyModel>();
        private readonly List<ProbabilityModel> probabilities = new List<ProbabilityModel>();

        public Matrix MainMatrix { get; private set; }

        public MongoDbMatrixLoader(string connectionString, string databaseName)
        {
            MainMatrix = new Matrix();
            this.connectionString = connectionString;
            this.databaseName = databaseName;
        }

        public override Matrix Load()
        {
            GetDatabase(connectionString, databaseName);
            return GenerateMatrix();
        }

        public void UpdateDocuments()
        {
            counterStrategies.Clear();
            strategies.Clear();
            probabilities.Clear();
            ExtractDocuments();
        }

        public void GetDatabase(string connectionString, string databaseName)
        {
            this.connectionString = connectionString;
            this.databaseName = databaseName;
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            counterStrategiesCollection = database.GetCollection<StrategyModel>("counter_strategies");
            strategiesCollection = database.GetCollection<StrategyModel>("strategies");
            probabilitiesCollection = database.GetCollection<ProbabilityModel>("probabilities");

            UpdateDocuments();
        }

        private Matrix GenerateMatrix()
        {
            var namesStrategies = new string[strategies.Count];
            var namesCounterStrategies = new string[counterStrategies.Count];

            for (int i = 0; i < namesStrategies.Length; i++)
                namesStrategies[i] = strategies[i].Name;

            for (int i = 0; i < namesCounterStrategies.Length; i++)
                namesCounterStrategies[i] = counterStrategies[i].Name;

            var matrixStrategies = namesStrategies.Combinations();
            var matrixCounterStrategies = namesCounterStrategies.Combinations();

            foreach (var row in matrixStrategies)
                MainMatrix.AddRow(row);

            foreach (var column in matrixCounterStrategies)
                MainMatrix.AddColumn(column);

            var fromProbabilitiesStrategies = new List<string>();
            var fromProbabilitiesCounterStrategies = new List<string>();

            for (int i = 0; i < probabilities.Count; i++)
            {
                foreach (var strategy in probabilities[i].Strategies)
                    fromProbabilitiesStrategies.Add(FindStrategyName(strategies, strategy));

                foreach (var strategy in probabilities[i].CounterStrategies)
                    fromProbabilitiesCounterStrategies.Add(FindStrategyName(counterStrategies, strategy));

                foreach (var row in matrixStrategies)
                    foreach (var column in matrixCounterStrategies)
                        if (new HashSet<string>(column).SetEquals(fromProbabilitiesCounterStrategies) && new HashSet<string>(row).SetEquals(fromProbabilitiesStrategies))
                            MainMatrix[fromProbabilitiesCounterStrategies, fromProbabilitiesStrategies] = probabilities[i].Probability;

                fromProbabilitiesStrategies.Clear();
                fromProbabilitiesCounterStrategies.Clear();
            }

            return MainMatrix;
        }

        private string FindStrategyName(List<StrategyModel> strategies, ObjectId id)
        {
            for (int i = 0; i < strategies.Count; i++)
            {
                if (strategies[i].Id == id)
                    return strategies[i].Name;
            }
            return null;
        }

        private void ExtractDocuments()
        {
            var strategiesFilter = Builders<StrategyModel>.Filter.Empty;
            var probabilitiesFilter = Builders<ProbabilityModel>.Filter.Empty;
            var docs = counterStrategiesCollection.Find(strategiesFilter).ToList();
            foreach (StrategyModel doc in docs)
            {
                counterStrategies.Add(doc);
            }
            docs = strategiesCollection.Find(strategiesFilter).ToList();
            foreach (StrategyModel doc in docs)
            {
                strategies.Add(doc);
            }
            var probDocs = probabilitiesCollection.Find(probabilitiesFilter).ToList();
            foreach (ProbabilityModel doc in probDocs)
            {
                probabilities.Add(doc);
            }
        }

        private class StrategyModel
        {
            [BsonElement("_id")]
            public ObjectId Id { get; set; }
            [BsonElement("name")]
            public string Name { get; set; }
        }

        private class ProbabilityModel
        {
            [BsonElement("_id")]
            public ObjectId Id { get; set; }
            [BsonElement("strategies")]
            public ObjectId[] Strategies { get; set; }
            [BsonElement("counter_strategies")]
            public ObjectId[] CounterStrategies { get; set; }
            [BsonElement("probability")]
            public double Probability { get; set; }
        }
    }
}
using System.Collections.Generic;
using Strategist.Core.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Strategist.Core.MatrixLoaders
{
    public class MongoDBMatrixLoader
    {
        MongoClient client;
        IMongoDatabase database;
        public Matrix MainMatrix { get; private set; }

        IMongoCollection<StrategyModel> counterStrategiesCollection;
        IMongoCollection<StrategyModel> strategiesCollection;
        IMongoCollection<ProbabilityModel> probabilitiesCollection;

        List<StrategyModel> counterStrategies = new List<StrategyModel>();
        List<StrategyModel> strategies = new List<StrategyModel>();
        List<ProbabilityModel> probabilities = new List<ProbabilityModel>();

        public MongoDBMatrixLoader(string connectionString, string databaseName)
        {
            MainMatrix = new Matrix();
            GetDatabase(connectionString, databaseName);
        }

        public Matrix GenerateMatrix()
        {
            string[] namesStrategies = new string[strategies.Count];
            string[] namesСounterStrategies = new string[counterStrategies.Count];

            for (int i = 0; i < namesStrategies.Length; i++)
                namesStrategies[i] = strategies[i].Name;

            for (int i = 0; i < namesСounterStrategies.Length; i++)
                namesСounterStrategies[i] = counterStrategies[i].Name;

            foreach (var row in namesStrategies.Combinations())
                MainMatrix.AddRow(row.ToArray());

            foreach (var row in namesСounterStrategies.Combinations())
                MainMatrix.AddColumn(row.ToArray());


            /*for (int i = 0; i < probabilities.Count; i++)
            {
                for (int j = 0; j < probabilities[i].CounterStrategies.Length; j++)
                {
                    FindStrategyName(counterStrategies, probabilities[i].CounterStrategies[j]);

                }
            }*/

            return MainMatrix;
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
            //Console.WriteLine("Подключение...");
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            //Console.WriteLine("Соединение установленно!\n\nПолучение коллекций...");
            counterStrategiesCollection = database.GetCollection<StrategyModel>("counter_strategies");
            strategiesCollection = database.GetCollection<StrategyModel>("strategies");
            probabilitiesCollection = database.GetCollection<ProbabilityModel>("probabilities");
            //Console.WriteLine("Коллекции извлечены!\n");

            UpdateDocuments();
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
            //Console.WriteLine("Извлечение документов...");
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
            //Console.WriteLine("Документы извлечены!");
        }
    }

    internal class StrategyModel
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
    }
    internal class ProbabilityModel
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
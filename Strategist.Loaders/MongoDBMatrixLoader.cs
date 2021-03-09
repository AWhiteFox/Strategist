using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Strategist.Core.MatrixLoaders
{
    public class MongoDBMatrixLoader
    {
        MongoClient client;
        IMongoDatabase database;
        Matrix matrix;

        IMongoCollection<StrategyModel> counter_strategiesCollection;
        IMongoCollection<StrategyModel> strategiesCollection;
        IMongoCollection<ProbabilityModel> probabilitiesCollection;

        List<StrategyModel> counter_strategies = new List<StrategyModel>();
        List<StrategyModel> strategies = new List<StrategyModel>();
        List<ProbabilityModel> probabilities = new List<ProbabilityModel>();

        public MongoDBMatrixLoader(string connectionString, string databaseName)
        {
            matrix = new Matrix();
            GetDatabase(connectionString, databaseName);
        }

        public void GenerateMatrix()
        {
            for (int i = 0; i < probabilities.Count; i++)
            {
                for (int j = 0; j < probabilities[i].CounterStrategies.Length; j++)
                {
                    //FindStrategyName(counter_strategies, probabilities[i].CounterStrategies[j]);
                }
            }
        }

        public void UpdateDocuments()
        {
            counter_strategies.Clear();
            strategies.Clear();
            probabilities.Clear();
            ExtractDocuments().GetAwaiter().GetResult();
        }

        public void GetDatabase(string connectionString, string databaseName)
        {
            //Console.WriteLine("Подключение...");
            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            //Console.WriteLine("Соединение установленно!\n\nПолучение коллекций...");
            counter_strategiesCollection = database.GetCollection<StrategyModel>("counter_strategies");
            strategiesCollection = database.GetCollection<StrategyModel>("strategies");
            probabilitiesCollection = database.GetCollection<ProbabilityModel>("probabilities");
            //Console.WriteLine("Коллекции извлечены!\n");
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

        private async Task ExtractDocuments()
        {
            //Console.WriteLine("Извлечение документов...");
            var filter = new BsonDocument();
            using (var cursor = await counter_strategiesCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var cs = cursor.Current;
                    foreach (StrategyModel doc in cs)
                    {
                        counter_strategies.Add(doc);
                    }
                }
            }
            using (var cursor = await strategiesCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var s = cursor.Current;
                    foreach (StrategyModel doc in s)
                    {
                        strategies.Add(doc);
                    }
                }
            }
            using (var cursor = await probabilitiesCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var prob = cursor.Current;
                    foreach (ProbabilityModel doc in prob)
                    {
                        probabilities.Add(doc);
                    }
                }
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
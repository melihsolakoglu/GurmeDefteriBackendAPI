using MongoDB.Bson;
using MongoDB.Driver;
using System;
using GurmeDefteriBackEndAPI.Models;

namespace GurmeDefteriBackEndAPI.DatabaseContext
{
    public class Database
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _collectionPerson;
        private readonly IMongoCollection<Food> _collectionFood;
        private readonly IMongoCollection<ScoredFoods> _collectionScoredFoods;

        public Database()
        {
            string connectionString = "mongodb://gurme:12345p@20.81.205.102:27017";
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("gurmedefteri");
            _collectionPerson = _database.GetCollection<User>("users");
            _collectionFood = _database.GetCollection<Food>("foods");
            _collectionScoredFoods = _database.GetCollection<ScoredFoods>("scored_foods");
        }

        public IMongoCollection<User> CollectionPerson => _collectionPerson;
        public IMongoCollection<Food> CollectionFood => _collectionFood;
        public IMongoCollection<ScoredFoods> CollectionScoredFoods => _collectionScoredFoods;
    }
}

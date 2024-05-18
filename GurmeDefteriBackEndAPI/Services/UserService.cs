﻿
using GurmeDefteriBackEndAPI.DatabaseContext;
using GurmeDefteriBackEndAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using GurmeDefteriBackEndAPI.Models.Dto;

namespace GurmeDefteriBackEndAPI.Services
{
    public class UserService
    {
        private readonly Database _database;

        public UserService()
        {
            _database = new Database();
        }
        public User GetUserById(string userId)
        {
            var user = _database.CollectionPerson.Find(u => u.Id == new ObjectId(userId)).FirstOrDefault();
            return user;
        }

        public User GetUserByMail(string mail)
        {
            var user = _database.CollectionPerson.Find(u => u.Email == mail).FirstOrDefault();
            return user;
        }
        public User GetUserWithId(string id)
        {
            var objectId = new ObjectId(id);
            var user = _database.CollectionPerson.Find(u => u.Id == objectId).FirstOrDefault();
            return user;
        }

        public void UpdateUser(UserAPI updatedUser)
        {
            var objectId = new ObjectId(updatedUser.Id);
            var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            var update = Builders<User>.Update
                .Set(u => u.Name, updatedUser.Name)
                .Set(u => u.Age, updatedUser.Age)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.Password, updatedUser.Password)
                .Set(u => u.Role, updatedUser.Role);

            _database.CollectionPerson.UpdateOne(filter, update);
        }

        public void DeleteUser(string userId)
        {
            try
            {
                var objectId = new ObjectId(userId);
                var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
                _database.CollectionPerson.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddUser(User newUser)
        {
            try
            {
                _database.CollectionPerson.InsertOne(newUser);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public List<Food> GetFoods()
        {
            var _foods = _database.CollectionFood;
            return _foods.Find(food => true).ToList();
        }
        public int GetFoodCount()
        {
            var foodCount = _database.CollectionFood.CountDocuments(new BsonDocument());
            int documentCountInt = Convert.ToInt32(foodCount);
            return documentCountInt;
        }
        public int GetFoodCountByName(string name)
        {
            FilterDefinition<Food> filter = Builders<Food>.Filter.Regex("Name", new BsonRegularExpression(name, "i"));

            var foodCount = _database.CollectionFood.CountDocuments(filter);
            int documentCountInt = Convert.ToInt32(foodCount);
            return documentCountInt;
        }
        //Daha sonra bu fonksiyonları kullanabilirim

        public List<ScoredFoods> GetScoredFoodsByUserId(string userId)
        {
            var objectId = new ObjectId(userId);
            var filter = Builders<ScoredFoods>.Filter.Eq(s => s.UserId, userId);

            return _database.CollectionScoredFoods.Find(filter).ToList();
        }

        public List<ScoredFoods> GetScoredFoodsByFoodId(string foodId)
        {
            var objectId = new ObjectId(foodId);
            var filter = Builders<ScoredFoods>.Filter.Eq(s => s.FoodId, foodId);

            return _database.CollectionScoredFoods.Find(filter).ToList();
        }
        public List<Food> FoodCategoryFilter(string category)
        {
            var filter = Builders<Food>.Filter.Eq(f => f.Category, category);
            var foods = _database.CollectionFood.Find(filter).ToList();
            return foods;
        }
        public List<Food> SearchFoodsByTerm(string term)
        {
            var filter = Builders<Food>.Filter.Regex("Name", new BsonRegularExpression(term, "i"));
            var foods = _database.CollectionFood.Find(filter).ToList();
            return foods;
        }
        public List<Food> SearchFoodsByTermAndCategory(string term, string category)
        {
            var filter = Builders<Food>.Filter.Regex("Name", new BsonRegularExpression(term, "i")) & Builders<Food>.Filter.Eq("Category", category);
            var foods = _database.CollectionFood.Find(filter).ToList();
            return foods;
        }


        //public List<Food> GetUnscoredFoodsByUserId(string userId)
        //{
        //    var scoredFoodIds = _database.CollectionScoredFoods.Find(sf => sf.UserId == userId)
        //                                            .Project(sf => ObjectId.Parse(sf.FoodId))
        //                                            .ToList();


        //    var unscoredFoods = _database.CollectionFood.Find(f => !scoredFoodIds.Contains(f.Id))
        //                                                 .ToList();

        //    return unscoredFoods;
        //}





    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GurmeDefteriBackEndAPI.DatabaseContext;
using GurmeDefteriBackEndAPI.Models;
using GurmeDefteriBackEndAPI.Models.ViewModel;
using GurmeDefteriWebUI.Models.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
namespace GurmeDefteriBackEndAPI.Services
{
    public class AdminService
    {
        private readonly Database _database;

        public AdminService()
        {
            _database = new Database();
        }

        public List<User> GetAllUsers()
        {
            var _users = _database.CollectionPerson;
            return _users.Find(user => true).ToList();
        }
        public User GetUserById(string userId)
        {
            var user = _database.CollectionPerson.Find(u => u.Id == new ObjectId(userId)).FirstOrDefault();
            return user;
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

        public List<ScoredFoods> GetScoredFoods()
        {
            var _scoredFoods = _database.CollectionScoredFoods;
            return _scoredFoods.Find(ScoredFoods => true).ToList();
        }

        public User GetUserWithId(string id)
        {
            var objectId = new ObjectId(id);
            var user = _database.CollectionPerson.Find(u => u.Id == objectId).FirstOrDefault();
            return user;
        }

        public void UpdateUser(string userId, User updatedUser)
        {
            var objectId = new ObjectId(userId);
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

        public void AddFood(string name, string country, string imagefile, string category)
        {
            try
            {
                var food = new Food
                {
                    Name = name,
                    Country = country,
                    Image = imagefile,
                    Category = category
                };

                _database.CollectionFood.InsertOne(food);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void UpdateFood(FoodItemWithImageBytes foodTemp)
        {

            var filter = Builders<Food>.Filter.Eq(f => f.Id, ObjectId.Parse(foodTemp.Id));
            var update = Builders<Food>.Update
                .Set(f => f.Name, foodTemp.Name)
                .Set(f => f.Country, foodTemp.Country)
                 .Set(f => f.Image, foodTemp.ImageBytes)
                 //Category Yoktu
                 .Set(f => f.Category, foodTemp.Category);
            _database.CollectionFood.UpdateOne(filter, update);
       
        }
        public void DeleteFood(string foodId)
        {
            if (!ObjectId.TryParse(foodId, out var objectId))
            {
                throw new ArgumentException("Invalid ObjectId format", nameof(foodId));
            }

            var filter = Builders<Food>.Filter.Eq(f => f.Id, objectId);
            var food = _database.CollectionFood.Find(filter).FirstOrDefault();
            if (food != null)
            {
                _database.CollectionFood.DeleteOne(filter);         
            }
        }
        public int GetUserCount()
        {
            var userCount = _database.CollectionPerson.CountDocuments(new BsonDocument());
            int documentCountInt = Convert.ToInt32(userCount);
            return documentCountInt;
        }

        public int GetUserCountByName(string name)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Regex("Name", new BsonRegularExpression(name, "i"));

            var userCount = _database.CollectionPerson.CountDocuments(filter);
            int documentCountInt = Convert.ToInt32(userCount);
            return documentCountInt;
        }



    }
}

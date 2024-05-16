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
using GurmeDefteriBackEndAPI.Models.Dto;
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

        public User GetUserByMail(string mail)
        {
            var user = _database.CollectionPerson.Find(u => u.Email == mail).FirstOrDefault();
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
        public int GetScoredFoodCount()
        {
            var scoredFoodCount = _database.CollectionScoredFoods.CountDocuments(new BsonDocument());
            int documentCountInt = Convert.ToInt32(scoredFoodCount);
            return documentCountInt;
        }
        public int GetScoredFoodCountByName(string name)
        {
            var user = _database.CollectionPerson.Find(u => u.Email == name).FirstOrDefault();
            var food = _database.CollectionFood.Find(u => u.Name == name).FirstOrDefault();

            FilterDefinition<ScoredFoods> filter = null;

            if (user != null)
            {
                filter = Builders<ScoredFoods>.Filter.Eq("UserId", user.Id.ToString());
            }
            else if (food != null)
            {
                filter = Builders<ScoredFoods>.Filter.Eq("FoodId", food.Id.ToString());
            }
            else
            {
                return 0;
            }

            var foodCount = _database.CollectionScoredFoods.CountDocuments(filter);
            return (int)foodCount;
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

        public void UpdateUser( UserAPI updatedUser)
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
        public void AddScoredFoods(ScoredFoods scoredFoods)
        {
            _database.CollectionScoredFoods.InsertOne(scoredFoods);
        }
        public void UpdateScoredFoods(string id, int score)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ObjectId format", nameof(id));
            }

            var filter = Builders<ScoredFoods>.Filter.Eq(s => s.Id, objectId);
            var update = Builders<ScoredFoods>.Update.Set(s => s.Score, score);

            _database.CollectionScoredFoods.UpdateOne(filter, update);
        }

        public void DeleteScoredFoods(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ObjectId format", nameof(id));
            }

            var filter = Builders<ScoredFoods>.Filter.Eq(s => s.Id, objectId);
            _database.CollectionScoredFoods.DeleteOne(filter);
        }
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
        //Geliştirilme aşamasında
        public List<AdminShowScoredFood> ShowAdminScoredFoods(int pageNumber, int pageSize)
        {
            try
            {
                var scoredFoods = _database.CollectionScoredFoods.Find(_ => true)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Limit(pageSize)
                                     .ToList();
                var result = new List<AdminShowScoredFood>();

                foreach (var scoredFood in scoredFoods)
                {
                    var userId = ObjectId.Parse(scoredFood.UserId);
                    var foodId = ObjectId.Parse(scoredFood.FoodId);

                    var user = _database.CollectionPerson.Find(u => u.Id == userId).FirstOrDefault();
                    var food = _database.CollectionFood.Find(f => f.Id == foodId).FirstOrDefault();

                    if (user != null && food != null)
                    {
                        result.Add(new AdminShowScoredFood
                        {
                            ScoredFoodID = scoredFood.FoodId.ToString(),
                            Email = user.Email,
                            Foodname = food.Name,
                            Score = scoredFood.Score
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAdminScoredFoods: {ex.Message}");
                throw;
            }
        }
        public List<AdminShowScoredFood> SearchScoredFoodsByUserEmail(string userEmail, int pageNumber, int pageSize)
        {
            try
            {
                var user = _database.CollectionPerson.Find(u => u.Email == userEmail).FirstOrDefault();
                if (user == null)
                {
                    return new List<AdminShowScoredFood>(); // Return an empty list if user is not found
                }

                var scoredFoods = _database.CollectionScoredFoods.Find(sf => sf.UserId == user.Id.ToString())
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Limit(pageSize)
                                     .ToList();
                var result = new List<AdminShowScoredFood>();

                foreach (var scoredFood in scoredFoods)
                {
                    var food = _database.CollectionFood.Find(f => f.Id == ObjectId.Parse(scoredFood.FoodId)).FirstOrDefault();
                    if (food != null)
                    {
                        result.Add(new AdminShowScoredFood
                        {
                            ScoredFoodID = scoredFood.FoodId.ToString(),
                            Email = user.Email,
                            Foodname = food.Name,
                            Score = scoredFood.Score
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchScoredFoodsByUserEmail: {ex.Message}");
                throw;
            }
        }

        public List<AdminShowScoredFood> SearchScoredFoodsByFoodName(string foodName, int pageNumber, int pageSize)
        {
            try
            {
                var food = _database.CollectionFood.Find(f => f.Name == foodName).FirstOrDefault();
                if (food == null)
                {
                    return new List<AdminShowScoredFood>(); // Yiyecek bulunamazsa boş liste döndür
                }

                var scoredFoods = _database.CollectionScoredFoods.Find(sf => sf.FoodId == food.Id.ToString())
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Limit(pageSize)
                                     .ToList();
                var result = new List<AdminShowScoredFood>();

                foreach (var scoredFood in scoredFoods)
                {
                    var user = _database.CollectionPerson.Find(u => u.Id == ObjectId.Parse(scoredFood.UserId)).FirstOrDefault();
                    if (user != null)
                    {
                        result.Add(new AdminShowScoredFood
                        {
                            ScoredFoodID = scoredFood.FoodId.ToString(),
                            Email = user.Email,
                            Foodname = food.Name,
                            Score = scoredFood.Score
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchScoredFoodsByFoodName: {ex.Message}");
                throw;
            }
        }
        public List<AdminShowScoredFood> SearchScoredFoods(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                var user = _database.CollectionPerson.Find(u => u.Email == searchTerm).FirstOrDefault();
                var food = _database.CollectionFood.Find(f => f.Name == searchTerm).FirstOrDefault();

                if (user == null && food == null)
                {
                    return new List<AdminShowScoredFood>(); // Return an empty list if user and food are not found
                }

                var result = new List<AdminShowScoredFood>();

                if (user != null)
                {
                    var userScoredFoods = _database.CollectionScoredFoods.Find(sf => sf.UserId == user.Id.ToString())
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Limit(pageSize)
                                             .ToList();
                    foreach (var scoredFood in userScoredFoods)
                    {
                        var scoredFoodFood = _database.CollectionFood.Find(f => f.Id == ObjectId.Parse(scoredFood.FoodId)).FirstOrDefault();
                        if (scoredFoodFood != null)
                        {
                            result.Add(new AdminShowScoredFood
                            {
                                ScoredFoodID = scoredFood.FoodId.ToString(),
                                Email = user.Email,
                                Foodname = scoredFoodFood.Name,
                                Score = scoredFood.Score
                            });
                        }
                    }
                }

                if (food != null)
                {
                    var foodScoredFoods = _database.CollectionScoredFoods.Find(sf => sf.FoodId == food.Id.ToString())
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Limit(pageSize)
                                             .ToList();
                    foreach (var scoredFood in foodScoredFoods)
                    {
                        var scoredFoodUser = _database.CollectionPerson.Find(u => u.Id == ObjectId.Parse(scoredFood.UserId)).FirstOrDefault();
                        if (scoredFoodUser != null)
                        {
                            result.Add(new AdminShowScoredFood
                            {
                                ScoredFoodID = scoredFood.FoodId.ToString(),
                                Email = scoredFoodUser.Email,
                                Foodname = food.Name,
                                Score = scoredFood.Score
                            });
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchScoredFoods: {ex.Message}");
                throw;
            }
        }
        public List<string> GetAllUserEmails()
        {
            var users = _database.CollectionPerson;
            var userNames = users.Find(user => true).ToList().Select(user => user.Email).ToList();
            return userNames;
        }
        public List<string> GetAllFoodNames()
        {
            var foods = _database.CollectionFood;
            var foodNames = foods.Find(food => true).ToList().Select(food => food.Name).ToList();
            return foodNames;
        }
        public void AddAdminScoredFoods(string userEmail, string foodName, int score)
        {
            var user = _database.CollectionPerson.Find(u => u.Email == userEmail).FirstOrDefault();
            var food = _database.CollectionFood.Find(f => f.Name == foodName).FirstOrDefault();

            if (user != null && food != null)
            {
                var scoredFoods = new ScoredFoods
                {
                    UserId = user.Id.ToString(),
                    FoodId = food.Id.ToString(),
                    Score = score
                };

                _database.CollectionScoredFoods.InsertOne(scoredFoods);
            }
            else
            {
                throw new ArgumentException("User or food not found");
            }
        }
        public AdminShowScoredFood GetScoredFoodWithId(string scoredFoodId)
        {
            try
            {
                var scoredFood = _database.CollectionScoredFoods.Find(sf => sf.Id == ObjectId.Parse(scoredFoodId)).FirstOrDefault();

                if (scoredFood == null)
                {
                    return null; // Eğer puanlanmış yiyecek bulunamazsa null döndür
                }

                var user = _database.CollectionPerson.Find(u => u.Id == ObjectId.Parse(scoredFood.UserId)).FirstOrDefault();
                var food = _database.CollectionFood.Find(f => f.Id == ObjectId.Parse(scoredFood.FoodId)).FirstOrDefault();

                if (user == null || food == null)
                {
                    return null; // Eğer kullanıcı veya yiyecek bulunamazsa null döndür
                }

                return new AdminShowScoredFood
                {
                    ScoredFoodID = scoredFood.FoodId.ToString(),
                    Email = user.Email,
                    Foodname = food.Name,
                    Score = scoredFood.Score
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetScoredFoodWithId: {ex.Message}");
                throw;
            }
        }
        public bool CheckScoredFood(string userEmail, string foodName)
        {
            try
            {
                var user = _database.CollectionPerson.Find(u => u.Email == userEmail).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("User not found."); // Kullanıcı bulunamazsa istisna fırlat
                }

                var food = _database.CollectionFood.Find(f => f.Name == foodName).FirstOrDefault();
                if (food == null)
                {
                    throw new Exception("Food not found."); // Yiyecek bulunamazsa istisna fırlat
                }

                var scoredFood = _database.CollectionScoredFoods.Find(sf => sf.UserId == user.Id.ToString() && sf.FoodId == food.Id.ToString()).FirstOrDefault();
                return scoredFood != null; // Eğer puanlanmış yiyecek varsa true, yoksa false döndür
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckScoredFood: {ex.Message}");
                throw;
            }
        }






    }
}

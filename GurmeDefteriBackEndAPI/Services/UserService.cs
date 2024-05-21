﻿
using GurmeDefteriBackEndAPI.DatabaseContext;
using GurmeDefteriBackEndAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using GurmeDefteriBackEndAPI.Models.Dto;
using GurmeDefteriBackEndAPI.Models.ViewModel;
using MongoDB.Bson.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

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

        public List<FoodItemWithImageBytes> GetScoredFoodsByUserId(string userId, int page, int pageSize)
        {
            var objectId = new ObjectId(userId);
            var filter = Builders<ScoredFoods>.Filter.Eq(s => s.UserId, userId);
            var scoredFoods = _database.CollectionScoredFoods.Find(filter)
                                                              .Skip((page - 1) * pageSize)
                                                              .Limit(pageSize)
                                                              .ToList();

            var foodIds = scoredFoods.Select(sf => sf.FoodId).ToList();
            var foods = _database.CollectionFood.Find(f => foodIds.Contains(f.Id.ToString())).ToList();

            var result = (from sf in scoredFoods
                          join f in foods on sf.FoodId equals f.Id.ToString()
                          select new FoodItemWithImageBytes
                          {
                              Id = f.Id.ToString(),
                              Name = f.Name,
                              Country = f.Country,
                              ImageBytes = f.Image,
                              Category = f.Category
                          }).ToList();

            return result;
        }

        //Şu anda kullanılmıyor objectid olarak gönderme
        public List<ScoredFoods> GetScoredFoodsByFoodId(string foodId, int page, int pageSize)
        {
            var objectId = new ObjectId(foodId);
            var filter = Builders<ScoredFoods>.Filter.Eq(s => s.FoodId, foodId);
            var scoredFoods = _database.CollectionScoredFoods.Find(filter)
                                                             .Skip((page - 1) * pageSize)
                                                             .Limit(pageSize)
                                                             .ToList();
            return scoredFoods;
        }

        public List<FoodItemWithImageBytes> FoodCategoryFilter(string category, int page, int pageSize)
        {
            var filter = Builders<Food>.Filter.Eq(f => f.Category, category);
            var foods = _database.CollectionFood.Find(filter)
                                                 .Skip((page - 1) * pageSize)
                                                 .Limit(pageSize)
                                                 .ToList()
                                                 .Select(f => new FoodItemWithImageBytes
                                                 {
                                                     Name = f.Name,
                                                     Country = f.Country,
                                                     ImageBytes = f.Image,
                                                     Id = f.Id.ToString(),
                                                     Category = f.Category
                                                 })
                                                 .ToList();
            return foods;
        }


        //public List<Food> SearchFoodsByTerm(string term, int skip, int limit)
        //{
        //    var filter = Builders<Food>.Filter.Regex("Name", new BsonRegularExpression(term, "i"));
        //    var foods = _database.CollectionFood.Find(filter).Skip(skip).Limit(limit).ToList();
        //    return foods;
        //}
        public List<FoodItemWithImageBytes> SearchFoodsByTermAndCategory(string term, string category, int page, int pageSize)
        {
            var filter = Builders<Food>.Filter.Regex("Name", new BsonRegularExpression(term, "i")) & Builders<Food>.Filter.Eq("Category", category);
            var foods = _database.CollectionFood.Find(filter)
                                                 .Skip((page - 1) * pageSize)
                                                 .Limit(pageSize)
                                                 .ToList()
                                                 .Select(f => new FoodItemWithImageBytes
                                                 {
                                                     Name = f.Name,
                                                     Country = f.Country,
                                                     ImageBytes = f.Image,
                                                     Id = f.Id.ToString(),
                                                     Category = f.Category
                                                 })
                                                 .ToList();
            return foods;
        }

        public List<FoodItemWithImageBytes> GetUnscoredFoodsByUserId(string userId, int page, int pageSize)
        {
            var scoredFoodIds = _database.CollectionScoredFoods.Find(sf => sf.UserId == userId)
                                                                .Project(sf => sf.FoodId)
                                                                .ToList();

            var filter = Builders<Food>.Filter.Nin(f => f.Id, scoredFoodIds.Select(id => new ObjectId(id)));
            var unscoredFoods = _database.CollectionFood.Find(filter)
                                                        .Skip((page - 1) * pageSize)
                                                        .Limit(pageSize)
                                                        .ToList();

            var foodItems = unscoredFoods.Select(food => new FoodItemWithImageBytes
            {
                Name = food.Name,
                Country = food.Country,
                ImageBytes = food.Image,
                Id = food.Id.ToString(),
                Category = food.Category
            }).ToList();

            return foodItems;
        }

        public void AddScoredFoods(ScoredFoods scoredFoods)
        {
            _database.CollectionScoredFoods.InsertOne(scoredFoods);
        }
        public void UpdateScoredFood(string userId, string foodId, int score)
        {
            var updateFilter = Builders<ScoredFoods>.Filter.Eq(sf => sf.UserId, userId) &
                               Builders<ScoredFoods>.Filter.Eq(sf => sf.FoodId, foodId);

            var update = Builders<ScoredFoods>.Update.Set(sf => sf.Score, score);

            _database.CollectionScoredFoods.UpdateOne(updateFilter, update);
        }
        public int CheckScoredFood(string userId, string foodId)
        {
            try
            {
                var scoredFood = _database.CollectionScoredFoods.Find(sf => sf.UserId == userId && sf.FoodId == foodId).FirstOrDefault();
                if (scoredFood != null)
                {
                    return scoredFood.Score; // Eğer yiyecek puanlanmışsa skoru döndür
                }
                else
                {
                    return 0; // Eğer yiyecek puanlanmamışsa 0 döndür
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckScoredFood: {ex.Message}");
                throw;
            }
        }
        //geliştirilme aşamasında
        //public IEnumerable<string> SuggestScore(string userId)
        //{
        //    var scoredFoodIds = _database.CollectionScoredFoods.Find(sf => sf.UserId == userId)
        //                                               .ToList()
        //                                               .Select(sf => sf.FoodId);

        //    var allFoodIds = _database.CollectionScoredFoods.Find(sf => true)
        //                                            .ToList()
        //                                            .Select(sf => sf.FoodId);

        //    var unscoredFoodIds = allFoodIds.Except(scoredFoodIds).Take(50);

        //    var client = new HttpClient();
        //    var url = "http://20.81.205.102:92/api/eniyiteklif";
        //    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = userId, FoodIds = unscoredFoodIds }), Encoding.UTF8, "application/json");
        //    var response = client.PostAsync(url, content).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = response.Content.ReadAsStringAsync().Result;
        //        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(result, new { UnsuggestedFoodIds = new List<string>(), UserId = "" });
        //        return responseObject.UnsuggestedFoodIds;
        //    }
        //    else
        //    {
        //        // Handle unsuccessful response
        //        return new List<string>();
        //    }
        //}
        public (FoodItemWithImageBytes Food, int Score) GetFoodScoreSuggestion(string userId)
        {
            var scoredFoods = _database.CollectionScoredFoods.Find(sf => sf.UserId == userId).ToList();

            if (scoredFoods.Count == 0)
            {
                throw new InvalidOperationException("No scored foods found for the given user.");
            }

            var random = new Random();
            var randomIndex = random.Next(scoredFoods.Count);
            var randomScoredFood = scoredFoods[randomIndex];

            var food = _database.CollectionFood.Find(f => f.Id == new ObjectId(randomScoredFood.FoodId)).FirstOrDefault();
            if (food == null)
            {
                throw new InvalidOperationException("Food not found.");
            }

            var foodItemWithImageBytes = new FoodItemWithImageBytes
            {
                Id = food.Id.ToString(),
                Name = food.Name,
                Country = food.Country,
                ImageBytes = food.Image,
                Category = food.Category
            };

            return (foodItemWithImageBytes, randomScoredFood.Score);
        }


    }

}
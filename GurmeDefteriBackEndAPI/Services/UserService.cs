﻿
using GurmeDefteriBackEndAPI.DatabaseContext;
using GurmeDefteriBackEndAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using GurmeDefteriBackEndAPI.Models.Dto;
using MongoDB.Bson.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Serilog;
using Microsoft.IdentityModel.Tokens;

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
            Log.Information("Kullanıcı bilgileri güncellendi:{Username}", updatedUser.Email);
        }

        public void DeleteUser(string userId)
        {
            var objectId = new ObjectId(userId);

            // Kullanıcıyı al
            var userFilter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            var deletedUser = _database.CollectionPerson.FindOneAndDelete(userFilter);

            // ScoredFood için filtre oluştur
            var scoredFoodFilter = Builders<ScoredFoods>.Filter.Eq(sf => sf.UserId, userId);
            _database.CollectionScoredFoods.DeleteMany(scoredFoodFilter);

            if (deletedUser != null)
            {
                Log.Information("Kullanıcı silindi : {username}", deletedUser.Email);
            }
            else
            {
                Log.Information("Kullanıcı bulunamadı");
            }
        }


        public void AddUser(User newUser)
        {
            try
            {
                _database.CollectionPerson.InsertOne(newUser);
                if(newUser != null)
                {
                    Log.Information($"Kullanıcı eklendi: {newUser.Email}");
                }
                else 
                {
                    Log.Information("Kullanıcı bilgileri eksik.");
                }
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
                              Category = f.Category,
                              Description= f.Description
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
                                                     Category = f.Category,
                                                     Description=f.Description
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
                                                     Category = f.Category,
                                                     Description=f.Description
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
                Category = food.Category,
                Description = food.Description
            }).ToList();

            return foodItems;
        }

        public void AddScoredFoods(ScoredFoods scoredFoods)
        {
            _database.CollectionScoredFoods.InsertOne(scoredFoods);

            var userObjectId = new ObjectId(scoredFoods.UserId);
            var userFilter = Builders<User>.Filter.Eq(u => u.Id, userObjectId);
            var logUser = _database.CollectionPerson.Find(userFilter).FirstOrDefault();

            var foodObjectId = new ObjectId(scoredFoods.FoodId);
            var foodFilter = Builders<Food>.Filter.Eq(u => u.Id, foodObjectId);
            var logFood = _database.CollectionFood.Find(foodFilter).FirstOrDefault();

            if (logUser != null && logFood != null)
            {
                Log.Information("{Username} kullanıcısı {Foodname} adlı yemeğe {Skor} oy verdi.", logUser.Email, logFood.Name, scoredFoods.Score);
            }
            else
            {
                Log.Information("Kullanıcı veya yemek bulunamadı.");
            }
        }


        public void UpdateScoredFood(string userId, string foodId, int score)
        {
            var updateFilter = Builders<ScoredFoods>.Filter.Eq(sf => sf.UserId, userId) &
                               Builders<ScoredFoods>.Filter.Eq(sf => sf.FoodId, foodId);

            var update = Builders<ScoredFoods>.Update.Set(sf => sf.Score, score);

            _database.CollectionScoredFoods.UpdateOne(updateFilter, update);
            var userObjectId=new ObjectId(userId);
            var foodObjectId = new ObjectId(foodId);
            var userFilter=Builders<User>.Filter.Eq(u=>u.Id, userObjectId);
            var logUser=_database.CollectionPerson.Find(userFilter).FirstOrDefault();
            var foodFilter=Builders<Food>.Filter.Eq(u=> u.Id, foodObjectId);
            var logFood=_database.CollectionFood.Find(foodFilter).FirstOrDefault();

            if (logUser != null && logFood != null)
            {
                Log.Information("{Username} kullanıcısı {Foodname} adlı yemeğe verdiği oyu değiştirdi.Yeni oy : {Skor}", logUser.Email, logFood.Name,score);
            }
            else
            {
                Log.Information("Kullanıcı veya yemek bulunamadı.");
            }

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
        public async Task<SuggestFoodAPI> GetFoodScoreSuggestion(string userId)
        {
            var userObjectId = new ObjectId(userId);
            var userScoredFoodIds = _database.CollectionScoredFoods
                .Find(sf => sf.UserId == userId)
                .Project(sf => sf.FoodId)
                .ToList();

            var allScoredFoodIds = _database.CollectionScoredFoods
                .Find(Builders<ScoredFoods>.Filter.Empty)
                .Project(sf => sf.FoodId)
                .ToList();

            var unscoredFoodIds = _database.CollectionFood
                .Find(f => !userScoredFoodIds.Contains(f.Id.ToString()) && allScoredFoodIds.Contains(f.Id.ToString()))
                .Project(f => f.Id.ToString())
                .Limit(50)
                .ToList();

            if (unscoredFoodIds.Count == 0)
            {
                throw new InvalidOperationException("No unscored foods found for the given user.");
            }

            var requestBody = new
            {
                user_id = userId,
                food_ids = unscoredFoodIds
            };
            var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("http://20.81.205.102:92/api/eniyiteklif", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"API call failed with status code: {response.StatusCode}");
            }

            var apiResponseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API Response: " + apiResponseString);

            var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResponse>(apiResponseString, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            if (apiResponse == null || string.IsNullOrEmpty(apiResponse.BestFoodId))
            {
                throw new InvalidOperationException("API did not return a valid response.");
            }

            var bestFoodId = new ObjectId(apiResponse.BestFoodId);
            var food = _database.CollectionFood.Find(f => f.Id == bestFoodId).FirstOrDefault();

            if (food == null)
            {
                throw new InvalidOperationException("Food not found.");
            }

            var suggestFoodAPI = new SuggestFoodAPI
            {
                Id = food.Id.ToString(),
                Name = food.Name,
                Country = food.Country,
                ImageBytes = food.Image,
                Category = food.Category,
                Description= food.Description,
                Score = (int)Math.Round(apiResponse.Score) // Skoru int olarak ayarlayın
            };

            return suggestFoodAPI;
        }
        public List<FoodItemWithImageBytes> GetUnscoredFoodsByUserIdAndCategory(string userId, string category, int page, int pageSize)
        {
            var scoredFoodIds = _database.CollectionScoredFoods.Find(sf => sf.UserId == userId)
                                                                .Project(sf => sf.FoodId)
                                                                .ToList();

            var filter = Builders<Food>.Filter.Nin(f => f.Id, scoredFoodIds.Select(id => new ObjectId(id)))
                                            & Builders<Food>.Filter.Eq(f => f.Category, category);

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
                Category = food.Category,
                Description= food.Description
            }).ToList();

            return foodItems;
        }
        public List<FoodItemWithImageBytes> GetScoredFoodsByUserIdAndCategory(string userId, string category, int page, int pageSize)
        {
            var filter = Builders<ScoredFoods>.Filter.Eq(s => s.UserId, userId);
            var scoredFoods = _database.CollectionScoredFoods.Find(filter)
                                                              .Skip((page - 1) * pageSize)
                                                              .Limit(pageSize)
                                                              .ToList();

            var foodIds = scoredFoods.Select(sf => sf.FoodId).ToList();
            var foods = _database.CollectionFood.Find(f => foodIds.Contains(f.Id.ToString()) && f.Category == category)
                                                 .ToList();

            var result = (from sf in scoredFoods
                          join f in foods on sf.FoodId equals f.Id.ToString()
                          select new FoodItemWithImageBytes
                          {
                              Id = f.Id.ToString(),
                              Name = f.Name,
                              Country = f.Country,
                              ImageBytes = f.Image,
                              Category = f.Category,
                              Description= f.Description
                          }).ToList();

            return result;
        }
        public bool CheckUserIdExistsInScoredFoods(string userId)
        {
            var filter = Builders<ScoredFoods>.Filter.Eq(s => s.UserId, userId);
            var result = _database.CollectionScoredFoods.Find(filter).Any();
            return result;
        }
        public async Task<string> GetFoodExpectedScore(string userId, string foodId)
        {
            var requestBody = new
            {
                user_id = userId,
                food_id = foodId
            };
            var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("http://20.81.205.102:92/api/yemeknumarasi", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"API call failed with status code: {response.StatusCode}");
            }

            var apiResponseString = await response.Content.ReadAsStringAsync();
            var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FoodApiResponse>(apiResponseString);

            if (apiResponse == null || string.IsNullOrEmpty(apiResponse.Score))
            {
                throw new InvalidOperationException("API did not return a valid response.");
            }

            return apiResponse.Score;
        }











    }

}
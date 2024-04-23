using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GurmeDefteriBackEndAPI.DatabaseContext;
using GurmeDefteriBackEndAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GurmeDefteriBackEndAPI.Services
{
    public class AdminService
    {
        private Database database;


        public List<User> GetAllUsers()
        {
            var database = new Database();
            var _users = database.CollectionPerson;
            return _users.Find(user => true).ToList();
        }
        public List<Food> GetFoods()
        {
            var database = new Database();
            var _foods = database.CollectionFood;
            return _foods.Find(food => true).ToList();
        }
        public List<ScoredFoods> GetScoredFoods()
        {
            var database = new Database();
            var _scoredFoods = database.CollectionScoredFoods;
            return _scoredFoods.Find(ScoredFoods => true).ToList();
        }
        public User GetUserWithId(string id)
        {
            var database = new Database();
            var objectId = new ObjectId(id);
            var user = database.CollectionPerson.Find(u => u.Id == objectId).FirstOrDefault();
            return user;
        }
        public void UpdateUser(string userId, User updatedUser)
        {
            var database = new Database();

            var objectId = new ObjectId(userId);
            var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            var update = Builders<User>.Update
                .Set(u => u.Name, updatedUser.Name)
                .Set(u => u.Age, updatedUser.Age)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.Password, updatedUser.Password)
                .Set(u => u.Role, updatedUser.Role);

            database.CollectionPerson.UpdateOne(filter, update);
        }
        public void DeleteUser(string userId)
        {
            var database = new Database();
            try
            {
                var objectId = new ObjectId(userId);
                var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
                database.CollectionPerson.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public void AddUser(User newUser)
        {
            var database = new Database();
            try
            {
                
                database.CollectionPerson.InsertOne(newUser);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public void AddFood([Required, StringLength(70, MinimumLength = 2, ErrorMessage = "İsim en az 2, en fazla 70 karakter olmalıdır.")] string name,
                             [Required(ErrorMessage = "Ülke alanı girilmelidir.")] string country,
                             [Required(ErrorMessage = "Resim alanı girilmelidir.")] IFormFile image)
        {
            var database = new Database();
            try
            {
                var food = new Food
                {
                    Name = name,
                    Country = country,
                    Image = image.FileName  // You might want to store the image in a file system or a cloud storage service and store the URL here instead
                };

                database.CollectionFood.InsertOne(food);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

    }
}


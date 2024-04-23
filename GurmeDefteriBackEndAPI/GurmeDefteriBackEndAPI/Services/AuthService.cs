using GurmeDefteriBackEndAPI.DatabaseContext;
using GurmeDefteriBackEndAPI.Models;
using MongoDB.Driver;

namespace GurmeDefteriBackEndAPI.Services
{
    public class AuthService
    {
        private Database database;
        public bool ValidateUser(LoginUser user)
        {
            database = new();
            var _users = database.CollectionPerson;

            var results = _users.Find(x => x.Email == user.Email && x.Password == user.Password).ToList();

            return results.Count > 0;
        }
        public User FindUser(string email, string password)
        {
            database = new();
            var _users = database.CollectionPerson;
            return _users.Find(x => x.Email == email && x.Password == password).FirstOrDefault();
        }
    }
}

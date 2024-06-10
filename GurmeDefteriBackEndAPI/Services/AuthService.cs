using GurmeDefteriBackEndAPI.DatabaseContext;
using GurmeDefteriBackEndAPI.Models;
using MongoDB.Driver;

namespace GurmeDefteriBackEndAPI.Services
{
    public class AuthService
    {
        private Database _database;
        public AuthService()
        {
            _database = new Database();
        }
        public bool ValidateUser(LoginUser user)
        {
            var _users = _database.CollectionPerson;

            var results = _users.Find(x => x.Email == user.Email && x.Password == user.Password).ToList();

            return results.Count > 0;
        }
        public User FindUser(string email, string password)
        {
            
            var _users = _database.CollectionPerson;
            return _users.Find(x => x.Email == email && x.Password == password).FirstOrDefault();
        }
        public bool IsAdmin(LoginUser user)
        {
            var _users = _database.CollectionPerson;

            var result = _users.Find(x => x.Email == user.Email && x.Password == user.Password && x.Role == "Admin").FirstOrDefault();

            return result != null;
        }

    }
}

using MongoDB.Bson;

namespace GurmeDefteriBackEndAPI.Models.Dto
{
    public class UserAPI
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}

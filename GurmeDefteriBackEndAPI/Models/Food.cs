using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GurmeDefteriBackEndAPI.Models
{
    public class Food
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }

    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GurmeDefteriBackEndAPI.Models
{
    //daha sonra tekrar bak buraya 
    public class ScoredFoods
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string FoodId { get; set; }

        public int Score { get; set; }
    }
}

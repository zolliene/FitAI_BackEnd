using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Repository.Models
{
    [BsonIgnoreExtraElements]
    public class ProgressTracking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Weight { get; set; }
        public string Data { get; set; } 
        public int BodyFat { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; } 
        public DateTime LastCreate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
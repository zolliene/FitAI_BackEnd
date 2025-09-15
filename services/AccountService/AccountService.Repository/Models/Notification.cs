using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Repository.Models
{
    [BsonIgnoreExtraElements]
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public string Data { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime LastCreate { get; set; }
        public string UserId { get; set; } 
    }
}
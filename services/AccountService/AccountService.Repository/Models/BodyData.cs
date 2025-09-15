using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Repository.Models
{
    [BsonIgnoreExtraElements]
    public class BodyData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public DateTime LastUpdate { get; set; }
        public string BodyFatPerc { get; set; }
        public bool Status { get; set; }
        public DateTime LastCreate { get; set; }
        public string UserId { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Repository.Models
{
    [BsonIgnoreExtraElements]
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Status { get; set; }
        public string ReplyContent { get; set; }
        public string UserId { get; set; } 
        public DateTime LastCreate { get; set; }
        public string VoucherId { get; set; } 
    }
}
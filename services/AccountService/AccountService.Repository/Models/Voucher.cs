using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Repository.Models
{
    [BsonIgnoreExtraElements]
    public class Voucher
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public float Value { get; set; }
        public float MinOrderAmount { get; set; }
        public int MaxUses { get; set; }
        public int UsedCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
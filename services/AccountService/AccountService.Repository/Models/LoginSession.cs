using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Repository.Models
{
    [BsonIgnoreExtraElements]
    public class LoginSession
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int UserType { get; set; }
        public string SessionTokenHash { get; set; }
        public string RefreshTokenHash { get; set; }
        public string DeviceFingerprint { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public DateTime LoginAt { get; set; }
        public DateTime LastActivityAt { get; set; }
        public DateTime? LogoutAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Browser { get; set; }
        public string UserId { get; set; } 
        public string AdminId { get; set; } 
    }
}
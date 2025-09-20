using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Repository.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string GoogleId { get; set; }
        public int Weight { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime LastCreate { get; set; }
        public string Avatar { get; set; }
        public int Height { get; set; }
        public DateTime DateOfBirth { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Gender Gender { get; set; }
        public string Type { get; set; }
        // OTP and verification fields
        public string? OtpCode { get; set; }
        public DateTime? OtpGeneratedAt { get; set; }
        public bool IsEmailVerified { get; set; }
        // Gmail sign-in support
        public string? GoogleAccessToken { get; set; }
        public string? GoogleRefreshToken { get; set; }
        public string Goal { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum Gender
    {
        M,
        F,
        O // Other
    }
}
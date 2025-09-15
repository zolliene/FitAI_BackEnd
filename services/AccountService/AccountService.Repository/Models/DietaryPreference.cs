using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Repository.Models
{
    [BsonIgnoreExtraElements]
    public class DietaryPreference
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; } 
        public string DietType { get; set; }
        public int MealsPerDay { get; set; }
        public string EatingWindow { get; set; }
        public string Allergies { get; set; }
        public string AvoidIngredients { get; set; }
        public string CuisinePref { get; set; }
        public string Notes { get; set; }
        public DateTime LastCreate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
using AccountService.Repository.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Repository
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoClient client)
        {
            var db = client.GetDatabase("FitAI");
            _users = db.GetCollection<User>("Users");
        }

        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }
        public async Task<bool> SoftDeleteAsync(string id)
        {
            var update = Builders<User>.Update.Set(u => u.IsActive, false);
            var result = await _users.UpdateOneAsync(u => u.Id == id, update);
            return result.ModifiedCount > 0;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email && u.IsActive).FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
        public async Task<User> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id && u.IsActive).FirstOrDefaultAsync();
        }
    }
}

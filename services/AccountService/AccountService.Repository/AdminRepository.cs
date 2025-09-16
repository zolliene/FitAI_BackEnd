using AccountService.Repository.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Repository
{
    public class AdminRepository
    {
        private readonly IMongoCollection<Admin> _admins;

        public AdminRepository(IMongoClient client)
        {
            var database = client.GetDatabase("FitAI");
            _admins = database.GetCollection<Admin>("Admins");
        }

        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await _admins.Find(a => a.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Admin> GetByIdAsync(string id)
        {
            return await _admins.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Admin>> GetAllAsync()
        {
            return await _admins.Find(_ => true).ToListAsync();
        }

        public async Task<Admin> CreateAsync(Admin admin)
        {
            await _admins.InsertOneAsync(admin);
            return admin;
        }

        public async Task<bool> UpdateAsync(Admin admin)
        {
            var result = await _admins.ReplaceOneAsync(a => a.Id == admin.Id, admin);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _admins.DeleteOneAsync(a => a.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}

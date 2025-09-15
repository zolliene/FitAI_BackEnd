using AccountService.Repository.Models;
using AccountService.Repository; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Service
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService(UserRepository repo)
        {
            _repo = repo;
        }

        public async Task<User> RegisterAsync(string email, string password)
        {
            var existing = await _repo.GetByEmailAsync(email);
            if (existing != null)
                throw new Exception("Email already exists");

            var user = new User
            {
                Email = email,
               // Password = BCrypt.Net.BCrypt.HashPassword(password)
                Password = (password)
            };
            return await _repo.CreateAsync(user);
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}

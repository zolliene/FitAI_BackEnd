using AccountService.Repository;
using AccountService.Repository.Models;
using AccountService.Service.DTO.User;
using AccountService.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Service
{
    public class UserService: IUserService
    {
        private readonly UserRepository _repo;

        public UserService(UserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");

            return user;
        }
        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");

            return user;
        }
        public async Task<User> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            // Lấy thông tin user hiện tại
            var user = await _repo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            // Cập nhật thông tin
            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;
            
            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;
            
            if (request.Weight.HasValue && request.Weight.Value > 0)
                user.Weight = request.Weight.Value;
            
            if (request.Height.HasValue && request.Height.Value > 0)
                user.Height = request.Height.Value;
            
            if (request.Gender.HasValue)
                user.Gender = request.Gender.Value;
            
            if (request.DateOfBirth.HasValue)
                user.DateOfBirth = request.DateOfBirth.Value;
            
            if (!string.IsNullOrEmpty(request.Goal))
                user.Goal = request.Goal;
            
            // Cập nhật thời gian
            user.LastUpdate = DateTime.UtcNow;
            
            // Lưu vào database
            await _repo.UpdateAsync(user);
            
            return user;
        }
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");
                
            return await _repo.SoftDeleteAsync(id);
        }
    }
}

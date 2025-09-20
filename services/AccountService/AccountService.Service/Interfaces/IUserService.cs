using AccountService.Repository.Models;
using AccountService.Service.DTO.Auth;
using AccountService.Service.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Service.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);
        Task<User> UpdateProfileAsync(string userId, UpdateProfileRequest request);
        Task<User> GetByIdAsync(string id);
        Task<bool> DeleteUserAsync(string id);
    }
}

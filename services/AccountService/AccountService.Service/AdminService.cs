using AccountService.Repository;
using AccountService.Repository.Models;
using AccountService.Service.DTO.Admin;
using AccountService.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace AccountService.Service
{
    public class AdminService: IAdminService
    {
        private readonly AdminRepository _repo; 
        public AdminService(AdminRepository repo)
        {
            _repo=repo;
        }
       
        public async Task<Admin> CreateAdminAsync(CreateAdminRequest request)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingAdmin = await _repo.GetByEmailAsync(request.Email);
            if (existingAdmin != null)
                throw new Exception("Admin with this email already exists");

            // Mã hóa mật khẩu
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Tạo admin mới với các trường tối thiểu
            var admin = new Admin
            {
                Email = request.Email,
                Password = hashedPassword,
                RoleId = "admin", // Mặc định role là admin
                LastCreate = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
                FailedLoginAttempts = 0
            };

            return await _repo.CreateAsync(admin);
        }
    }
}

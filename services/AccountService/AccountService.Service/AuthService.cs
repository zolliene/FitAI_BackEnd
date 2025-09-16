using AccountService.Repository;
using AccountService.Repository.Models;
using AccountService.Service.DTO.Auth;
using AccountService.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _repo;
        private readonly AdminRepository _adminRepo;
        private readonly IConfiguration _configuration;

        public AuthService(UserRepository userRepository, AdminRepository adminRepo, IConfiguration configuration)
        {
            _repo = userRepository;
            _adminRepo = adminRepo;
            _configuration = configuration;
        }

        public async Task<AuthLoginResponse> LoginAsync(string email, string password)
        {
            // Kiểm tra user tồn tại
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid email or password");

            // Kiểm tra mật khẩu
            // Nếu dùng BCrypt: bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            bool isPasswordValid = password == user.Password; // Temporary plain comparison - use BCrypt in production

            if (!isPasswordValid)
                throw new Exception("Invalid email or password");

            // Tạo JWT token
            string token = GenerateJwtToken(user.Id, user.Email, "user");

            return new AuthLoginResponse
            {
                Id = user.Id,
                Email = user.Email,
                Token = token
            };
        }
        public async Task<AuthLoginResponse> LoginAdminAsync(string email, string password)
        {
            var admin = await _adminRepo.GetByEmailAsync(email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.Password))
                throw new Exception("Invalid email or password");

            string token = GenerateJwtToken(admin.Id, admin.Email, "admin");

            return new AuthLoginResponse
            {
                Id = admin.Id,
                Email = admin.Email,
                Token = token
            };
        }
        private string GenerateJwtToken(string id, string email, string role)
        {
            {
                var jwtKey = _configuration["JwtSettings:Secret"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? "defaultsecretkey123456789012345678901234"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, email),
                 new Claim(ClaimTypes.Role, role)
            };

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"] ?? "60")),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

        }
    }
}

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
using FirebaseAdmin;
using FirebaseAdmin.Auth;

namespace AccountService.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _repo;
        private readonly AdminRepository _adminRepo;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(UserRepository userRepository, AdminRepository adminRepo, IConfiguration configuration, IEmailService emailService)
        {
            _repo = userRepository;
            _adminRepo = adminRepo;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<AuthLoginResponse> LoginAsync(string email, string password)
        {
            // Kiểm tra user tồn tại
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid email or password");

            // Kiểm tra mật khẩu với BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
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
            // var admin = await _adminRepo.GetByEmailAsync(email);
            var admin = await _repo.GetByEmailAsync(email);
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

        public async Task<AuthRegisterResponse> RegisterAsync(string email, string password)
        {
            // Check if user already exists
            var existingUser = await _repo.GetByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("Email is already registered");

            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Email = email,
                Password = hashedPassword,
                OtpCode = otp,
                OtpGeneratedAt = DateTime.UtcNow,
                IsEmailVerified = false,
                LastCreate = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow
            };
            await _repo.CreateAsync(user);

            // TODO: Send OTP to email (implement email sending logic)
            await _emailService.SendEmailAsync(email, "Your OTP Code", $"Your OTP code is: {otp}");

            return new AuthRegisterResponse { OtpCode = otp };
        }

        public async Task<OtpVerifyResponse> VerifyOtpAsync(string email, string otpCode)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");
            if (user.OtpCode == null || user.OtpGeneratedAt == null)
                throw new Exception("No OTP requested for this user");
            if (user.OtpGeneratedAt.Value.AddMinutes(10) < DateTime.UtcNow)
                throw new Exception("OTP has expired");
            if (user.OtpCode != otpCode)
                throw new Exception("Invalid OTP code");
            user.IsEmailVerified = true;
            user.OtpCode = null;
            user.OtpGeneratedAt = null;
            await _repo.UpdateAsync(user);
            return new OtpVerifyResponse { Id = user.Id, Email = user.Email };
        }

        public async Task<AuthLoginResponse> GoogleSignInAsync(string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
                throw new Exception("ID token is required");

            // Verify the Firebase ID token
            FirebaseAdmin.FirebaseApp? defaultApp = null;
            if (FirebaseAdmin.FirebaseApp.DefaultInstance == null)
            {
                defaultApp = FirebaseAdmin.FirebaseApp.Create();
            }
            var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
            FirebaseAdmin.Auth.FirebaseToken decodedToken;
            try
            {
                decodedToken = await auth.VerifyIdTokenAsync(idToken);
            }
            catch (Exception)
            {
                throw new Exception("Invalid Google ID token");
            }

            var email = decodedToken.Claims.ContainsKey("email") ? decodedToken.Claims["email"]?.ToString() : null;
            var googleId = decodedToken.Uid;
            if (string.IsNullOrEmpty(email))
                throw new Exception("Google account does not have an email");

            // Check if user exists
            var user = await _repo.GetByEmailAsync(email);
            if (user == null)
            {
                // Create new user
                user = new User
                {
                    Email = email,
                    GoogleId = googleId,
                    IsEmailVerified = true,
                    LastCreate = DateTime.UtcNow,
                    LastUpdate = DateTime.UtcNow
                };
                await _repo.CreateAsync(user);
            }
            else
            {
                // Update GoogleId if not set
                if (string.IsNullOrEmpty(user.GoogleId))
                {
                    user.GoogleId = googleId;
                    user.LastUpdate = DateTime.UtcNow;
                    await _repo.UpdateAsync(user);
                }
            }

            // Generate JWT token
            string token = GenerateJwtToken(user.Id, user.Email, "user");

            return new AuthLoginResponse
            {
                Id = user.Id,
                Email = user.Email,
                Token = token
            };
        }
    }
}

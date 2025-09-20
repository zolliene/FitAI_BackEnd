using AccountService.Service.DTO.Auth;
using AccountService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;
using AccountService.Service.Interfaces;

namespace AccountService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
        {
            try
            {
                AuthLoginResponse user = await _service.LoginAsync(request.email, request.password);

                return Ok(ApiResponse<AuthLoginResponse>.Ok(
                    new AuthLoginResponse
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Token = user.Token,
                    }, "Login successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }
        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] AuthLoginRequest request)
        {
            try
            {
                AuthLoginResponse admin = await _service.LoginAdminAsync(request.email, request.password);
                return Ok(ApiResponse<AuthLoginResponse>.Ok(admin, "Admin login successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterRequest request)
        {
            try
            {
                var result = await _service.RegisterAsync(request.Email, request.Password);
                return Ok(ApiResponse<AuthRegisterResponse>.Ok(result, "Registration successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<AuthRegisterResponse>.Fail(ex.Message));
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyRequest request)
        {
            try
            {
                var result = await _service.VerifyOtpAsync(request.Email, request.OtpCode);
                return Ok(ApiResponse<OtpVerifyResponse>.Ok(result, "OTP verified successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<OtpVerifyResponse>.Fail(ex.Message));
            }
        }

        [HttpPost("google-signin")]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInRequest request)
        {
            try
            {
                var result = await _service.GoogleSignInAsync(request.IdToken);
                return Ok(ApiResponse<AuthLoginResponse>.Ok(result, "Google sign-in successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }
    }
}

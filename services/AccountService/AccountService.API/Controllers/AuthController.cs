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
    }
}

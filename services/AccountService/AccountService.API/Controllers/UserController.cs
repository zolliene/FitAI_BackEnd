using AccountService.Service.DTO.User;
using AccountService.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Endpoint chỉ cho admin truy cập
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllAsync();

                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }).ToList();

                return Ok(ApiResponse<List<UserDto>>.Ok(userDtos, "Users retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail(ex.Message));
            }
        }

    }
}

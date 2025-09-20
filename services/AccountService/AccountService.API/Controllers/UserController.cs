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

        // Admin-only endpoint with pagination, excludes admins
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var users = await _userService.GetAllAsync();
                var filtered = users.Where(u => u.Type != "admin");
                var total = filtered.Count();
                var userDtos = filtered
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Username = u.Username,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    }).ToList();
                var result = new AccountService.Service.DTO.Common.PagedResult<UserDto>
                {
                    Total = total,
                    Page = page,
                    PageSize = pageSize,
                    Items = userDtos
                };
                return Ok(ApiResponse<AccountService.Service.DTO.Common.PagedResult<UserDto>>.Ok(result, "Users retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Fail(ex.Message));
            }
        }

    }
}

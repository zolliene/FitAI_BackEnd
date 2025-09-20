using AccountService.Service.DTO.User;
using AccountService.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        /// <summary>
        /// Admin: get all users with pagination
        /// </summary>
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
        /// <summary>
        /// User: set up profile and update profile
        /// </summary>
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                // Lấy userId từ token
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<string>.Fail("User not authenticated"));
                }

                var updatedUser = await _userService.UpdateProfileAsync(userId, request);

                // Trả về thông tin đã cập nhật
                return Ok(ApiResponse<object>.Ok(new
                {
                    FirstName = updatedUser.FirstName,
                    LastName = updatedUser.LastName,
                    Weight = updatedUser.Weight,
                    Height = updatedUser.Height,
                    Gender = updatedUser.Gender.ToString(),
                    DateOfBirth = updatedUser.DateOfBirth,
                    Goal = updatedUser.Goal
                }, "Profile updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }
        /// <summary>
        /// Admin: Delete a user (soft delete)
        /// </summary>
        /// <param name="id">User ID to delete</param>
        /// <returns>Success message or error</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (result)
                    return Ok(ApiResponse<object>.Ok(null, "User deleted successfully"));
                else
                    return NotFound(ApiResponse<string>.Fail("User not found or could not be deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }
    }
}

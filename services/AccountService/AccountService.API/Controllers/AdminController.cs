using AccountService.Service.DTO.Admin;
using AccountService.Service.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccountService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")] // Chỉ admin mới truy cập được
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Tạo tài khoản admin mới
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminRequest request)
        {
            try
            {
                var admin = await _adminService.CreateAdminAsync(request);

                return Ok(ApiResponse<object>.Ok(new
                {
                    Id = admin.Id,
                    Email = admin.Email
                }, "Admin created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }
    }
}
using AccountService.Service.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthLoginResponse> LoginAsync(string email, string password);
        Task<AuthLoginResponse> LoginAdminAsync(string email, string password);
    }
}

using AccountService.Repository.Models;
using AccountService.Service.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Service.Interfaces
{
    public interface IAdminService
    {
        Task<Admin> CreateAdminAsync(CreateAdminRequest request);

    }
}

using AccountService.Repository.Models;
using System;

namespace AccountService.Service.DTO.User
{
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Goal { get; set; }
    }
}
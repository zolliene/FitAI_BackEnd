using System.Threading.Tasks;

namespace AccountService.Service.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}

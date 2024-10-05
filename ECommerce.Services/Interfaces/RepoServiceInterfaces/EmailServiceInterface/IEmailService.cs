
namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.EmailServiceInterface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}

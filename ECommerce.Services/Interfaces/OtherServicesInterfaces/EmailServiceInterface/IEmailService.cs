namespace ECommerce.Services.Interfaces.OtherServicesInterfaces.EmailServiceInterface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}

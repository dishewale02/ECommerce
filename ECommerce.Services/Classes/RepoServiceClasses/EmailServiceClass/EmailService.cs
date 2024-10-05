using ECommerce.Models.InputModelsDTO.EmailSenderDTO;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.EmailServiceInterface;
using MailKit.Net.Smtp;
using MimeKit;


namespace ECommerce.Services.Classes.RepoServiceClasses.EmailServiceClass
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSetting;

        public EmailService(EmailSettings emailSetting)
        {
            _emailSetting = emailSetting;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.To.Add(new MailboxAddress("email", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = body };
            MailboxAddress fromAddress = new MailboxAddress("email", _emailSetting.From);
            emailMessage.From.Add(fromAddress);

            //emailMessage.From.Add(new MailboxAddress(_emailSetting.SenderEmail, _emailSetting.SenderEmail));
            //emailMessage.To.Add(new MailboxAddress("", toEmail));
            //emailMessage.Subject = subject;
            //emailMessage.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailSetting.SmtpServer, _emailSetting.SmtpPort, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailSetting.Username, _emailSetting.Password);

                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Disconnect(true);
                    }

                    client.Dispose();
                }

                //client.Connect(_emailSetting.SmtpServer, _emailSetting.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);
                //client.Authenticate(_emailSetting.Username, _emailSetting.Password);
                //await client.SendAsync(emailMessage);
                //client.Disconnect(true);
            }
        }
    }
}

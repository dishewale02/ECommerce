
namespace ECommerce.Models.InputModelsDTO.EmailSenderDTO
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderName { get; set; }
        public string ToEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string ReturnRequestServer { get; set; }
    }
}

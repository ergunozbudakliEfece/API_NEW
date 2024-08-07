using System.Net.Mail;
using SQL_API.Services.MailService.Abstract;

namespace SQL_API.Services.MailService.Concrete
{
    public class Email : IEmail
    {
        public required string From { get; set; }
        public required string To { get; set; }
        public string Cc { get; set; } = "";
        public string Bcc { get; set; } = "";
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public string? Signature { get; set; } = null;
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}

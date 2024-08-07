using System.Net.Mail;

namespace SQL_API.Services.MailService.Abstract
{
    public interface IEmail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Signature { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}

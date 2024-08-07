using System.Net.Mail;
using SQL_API.Services.MailService.Abstract;

namespace SQL_API.Services.MailService.Concrete
{
    public class EmailService : IEmailService
    {
        public void SendEmail(IEmail Mail)
        {
            SmtpClient Client = new SmtpClient("192.168.2.13");

            Client.Send(CreateMail(Mail));
        }

        public Task SendEmailAsync(Email Mail)
        {

            throw new NotImplementedException();
        }

        private MailMessage CreateMail(IEmail Mail)
        {
            MailMessage Message = new()
            {
                From = new MailAddress(Mail.From),
                Subject = Mail.Subject,
                Body = Mail.Signature is not null ? Mail.Body + Mail.Signature : Mail.Body,
                IsBodyHtml = true
            };

            Message.From = new MailAddress(Mail.From);

            foreach (string ToAddress in Mail.To.Replace(",", ";").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                Message.To.Add(ToAddress);
            }
            foreach (string CcAddress in Mail.Cc.Replace(",", ";").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                Message.CC.Add(CcAddress);
            }
            foreach (string BccAddress in Mail.Bcc.Replace(",", ";").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                Message.Bcc.Add(BccAddress);
            }

            foreach (Attachment Attachment in Mail.Attachments)
            {
                Message.Attachments.Add(Attachment);
            }

            return Message;
        }
    }
}

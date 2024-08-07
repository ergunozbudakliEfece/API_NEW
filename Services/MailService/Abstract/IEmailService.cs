namespace SQL_API.Services.MailService.Abstract
{
    public interface IEmailService
    {
        void SendEmail(IEmail Mail);
    }
}

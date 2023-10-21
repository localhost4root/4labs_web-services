using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FarmEquipmentShop.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly MailSettings _configuration;

        public EmailSenderService(IOptions<MailSettings> configuration)
        {
            _configuration = configuration.Value;
        }

        public void SendEmail(string to, string subject, string content)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_configuration.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = content };

            using var client = new SmtpClient();
            try
            {
                client.Connect(_configuration.MailHost, _configuration.MailPort, true);
                client.Authenticate(_configuration.Name, _configuration.Password);
                client.Send(email);
            }
            catch (Exception ex)
            {
                throw new Exception("Can`t send an email", ex);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}

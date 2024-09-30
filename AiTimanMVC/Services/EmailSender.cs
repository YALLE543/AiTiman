using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AiTimanMVC.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;

        public EmailSender()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("aitiman.soc@gmail.com", "ekjil fbgd pued hzsm"),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage("aitiman.soc@gmail.com", email, subject, message);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}

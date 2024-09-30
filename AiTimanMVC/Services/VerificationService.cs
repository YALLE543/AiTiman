namespace AiTimanMVC.Services
{
    public class VerificationService
    {
        public string GenerateVerificationCode(int length = 6)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task SendVerificationEmail(string email, string code, IEmailSender emailSender)
        {
            var subject = "WELCOME Ka-AiTiman!";
            var message = $"Thank you for registering with AiTiman! To complete your account setup and verify your identity, please use the verification code below."+
                $"Your verification code is: {code}";

            await emailSender.SendEmailAsync(email, subject, message);
        }
    }
}

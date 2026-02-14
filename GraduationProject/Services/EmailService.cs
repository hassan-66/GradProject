using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GraduationProject.Services
{
    public class EmailService
    {
        public void SendEmail(string toEmail, string code)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Smart Transport", "yourgmail@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Password Reset Code";

            message.Body = new TextPart("plain")
            {
                Text = $"Your reset code is: {code}"
            };

            using var client = new SmtpClient();

            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate("hassanelhellw02@gmail.com", "xwiyofqyhmnqztzj");

            client.Send(message);
            client.Disconnect(true);
        }
    }
}
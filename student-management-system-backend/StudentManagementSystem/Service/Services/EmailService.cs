using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EmailService : IEmailService
    {
        #region DI
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        public async Task SendEmailAsync(string toEmail, string subject, string loginEmail, string loginPassword, List<string> ccEmails = null)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var smtpClient = new SmtpClient(smtpSettings["Server"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = true 
            };

            string htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Welcome to Our Service</title>
                </head>
                <body style='font-family: Arial, sans-serif; margin: 0; padding: 0;'>
                    <div style='background-color: #f8f9fa; padding: 20px;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px;'>
                            <h1 style='color: #333333;'>Welcome!</h1>
                            <p style='color: #555555;'>Thank you for registering with us. Here are your login details:</p>
                            <p><strong>Email:</strong> {loginEmail}</p>
                           <p><strong>Password:</strong> Your password follows the format YourName@YearOfBirth</p>
                            <p style='color: #555555;'>Best regards,<br>The Bacancy School</p>
                        </div>
                    </div>
                </body>
                </html>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["Email"]),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            if (ccEmails != null)
            {
                foreach (var ccEmail in ccEmails)
                {
                    mailMessage.CC.Add(ccEmail);
                }
            }

            await smtpClient.SendMailAsync(mailMessage);
        }



    }
}

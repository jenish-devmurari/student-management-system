using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.Interfaces;
using System.Net;
using System.Net.Mail;

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

        public async Task SendEmailToStudentAsync(StudentRegisterDTO student, List<string> ccEmails = null)
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
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            background-color: #ffffff;
                            margin: 0 auto;
                            padding: 20px;
                            max-width: 600px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        .header {{
                            background-color: #4CAF50;
                            color: #ffffff;
                            padding: 10px 0;
                            text-align: center;
                        }}
                        .content {{
                            padding: 20px;
                            line-height: 1.6;
                        }}
                        .content h1 {{
                            color: #333333;
                        }}
                        .content p {{
                            margin: 0 0 10px;
                        }}
                        .footer {{
                            background-color: #4CAF50;
                            color: #ffffff;
                            text-align: center;
                            padding: 10px 0;
                            font-size: 12px;
                        }}
                        @media (max-width: 600px) {{
                            .container {{
                                padding: 10px;
                            }}
                            .content {{
                                padding: 10px;
                            }}
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Admission Enrollment Notification</h1>
                        </div>
                        <div class='content'>
                            <h1>Dear {student.Name},</h1>
                            <p>We are pleased to inform you that your enrollment has been successfully processed. Below are the details of your admission:</p>
                            <p><strong>Student Name:</strong> {student.Name}</p>
                            <p><strong>Class:</strong> {student.Class}</p>
                            <p>We look forward to seeing you excel in your studies. If you have any questions, please feel free to contact us.</p>
                            <p>Below are your login details:</p>
                            <p><strong>Email:</strong> {student.Email}</p>
                            <p><strong>Password:</strong> Your password follows the format YourName@YearOfBirth</p>
                            <p>Best regards,</p>
                            <p><strong>The Bacancy School</strong></p>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 The Bacancy School. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["Email"]),
                Subject = "Admission Enrollment Notification",
                Body = htmlContent,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(student.Email);

            if (ccEmails != null)
            {
                foreach (var ccEmail in ccEmails)
                {
                    mailMessage.CC.Add(ccEmail);
                }
            }

            await smtpClient.SendMailAsync(mailMessage);
        }


        public async Task SendEmailTeacherAsync(string email)
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
                            <p><strong>Email:</strong> {email}</p>
                           <p><strong>Password:</strong> Your password follows the format YourName@YearOfBirth</p>
                            <p style='color: #555555;'>Best regards,<br>The Bacancy School</p>
                        </div>
                    </div>
                </body>
                </html>";


            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["Email"]),
                Subject = "Registration Successful",
                Body = htmlContent,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}









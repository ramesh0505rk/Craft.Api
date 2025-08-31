using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Infrastructure.ExternalServices
{
    public class MailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpToUserEmail(string UserEmail, string Otp)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var message = new MailMessage
            {
                From = new MailAddress(emailSettings["SenderEmail"], "Craft App"),
                Subject = "Your OTP for password reset",
                Body = @$"Your OTP for password reset is: <strong>{Otp}</strong>
                        <br/>This OTP is valid for 10 minutes",
                IsBodyHtml = true
            };
            message.To.Add(UserEmail);

            using var smtpClient = new SmtpClient(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]))
            {
                Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["SenderAppPassword"]),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}

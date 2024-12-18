using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using api.Data;
using api.models;
using api.Services;
using Microsoft.AspNetCore.Identity;
using MimeKit;

namespace api.Repositories
{
    public class EmailVerificationsRepo(
        AppDBContext _context,
         IConfiguration _configuration
    ) : IEmailVerification
    {
        private readonly AppDBContext context = _context;
        private readonly IConfiguration configuration = _configuration;
        public async Task<EmailVerification> CreateVerification(EmailVerification verification)
        {
            await context.EmailVerifications.AddAsync(verification);
            await context.SaveChangesAsync();
            return verification;
        }

        public int GenerateCode()
        {
            int random = new Random().Next(1000, 9999);
            return random;
        }



        public async Task SendVerificationEmail(string email, string subject, string code)
        {
            var client = new SmtpClient(configuration.GetSection("Smtp").GetValue<string>("Host"), configuration.GetSection("Smtp").GetValue<int>("Port"))
            {
                Credentials = new NetworkCredential(configuration.GetSection("Smtp").GetValue<string>("UserName"), configuration.GetSection("Smtp").GetValue<string>("Password")),
                EnableSsl = configuration.GetSection("Smtp").GetValue<bool>("EnableSsl"),

            };

            var mailMessage = new MailMessage("noreply@yourapp.com", email, subject, code);
            await client.SendMailAsync(mailMessage);
        }
    }
}
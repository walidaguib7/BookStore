using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.Services
{
    public interface IEmailVerification
    {
        public Task SendVerificationEmail(string email, string subject, string code);
        public int GenerateCode();
        public Task<EmailVerification> CreateVerification(EmailVerification verification);

    }
}
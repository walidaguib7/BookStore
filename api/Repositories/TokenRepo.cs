using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.models;
using api.Services;
using Microsoft.IdentityModel.Tokens;

namespace api.Repositories
{
    public class TokenRepo : ITokens
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey key;
        public TokenRepo(IConfiguration configuration)
        {
            _config = configuration;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SignInKey"]));
        }
        public string CreateToken(User user, bool isAdmin)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role , isAdmin == true ? "admin" : "customer")
            };

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMonths(2),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tk = tokenHandler.CreateToken(token);
            return tokenHandler.WriteToken(tk);
        }
    }
}
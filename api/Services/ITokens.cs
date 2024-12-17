using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.Services
{
    public interface ITokens
    {
        public string CreateToken(User user, bool isAdmin);
    }
}
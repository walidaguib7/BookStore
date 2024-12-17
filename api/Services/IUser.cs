using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using api.models.users.dtos;

namespace api.Services
{
    public interface IUser
    {
        public Task<User?> Register(RegisterDto dto);
        public Task<LoginResponse?> Login(LoginDto dto);
    }
}
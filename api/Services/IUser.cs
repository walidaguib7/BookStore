
using api.models;
using api.models.users.dtos;

namespace api.Services
{
    public interface IUser
    {
        public Task<User?> Register(RegisterDto dto);
        public Task<LoginResponse?> Login(LoginDto dto);
        public Task DeleteAll();
        public Task<User?> DeleteUser(string userId);
        public Task<User?> updateUser(string userId, UpdateUserDto dto);
        public Task<string?> GenerateResetPasswordToken(string userId);
        public Task<User?> PasswordReset(string userId, PasswordDto dto);
        public Task<bool?> VerifyUser(string userId, int verificationCode);

        public Task<User?> getUser(string userId);
    }
}
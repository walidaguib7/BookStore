using api.Data;
using api.models;
using api.models.users.dtos;
using api.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class UserRepo(
        UserManager<User> _manager,
        SignInManager<User> _signinManager,
        [FromKeyedServices("register")] IValidator<RegisterDto> _registerValidator,
        [FromKeyedServices("login")] IValidator<LoginDto> _loginValidator,
        [FromKeyedServices("updateUser")] IValidator<UpdateUserDto> _updateUserValidator,
        ITokens _tokenService,
        AppDBContext _context,
        IEmailVerification _emailVerification
    ) : IUser
    {

        private readonly UserManager<User> manager = _manager;
        private readonly SignInManager<User> signinManager = _signinManager;
        private readonly IValidator<RegisterDto> registerValidator = _registerValidator;
        private readonly IValidator<LoginDto> loginvalidator = _loginValidator;
        private readonly IValidator<UpdateUserDto> updateUserValidator = _updateUserValidator;
        private readonly ITokens tokenService = _tokenService;
        private readonly AppDBContext context = _context;
        private readonly IEmailVerification emailVerification = _emailVerification;

        public async Task DeleteAll()
        {
            var users = await context.Users.ToListAsync();
            foreach (var user in users)
            {
                context.Users.Remove(user);
            }
            await context.SaveChangesAsync();
        }

        public async Task<User?> DeleteUser(string userId)
        {
            User? user = await context.Users.Where(u => u.Id == userId).FirstAsync();
            if (user == null) return null;
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<string?> GenerateResetPasswordToken(string userId)
        {
            var user = await context.Users.Where(u => u.Id == userId).FirstAsync();
            if (user == null) return null;
            string token = await manager.GeneratePasswordResetTokenAsync(user);
            string numericString = new string(token.Where(char.IsDigit).ToArray())[..5];
            await emailVerification.SendVerificationEmail(user.Email, "Password Reset", token);
            await emailVerification.CreateVerification(new EmailVerification
            {
                code = int.Parse(numericString),
                userId = user.Id
            });

            await context.SaveChangesAsync();
            return token;
        }

        public async Task<User?> getUser(string userId)
        {
            User? user = await context.Users
            .Include(u => u.media).Where(u => u.Id == userId).FirstAsync();
            if (user == null) return null;
            return user;
        }

        public async Task<LoginResponse?> Login(LoginDto dto)
        {
            ValidationResult result = loginvalidator.Validate(dto);
            if (result.IsValid)
            {
                var user = await manager.Users.FirstOrDefaultAsync(u => u.UserName == dto.username);
                if (user == null) return null;
                var response = await signinManager.CheckPasswordSignInAsync(user, dto.password, false);
                var role = await manager.IsInRoleAsync(user, "admin");
                if (response.Succeeded)
                {
                    return new LoginResponse
                    {
                        Id = user.Id,
                        token = tokenService.CreateToken(user, role)
                    };
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task<User?> PasswordReset(string userId, PasswordDto dto)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null) return null;
            var result = await manager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                throw new Exception("Cannot reset password , try again!");
            }
        }

        public async Task<User?> Register(RegisterDto dto)
        {
            var result = registerValidator.Validate(dto);
            if (result.IsValid)
            {
                var user = new User
                {
                    UserName = dto.username,
                    Email = dto.email,
                };
                var model = await manager.GetUserNameAsync(user);
                var users = await context.Users.Where(u => u.UserName == model).ToListAsync();
                if (users.Count > 0)
                {
                    throw new Exception("username must be unique!");
                }
                else
                {
                    var response = await manager.CreateAsync(user, dto.password);
                    if (response.Succeeded)
                    {
                        if (dto.role == utils.Roles.Admin)
                        {
                            await manager.AddToRoleAsync(user, "admin");
                        }
                        else
                        {
                            await manager.AddToRoleAsync(user, "customer");
                        }


                        // send email verification
                        int code = emailVerification.GenerateCode();
                        await emailVerification.SendVerificationEmail(dto.email, "", code.ToString());
                        await emailVerification.CreateVerification(new EmailVerification
                        {
                            code = code,
                            userId = user.Id
                        });

                    }
                }

                return user;
            }
            else
            {
                throw new ValidationException(result.Errors);
            }


        }

        public async Task<User?> updateUser(string userId, UpdateUserDto dto)
        {
            var user = await context.Users.Where(u => u.Id == userId).FirstAsync();
            if (user == null) return null;
            var result = updateUserValidator.Validate(dto);
            if (result.IsValid)
            {
                user.UserName = dto.Username;
                user.NormalizedUserName = dto.Username.ToUpper();
                user.First_name = dto.First_name;
                user.Last_Name = dto.Last_Name;
                user.Bio = dto.Bio;
                user.Gender = dto.Gender;
                user.Age = dto.Age;
                user.mediaId = dto.fileId;
                await context.SaveChangesAsync();
                return user;
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task<bool?> VerifyUser(string userId, int verificationCode)
        {
            var verification = await context.EmailVerifications.Where(e => e.code == verificationCode).FirstAsync();

            var user = await context.Users.Where(u => u.Id == userId).FirstAsync();
            if (verification == null || user == null) return null;

            if (verification.code == verificationCode)
            {
                user.EmailConfirmed = true;
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
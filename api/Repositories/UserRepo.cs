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
        ITokens _tokenService,
        AppDBContext _context
    ) : IUser
    {

        private readonly UserManager<User> manager = _manager;
        private readonly SignInManager<User> signinManager = _signinManager;
        private readonly IValidator<RegisterDto> registerValidator = _registerValidator;
        private readonly IValidator<LoginDto> loginvalidator = _loginValidator;
        private readonly ITokens tokenService = _tokenService;
        private readonly AppDBContext context = _context;


        public async Task<LoginResponse?> Login(LoginDto dto)
        {
            ValidationResult result = loginvalidator.Validate(dto);
            if (result.IsValid)
            {
                User? user = await manager.Users.FirstOrDefaultAsync(u => u.UserName == dto.username);
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

        public async Task<User> Register(RegisterDto dto)
        {
            ValidationResult result = registerValidator.Validate(dto);
            if (result.IsValid)
            {
                User user = new User
                {
                    UserName = dto.username,
                    Email = dto.email,
                };

                var model = await manager.CreateAsync(user, dto.password);
                if (model.Succeeded)
                {
                    if (dto.role == utils.Roles.Admin)
                    {
                        await manager.AddToRoleAsync(user, "admin");
                    }
                    else if (dto.role == utils.Roles.Customer)
                    {
                        await manager.AddToRoleAsync(user, "customer");
                    }
                }
                return user;
            }
            else
            {
                throw new ValidationException(result.Errors);
            }


        }
    }
}
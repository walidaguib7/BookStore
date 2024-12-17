using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.models;
using api.models.users.dtos;
using api.models.users.validations;
using api.Repositories;
using api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Config
{
    public static class ServicesContainer
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITokens, TokenRepo>();
            services.AddScoped<IUser, UserRepo>();
        }

        public static void AddValidations(this IServiceCollection services)
        {
            services.AddKeyedScoped<IValidator<RegisterDto>, RegisterValidation>("register");
            services.AddKeyedScoped<IValidator<LoginDto>, LoginValidation>("login");
        }

        public static void addDB(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void addIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 12;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();
        }

        public static void addAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthentication(options =>
           {
               options.DefaultAuthenticateScheme =
               options.DefaultChallengeScheme =
               options.DefaultForbidScheme =
               options.DefaultScheme =
               options.DefaultSignInScheme =
               options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

           })
               .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {

                   ValidateIssuer = true,
                   ValidIssuer = builder.Configuration["JWT:Issuer"],
                   ValidateAudience = true,
                   ValidAudience = builder.Configuration["JWT:Audience"],
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SignInKey"]))


               };
           }); ;
        }
    }
}
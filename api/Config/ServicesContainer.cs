using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.models;
using api.models.authors.dtos;
using api.models.authors.validations;
using api.models.media.dtos;
using api.models.media.validations;
using api.models.users.dtos;
using api.models.users.validations;
using api.Repositories;
using api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace api.Config
{
    public static class ServicesContainer
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICache, CachingRepo>();
            services.AddTransient<ITokens, TokenRepo>();
            services.AddScoped<IUser, UserRepo>();
            services.AddScoped<IEmailVerification, EmailVerificationsRepo>();
            services.AddScoped<IAuthor, AuthorRepo>();
            services.AddScoped<ICache, CachingRepo>();
            services.AddScoped<IMedia, MediaRepo>();
        }

        public static void AddValidations(this IServiceCollection services)
        {
            services.AddKeyedScoped<IValidator<RegisterDto>, RegisterValidation>("register");
            services.AddKeyedScoped<IValidator<LoginDto>, LoginValidation>("login");
            services.AddKeyedScoped<IValidator<UpdateUserDto>, UpdateUserValidation>("updateUser");
            // author validations
            services.AddKeyedScoped<IValidator<CreateAuthorDto>, CreateAuthorValidation>("createAuthor");
            services.AddKeyedScoped<IValidator<UpdateAuthorDto>, UpdateAuthorValidation>("updateAuthor");
            //media validations
            services.AddKeyedScoped<IValidator<CreateFileDto>, CreateFileValidation>("createFile");
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
                options.User.RequireUniqueEmail = true;
                options.Password.RequireUppercase = false;           // Uppercase is not required
                options.Password.RequireLowercase = true;
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

        public static void AddMailing(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var smtpSettings = builder.Configuration.GetSection("Smtp");
            services.AddSingleton(smtpSettings);
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
        });
        }

        public static void AddRedis(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddSingleton<IConnectionMultiplexer>
            (
                x => ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("RedisConnectionString"))
            )
            ;
        }
    }
}
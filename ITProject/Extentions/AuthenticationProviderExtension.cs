using System;
using System.Text;
using Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AuthService;

namespace ITProject.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Берем настройки из appsettings.json из секции "JwtToken"
            var configSection = configuration.GetSection("JwtToken");
            var jwtConfig = configSection.Get<JwtTokenConfiguration>();

            if (jwtConfig == null)
            {
                throw new InvalidOperationException("Секция 'JwtToken' не найдена или неверно настроена в appsettings.json");
            }

            // Настраиваем правила валидации токена
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.IncludeErrorDetails = true;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
                    };
                });

            return services;
        }
    }
}
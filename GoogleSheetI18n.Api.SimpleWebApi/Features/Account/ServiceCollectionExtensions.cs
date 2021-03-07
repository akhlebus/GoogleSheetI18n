﻿using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GoogleSheetI18n.Api.SimpleWebApi.Features.Account
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInMemoryAuthentication(this IServiceCollection services)
        {
            var users = new Dictionary<string, string>
            {
                {"john", "john123"},
                {"jane", "jane123"}
            };

            services.AddSingleton<IUserService>(new UserService(users));

            var key = Encoding.ASCII.GetBytes(TokenConfigs.Secret);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}

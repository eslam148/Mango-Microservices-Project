﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.ProductAPI.Extentions
{
    public static class WebApplicationBuilderExtentions
    {
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder) {
             var settingSection = builder.Configuration.GetSection("ApiSettings");
            var secret = settingSection.GetValue<string>("Secret");
            var Audience = settingSection.GetValue<string>("Audience");
            var Issuer = settingSection.GetValue<string>("Issuer");

            var Key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    ValidateAudience = true,
                };
            });

            return builder;
        }
    }
}

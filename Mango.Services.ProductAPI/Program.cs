
using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Mango.Services.ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Add services to the container.
            #region DatabaseConfigration
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DbKey"));
            });
            #endregion
            #region AutoMaper Configration 
            IMapper? mapper = MappingConfig.RegisterMaps().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //#region Create Bearer API Key
            //builder.Services.AddSwaggerGen(option =>
            //{
            //    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer"
            //    });
            //    option.AddSecurityRequirement(new OpenApiSecurityRequirement
            //     {
            //            {
            //                new OpenApiSecurityScheme
            //                {
            //                    Reference= new OpenApiReference
            //                    {
            //                        Type=ReferenceType.SecurityScheme,
            //                        Id=JwtBearerDefaults.AuthenticationScheme
            //                    }
            //                }, new string[]{}
            //            }
            //     });
            //});
            //#endregion
            //builder.AddAppAuthentication();


            builder.AddAppAuthentication();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}

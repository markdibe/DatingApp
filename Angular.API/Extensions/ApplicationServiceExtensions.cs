using Angular.API.DTO;
using Angular.API.Entities;
using Angular.API.Helpers;
using Angular.API.Interfaces;
using Angular.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
           
            services.AddDbContext<ApplicationDbContext>(options =>
            { options.UseSqlServer(config.GetConnectionString("default")); });
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}

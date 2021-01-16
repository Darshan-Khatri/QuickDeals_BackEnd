using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickDeals.Core.IRepositories;
using QuickDeals.Helper;
using QuickDeals.Persistance;
using QuickDeals.Persistance.Repositories;
using QuickDeals.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
          
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);


            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DataBaseConnection"));
            });

            return services;
        }
    }
}

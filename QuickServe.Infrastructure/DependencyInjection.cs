﻿using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using QuickServe.Application.Interfaces.Authenticates;
using QuickServe.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickServe.Application.IRepositories.Users;
using QuickServe.Application.Interfaces.Users;
using QuickServe.Application.Services.Users;
using QuickServe.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using QuickServe.Infrastructure.Mappers;
using QuickServe.Application;
using QuickServe.Application.IRepositories.Roles;
using QuickServe.Infrastructure.Repositories.Roles;
using QuickServe.Application.Services.Authenticates;
using QuickServe.Application.IRepositories.RefreshTokens;
using QuickServe.Infrastructure.Repositories.RefreshTokens;
using QuickServe.Application.IRepositories.Ingredient_Type;
using QuickServe.Application.Interfaces.IngredientTypes;
using QuickServe.Application.Services.Ingredient_Type;
using QuickServe.Infrastructure.Repositories.IngredientTypeRepository;

namespace QuickServe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, string databaseConnection)
        {
            if (string.IsNullOrEmpty(databaseConnection))
            {
                throw new ArgumentNullException(nameof(databaseConnection), "Connection string cannot be null or empty.");
            }
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IIngredientTypeRepository, IngredientTypeRepository>();
            services.AddScoped<IIngredientTypeService, IngredientTypeService>();

            services.AddSingleton<ICurrentTime, CurrentTime>();
            services.AddDbContext<QuickServeContext>(options =>
            {
                options.UseSqlServer(databaseConnection);
            });
            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);
            return services;
        }
    }
}

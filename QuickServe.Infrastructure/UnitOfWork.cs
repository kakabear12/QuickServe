using Microsoft.EntityFrameworkCore.Migrations;
using QuickServe.Application.Interfaces;
using QuickServe.Application.IRepositories.Ingredient_Type;
using QuickServe.Application.IRepositories.RefreshTokens;
using QuickServe.Application.IRepositories.Roles;
using QuickServe.Application.IRepositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuickServeContext _quickServeContext;

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IIngredientTypeRepository _ingredientTypeRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UnitOfWork(QuickServeContext quickServeContext, IUserRepository userRepository, IRoleRepository roleRepository,
            IRefreshTokenRepository refreshTokenRepository, IIngredientTypeRepository ingredientTypeRepository)
        {
            _quickServeContext = quickServeContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _ingredientTypeRepository = ingredientTypeRepository;
        }
        public IUserRepository UserRepository => _userRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;
        public IIngredientTypeRepository IngredientTypeRepository => _ingredientTypeRepository;
        public async Task<int> SaveChangeAsync()
        {
            return await _quickServeContext.SaveChangesAsync();
        }
    }
}

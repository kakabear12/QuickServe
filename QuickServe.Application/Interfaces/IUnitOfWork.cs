using QuickServe.Application.IRepositories.Ingredient_Type;
using QuickServe.Application.IRepositories.RefreshTokens;
using QuickServe.Application.IRepositories.Roles;
using QuickServe.Application.IRepositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IIngredientTypeRepository IngredientTypeRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}

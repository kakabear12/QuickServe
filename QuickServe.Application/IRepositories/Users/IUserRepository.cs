using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.IRepositories.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAndPasswordHash(string email, string passwordHash);
        Task<bool> CheckEmailNameExited(string email);
        Task<User> GetUserByConfirmationToken(string token);
        Task<IEnumerable<User>> SearchUserByNameAsync(string name);
        Task<IEnumerable<User>> SearchUserByRoleNameAsync(string roleName);
        Task<IEnumerable<User>> GetSortedUserAsync();
        Task<bool> CheckUsernameExited(string username);
        Task<User> GetUserWithRefreshTokenById(int id);
        Task<User> GetUserInfoByIdAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using QuickServe.Application.Interfaces;
using QuickServe.Application.IRepositories;
using QuickServe.Application.IRepositories.Users;
using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly QuickServeContext _dbContext;
        public UserRepository(
            QuickServeContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
        )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public Task<bool> CheckEmailNameExited(string email) =>
            _dbContext.Users.AnyAsync(u => u.Email == email);

        public async Task<User> GetUserByEmailAndPasswordHash(string email, string passwordHash)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(
                record => record.Email == email && record.Password == passwordHash
            );
            if (user is null)
            {
                throw new Exception("Email & password is not correct");
            }

            return user;
        }

        public async Task<User> GetUserByConfirmationToken(string token)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(
                u => u.ConfirmationToken == token
            );
        }

        public async Task<IEnumerable<User>> SearchUserByNameAsync(string name)
        {
            return await _dbContext.Users.Where(u => u.Username.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchUserByRoleNameAsync(string roleName)
        {
            return await _dbContext.Users
                .Where(u => u.Role.RoleName.Contains(roleName))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetSortedUserAsync()
        {
            return await _dbContext.Users.OrderByDescending(a => a.CreatedDate).ToListAsync();
        }

        public async Task<bool> CheckUsernameExited(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User> GetUserWithRefreshTokenById(int id)
        {
            return await _dbContext.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(e => e.Id == id); 
        }

        public async Task<User> GetUserInfoByIdAsync(int id)
        {
            return await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _dbContext.Users.Include(u=>u.Role).ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using QuickServe.Application.Interfaces;
using QuickServe.Application.IRepositories.Roles;
using QuickServe.Application.IRepositories.Users;
using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Infrastructure.Repositories.Roles
{
    public class RoleRepository :  IRoleRepository
    {
        private readonly QuickServeContext _dbContext;
        public RoleRepository(QuickServeContext context)
        {
            _dbContext = context;
        }

        public async Task<Role> GetRoleByRoleName(string name)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == name);
            if(role == null)
            {
                throw new Exception("Vai trò không được tìm thấy.");
            }
            return role;
        }
    }
}

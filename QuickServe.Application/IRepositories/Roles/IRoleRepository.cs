using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.IRepositories.Roles
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByRoleName(string name);
    }
}

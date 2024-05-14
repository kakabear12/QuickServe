using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.IRepositories.RefreshTokens
{
    public interface IRefreshTokenRepository
    {
        Task RemoveRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByUserId(int userId);
    }
}

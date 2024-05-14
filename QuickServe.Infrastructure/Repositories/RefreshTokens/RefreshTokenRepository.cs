using QuickServe.Application.IRepositories.RefreshTokens;
using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Infrastructure.Repositories.RefreshTokens
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly QuickServeContext _dbContext;
        public RefreshTokenRepository(QuickServeContext context)
        {
            _dbContext = context;
        }

        public async Task<RefreshToken> GetRefreshTokenByUserId(int userId)
        {
           return await _dbContext.RefreshTokens.FirstOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task  RemoveRefreshToken(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}

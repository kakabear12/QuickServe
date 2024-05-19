using QuickServe.Application.Interfaces;
using QuickServe.Application.IRepositories.Ingredient_Type;
using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Infrastructure.Repositories.IngredientTypeRepository
{
    public class IngredientTypeRepository : GenericRepository<IngredientType>, IIngredientTypeRepository
    {
        private readonly QuickServeContext _dbContext;
        public IngredientTypeRepository(QuickServeContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }
    }
}

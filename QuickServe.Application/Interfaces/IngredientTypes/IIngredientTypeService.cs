using QuickServe.Application.Services;
using QuickServe.Application.ViewModels.IngredientType;
using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.Interfaces.IngredientTypes
{
    public interface IIngredientTypeService
    {
        Task<ServiceResponse<IEnumerable<IngredientTypeDTO>>> GetAllIngredientTypeAsync();
        Task<ServiceResponse<IngredientTypeDTO>> GetIngredientTypeAsync(int id);
        Task<ServiceResponse<IngredientTypeDTO>> CreateIngredientTypeAsync(AddUpdateIngredientTypeDTO IngredientTypeDto, int userId);
        Task<ServiceResponse<IngredientTypeDTO>> UpdateIngredientTypeAsync(int id, AddUpdateIngredientTypeDTO IngredientTypeDto, int userId);
        Task<ServiceResponse<bool>> DeleteIngredientTypeAsync(int id);
        
    }
}

using Microsoft.AspNetCore.Mvc;
using QuickServe.Application.Interfaces.IngredientTypes;
using QuickServe.Application.ViewModels.IngredientType;
using System.Security.Claims;

namespace QuickServe.WebAPI.Controllers
{
    public class IngredientTypeController : BaseController
    {
        private readonly IIngredientTypeService _IngredientTypeService;
        private int UserID => int.Parse(FindClaim(ClaimTypes.NameIdentifier));
        private string FindClaim(string claimName)
        {

            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(claimName);

            if (claim == null)
            {
                return null;
            }

            return claim.Value;

        }
        public IngredientTypeController(IIngredientTypeService IngredientTypeService)
        {
            _IngredientTypeService = IngredientTypeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetIngredientTypeList()
        {
            var result = await _IngredientTypeService.GetAllIngredientTypeAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngredientType(int id)
        {
            var result = await _IngredientTypeService.GetIngredientTypeAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredientType([FromBody] AddUpdateIngredientTypeDTO createdIngredientTypeDTO)
        {
            var result = await _IngredientTypeService.CreateIngredientTypeAsync(createdIngredientTypeDTO, UserID);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }

        /*[Authorize(Roles = "admin")]*/
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredientType(int id, [FromBody] AddUpdateIngredientTypeDTO IngredientTypeDTO)
        {
            var result = await _IngredientTypeService.UpdateIngredientTypeAsync(id, IngredientTypeDTO, UserID);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredientType(int id)
        {
            var result = await _IngredientTypeService.DeleteIngredientTypeAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        
    }
}

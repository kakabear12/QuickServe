using Microsoft.AspNetCore.Mvc;
using QuickServe.Application.Interfaces.Users;
using QuickServe.Application.Services;
using QuickServe.Application.ViewModels.UserDTO;
using QuickServe.Domain.Entities;

namespace QuickServe.WebAPI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _accountService;

        public UserController(IUserService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountList()
        {
            var users = await _accountService.GetUserAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountAsyncById(int id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            return Ok(user);
        }
        [HttpGet("Profile/{id}")]
        public async Task<IActionResult> GetAccountProfileById(int id)
        {
            var user = await _accountService.GetUserProfileById(id);
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreatedUserDTO createdAccountDTO)
        {
            var createdAccount = await _accountService.CreateUserAsync(createdAccountDTO);

            

            if (!createdAccount.Success)
            {
                return BadRequest(createdAccount);
            }
            else
            {
                return Ok(createdAccount);
            }
        }

        [HttpPut("Profile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileDTO accountDTO)
        {
            var updateProfile = await _accountService.UpdateProfileAsync(id, accountDTO);
            if (!updateProfile.Success)
            {
                return NotFound(updateProfile);
            }
            return Ok(updateProfile);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO accountDTO)
        {
            var updatedUser = await _accountService.UpdateUserAsync(id, accountDTO);
            if (!updatedUser.Success)
            {
                return NotFound(updatedUser);
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deletedUser = await _accountService.DeleteUserAsync(id);
            if (!deletedUser.Success)
            {
                return NotFound(deletedUser);
            }
            return Ok(deletedUser);
        }

        /*HttpPut("Status/{id}")]
        public async Task<IActionResult> UpdateIsDelete(int id, [FromQuery] bool? isDeleted)
        {
            var updatedUser = await _accountService.UpdateIsDelete(id, isDeleted);
            if (!updatedUser.Success)
            {
                return NotFound(updatedUser);
            }
            return Ok(updatedUser);
        */
    }
}

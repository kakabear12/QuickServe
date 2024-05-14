using QuickServe.Application.Services;
using QuickServe.Application.ViewModels.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<ServiceResponse<IEnumerable<UserDTO>>> GetUserAsync();
        Task<ServiceResponse<UserDTO>> CreateUserAsync(CreatedUserDTO createdUserDTO);
        Task<ServiceResponse<UserDTO>> UpdateUserAsync(int id, UserDTO userDTO);
        Task<ServiceResponse<bool>> DeleteUserAsync(int id);
        Task<ServiceResponse<string>> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDto);
        Task<ServiceResponse<UpdateProfileDTO>> UpdateProfileAsync(int id, UpdateProfileDTO userDTO);
        Task<ServiceResponse<UserDTO>> GetUserByIdAsync(int id);
        Task<ServiceResponse<ProfileUserDTO>> GetUserProfileById(int id);
    }
}

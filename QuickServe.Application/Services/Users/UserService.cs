using AutoMapper;
using QuickServe.Application.Interfaces;
using QuickServe.Application.Interfaces.Users;
using QuickServe.Application.Utils;
using QuickServe.Application.ViewModels.UserDTO;
using QuickServe.Domain.Entities;
using QuickServe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserDTO>> CreateUserAsync(CreatedUserDTO createdUserDTO)
        {
            var response = new ServiceResponse<UserDTO>();

           

            var exist = await _unitOfWork.UserRepository.CheckEmailNameExited(createdUserDTO.Email);
            if (exist)
            {
                response.Success = false;
                response.Message = "Email đã tồn tại.";
                return response;
            }
            // Kiểm tra giới tính
            if (!Enum.IsDefined(typeof(GenderType), createdUserDTO.Gender))
            {
                response.Success = false;
                response.Message = "Giới tính không hợp lệ. Giới tính phải là Male, Female hoặc Other.";
                return response;
            }

            // Kiểm tra roleName
            if (!Enum.IsDefined(typeof(RoleName), createdUserDTO.RoleName))
            {
                response.Success = false;
                response.Message = "Tên vai trò không hợp lệ. Tên vai trò phải là Admin| BrandManager| StoreManager| Staff| Customer.";
                return response;
            }

            if (createdUserDTO.Birthday > DateTime.Now)
            {
                response.Success = false;
                response.Message = "Ngày sinh không thể trong tương lai.";
                return response;
            }

            try
            {
                var account = _mapper.Map<User>(createdUserDTO);
             
                if (createdUserDTO.RoleName.Trim().Equals(RoleName.Admin))
                {
                    account.Username = await GenerateUniqueUsernameAsync("AD"); 
                } else if (createdUserDTO.RoleName.Trim().Equals(RoleName.StoreManager))
                {
                    account.Username = await GenerateUniqueUsernameAsync("SM");
                } else if (createdUserDTO.RoleName.Trim().Equals(RoleName.BrandManager))
                {
                    account.Username = await GenerateUniqueUsernameAsync("BM");
                } else if (createdUserDTO.RoleName.Trim().Equals(RoleName.Staff))
                {
                    account.Username = await GenerateUniqueUsernameAsync("ST");
                } else
                {
                    response.Success = false;
                    response.Message = "Vai trò không hợp lệ.";
                    return response;
                }

                

                account.Password = HashPassword.HashWithSHA256("123456@");

                account.Status = UserStatus.Active.ToString();

                account.Role = await _unitOfWork.RoleRepository.GetRoleByRoleName(createdUserDTO.RoleName);


                await _unitOfWork.UserRepository.AddAsync(account);

                _unitOfWork.UserRepository.Update(account);

                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    var accountDTO = _mapper.Map<UserDTO>(account);
                    response.Data = accountDTO;
                    response.Success = true;
                    response.Message = "Tạo người dùng thành công.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Lỗi lưu người dùng.";
                }
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Đã xảy ra lỗi cơ sở dữ liệu.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Lỗi!!!";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(int id)
        {
            var response = new ServiceResponse<bool>();

            var exist = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (exist == null)
            {
                response.Success = false;
                response.Message = "User is not existed";
                return response;
            }

            try
            {
               // _unitOfWork.UserRepository.SoftRemove(exist);

                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.Success = true;
                    response.Message = "User deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error deleting the User.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ServiceResponse<IEnumerable<UserDTO>>> GetUserAsync()
        {
            var _response = new ServiceResponse<IEnumerable<UserDTO>>();

            try
            {
                var accounts = await _unitOfWork.UserRepository.GetUsersAsync();

                var accountDTOs = new List<UserDTO>();

                foreach (var acc in accounts)
                {
                    accountDTOs.Add(_mapper.Map<UserDTO>(acc));
                }

                if (accountDTOs.Count != 0)
                {
                    _response.Success = true;
                    _response.Message = "User retrieved successfully";
                    _response.Data = accountDTOs;
                }
                else
                {
                    _response.Success = true;
                    _response.Message = "Not have Account";
                }

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        public async Task<ServiceResponse<UserDTO>> UpdateUserAsync(int id, UserDTO accountDTO)
        {
            var response = new ServiceResponse<UserDTO>();

            try
            {
                var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(id);

                if (existingUser == null)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                    return response;
                }

                if ((bool)existingUser.Status.Equals("Deleted"))
                {
                    response.Success = false;
                    response.Message = "User is deleted in system";
                    return response;
                }

                // Map accountDT0 => existingUser
                var updated = _mapper.Map(accountDTO, existingUser);
               

                _unitOfWork.UserRepository.Update(updated);

                var updatedUserDto = _mapper.Map<UserDTO>(updated);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.Data = updatedUserDto;
                    response.Success = true;
                    response.Message = "Account updated successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error updating the account.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ServiceResponse<string>> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDto)
        {
            var response = new ServiceResponse<string>();

            // Kiểm tra xem người dùng có tồn tại không
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            // Xác minh mật khẩu cũ
            var hashedOldPassword = HashPassword.HashWithSHA256(changePasswordDto.OldPassword);
            if (user.Password != hashedOldPassword)
            {
                response.Success = false;
                response.Message = "Incorrect old password";
                return response;
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới (nếu cần)
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            {
                response.Success = false;
                response.Message = "New password and confirmation do not match";
                return response;
            }

            // Băm mật khẩu mới
            var hashedNewPassword = HashPassword.HashWithSHA256(changePasswordDto.NewPassword);

            // Lưu mật khẩu mới vào cơ sở dữ liệu
            user.Password = hashedNewPassword;
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

            if (!isSuccess)
            {
                response.Success = true;
                response.Message = "Password changed fail.";
                return response;
            }

            response.Success = true;
            response.Message = "Password changed successfully.";
            return response;

        }



      
        public async Task<ServiceResponse<UpdateProfileDTO>> UpdateProfileAsync(int id, UpdateProfileDTO accountDTO)
        {
            var response = new ServiceResponse<UpdateProfileDTO>();
            var exist = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (exist == null)
            {
                response.Success = false;
                response.Message = "account not found";
                return response;
            }
            try
            {
                var acc = _mapper.Map(accountDTO, exist);
                _unitOfWork.UserRepository.Update(acc);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.Success = true;
                    response.Message = "User updated successfully";
                    response.Data = _mapper.Map<UpdateProfileDTO>(acc);
                }
                else
                {
                    response.Success = false;
                    response.Message = "User News failed";
                }
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceResponse<UserDTO>> GetUserByIdAsync(int id)
        {
            var response = new ServiceResponse<UserDTO>();
            try
            {
                var acc = await _unitOfWork.UserRepository.GetAsync(x => x.Id == id);
                if (acc != null)
                {
                    response.Success = true;
                    response.Message = "User retrieved successfully";
                    response.Data = _mapper.Map<UserDTO>(acc);
                }
                else
                {
                    response.Success = true;
                    response.Message = "User not found";
                }
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return response;
        }

        public async Task<ServiceResponse<ProfileUserDTO>> GetUserProfileById(int id)
        {
            var response = new ServiceResponse<ProfileUserDTO>();
            try
            {
                var acc = await _unitOfWork.UserRepository.GetAsync(x => x.Id == id);
                if (acc != null)
                {
                    response.Success = true;
                    response.Message = "User retrieved successfully";
                    response.Data = _mapper.Map<ProfileUserDTO>(acc);
                }
                else
                {
                    response.Success = true;
                    response.Message = "User not found";
                }
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return response;
        }

        public async Task<string> GenerateUniqueUsernameAsync(string prefix)
        {
            Random random = new Random();
            string username;
            do
            {
                int randomNumber = random.Next(1000, 9999); 
                username = prefix + randomNumber.ToString();
            } while (await _unitOfWork.UserRepository.CheckUsernameExited(username)); 
            return username;
        }
    }
}

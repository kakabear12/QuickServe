using AutoMapper;
using QuickServe.Application.Commons;
using QuickServe.Application.Interfaces.Authenticates;
using QuickServe.Application.Interfaces;
using QuickServe.Application.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using QuickServe.Application.ViewModels.UserDTO;
using QuickServe.Domain.Enums;
using QuickServe.Domain.Entities;
using QuickServe.Application.ViewModels.RefreshTokenDTO;
using QuickServe.Application.IRepositories.Users;

namespace QuickServe.Application.Services.Authenticates
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;

        private readonly IMapper _mapper;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            ICurrentTime currentTime,
            AppConfiguration configuration,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;

            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserDTO>> GetProfilerAsync(int userId)
        {
            var response = new ServiceResponse<UserDTO>();
            var eixted = await _unitOfWork.UserRepository.GetUserInfoByIdAsync(userId);
            if(eixted == null)
            {
                response.Success = false;
                response.Message = "Người dùng không tìm thấy.";
                return response;
            }

            var user = _mapper.Map<UserDTO>(eixted);
           
            response.Success = true;
            response.Message = "Lấy thông tin người dùng thành công.";
            response.Data = user;
            return response;
        }

        public async Task<ServiceResponse<TokenResponse>> LoginAsync(AuthenUserDTO authenAccountDto)
        {
            var response = new ServiceResponse<TokenResponse>();
            try
            {
                var hashedPassword = HashPassword.HashWithSHA256(authenAccountDto.Password.Trim());
                var user = await _unitOfWork.UserRepository.GetUserByEmailAndPasswordHash(
                    authenAccountDto.Email.Trim(),
                    hashedPassword
                );

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Email hoặc mật khẩu không đúng.";
                    return response;
                }
                if (user.Status == UserStatus.InActive.ToString())
                {
                    response.Success = true;
                    response.Message = "Tài khoản bị vô hiệu hoá.";
                    return response;
                }
                /*if (user.IsConfirmed == false)
                {
                    response.Success = true;
                    response.Message = "Please confirm via link in your email box";
                    return response;
                */
                var token = await GenerateJsonWebTokenString.GenerateTokensAsync(_unitOfWork, user,
                    _configuration,
                    _configuration.JWTSection.SecretKey,
                    _currentTime.GetCurrentTime()
                );
                
                response.Success = true;
                response.Message = "Đăng nhập thành công.";
                response.Data = new TokenResponse
                {
                    AccessToken = token.Item1,
                    RefreshToken = token.Item2
                };
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

        public Task<ServiceResponse<string>> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<string>> RefreshToken(int userId, RefreshTokenRequest refreshTokenRequest)
        {
            var response = new ServiceResponse<string>();
            var validateRefreshTokenResponse = await GenerateJsonWebTokenString.ValidateRefreshTokenAsync(userId,refreshTokenRequest, _unitOfWork);

            if (!validateRefreshTokenResponse.Success)
            {
                response.Success = false;
                response.Message = validateRefreshTokenResponse.Message;
                return response;
            }
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            var tokenResponse = GenerateJsonWebTokenString.CreateAccessToken(user,
                    _configuration,
                    _configuration.JWTSection.SecretKey,
                    _currentTime.GetCurrentTime());
            if(tokenResponse!=null)
            {
                response.Data = tokenResponse;
                response.Success = true;
                response.Message = "Tạo mới mã đăng nhập thành công.";
                return response;
            }
            response.Success = false;
            response.Message = "Tạo mới mã đăng nhập thất bại.";
            return response;
        }

        public async Task<ServiceResponse<RegisterUserResponse>> RegisterAsync(RegisterUserDTO registerAccountDTO)
        {
            var response = new ServiceResponse<RegisterUserResponse>();

            try
            {
                var exist = await _unitOfWork.UserRepository.CheckEmailNameExited(registerAccountDTO.Email.Trim());
                if (exist)
                {
                    response.Success = false;
                    response.Message = "Email đã tồn tại.";
                    return response;
                }

                var account = _mapper.Map<User>(registerAccountDTO);
                account.Password = HashPassword.HashWithSHA256(
                    registerAccountDTO.Password.Trim()
                );
                
                if(await _unitOfWork.UserRepository.CheckUsernameExited(registerAccountDTO.Username.Trim()))
                {
                    response.Success = false;
                    response.Message = "Tên người dùng đã tồn tại.";
                    return response;
                }

                // Kiểm tra giới tính
                if (!Enum.IsDefined(typeof(GenderType), registerAccountDTO.Gender))
                {
                    response.Success = false;
                    response.Message = "Giới tính không hợp lệ. Giới tính phải là Male, Female hoặc Other.";
                    return response;
                }


                if (registerAccountDTO.Birthday > DateTime.Now)
                {
                    response.Success = false;
                    response.Message = "Ngày sinh không thể trong tương lai.";
                    return response;
                }
                


                // Tạo token ngẫu nhiên
                account.ConfirmationToken = Guid.NewGuid().ToString();

                account.Status = UserStatus.Active.ToString();
                DateTime now = _currentTime.GetCurrentTime();
                account.CreatedBy = account.Username;
                account.CreatedDate = now;
                account.UpdatedDate = now;
                account.UpdatedBy = account.Username;

                
                account.Role = await _unitOfWork.RoleRepository.GetRoleByRoleName("Customer");
                await _unitOfWork.UserRepository.AddAsync(account);

                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    var accountDTO = _mapper.Map<RegisterUserResponse>(account);
                    response.Data = accountDTO; // Chuyển đổi sang AccountDTO
                    response.Success = true;
                    response.Message = "Đăng ký tài khoản thành công.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Lỗi lưu tài khoản.";
                }

                //var confirmationLink = $"https://mixfood.azurewebsites.net/swagger/confirm?token={account.ConfirmationToken}";
                //var emailSent = await SendEmail.SendConfirmationEmail(account.Email, confirmationLink);
                /*if (!emailSent)
                {
                    // Xử lý khi gửi email không thành công
                    response.Success = false;
                    response.Message = "Error sending confirmation email.";
                    return response;
                }
                else
                {
                    
                }*/
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
    }
}

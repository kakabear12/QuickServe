using QuickServe.Application.Services;
using QuickServe.Application.ViewModels.RefreshTokenDTO;
using QuickServe.Application.ViewModels.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.Interfaces.Authenticates
{
    public interface IAuthenticationService
    {
        public Task<ServiceResponse<RegisterUserResponse>> RegisterAsync(RegisterUserDTO registerAccountDTO);
        public Task<ServiceResponse<TokenResponse>> LoginAsync(AuthenUserDTO accountDto);
        public Task<ServiceResponse<string>> RefreshToken(int userId,RefreshTokenRequest refreshTokenRequest);
        public Task<ServiceResponse<string>> LogoutAsync();
        public Task<ServiceResponse<UserDTO>> GetProfilerAsync(int userId);
    }
}

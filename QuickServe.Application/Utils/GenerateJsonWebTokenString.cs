using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuickServe.Application.Commons;
using QuickServe.Application.Interfaces;
using QuickServe.Application.Services;
using QuickServe.Application.ViewModels.RefreshTokenDTO;
using QuickServe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.Utils
{
    public static class GenerateJsonWebTokenString
    {

        public static string CreateAccessToken(this User user, AppConfiguration appSettingConfiguration, string secretKey, DateTime now)
        {
            try
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,  user.Email),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.RoleName)
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(issuer: appSettingConfiguration.JWTSection.Issuer,
                    audience: appSettingConfiguration.JWTSection.Audience,
                    claims: claims,
                    expires: now.AddMinutes(30),
                    signingCredentials: cred) ;
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<string> GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            await System.Threading.Tasks.Task.Run(() => randomNumberGenerator.GetBytes(secureRandomBytes));

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }
        public static async Task<Tuple<string, string>> GenerateTokensAsync(IUnitOfWork unitOfWork,User user, AppConfiguration appSettingConfiguration, string secretKey, DateTime now)
        {
            var accessToken = CreateAccessToken(user, appSettingConfiguration, secretKey, now);
            var refreshToken = await GenerateRefreshToken();

            var userRecord = await unitOfWork.UserRepository.GetUserWithRefreshTokenById(user.Id);

            if (userRecord == null)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }

            var salt = GetSecureSalt();

            var refreshTokenHashed = HashUsingPbkdf2(refreshToken, salt);

            if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
            {
                await RemoveRefreshTokenAsync(userRecord, unitOfWork);

            }
            userRecord.RefreshTokens?.Add(new RefreshToken
            {
                ExpiryDate = now.AddDays(14),
                Ts = now,
                UserId = user.Id,
                TokenHash = refreshTokenHashed,
                TokenSalt = Convert.ToBase64String(salt)

            });

            await unitOfWork.SaveChangeAsync();

            var token = new Tuple<string, string>(accessToken, refreshToken);

            return token;
        }
        public static async Task<bool> RemoveRefreshTokenAsync(User user, IUnitOfWork unitOfWork)
        {
            var userRecord = await unitOfWork.UserRepository.GetUserWithRefreshTokenById(user.Id);

            if (userRecord == null)
            {
                return false;
            }

            if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
            {
                var currentRefreshToken = userRecord.RefreshTokens.First();
                await unitOfWork.RefreshTokenRepository.RemoveRefreshToken(currentRefreshToken);
            }

            return false;
        }


        public static async Task<ServiceResponse<ValidateRefreshTokenResponse>> ValidateRefreshTokenAsync(int userId,RefreshTokenRequest refreshTokenRequest, IUnitOfWork unitOfWork)
        {
            var refreshToken= await unitOfWork.RefreshTokenRepository.GetRefreshTokenByUserId(userId);
           
            var response = new ServiceResponse<ValidateRefreshTokenResponse>();
            if (refreshToken == null)
            {
                response.Success = false;
                response.Message = "Phiên không hợp lệ hoặc người dùng đã đăng xuất";
                return response;
            }

            var refreshTokenToValidateHash = HashUsingPbkdf2(refreshTokenRequest.RefreshToken, Convert.FromBase64String(refreshToken.TokenSalt));

            if (refreshToken.TokenHash != refreshTokenToValidateHash)
            {
                response.Success = false;
                response.Message = "Mã thông báo làm mới không hợp lệ";
                return response;
            }

            if (refreshToken.ExpiryDate < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Mã thông báo làm mới đã hết hạn";
                return response;
            }

            response.Success = true;
            response.Data.UserId = refreshToken.UserId;

            return response;
        }
        public static DateTime GetAccessTokenExpiration(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new ArgumentException("Invalid access token");
            }

            // Lấy giá trị Exp (Expiration Time) từ mã JWT
            var expValue = jwtToken.Payload.Exp;

            // Kiểm tra xem giá trị Exp có hợp lệ hay không
            if (expValue == null || !long.TryParse(expValue.ToString(), out long exp))
            {
                throw new ArgumentException("Invalid expiration time");
            }

            // Chuyển đổi từ giá trị Unix timestamp sang đối tượng DateTime
            var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime;

            // Lấy múi giờ của Việt Nam từ các thông tin múi giờ chuẩn sẵn có trong .NET
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển đổi thời gian sang múi giờ Việt Nam
            var vietnamExpirationDateTime = TimeZoneInfo.ConvertTimeFromUtc(expirationDateTime, vietnamTimeZone);

            return vietnamExpirationDateTime;
        }
        public static byte[] GetSecureSalt()
        {
            byte[] salt = new byte[32];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
        public static string HashUsingPbkdf2(string password, byte[] salt)
        {
            byte[] derivedKey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, iterationCount: 100000, 32);

            return Convert.ToBase64String(derivedKey);
        }
    }
}

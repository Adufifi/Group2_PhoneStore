﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace PhoneStore.Application.Services
{
    public class RefreshTokenServices : BaseServices<RefreshToken>, IRefreshTokenServices
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenServices(IUnitOfWork unitOfWork, IConfiguration configuration) : base(unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;

        }

        public async Task<AuthResultVm> GenerateJwtToken(Account account)
        {
            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, account.Id.ToString()),
                new Claim(ClaimTypes.Email,account.Email)
            };
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"] ?? throw new Exception("Sai Secret key")));
            var timeToken = int.Parse(_configuration["JWT:Time"] ?? throw new Exception("Time Token is not valid"));
            var tokenGennerate = GenerateRefreshToken(50);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(timeToken),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refershToken = new RefreshToken
            {
                JwtId = token.Id,
                IsRevoked = false,
                AccountId = account.Id,
                DateAdded = DateTime.UtcNow,
                Token = tokenGennerate
            };
            await _unitOfWork.GenericRepository<RefreshToken>().AddAsync(refershToken);
            await _unitOfWork.SaveChangeAsync();
            var response = new AuthResultVm
            {
                Token = jwtToken,
                RefreshToken = refershToken.Token,
                ExpireAt = token.ValidTo
            };
            return response;
        }
        public static string GenerateRefreshToken(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|;:,.<>/?";
            char[] token = new char[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomData = new byte[length];
                rng.GetBytes(randomData);

                for (int i = 0; i < length; i++)
                {
                    int index = randomData[i] % allowedChars.Length;
                    token[i] = allowedChars[index];
                }
            }

            return new string(token);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniETrade.Application.Common.Abstractions.Security;
using MiniETrade.Application.DTOs;
using MiniETrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Security
{
    public class TokenHelper : ITokenHelper
    {
        private DateTime _accessTokenExpiration;
        private readonly IConfiguration Configuration;
        private readonly TokenOptions _tokenOptions;

        public TokenHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public Task<Token> CreateAccessToken(AppUser user, IList<string> operationClaims)
        {
            var result = Task.Run(() =>
            {
                _accessTokenExpiration = DateTime.Now.AddDays(_tokenOptions.AccessTokenExpiration);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);  
                var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtSecurityTokenHandler.WriteToken(jwt);

                return new Token
                {
                    AccessToken = token,
                    Expiration = _accessTokenExpiration,
                    RefreshToken = CreateRefreshToken()
                };
            });
            return result;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, AppUser user,
            SigningCredentials signingCredentials, IList<string> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims.ToList()),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private static IEnumerable<Claim> SetClaims(AppUser user, List<string> operationClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("username", user.UserName),
                new Claim(ClaimTypes.Email, user.UserName)
            };
            operationClaims.ToArray().ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

            return claims;
        }
    }
}
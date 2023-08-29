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

        public Token CreateAccessToken2(int second, AppUser user)
        {
            Application.DTOs.Token token = new();

            //Security Key'in simetriğini alıyoruz.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Şifrelenmiş kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            //Oluşturulacak token ayarlarını veriyoruz.
            token.Expiration = DateTime.UtcNow.AddSeconds(second);
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: new List<Claim> { new(ClaimTypes.Name, user.UserName) }
                );

            //Token oluşturucu sınıfından bir örnek alalım.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            //string refreshToken = CreateRefreshToken();

            token.RefreshToken = CreateRefreshToken();
            return token;
        }

        public Task<Token> CreateToken(IdentityUser user, IList<string> operationClaims)
        {
            var result = Task.Run(() =>
            {
                _accessTokenExpiration = DateTime.Now.AddDays(_tokenOptions.AccessTokenExpiration);
                var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
                var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
                var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtSecurityTokenHandler.WriteToken(jwt);

                return new AccessToken
                {
                    Token = token,
                    Expiration = _accessTokenExpiration
                };
            });
            return result;
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, IdentityUser user,
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

        private IEnumerable<Claim> SetClaims(IdentityUser user, List<string> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.Add(new Claim("username", user.UserName));
            claims.AddRoles(operationClaims.ToArray());

            return claims;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}

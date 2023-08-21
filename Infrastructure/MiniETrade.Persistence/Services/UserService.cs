using Microsoft.AspNetCore.Identity;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Application.Features.AppUsers.Commands;
using MiniETrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> CreateAsync(CreateUserCommandRequest requestModel)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = requestModel.Username,
                Email = requestModel.Email,
                NameSurname = requestModel.NameSurname,
            }, requestModel.Password);

            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;
        }

        //public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        //{
        //    if (user != null)
        //    {
        //        user.RefreshToken = refreshToken;
        //        user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
        //        await _userManager.UpdateAsync(user);
        //    }
        //    else
        //        throw new NotFoundUserException();
        //}
    }
}

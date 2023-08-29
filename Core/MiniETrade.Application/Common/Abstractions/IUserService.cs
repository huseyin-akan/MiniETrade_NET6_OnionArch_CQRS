using MiniETrade.Application.Features.AppUsers.Commands;
using MiniETrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions
{
    public interface IUserService
    {
        Task<CreateUserCommandResponse> CreateAsync(CreateUserCommandRequest request);
        //Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);

        string? UserId { get; }
    }
}

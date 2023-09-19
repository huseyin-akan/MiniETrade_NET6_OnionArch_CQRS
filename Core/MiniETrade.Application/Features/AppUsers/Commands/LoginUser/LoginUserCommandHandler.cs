using MediatR;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IIdentityService _identityService;

    public LoginUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserOrThrowError(request);

        await CheckIfPasswordMatches(user, request.Password);

        var accessToken = await _identityService.CreateAccessToken(user);

        return new LoginUserResponse(accessToken);
    }

    private async Task<AppUser> GetUserOrThrowError(LoginUserCommand request)
    {
        AppUser? user = await _identityService.FindByUserNameAsync(request.UsernameOrEmail);
        user ??= await _identityService.FindByEmailAsync(request.UsernameOrEmail);
        if (user is null) throw new BusinessException(AppMessages.LoginUnsuccessful);
        return user;
    }

    private async Task CheckIfPasswordMatches(AppUser user, string password)
    {
        if (await _identityService.CheckPasswordAsync(user, password) != true)
            throw new BusinessException(AppMessages.LoginUnsuccessful);
    }
}
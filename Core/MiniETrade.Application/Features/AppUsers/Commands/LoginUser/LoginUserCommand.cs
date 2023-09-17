using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.LoginUser;

public class LoginUserCommandRequestHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    //private readonly IAuthService _authService;
    //public LoginUserCommandRequestHandler(IAuthService authService)
    //{
    //    _authService = authService;
    //}

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        //var token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, 900);
        //return new LoginUserSuccessCommandResponse()
        //{
        //    Token = token
        //};
        return null;
    }
}

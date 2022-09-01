using MediatR;
using MiniETrade.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands
{
    public class LoginUserCommandRequest : IRequest<LoginUserCommandResponse>
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
    
    public class LoginUserCommandRequestHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        //private readonly IAuthService _authService;
        //public LoginUserCommandRequestHandler(IAuthService authService)
        //{
        //    _authService = authService;
        //}

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            //var token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, 900);
            //return new LoginUserSuccessCommandResponse()
            //{
            //    Token = token
            //};
            return null;
        }
    }

    public class LoginUserCommandResponse
    {
    }

    public class LoginUserSuccessCommandResponse : LoginUserCommandResponse
    {
        public Token Token { get; set; }
    }
    public class LoginUserErrorCommandResponse : LoginUserCommandResponse
    {
        public string Message { get; set; }
    }
}

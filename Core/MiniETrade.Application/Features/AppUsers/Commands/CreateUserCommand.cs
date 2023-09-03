using MediatR;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }

    public class CreateUserCommandRequestHandler :IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IIdentityService _identityService;
        public CreateUserCommandRequestHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

            if (request.Password != request.PasswordConfirm) throw new BusinessException("Şifreler uyuşmuyor."); //TODO-HUS magic string, business rules olarak ayır.

            var response = await _identityService.CreateUserAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                NormalizedUserName = request.Username.Normalize(),
                Email = request.Email,
                NormalizedEmail = request.Email.Normalize(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = request.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                Status = true
            }, request.Password);

            return new()
            {
                Message = "User registered with userId: " + response,
            };

            //throw new UserCreateFailedException();
        }
    }

    public class CreateUserCommandResponse
    {
        public string Message { get; set; }
    }
}

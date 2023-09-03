using MediatR;
using MiniETrade.Application.BusinessRules.AppUsers;
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
        private readonly IIdentityService _identityService;
        private readonly AppUserBusinessRules _appUserBusinessRules;

        public CreateUserCommandRequestHandler(IIdentityService identityService, AppUserBusinessRules appUserBusinessRules)
        {
            _identityService = identityService;
            _appUserBusinessRules = appUserBusinessRules;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

            AppUserBusinessRules.CheckIfPasswordMatches(request.Password, request.PasswordConfirm); 

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

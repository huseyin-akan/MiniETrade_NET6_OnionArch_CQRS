using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETrade.Application.Features.AppUsers.Commands;
using MiniETrade.Application.Features.AppUsers.Commands.CreateUser;
using MiniETrade.Application.Features.AppUsers.Commands.LoginUser;

namespace MiniETrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser(CreateUserCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}

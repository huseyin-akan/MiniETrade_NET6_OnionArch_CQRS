using MiniETrade.Application.Features.AppUsers.Commands;
using MiniETrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions
{
    public interface ICurrentUserService
    {

        Guid UserId { get; }

        string? UserName { get; }

        string? UserMail { get; }
    }
}

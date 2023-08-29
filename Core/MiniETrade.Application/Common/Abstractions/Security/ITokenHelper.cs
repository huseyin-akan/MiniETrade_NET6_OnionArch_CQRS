using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Security
{
    public interface ITokenHelper
    {
        Token CreateAccessToken(int second, AppUser user);
        string CreateRefreshToken();
    }
}

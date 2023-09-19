using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MiniETrade.Application.Common.Abstractions.Security;

public interface ITokenHelper
{
    Task<Token> CreateAccessToken(AppUser user, IList<string> operationClaims);
    string CreateRefreshToken();
}
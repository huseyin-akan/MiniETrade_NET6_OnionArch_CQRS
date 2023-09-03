using Microsoft.AspNetCore.Identity;
using MiniETrade.Application.Common.Models;
using MiniETrade.Application.DTOs;
using MiniETrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Identity
{
    public interface IIdentityService
    {

        Task<bool> AuthorizeAsync(string userId, string policyName);
        Task<bool> AddUserToRole(string userId, string role);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<Token> CreateAccessToken(AppUser user);
        Task<bool> CreateRole(string roleName);
        Task<string> CreateUserAsync(AppUser user, string password);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<IdentityResult> DeleteUserAsync(AppUser user);
        Task<AppUser> FindByEmailAsync(string email);
        Task<AppUser> FindByNameAsync(string userName);
        List<AppUser> GetAllUsers();
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<string> GetUserNameAsync(string userId);
        Task<AppUser?> GetUserByIdAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task UpdatePassword(Guid userId, string newPassword);
        Task UpdateUser(AppUser user);
    }
}

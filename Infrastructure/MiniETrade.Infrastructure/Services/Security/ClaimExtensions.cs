using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Security;

public static class ClaimExtensions
{
    //TODO-HUS bu extension metodları uçuralım. Çok da gerekli değil gibi. Direk TokenHelper içinden yönetilebilir sanki.

    public static void AddRoles(this ICollection<Claim> claims, string[] roles)
    {
        
    }
}

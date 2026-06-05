using FinananzasAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace FinananzasAPI.Application.Interfaces
{
    public interface ITokenService
    {
        string GenereteAccessToken(User user);
        string GenereteRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken (string token);
    }
}

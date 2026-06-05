using FinananzasAPI.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(Guid userId);
    }
}

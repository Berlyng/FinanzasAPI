using FinananzasAPI.Application.DTOs.Auth;
using FinananzasAPI.Application.Interfaces;
using FinananzasAPI.Domain.Entities;
using FinananzasAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, ITokenService tokenService, IConfiguration config)
        {
            _db = db;
            _tokenService = tokenService;
            _config = config;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLower().Trim());

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciales Invalidas");

            return await GenerateAuthResponse(user);
        }

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is null) return;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _db.SaveChangesAsync();

        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken 
            && u.RefreshTokenExpiry > DateTime.UtcNow);

            if (user is null)
                throw new UnauthorizedAccessException("Refresh token invalido o expirado");
            
            return await GenerateAuthResponse(user);
                    
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var exists = await _db.Users.AnyAsync(u => u.Email == request.Email);
            if (exists)
                throw new InvalidOperationException("El email ya está registrado");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email.ToLower().Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12)
            };
            _db.Users.Add(user);
           await _db.SaveChangesAsync();

            return await GenerateAuthResponse(user);
        }

        private async Task<AuthResponse> GenerateAuthResponse(User user)
        {
            var accessToken = _tokenService.GenereteAccessToken(user);
            var refreshToken = _tokenService.GenereteRefreshToken();
            var refreshDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"]);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(refreshDays);

            await _db.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccesTokenExpiry = DateTime.UtcNow.AddMinutes(15),
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                }
            };

        }
    }
}

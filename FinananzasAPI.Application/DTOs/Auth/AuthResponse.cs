using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccesTokenExpiry {  get; set; }
        public UserDto User { get; set; }

    }

    public class UserDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }

    }
}

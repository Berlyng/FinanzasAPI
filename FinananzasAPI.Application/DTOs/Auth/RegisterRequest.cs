using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [MinLength(8, ErrorMessage ="Minimo 8 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
}

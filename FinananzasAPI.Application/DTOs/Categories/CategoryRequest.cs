using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Categories
{
    public class CategoryRequest
    {
        [Required (ErrorMessage = "El nombre es requerido")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(7, ErrorMessage ="El color debe de ser un hex valid (#FFFFFF)")]
        public string? Color { get; set; }

        [MaxLength(50)]
        public string? Icon { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Categories
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public bool isDefault { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

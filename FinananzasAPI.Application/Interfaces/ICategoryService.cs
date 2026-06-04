using FinananzasAPI.Application.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllAsync(Guid userId);
        Task<CategoryResponse> GetByIdAsync(Guid id, Guid userId);
        Task<CategoryResponse> CreateAsync(CategoryRequest request, Guid userId);
        Task<CategoryResponse> UpdateAsync(Guid id,  CategoryRequest request, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}

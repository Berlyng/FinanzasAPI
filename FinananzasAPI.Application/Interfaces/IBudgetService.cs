using FinananzasAPI.Application.DTOs.Budgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.Interfaces
{
    public interface IBudgetService
    {
        Task<List<BudgetResponse>> GetAllAsync(Guid userId, int? month, int? year);
        Task<BudgetStatusResponse> GetStatusAsync(Guid budgetId, Guid userId);
        Task<BudgetResponse> CreateAsync(BudgetRequest request, Guid userId);
        Task<BudgetResponse> UpdateAsync(Guid id, BudgetRequest request, Guid userId);
        Task DeleteAsync(Guid id,  Guid userId);
    }
}

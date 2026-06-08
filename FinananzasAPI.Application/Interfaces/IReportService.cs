using FinananzasAPI.Application.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.Interfaces
{
    public interface IReportService
    {
        Task<SumaryResponse> GetSumaryAsync(Guid userId, int month, int year);
        Task<List<CategoryBrakedownResponse>> GetByCategoryAsync(Guid userId, int month, int year);
        Task<TrendResponse> GetTrendAsync(Guid userId, int months);
    }
}

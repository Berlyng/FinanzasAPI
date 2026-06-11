using FinananzasAPI.Application.DTOs.Reports;
using FinananzasAPI.Application.Interfaces;
using FinananzasAPI.Domain.Commons;
using FinananzasAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinananzasAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        private Guid userId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public ReportsController(IReportService reportService) => _reportService = reportService;

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] int? month,  [FromQuery] int? year)
        {
            var now = DateTime.UtcNow;
            var result = await _reportService.GetSumaryAsync(userId, month ?? now.Month, year ?? now.Year);

            return Ok(ApiResponse<SumaryResponse>.Ok(result));

        }

        [HttpGet("by-category")]
        public async Task<IActionResult> GetByCategory([FromQuery] int? month, [FromQuery] int? year)
        {
            var now = DateTime.UtcNow;
            var result = await _reportService.GetByCategoryAsync(userId, month ?? now.Month, year ?? now.Year);

            return Ok(ApiResponse<List<CategoryBrakedownResponse>>.Ok(result));
        }

        [HttpGet("trends")]
        public async Task<IActionResult> GetTrends([FromQuery] int months = 6)
        {
            var result = await _reportService.GetTrendAsync(userId, months);
            return Ok(ApiResponse<TrendResponse>.Ok(result));
        }
    }
}

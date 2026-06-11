using FinananzasAPI.Application.DTOs.Budgets;
using FinananzasAPI.Application.Interfaces;
using FinananzasAPI.Domain.Commons;
using FinananzasAPI.Domain.Entities;
using FinananzasAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace FinananzasAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetsController : Controller
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }
        
        private Guid userId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? month, [FromQuery] int? year)
        {
            var budget = await _budgetService.GetAllAsync(userId, month, year);
            return Ok(ApiResponse<List<BudgetResponse>>.Ok(budget));
        }

        [HttpGet("{id:guid}/status")]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            var status = await _budgetService.GetStatusAsync(id, userId);
            return Ok(ApiResponse<BudgetStatusResponse>.Ok(status));
        }

        [HttpPost]
        public async Task<IActionResult> Create(BudgetRequest request)
        {
            var budget = await _budgetService.CreateAsync(request, userId);
            return CreatedAtAction(nameof(GetAll), ApiResponse<BudgetResponse>.Ok(budget, "Presupuesto creado"));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, BudgetRequest request)
        {
            var budget = await _budgetService.UpdateAsync(id, request, userId);
            return Ok(ApiResponse<BudgetResponse>.Ok(budget, "Presupuesto actualizado"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _budgetService.DeleteAsync(id, userId);
            return NoContent();
        }

        

     
    }
}

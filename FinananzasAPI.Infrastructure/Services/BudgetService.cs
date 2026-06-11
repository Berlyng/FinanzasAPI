using FinananzasAPI.Application.DTOs.Budgets;
using FinananzasAPI.Application.Interfaces;
using FinananzasAPI.Domain.Entities;
using FinananzasAPI.Domain.Enums;
using FinananzasAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Infrastructure.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly AppDbContext _db;

        public BudgetService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<BudgetResponse> CreateAsync(BudgetRequest request, Guid userId)
        {
            var categoryExist  = await _db.Categories
                .AnyAsync(c => c.Id == request.CategoryId && c.UserId == userId);

            if (!categoryExist)
                throw new KeyNotFoundException("Categoria no encontrada");
            
            var duplicate = await _db.Budgets.AnyAsync(b => 
            b.UserId ==  userId &&
            b.CategoryId == request.CategoryId &&
            b.Month == request.Month &&
            b.Year == request.Year  );

            if (duplicate)
            {
                throw new InvalidOperationException("Ya existe un presupuesto para esta categoria");
            }

            var budget = new Budget
            {
                CategoryId = request.CategoryId,
                LimitAmount = request.LimitAmount,
                Month = request.Month,
                Year = request.Year,
                UserId = userId
            };

            _db.Budgets.Add(budget);
            await _db.SaveChangesAsync();
            await _db.Entry(budget).Reference(b => b.Category).LoadAsync();

            return ToResponse(budget);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var budget = await _db.Budgets
                 .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId)
                 ?? throw new KeyNotFoundException("Presupuesto no encontrado");

            _db.Budgets.Remove(budget);
            await _db.SaveChangesAsync();
        }

        public async Task<List<BudgetResponse>> GetAllAsync(Guid userId, int? month, int? year)
        {
            var now = DateTime.UtcNow;
            var query = _db.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId
                && b.Month == (month ?? now.Month)
                && b.Year == (year ?? now.Year));

            return await query
                .OrderBy(b => b.Category.Name)
                .Select(b => ToResponse(b))
                .ToListAsync();
        }



        public async Task<BudgetStatusResponse> GetStatusAsync(Guid budgetId, Guid userId)
        {
            var budget = await _db.Budgets
                .Include (b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == budgetId && b.UserId == userId)
                ?? throw new KeyNotFoundException("Presupuesto no encontrado");

            //Total gastado en esta categoria mes/año
            var spent = await _db.Transactions
                .Where(t => t.UserId == userId
                && t.CategoryId == budget.CategoryId
                && t.Type == TransactionType.Expanse
                && t.Date.Month == budget.Month
                && t.Date.Year == budget.Year)
                .SumAsync(t => (decimal?)t.Amount) ?? 0;

            //Proyeccion: calculamos cuanto se ha gastado hasta HOY
            //y lo extrapolamos al total del dia del mes

            var today = DateTime.UtcNow;
            var daysInMonth = DateTime.DaysInMonth(budget.Year, budget.Month);

            // si estamos en el mes actual, proyectamos si es pasado el gasto real de la proyeccion
            decimal projected;
            if(budget.Year == today.Year && budget.Month == today.Month)
            {
                var daysPassed = today.Day;
                projected = daysPassed > 0
                    ? Math.Round(spent / daysPassed * daysInMonth, 2) : 0;

            }
            else
            {
                projected = spent;
            }

            var usage = budget.LimitAmount > 0
                ? (double)(spent / budget.LimitAmount) * 100 : 0;

            var alert = usage switch
            {
                >= 100 => AlertLevel.Exceeded,
                >= 90 => AlertLevel.Critical,
                >= 70 => AlertLevel.Warning,
                _ => AlertLevel.Safe

            };

            return new BudgetStatusResponse
            {
                BudgetId = budget.Id,
                CategoryName = budget.Category.Name,
                CategoryColor = budget.Category.Color,
                LimitAmount = budget.LimitAmount,
                SpentAmount = spent,
                ProjectAmount = projected,
                Alert = alert
            };

        }

        public async Task<BudgetResponse> UpdateAsync(Guid id, BudgetRequest request, Guid userId)
        {
            var budget = await _db.Budgets
                 .Include(b => b.Category)
                 .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId)
                 ?? throw new KeyNotFoundException("Presupuesto no encontrado");

            budget.LimitAmount = request.LimitAmount;
            budget.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return ToResponse(budget);
        }

        private static BudgetResponse ToResponse(Budget b) => new()
        {
            Id = b.Id,
            CategoryId = b.CategoryId,
            CategoryName = b.Category.Name,
            CategoryColor = b.Category.Color,
            LimitAmount = b.LimitAmount,
            Month = b.Month,
            Year = b.Year,
            CreatedAt = b.CreatedAt,
        };
    }
}

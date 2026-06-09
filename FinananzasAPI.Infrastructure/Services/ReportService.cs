using FinananzasAPI.Application.DTOs.Reports;
using FinananzasAPI.Application.Interfaces;
using FinananzasAPI.Domain.Entities;
using FinananzasAPI.Domain.Enums;
using FinananzasAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _db;

        public ReportService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CategoryBrakedownResponse>> GetByCategoryAsync(Guid userId, int month, int year)
        {
            var grouped = await _db.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId
                && t.Type == TransactionType.Expanse
                && t.Date.Month == month
                && t.Date.Year == year)
                .GroupBy(t => new
                {
                    t.CategoryId,
                    t.Category.Name,
                    t.Category.Color
                })
                .Select(g => new
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.Name,
                    CategoryColor = g.Key.Color,
                    TotalAmount = g.Sum(t => t.Amount),
                    TransactionAcount = g.Count()
                })
                .OrderByDescending(g => g.TotalAmount)
                .ToListAsync();

            var total = grouped.Sum(g => g.TotalAmount);

            return grouped.Select(g => new CategoryBrakedownResponse
            {
                CategoryId = g.CategoryId,
                CategoryName = g.CategoryName,
                CategoryColor = g.CategoryColor,
                TotalAmount = g.TotalAmount,
                TransactionCount = g.TransactionAcount,
                Percentage = total > 0 ? Math.Round((double)(g.TotalAmount / total) * 100, 1) : 0
            }).ToList();

        }

        public async Task<SumaryResponse> GetSumaryAsync(Guid userId, int month, int year)
        {
            var transaction = await _db.Transactions
                .Where(t => t.UserId == userId
                && t.Date.Month ==  month
                && t.Date.Year == year)
                .ToListAsync();

            var income = transaction.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var expense = transaction.Where(t => t.Type == TransactionType.Expanse).Sum(t => t.Amount);

            //dias transacurridos en el mes para el promedio diario
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var today = DateTime.UtcNow;
            var daysPassed = (year == today.Year && month == today.Month)
                ? today.Day : daysInMonth;

            return new SumaryResponse
            {
                Month = month,
                Year = year,
                TotalIncome = income,
                TotalExpanse = expense,
                TransactionCount = transaction.Count(),
                AverageExpensePerDay = daysPassed > 0 ? Math.Round(expense / daysPassed, 2) : 0
            };

        }

        public async Task<TrendResponse> GetTrendAsync(Guid userId, int months)
        {
            //validamos el campo para evitar rangos absurdos
            months = Math.Clamp(months, 2, 24);

            var today = DateTime.UtcNow;
            var starDate = new DateTime(today.Year, today.Month, 1).AddMonths(-(months - 1));

            var transaction = await _db.Transactions
                .Where(t => t.UserId == userId && t.Date >= starDate).ToListAsync();

            //Construimos la lista de meses en orden cronologico
            //y buscamos los datos asi incluimos meses sin transaccion

            var trend = new TrendResponse();

            for (int i = months -1; i >= 0; i--)
            {
                var date = today.AddMonths(-i);
                var month = date.Month;
                var year = date.Year;

                var monthTransaction = transaction
                    .Where(t => t.Date.Month == month && t.Date.Year == year).ToList();

                trend.Month.Add(new MonthlyTrend
                {
                    Month = month,
                    Year = year,
                    Label = date.ToString("MMM yyyy", new System.Globalization.CultureInfo("es-ES")),
                    TotalIncome = monthTransaction.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                    TotalExpanse = monthTransaction.Where(t => t.Type == TransactionType.Expanse).Sum(t => t.Amount)
                });
            }
            return trend;

        }
    }
}

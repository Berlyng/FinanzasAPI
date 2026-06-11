using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Budgets
{
    public class BudgetStatusResponse
    {
        public Guid BudgetId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryColor {  get; set; }
        public decimal LimitAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmout => LimitAmount - SpentAmount;
        public double UsagePercentage => LimitAmount == 0 ? 0
            : Math.Round((double)(SpentAmount/LimitAmount) * 100, 1);

        // Proyección: si seguimos gastando al mismo ritmo, ¿cuánto gastaremos al fin del mes?
        public decimal ProjectAmount { get; set; }
        public bool IsOverBudget => SpentAmount > LimitAmount;
        public bool WillExceedBudget => ProjectAmount > LimitAmount;

        public AlertLevel Alert {  get; set; }

    }



    public enum AlertLevel
    {
        Safe, // < 70%
        Warning, //70% - 90%
        Critical, // 90% - 100%
        Exceeded // > 100%
    }
}

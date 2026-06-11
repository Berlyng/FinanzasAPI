using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Budgets
{
    public class BudgetResponse
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryColor { get; set; }
        public decimal LimitAmount { get; set; }
        public int Month {  get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}

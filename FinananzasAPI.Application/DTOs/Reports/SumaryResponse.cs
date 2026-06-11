using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Reports
{
    public class SumaryResponse
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpanse {  get; set; }
        public decimal Balance => TotalIncome - TotalExpanse;
        public int TransactionCount { get; set; }
        public decimal AverageExpensePerDay { get; set; }
        public string MonthName => new DateTime (Year, Month, 1)
            .ToString("MMMM yyyy", new System.Globalization.CultureInfo ("es-ES"));
    }
}

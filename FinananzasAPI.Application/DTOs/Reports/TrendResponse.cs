using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Reports
{
    public class TrendResponse
    {
        public List<MonthlyTrend> Month {  get; set; }
    }


    public class MonthlyTrend
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Label { get; set; } = string.Empty; // "Ene 2024"
        public decimal TotalIncome { get; set; }
        public decimal TotalExpanse { get; set; }
        public decimal Balance => TotalIncome - TotalExpanse;

    }

}

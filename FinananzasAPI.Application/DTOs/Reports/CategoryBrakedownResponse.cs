using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Reports
{
    public class CategoryBrakedownResponse
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryColor { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }

        //Porcentaje total de gastos que representa esta categoria
        public double Percentage { get; set; }


    }
}

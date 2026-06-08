using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Budgets
{
    public class BudgetRequest
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="El limite debe ser mayor a 0")]
        public decimal LimitAmount {  get; set; }

        [Required]
        [Range(1,12, ErrorMessage ="Mes invalido")]
        public int Month {  get; set; }

        [Required]
        [Range(2000, 2100, ErrorMessage ="Año invalido")]
        public int Year { get; set; }
    }
}

using FinananzasAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Transactions
{
    public class TransactionRequest
    {
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="El monto debe ser mayor a 0")]
        public decimal amount { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

             

    }
}

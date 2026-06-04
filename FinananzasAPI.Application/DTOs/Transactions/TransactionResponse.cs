using FinananzasAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Transactions
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string TypeLabel => Type == TransactionType.Income ? "Ingreso" : "Gasto";
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        //Datos de la categoria embebido - el cliente no tiene que
        //hacer una segunda llamada para sabel el nombre de la categoria
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryColor { get; set; }
        public string? CategoryIcon { get; set; }
    }
}

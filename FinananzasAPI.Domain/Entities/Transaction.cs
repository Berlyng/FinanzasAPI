using FinananzasAPI.Domain.Commons;
using FinananzasAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

    }
}

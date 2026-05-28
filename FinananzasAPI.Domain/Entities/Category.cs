using FinananzasAPI.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public bool IsDefault { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();


    }
}

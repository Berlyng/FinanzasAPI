using FinananzasAPI.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Domain.Entities
{
    public class Budget : BaseEntity
    {
        public decimal LimitAmount { get; set; }
        public int Month {  get; set; }
        public int Year { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

    }
}

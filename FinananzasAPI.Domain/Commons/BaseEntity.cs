using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Domain.Commons
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}

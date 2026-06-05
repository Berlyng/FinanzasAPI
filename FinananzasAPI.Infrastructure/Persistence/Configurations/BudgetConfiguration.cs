using FinananzasAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Infrastructure.Persistence.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.LimitAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(b => b.Month)
                .IsRequired();

            builder.Property(b => b.Year)
                .IsRequired();

            builder.HasIndex(b => new { b.UserId, b.CategoryId, b.Month, b.Year })
                .IsUnique();

            builder.ToTable("Budgets");

        }
    }
}

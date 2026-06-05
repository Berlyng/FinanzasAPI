using FinananzasAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
           builder.HasKey(t => t.Id);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(t => t.Type)
                .IsRequired();

            builder.Property(t => t.Date)
                .IsRequired();

            builder.Property(t => t.Notes)
                .HasMaxLength(500);

            builder.HasIndex(t => new { t.UserId, t.Date });

            builder.ToTable("Transactions");
                
        }
    }
}

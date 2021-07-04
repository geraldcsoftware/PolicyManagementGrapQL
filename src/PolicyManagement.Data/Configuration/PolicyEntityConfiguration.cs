using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyManagement.Data.Models;
using System;


namespace PolicyManagement.Data.Configuration
{
    public class PolicyEntityConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable("Policies");
            builder.HasKey(p => p.PolicyNumber);
            builder.Property(p => p.PolicyNumber).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(p => p.Name).IsRequired().IsUnicode().HasMaxLength(100);
            builder.Property(p => p.Created).IsRequired();
            builder.Property(p => p.CurrencyCode).IsRequired().IsUnicode(false).HasMaxLength(5);
        }
    }
}

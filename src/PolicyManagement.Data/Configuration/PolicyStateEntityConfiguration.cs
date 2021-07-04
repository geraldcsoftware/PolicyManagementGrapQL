using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyManagement.Data.Models;


namespace PolicyManagement.Data.Configuration
{
    public class PolicyStateEntityConfiguration : IEntityTypeConfiguration<PolicyState>
    {
        public void Configure(EntityTypeBuilder<PolicyState> builder)
        {
            builder.ToTable("PolicyStatus");
            builder.HasKey(p => p.PolicyNumber);
            builder.Property(p => p.PolicyNumber).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(p => p.ActivationStatus).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(p => p.BillingPeriod).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(p => p.BillingState).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(p => p.DateCancelled).IsRequired(false);

            builder.HasOne<Policy>().WithOne(p => p.State).HasForeignKey<PolicyState>(p => p.PolicyNumber);
        }
    }
}

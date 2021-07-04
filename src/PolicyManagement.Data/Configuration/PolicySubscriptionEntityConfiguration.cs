using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyManagement.Data.Models;


namespace PolicyManagement.Data.Configuration
{
    public class PolicySubscriptionEntityConfiguration : IEntityTypeConfiguration<PolicySubscription>
    {
        public void Configure(EntityTypeBuilder<PolicySubscription> builder)
        {
            builder.ToTable("PolicySubscriptions");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.PolicyNumber).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(p => p.DateSubscribed).IsRequired();
            builder.Property(p => p.PackageId).IsRequired();

            builder.HasOne(p => p.Package)
                   .WithMany(s => s.Subscriptions)
                   .HasForeignKey(s => s.PackageId);

            builder.HasOne<Policy>()
                   .WithOne(p => p.Subscription)
                   .HasForeignKey<PolicySubscription>(p => p.PolicyNumber);
                
        }
    }
}

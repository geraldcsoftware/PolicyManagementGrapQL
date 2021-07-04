using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyManagement.Data.Models;


namespace PolicyManagement.Data.Configuration
{
    public class MembershipStateEntityConfiguration : IEntityTypeConfiguration<MembershipState>
    {
        public void Configure(EntityTypeBuilder<MembershipState> builder)
        {
            builder.ToTable("PolicyMembershipStatus");
            builder.HasKey(m => new { m.PolicyNumber, m.IdNumber });
            builder.Property(m => m.ActivationStatus).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(m => m.IdNumber).IsRequired().IsUnicode().HasMaxLength(20);
            builder.Property(m => m.PolicyNumber).IsRequired().IsUnicode().HasMaxLength(20);
            builder.Property(m => m.DateAdded).IsRequired();
            builder.Property(m => m.IsPolicyHolder).IsRequired();

            builder.HasOne<PolicyMember>()
                   .WithOne(m => m.Status)
                   .HasForeignKey<MembershipState>(m => m.IdNumber);
        }
    }
}

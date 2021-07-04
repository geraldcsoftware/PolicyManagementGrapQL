using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PolicyManagement.Data.Models;


namespace PolicyManagement.Data.Configuration
{
    public class PolicyMemberEntityConfiguration : IEntityTypeConfiguration<PolicyMember>
    {
        public void Configure(EntityTypeBuilder<PolicyMember> builder)
        {
            builder.ToTable("PolicyMembers");
            builder.HasKey(m => m.IdNumber);
            builder.Property(m => m.DateOfBirth).IsRequired();
            builder.Property(m => m.FirstName).IsRequired().IsUnicode().HasMaxLength(50);
            builder.Property(m => m.LastName).IsRequired().IsUnicode().HasMaxLength(50);
            builder.Property(m => m.IdNumber).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(m => m.Gender).IsRequired().IsUnicode(false).HasMaxLength(10);
            builder.Property(m => m.PolicyNumber).IsRequired().IsUnicode(false).HasMaxLength(20);
            builder.Property(m => m.SuffixNumber).IsRequired().IsUnicode(false).HasMaxLength(3);
            builder.HasIndex(m => new { m.PolicyNumber, m.SuffixNumber }).IsUnique();

            builder.HasOne<Policy>().WithMany(p => p.Members).HasForeignKey(m => m.PolicyNumber);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data.Configuration;
using PolicyManagement.Data.Models;

namespace PolicyManagement.Data
{
    public class PolicyManagementDbContext : DbContext
    {
        public PolicyManagementDbContext(DbContextOptions<PolicyManagementDbContext> options) : base(options) { }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PolicySubscription> Subscriptions { get; set; }
        public DbSet<PolicyMember> PolicyMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PolicyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PackageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PolicySubscriptionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MembershipStateEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PolicyMemberEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PolicyStateEntityConfiguration());
        }
    }
}

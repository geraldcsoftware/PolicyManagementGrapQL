using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data.Configuration;
using System.Collections.Generic;

namespace PolicyManagement.Data.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PolicySubscription> Subscriptions { get; set; } = new List<PolicySubscription>();
    }
}
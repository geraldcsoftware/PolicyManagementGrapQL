using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyManagement.Data.Models
{
    /// <summary>
    /// Represents a policy
    /// </summary>
    public class Policy
    {
        /// <summary>
        /// The policy number for a policy
        /// </summary>
        public string PolicyNumber { get; set; }
        /// <summary>
        /// The name assigned for the policy
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The date on which the policy was created
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Describes the state of a policy
        /// </summary>
        public PolicyState State { get; set; }
        /// <summary>
        /// Identifies the package to which the policy subscribes
        /// </summary>
        public PolicySubscription Subscription { get; set; }
        /// <summary>
        /// The currency under which the policy runs
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Gets the members belonging to this policy
        /// </summary>
        public ICollection<PolicyMember> Members { get; set; } = new List<PolicyMember>();
    }
}

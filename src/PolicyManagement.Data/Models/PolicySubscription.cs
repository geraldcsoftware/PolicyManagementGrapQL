using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data.Configuration;
using System;

namespace PolicyManagement.Data.Models
{
    /// <summary>
    /// Represents the package to which a policy is subscribed
    /// </summary>
    public class PolicySubscription
    {
        /// <summary>
        /// Identifies the subscription instance
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Identifies the policy with this subscription
        /// </summary>
        public string PolicyNumber { get; set; }
        /// <summary>
        /// Identifies the package subscribed to
        /// </summary>
        public int PackageId { get; set; }
        /// <summary>
        /// The first date on which the subscription applies
        /// </summary>
        public DateTime DateSubscribed { get; set; }
        /// <summary>
        /// The package to which the policy is subscribed to
        /// </summary>
        public Package Package { get; set; }
        /// <summary>
        /// Identifies the policy with this subscription
        /// </summary>
        public Policy Policy { get; set; }
    }
}
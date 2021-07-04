using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data.Configuration;
using System;

namespace PolicyManagement.Data.Models
{
    /// <summary>
    /// Describes the status of a policy
    /// </summary>
    public class PolicyState
    {
        public string PolicyNumber { get; set; }
        /// <summary>
        /// Identifies the billing period
        /// </summary>
        public string BillingPeriod { get; set; }
        /// <summary>
        /// Identifies the billing state during the billing period
        /// </summary>
        public string BillingState { get; set; }
        /// <summary>
        /// Identifies whether a policy is active
        /// </summary>
        public string ActivationStatus { get; set; }
        /// <summary>
        /// If a policy is cancelled, identifies the cancellation date
        /// </summary>
        public DateTime? DateCancelled { get; set; }
    }
}
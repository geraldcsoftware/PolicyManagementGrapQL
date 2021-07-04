using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data.Configuration;
using System;

namespace PolicyManagement.Data.Models
{
    public class MembershipState
    {
        public string IdNumber { get; set; }
        public string PolicyNumber { get; set; }
        public bool IsPolicyHolder { get; set; }
        public DateTime DateAdded { get; set; }
        public string ActivationStatus { get; set; }
    }
}
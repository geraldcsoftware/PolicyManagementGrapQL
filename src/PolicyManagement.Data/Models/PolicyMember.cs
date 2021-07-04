using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data.Configuration;
using System;

namespace PolicyManagement.Data.Models
{
    public class PolicyMember
    {
        public string PolicyNumber { get; set; }
        public string SuffixNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdNumber { get; set; }
        public string Gender { get; set; }
        public MembershipState Status { get; set; }
    }
}
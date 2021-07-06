using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyManagement.Web.InputModels
{
    public class AddPolicyInputModel
    {
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int PackageId { get; set; }
        public string CurrencyCode { get; set; }

    }
}

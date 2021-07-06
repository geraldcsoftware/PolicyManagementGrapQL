using HotChocolate;
using MediatR;
using OneOf;
using PolicyManagement.Data.Models;
using System;

namespace PolicyManagement.Web.Mediator.Requests
{
    public class AddPolicyRequest : IRequest<OneOf<Policy, IError>>
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

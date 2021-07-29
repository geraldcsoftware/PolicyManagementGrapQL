using System;
using HotChocolate;
using MediatR;
using OneOf;
using PolicyManagement.Data.Models;

namespace PolicyManagement.Web.Mediator.Requests
{
    public class AddPolicyMemberRequest : IRequest<OneOf<PolicyMember, IError>>
    {
        public string PolicyNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdNumber { get; set; }
        public string Gender { get; set; }
    }
}

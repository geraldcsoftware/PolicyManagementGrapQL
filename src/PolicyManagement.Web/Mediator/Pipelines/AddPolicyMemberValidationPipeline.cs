using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HotChocolate;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using PolicyManagement.Data.Models;
using PolicyManagement.Web.Mediator.Requests;

namespace PolicyManagement.Web.Mediator.Pipelines
{
    public class AddPolicyMemberValidationPipeline : IPipelineBehavior<AddPolicyMemberRequest, OneOf<PolicyMember, IError>>
    {
        private readonly IValidator<AddPolicyMemberRequest> _validator;
        private readonly ILogger<AddPolicyMemberValidationPipeline> _logger;

        public AddPolicyMemberValidationPipeline(IValidator<AddPolicyMemberRequest> validator,
                                                 ILogger<AddPolicyMemberValidationPipeline> logger)
        {
            _validator = validator;
            _logger = logger;
        }

        public Task<OneOf<PolicyMember, IError>> Handle(AddPolicyMemberRequest request,
                                                        CancellationToken cancellationToken,
                                                        RequestHandlerDelegate<OneOf<PolicyMember, IError>> next)
        {

            var validationResults = _validator.Validate(request);
            if (validationResults.IsValid)
                return next();
            var error = ErrorBuilder.New()
                                    .SetCode("Invalid input")
                                    .SetMessage(string.Join(Environment.NewLine, validationResults.Errors))
                                    .Build();
            return Task.FromResult(OneOf<PolicyMember, IError>.FromT1(error));
        }
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneOf;
using PolicyManagement.Data;
using PolicyManagement.Data.Models;
using PolicyManagement.Web.Mediator.Requests;
using PolicyResult = OneOf.OneOf<PolicyManagement.Data.Models.PolicyMember, HotChocolate.IError>;

namespace PolicyManagement.Web.Mediator.Handlers
{
    public class AddPolicyMemberRequestHandler : IRequestHandler<AddPolicyMemberRequest, PolicyResult>
    {
        private readonly PolicyManagementDbContext _dbContext;
        private readonly ILogger<AddPolicyMemberRequestHandler> _logger;
        public AddPolicyMemberRequestHandler(IDbContextFactory<PolicyManagementDbContext> dbContextFactory,
                                             ILogger<AddPolicyMemberRequestHandler> logger)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _logger = logger;
        }

        public async Task<PolicyResult> Handle(AddPolicyMemberRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _logger.LogInformation("Executing handler for AddPolicyMemberRequest: {@Request}", request);
            var policy = await _dbContext.Policies
                                         .Where(p => p.PolicyNumber == request.PolicyNumber)
                                         .Select(p => new { p.PolicyNumber, MemberCount = p.Members.Count })
                                         .FirstOrDefaultAsync(cancellationToken);
            if (policy == null)
            {
                _logger.LogInformation("Policy number {PolicyNumber} not found", request.PolicyNumber);
                return PolicyResult.FromT1(ErrorBuilder.New()
                                     .SetCode("PolicyNotFound")
                                     .SetMessage($"Policy number '{request.PolicyNumber}' does not exist in the system")
                                     .Build());
            }

            _logger.LogInformation("Adding member to policy number {PolicyNumber}", policy.PolicyNumber);
            PolicyMember policyMember = new()
            {
                PolicyNumber = policy.PolicyNumber,
                DateOfBirth = request.DateOfBirth,
                FirstName = request.FirstName,
                Gender = request.Gender,
                IdNumber = request.IdNumber,
                LastName = request.LastName,
                Status = new()
                {
                    DateAdded = DateTime.UtcNow,
                    IdNumber = request.IdNumber,
                    IsPolicyHolder = false,
                    PolicyNumber = policy.PolicyNumber,
                    ActivationStatus = "ACTIVE"
                },
                SuffixNumber = (policy.MemberCount + 1).ToString("D2")
            };
            _dbContext.PolicyMembers.Add(policyMember);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Policy member saved {@PolicyMember}", policyMember);
                return PolicyResult.FromT0(policyMember);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.GetBaseException(), "Failed to save policy member because of an error");

                if (exception.GetBaseException() is Npgsql.PostgresException pgException)
                {
                    var error = pgException switch
                    {
                        { SqlState: Npgsql.PostgresErrorCodes.UniqueViolation } =>
                           ErrorBuilder.New()
                                       .SetMessage("Cannot create policy because there is another policy member with the same details")
                                       .SetCode("DUPLICATE_POLICY_MEMBER")
                                       .Build(),
                        _ => ErrorBuilder.New().SetMessage("Failed to save the policy becaouse of an error").Build()
                    };
                    return PolicyResult.FromT1(error);
                }
                return PolicyResult.FromT1(ErrorBuilder.New().SetMessage("Unknown error").SetCode("ERROR").Build());

            }
        }
    }
}

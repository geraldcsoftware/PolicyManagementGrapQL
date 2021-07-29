using HotChocolate;
using IdGen;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneOf;
using PolicyManagement.Data;
using PolicyManagement.Data.Models;
using PolicyManagement.Web.Mediator.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PolicyManagement.Web.Mediator.Handlers
{
    public class AddPolicyRequestHandler : IRequestHandler<AddPolicyRequest, OneOf<Policy, IError>>
    {
        private readonly PolicyManagementDbContext _dbContext;
        private readonly IIdGenerator<long> _idGenerator;
        private readonly ILogger<AddPolicyRequestHandler> _logger;

        public AddPolicyRequestHandler(IDbContextFactory<PolicyManagementDbContext> dbContextFactory,
                                       IIdGenerator<long> idGenerator,
                                       ILogger<AddPolicyRequestHandler> logger)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _idGenerator = idGenerator;
            _logger = logger;
        }

        public async Task<OneOf<Policy, IError>> Handle(AddPolicyRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Creating policy based on input {@Request}", request);
            var currentDate = DateTime.UtcNow;
            var policyNumber = $"{_idGenerator.CreateId():X}";
            var policy = new Policy
            {
                PolicyNumber = policyNumber,
                Created = currentDate,
                CurrencyCode = request.CurrencyCode,
                Name = string.Join(" ", new[] { request.FirstName, request.LastName }),
                State = new PolicyState
                {
                    ActivationStatus = "ACTIVE",
                    BillingPeriod = $"{DateTime.Today:MMMyyyy}",
                    BillingState = "PENDING",
                },
                Subscription = new PolicySubscription
                {
                    PackageId = request.PackageId,
                    DateSubscribed = currentDate
                },
                Members = {
                    new PolicyMember
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        DateOfBirth = request.DateOfBirth,
                        IdNumber = request.IdNumber,
                        Gender = request.Gender,
                        SuffixNumber = "01",
                        Status = new MembershipState
                        {
                            ActivationStatus = "ACTIVE",
                            DateAdded = currentDate,
                            IsPolicyHolder = true,
                            PolicyNumber = policyNumber
                        }
                    }
                }
            };

            _dbContext.Policies.Add(policy);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Saved new policy {@Policy}", policy);
                return policy;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.GetBaseException(), "Failed to save policy because of an error");

                if (exception.GetBaseException() is Npgsql.PostgresException pgException)
                {
                    var error = pgException switch
                    {
                        { SqlState: Npgsql.PostgresErrorCodes.UniqueViolation } =>
                           ErrorBuilder.New()
                                       .SetMessage("Cannot create policy because there is another policy member with the same details")
                                       .SetCode("DUPLICATE_POLICY_MEMBER")
                                       .Build(),

                        { SqlState: Npgsql.PostgresErrorCodes.ForeignKeyViolation } =>
                          ErrorBuilder.New()
                                      .SetMessage("Cannot create policy because the selected package could not be found")
                                      .SetCode("INVALID_PACKAGE")
                                      .Build(),
                        _ => ErrorBuilder.New().SetMessage("Failed to save the policy becaouse of an error").Build()
                    };
                    return OneOf<Policy, IError>.FromT1(error);
                }
                return OneOf<Policy, IError>.FromT1(ErrorBuilder.New().SetMessage("Unknown error").SetCode("ERROR").Build());
            }
        }
    }
}

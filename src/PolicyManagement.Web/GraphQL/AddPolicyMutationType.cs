using HotChocolate.Types;
using MediatR;
using PolicyManagement.Web.InputModels;
using PolicyManagement.Web.Mediator.Requests;

namespace PolicyManagement.Web.GraphQL
{
    public class AddPolicyMutationType : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("PolicyUpdates");
            descriptor.Field("createPolicy")
                      .Type<PolicyType>()
                      .Argument("input", arg => arg.Type<AddPolicyInputModelType>())
                      .Resolve(async context =>
                      {
                          var input = context.ArgumentValue<AddPolicyInputModel>("input");
                          var mediator = context.Service<IMediator>();
                          var request = new AddPolicyRequest
                          {
                              CurrencyCode = input.CurrencyCode,
                              DateOfBirth = input.DateOfBirth,
                              FirstName = input.FirstName,
                              Gender = input.Gender,
                              IdNumber = input.IdNumber,
                              LastName = input.LastName,
                              PackageId = input.PackageId
                          };

                          var result = await mediator.Send(request);
                          return result.IsT0 ? result.AsT0 : result.AsT1;
                      });
        }
    }
}

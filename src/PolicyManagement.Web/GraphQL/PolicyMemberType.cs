using HotChocolate.Types;
using PolicyManagement.Data.Models;

namespace PolicyManagement.Web.GraphQL
{
    public class PolicyMemberType : ObjectType<PolicyMember>
    {
        protected override void Configure(IObjectTypeDescriptor<PolicyMember> descriptor)
        {
            descriptor.Field("activationStatus")
                      .Type<StringType>()
                      .Resolve((ctx) => ctx.Parent<PolicyMember>().Status?.ActivationStatus);
            descriptor.Field("memberType")
                      .Type<StringType>()
                      .Resolve(ctx => ctx.Parent<PolicyMember>().Status?.IsPolicyHolder ?? false ? "Policy holder" : "Policy member");
        }
    }
}

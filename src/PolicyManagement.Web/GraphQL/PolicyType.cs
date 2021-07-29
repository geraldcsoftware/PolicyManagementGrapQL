using HotChocolate.Types;
using PolicyManagement.Data.Models;

namespace PolicyManagement.Web.GraphQL
{
    public class PolicyType : ObjectType<Policy>
    {
        protected override void Configure(IObjectTypeDescriptor<Policy> descriptor)
        {
            descriptor.Field(p => p.Members)
                      .UseFiltering();
            descriptor.Field("Status")
                      .Type<StringType>()
                      .Resolve((ctx) => ctx.Parent<Policy>().State?.ActivationStatus);
            descriptor.Field("package")
                      .Type<StringType>()
                      .Resolve(ctx => ctx.Parent<Policy>().Subscription?.Package?.Name);
            descriptor.Field("memberCount")
                      .Type<IntType>()
                      .Resolve(ctx => ctx.Parent<Policy>().Members.Count);
        }
    }
}

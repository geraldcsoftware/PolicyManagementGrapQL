using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data;
using System.Linq;

namespace PolicyManagement.Web.GraphQL
{
    public class PoliciesQueryType : ObjectTypeExtension

    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("PolicyManagement");
            descriptor.Field("policies")
                      .Type<ListType<PolicyType>>()
                      .UseDbContext<PolicyManagementDbContext>()
                      .UseProjection()
                      .UseFiltering()
                      .Resolve((ctx) =>
                      {
                          var factory = ctx.Service<IDbContextFactory<PolicyManagementDbContext>>();
                          var dbContext = factory.CreateDbContext();
                          return dbContext.Policies
                            .AsNoTracking()
                            .Include(p => p.State)
                            .Include(p => p.Members)
                            .AsQueryable();
                      });
        }
    }
}

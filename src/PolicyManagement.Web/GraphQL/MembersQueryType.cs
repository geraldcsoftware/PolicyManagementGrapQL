using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data;
using PolicyManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyManagement.Web.GraphQL
{

    public class MembersQueryType : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("PolicyManagement");
            descriptor
               .Field("members")
               .UseDbContext<PolicyManagementDbContext>()
               .UseProjection()
               .UseFiltering()
               .Resolver((ctx) =>
               {
                   var factory = ctx.Service<IDbContextFactory<PolicyManagementDbContext>>();
                   var dbContext = factory.CreateDbContext();
                   return dbContext.PolicyMembers.AsNoTracking().AsQueryable();
               });
        }
    }
}

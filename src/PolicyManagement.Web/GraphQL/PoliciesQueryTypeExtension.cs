using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using PolicyManagement.Data;
using PolicyManagement.Data.Models;
using System.Linq;

namespace PolicyManagement.Web.GraphQL
{
    [ExtendObjectType("PolicyManagement")]
    public class PoliciesQueryTypeExtension

    {
        [UseProjection]
        [UseFiltering]
        [UseDbContext(typeof(PolicyManagementDbContext))]
        public IQueryable<Policy> GetPolicies([Service] IDbContextFactory<PolicyManagementDbContext> dbContextFactory)
        {
            var dbContext = dbContextFactory.CreateDbContext();
            return dbContext.Policies
                            .Include(p => p.State)
                            .Include(p => p.Members)
                            .AsQueryable();
        }

    }
}

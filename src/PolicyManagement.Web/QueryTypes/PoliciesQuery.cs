using HotChocolate.Types;
using PolicyManagement.Data;
using PolicyManagement.Web.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyManagement.Web.QueryTypes
{
    //public class PoliciesQuery : ObjectTypeExtension
    //{
    //    protected override void Configure(IObjectTypeDescriptor descriptor)
    //    {
    //        descriptor.Name("hu");

    //        descriptor.Field("policies")
    //                  .UseDbContext<PolicyManagementDbContext>()
    //                  .Resolve(context => context.Service<PolicyManagementDbContext>().Policies)
    //                  .Type<PolicyType>();

    //    }
    //}
}

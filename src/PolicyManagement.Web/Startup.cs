using System;
using FluentValidation;
using HotChocolate.AspNetCore;
using HotChocolate.Types.Pagination;
using IdGen;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolicyManagement.Data;
using PolicyManagement.Web.GraphQL;
using PolicyManagement.Web.Mediator.Requests;
using PolicyManagement.Web.Mediator.Validators;

namespace PolicyManagement.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IIdGenerator<long>>(_ =>
            {
                var epoch = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var structure = new IdStructure(39, 4, 20);
                var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));
                var generator = new IdGenerator(1, options);
                return generator;
            });
            services.AddTransient<IValidator<AddPolicyMemberRequest>, AddPolicyMemberValidator>();
            services.AddPooledDbContextFactory<PolicyManagementDbContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var connectionString = configuration.GetConnectionString("DbConnection");

                options.UseNpgsql(connectionString).UseLoggerFactory(loggerFactory);
            });
            services.AddMediatR(typeof(Startup));
            services.AddGraphQLServer()
                    .AddFiltering()
                    .AddProjections()
                    .AddSorting()
                    .SetPagingOptions(new PagingOptions
                    {
                        IncludeTotalCount = true,
                        MaxPageSize = 100,
                        DefaultPageSize = 10
                    })
                    .AddQueryType(q => q.Name("PolicyManagement"))
                    .AddType<PolicyType>()
                    .AddTypeExtension<PoliciesQueryType>()
                    .AddTypeExtension<MembersQueryType>()
                    .AddMutationType(q => q.Name("PolicyUpdates"))
                    .AddTypeExtension<AddPolicyMutationType>()
                    .AddTypeExtension<AddPolicyMemberMutationType>()
                    .AddType<AddPolicyInputModelType>()
                    .AddType<AddPolicyMemberInputType>()
                    .AddType<PolicyMemberType>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<PolicyManagementDbContext>>();
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Database.EnsureCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UsePlayground();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapGraphQL());
        }
    }
}

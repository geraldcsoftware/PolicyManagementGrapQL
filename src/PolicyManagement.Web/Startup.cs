using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolicyManagement.Data;
using PolicyManagement.Web.GraphQL;

namespace PolicyManagement.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddPooledDbContextFactory<PolicyManagementDbContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var connectionString = configuration.GetConnectionString("DbConnection");

                options.UseNpgsql(connectionString).UseLoggerFactory(loggerFactory);
            });

            services.AddGraphQLServer()
                    .AddFiltering()
                    .AddProjections()
                    .AddQueryType(q => q.Name("PolicyManagement"))
                    .AddType<PolicyType>()
                    .AddTypeExtension<PoliciesQueryType>()
                    .AddTypeExtension<MembersQueryType>();
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

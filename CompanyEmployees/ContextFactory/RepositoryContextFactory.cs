using CompanyEmployees.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CompanyEmployees.Api.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                        .Build();
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                            .UseSqlServer(config.GetConnectionString(Constants.SqlConnection), b => b.MigrationsAssembly($"{Constants.CompanyEmployees}.Api"));

            return new RepositoryContext(builder.Options);
        }
    }
}

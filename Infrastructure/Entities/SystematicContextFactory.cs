using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure{

    public class SystematicContextFactory : IDesignTimeDbContextFactory<SystematicContext>
    {
        public SystematicContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("EventToolDB");

            var optionBuilder = new DbContextOptionsBuilder<SystematicContext>()
                .UseSqlServer(connectionString);

            return new SystematicContext(optionBuilder.Options);
        }
    }
}
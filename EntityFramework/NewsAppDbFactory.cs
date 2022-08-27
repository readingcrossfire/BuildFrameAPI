using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EntityFramework
{
    public class NewsAppDbFactory : IDesignTimeDbContextFactory<NewsAppDbContext>
    {
        public NewsAppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            string connectionStrings = configBuilder.GetConnectionString("DB_NEWSAPI");
            DbContextOptionsBuilder<NewsAppDbContext> builder = new DbContextOptionsBuilder<NewsAppDbContext>()
                .UseSqlServer(connectionStrings);
            return new NewsAppDbContext(builder.Options);
        }
    }
}
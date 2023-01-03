using EntityFramework.Configuration;
using ENTITYFRAMEWORK.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ML.Entities;
using ML.Logs;
using ML.MenuTypes;

namespace EntityFramework
{
    public class NewsAppDbContext : IdentityDbContext<NewsAppUser, NewsAppRole, Guid>
    {
        public DbSet<Logs> LogsEntity { get; set; }
        public DbSet<MenuTypesItem> MenuTypeEntity { get; set; }

        public NewsAppDbContext(DbContextOptions<NewsAppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserLoginConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new UserTokenConfiguration());
            builder.ApplyConfiguration(new UserClaimsConfiguration());
            builder.ApplyConfiguration(new RoleClaimsConfiguration());

            builder.ApplyConfiguration(new LogsConfiguration());
            builder.ApplyConfiguration(new MenuTypesConfiguration());
        }
    }
}
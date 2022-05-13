using Microsoft.EntityFrameworkCore;
using ML.Entity;

namespace EF
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        //public DbSet<DrawlsEntity> DbDrawlsEntity { get; set; }
        public DbSet<LogEntity> DbLogEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntity>(buildAction =>
            {
                buildAction.HasKey("ID");
                buildAction.Property("ID").HasColumnType("VARCHAR(100)").IsUnicode(false);
                buildAction.Property("APINAME").HasColumnType("VARCHAR(1000)").IsUnicode(false);
                buildAction.Property("METHODNAME").HasColumnType("VARCHAR(1000)").IsUnicode(false);
                buildAction.Property("IP").HasColumnType("VARCHAR(1000)").IsUnicode(false);
            });
        }
    }
}
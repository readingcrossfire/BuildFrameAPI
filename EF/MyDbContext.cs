using Microsoft.EntityFrameworkCore;
using ML.Entity;

namespace EF
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<DrawlsEntity> DbDrawlsEntity { get; set; }
        public DbSet<LogEntity> DbLogEntity { get; set; }
    }
}
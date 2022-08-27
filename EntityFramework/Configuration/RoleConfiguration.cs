using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ML.Entities;

namespace EntityFramework.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<NewsAppRole>
    {
        public void Configure(EntityTypeBuilder<NewsAppRole> builder)
        {
            builder.ToTable("ROLE");
            builder.Property(x => x.Description).IsRequired(true).HasColumnType("NVARCHAR(MAX)").IsUnicode(true);
        }
    }
}
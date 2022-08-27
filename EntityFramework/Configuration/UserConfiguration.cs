using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ML.Entities;

namespace EntityFramework.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<NewsAppUser>
    {
        public void Configure(EntityTypeBuilder<NewsAppUser> builder)
        {
            builder.ToTable("USER");
            builder.Property(x => x.FullName).IsRequired(true).HasColumnType("NVARCHAR(200)").IsUnicode(true);
        }
    }
}
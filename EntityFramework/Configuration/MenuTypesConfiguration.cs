using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ML.MenuTypes;

namespace ENTITYFRAMEWORK.Configuration
{
    public class MenuTypesConfiguration : IEntityTypeConfiguration<MenuTypesItem>
    {
        public void Configure(EntityTypeBuilder<MenuTypesItem> builder)
        {
            builder.ToTable("MENUTYPES");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().HasColumnType("VARCHAR(200)").IsUnicode(false);
            builder.Property(x => x.Key).IsRequired().HasColumnType("NVARCHAR(200)").IsUnicode(true);
            builder.Property(x => x.Name).IsRequired().HasColumnType("NVARCHAR(MAX)").IsUnicode(true);
            builder.Property(x => x.Type).IsRequired().HasColumnType("NVARCHAR(MAX)").IsUnicode(true);
        }
    }
}
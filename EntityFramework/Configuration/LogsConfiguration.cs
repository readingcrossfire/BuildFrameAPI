using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ML.Logs;

namespace EntityFramework.Configuration
{
    public class LogsConfiguration : IEntityTypeConfiguration<Logs>
    {
        public void Configure(EntityTypeBuilder<Logs> builder)
        {
            builder.ToTable("LOGS");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("VARCHAR(200)").IsUnicode(false);
            builder.Property(x => x.APIName).IsRequired().HasColumnType("NVARCHAR(MAX)").IsUnicode(true);
            builder.Property(x => x.MethodName).IsRequired().HasColumnType("NVARCHAR(MAX)").IsUnicode(true);
            builder.Property(x => x.Description).IsRequired(false).HasColumnType("NVARCHAR(MAX)").IsUnicode(true);
            builder.Property(x => x.Params).IsRequired(false).HasColumnType("NVARCHAR(MAX)").IsUnicode(true);
            builder.Property(x => x.CreatedDate).IsRequired(true).HasColumnType("DATETIME2").HasDefaultValue(DateTime.Now);
        }
    }
}
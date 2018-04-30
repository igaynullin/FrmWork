using System.Data;
using FrmWork.Data.Constants;
using FrmWork.Objects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrmWork.Data.EntityTypeConfigurations
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"{builder.Metadata.Name}s", nameof(DbSchemas.Identity));
            builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            builder.Property(e => e.Creator).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Modifier).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
        }
    }
}
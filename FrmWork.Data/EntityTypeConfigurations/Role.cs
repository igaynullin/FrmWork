using System.Data;
using FrmWork.Data.Constants;
using FrmWork.Objects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrmWork.Data.EntityTypeConfigurations
{
    public class RoleTypeConfiguration : IEntityTypeConfiguration<Role>

    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable($"{builder.Metadata.Name}s", nameof(DbSchemas.System));
            builder.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
            builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            builder.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            builder.Property(e => e.Description).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Creator).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Modifier).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
        }
    }
}
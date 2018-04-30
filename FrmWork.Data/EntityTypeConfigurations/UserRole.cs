using FrmWork.Data.Constants;
using FrmWork.Objects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace FrmWork.Data.EntityTypeConfigurations
{
    public class UserRoleTypeConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable($"{builder.Metadata.Name}s", nameof(DbSchemas.Identity));
            builder.HasKey(r => new { r.UserId, r.RoleId });
            builder.Property(e => e.Creator).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Modifier).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
        }
    }
}
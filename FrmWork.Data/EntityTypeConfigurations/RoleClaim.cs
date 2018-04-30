using System.Data;
using FrmWork.Data.Constants;
using FrmWork.Objects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrmWork.Data.EntityTypeConfigurations
{
    public class RoleClaimTypeConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable($"{builder.Metadata.Name}s", nameof(DbSchemas.Identity));
            builder.HasKey(rc => rc.Id);
            builder.Property(e => e.Creator).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Modifier).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
        }
    }
}
using System.Data;
using FrmWork.Data.Constants;
using FrmWork.Objects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrmWork.Data.EntityTypeConfigurations
{
    public class AuditEntityLogTypeConfiguration : IEntityTypeConfiguration<AuditEntityLog>

    {
        public void Configure(EntityTypeBuilder<AuditEntityLog> builder)
        {
            builder.ToTable($"{builder.Metadata.Name}s", nameof(DbSchemas.System));

            builder.Property(e => e.Action).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Creator).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.EntityName).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.HasIndex(e => new { e.EntityId, e.EntityName });
        }
    }
}
using FrmWork.Data.Constants;
using FrmWork.Objects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace FrmWork.Data.EntityTypeConfigurations
{
    public class UserTokenTypeConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable($"{builder.Metadata.Name}s", nameof(DbSchemas.Identity));
            builder.Property(m => m.LoginProvider).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(m => m.Name).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(m => m.Value).HasMaxLength(500).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Creator).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Modifier).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
        }
    }
}
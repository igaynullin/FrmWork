using FrmWork.Data.Constants;
using FrmWork.Objects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace FrmWork.Data.EntityTypeConfigurations
{
    public class UserLoginTypeConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable($"{builder.Metadata.Name}s", nameof(DbSchemas.Identity));
            builder.Property(m => m.LoginProvider).HasMaxLength(500).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(m => m.ProviderKey).HasMaxLength(500).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Creator).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
            builder.Property(e => e.Modifier).HasMaxLength(256).HasColumnType(nameof(SqlDbType.NVarChar));
        }
    }
}
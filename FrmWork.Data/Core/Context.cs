using System;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FrmWork.Data.AuditEntityLogger;
using FrmWork.Objects.Interfaces.General;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FrmWork.Objects.Entities;
using enitityTypeConfigurations = FrmWork.Data.EntityTypeConfigurations;

namespace FrmWork.Data.Core
{
    public class DbCtx : IdentityDbContext<
          User
        , Role
        , long
        , UserClaim
        , UserRole
        , UserLogin
        , RoleClaim
        , UserToken>

    {
        #region System

        protected Microsoft.EntityFrameworkCore.DbSet<AuditEntityLog> AuditEntityLog { get; set; }

        #endregion System

        protected IConfiguration _configuration { get; }
        protected IPrincipal _principal { get; }
        protected IAuditEntityLogger _auditor { get; }

        static DbCtx()
        {
            //  ObjectMapper.MapObjects();
        }

        public DbCtx(IConfiguration configuration, IPrincipal principal, IAuditEntityLogger auditor = null)
        {
            _configuration = configuration;
            _principal = principal;
            _auditor = auditor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //foreach (IEntityType entity in builder.Model.GetEntityTypes())
            //    foreach (PropertyInfo property in entity.ClrType.GetProperties())
            //        if (property.GetCustomAttribute<IndexAttribute>(false) is IndexAttribute index)
            //            builder.Entity(entity.ClrType).HasIndex(property.Name).IsUnique(index.IsUnique);

            //builder.Entity<Permission>().Property(model => model.Id).ValueGeneratedNever();
            //foreach (IMutableForeignKey key in builder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            //    key.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new enitityTypeConfigurations.AuditEntityLogTypeConfiguration());
            builder.ApplyConfiguration(new enitityTypeConfigurations.RoleTypeConfiguration());
            builder.ApplyConfiguration(new enitityTypeConfigurations.RoleClaimTypeConfiguration());
            builder.ApplyConfiguration(new enitityTypeConfigurations.UserTypeConfiguration());
            builder.ApplyConfiguration(new enitityTypeConfigurations.UserClaimTypeConfiguration());
            builder.ApplyConfiguration(new enitityTypeConfigurations.UserLoginTypeConfiguration());
            builder.ApplyConfiguration(new enitityTypeConfigurations.UserRoleTypeConfiguration());
            builder.ApplyConfiguration(new enitityTypeConfigurations.UserTokenTypeConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            _auditor?.Log(this.ChangeTracker.Entries<IHasId<long>>());

            this.SaveChanges();

            _auditor?.Save();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => this.SaveChanges(), cancellationToken);
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //    builder.UseSqlServer(Config["Data:Connection"]);
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //{
        //}
    }
}
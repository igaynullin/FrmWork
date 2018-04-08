using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FrmWork.Data.Logging;
using FrmWork.Objects.Interfaces.General;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using customEntities = FrmWork.Objects.Entities;

namespace FrmWork.Data.Core
{
    public class DbCtx : IdentityDbContext<
          customEntities.User
        , customEntities.Role
        , long
        , customEntities.UserClaim
        , customEntities.UserRole
        , customEntities.UserLogin
        , customEntities.RoleClaim
        , customEntities.UserToken>

    {
        #region System

        protected Microsoft.EntityFrameworkCore.DbSet<customEntities.AuditLog> AuditLog { get; set; }

        #endregion System

        protected IConfiguration _configuration { get; }
        protected IPrincipal _principal { get; }
        protected IAuditLogger _auditor { get; }

        static DbCtx()
        {
            //  ObjectMapper.MapObjects();
        }

        public DbCtx(IConfiguration configuration, IPrincipal principal, IAuditLogger auditor = null)
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
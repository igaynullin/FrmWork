using System.Security.Principal;
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

        static DbCtx()
        {
            //  ObjectMapper.MapObjects();
        }

        public DbCtx(IConfiguration configuration, IPrincipal principal)
        {
            _configuration = configuration;
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

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //    builder.UseSqlServer(Config["Data:Connection"]);
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //{
        //}
    }
}
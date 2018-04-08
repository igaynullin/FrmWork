using System;
using System.Linq;
using System.Security.Principal;
using FrmWork.Data.Core;
using FrmWork.Objects.Interfaces.General;
using Microsoft.EntityFrameworkCore;

namespace FrmWork.Data.Extentions
{
    public static class DbContextExtentions
    {
        public static void AuditEntities(this DbCtx db, IPrincipal principal)
        {
            try
            {
                var modifiedEntries = db.ChangeTracker.Entries()
               .Where(x => x.Entity is IHasAudit<string, DateTime?> && (x.State == EntityState.Added || x.State == EntityState.Modified));
                var username = principal != null ? principal.Identity.Name : "anonymous";
                DateTime now = DateTime.UtcNow;
                foreach (var entry in modifiedEntries)
                {
                    var entity = (IHasAudit<string, DateTime?>)entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                        entity.Creator = username;
                    }
                    else
                    {
                        db.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                        db.Entry(entity).Property(x => x.Creator).IsModified = false;
                    }

                    entity.ModifiedAt = now;
                    entity.Modifier = username;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
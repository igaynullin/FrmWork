using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using FrmWork.Data.Constants;
using FrmWork.Objects.Entities;
using FrmWork.Objects.Interfaces.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FrmWork.Data.AuditEntityLogger
{
    public class AuditEntityLogger : IAuditEntityLogger
    {
        private DbContext _db { get; }
        private IPrincipal _principal { get; }
        private List<LoggableEntity> _entities { get; }

        public AuditEntityLogger(DbContext db, IPrincipal principal)
        {
            _db = db;
            _principal = principal;
            _entities = new List<LoggableEntity>();
            // _db.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public void Log(IEnumerable<EntityEntry<IHasId<long>>> entries)
        {
            foreach (var entry in entries)
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        var entity = new LoggableEntity(entry);
                        if (entity.Properties.Any())
                            Log(entity);
                        break;
                }
        }

        public void Log(LoggableEntity entity)
        {
            _entities.Add(entity);
        }

        public void Save()
        {
            var creator = _principal?.Identity.Name;
            var table = new DataTable();

            foreach (var entity in _entities)
            {
                var row = table.NewRow();

                var changes = entity.ToString();
                if (string.IsNullOrEmpty(changes))
                    row[nameof(AuditEntityLog.Changes)] = DBNull.Value;
                else
                    row[nameof(AuditEntityLog.Changes)] = changes;

                if (string.IsNullOrEmpty(entity.Name))
                    row[nameof(AuditEntityLog.EntityName)] = DBNull.Value;
                else
                    row[nameof(AuditEntityLog.EntityName)] = entity.Name;

                if (string.IsNullOrEmpty(entity.Action))
                    row[nameof(AuditEntityLog.Action)] = DBNull.Value;
                else
                    row[nameof(AuditEntityLog.Action)] = entity.Action;

                row[nameof(AuditEntityLog.EntityId)] = entity.Id();
                row[nameof(AuditEntityLog.CreatedAt)] = DateTime.Now;
                row[nameof(AuditEntityLog.Creator)] = creator;

                table.Rows.Add(row);
            }

            using (var connection = (SqlConnection)_db.Database.GetDbConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                {
                    bulkCopy.BatchSize = 100;
                    bulkCopy.DestinationTableName = $"{nameof(DbSchemas.System)}{nameof(AuditEntityLog)}s";
                    try
                    {
                        bulkCopy.WriteToServer(table);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        connection.Close();
                    }
                }

                transaction.Commit();
            }

            _entities.Clear();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
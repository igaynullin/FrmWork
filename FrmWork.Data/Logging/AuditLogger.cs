using FrmWork.Objects;
using FrmWork.Objects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Principal;
using FrmWork.Objects.Interfaces.General;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Data.SqlClient;
using System.Text;

namespace FrmWork.Data.Logging
{
    public class AuditLogger : IAuditLogger
    {
        static AuditLogger()
        {
        }

        private StringBuilder _sql { get; }
        private DbContext _db { get; }
        private IPrincipal _principal { get; }
        private List<LoggableEntity> _entities { get; }

        public AuditLogger(DbContext db, IPrincipal principal)
        {
            _db = db;
            _principal = principal;
            _entities = new List<LoggableEntity>();
            // _db.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public void Log(IEnumerable<EntityEntry<IHasId<long>>> entries)
        {
            foreach (EntityEntry<IHasId<long>> entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        LoggableEntity entity = new LoggableEntity(entry);
                        if (entity.Properties.Any())
                            Log(entity);
                        break;
                }
            }
        }

        public void Log(LoggableEntity entity)
        {
            _entities.Add(entity);
        }

        public void Save()
        {
            AuditLog log = new AuditLog();
            var creator = _principal?.Identity.Name;
            var table = new DataTable();

            foreach (LoggableEntity entity in _entities)
            {
                var row = table.NewRow();

                var changes = entity.ToString();
                if (string.IsNullOrEmpty(changes))
                    row[nameof(log.Changes)] = DBNull.Value;
                else
                    row[nameof(log.Changes)] = changes;

                if (string.IsNullOrEmpty(entity.Name))
                    row[nameof(log.EntityName)] = DBNull.Value;
                else
                    row[nameof(log.EntityName)] = entity.Name;

                if (string.IsNullOrEmpty(entity.Action))
                    row[nameof(log.Action)] = DBNull.Value;
                else
                    row[nameof(log.Action)] = entity.Action;

                row[nameof(log.EntityId)] = entity.Id();
                row[nameof(log.CreatedAt)] = DateTime.Now;
                row[nameof(log.Creator)] = creator;

                table.Rows.Add(row);
            }

            using (var connection = (SqlConnection)_db.Database.GetDbConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                {
                    bulkCopy.BatchSize = 100;
                    bulkCopy.DestinationTableName = "System.AuditLogs";
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
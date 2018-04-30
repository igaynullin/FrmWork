using System;
using System.Collections.Generic;
using FrmWork.Objects.Interfaces.General;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FrmWork.Data.AuditEntityLogger
{
    public interface IAuditEntityLogger : IDisposable
    {
        void Log(IEnumerable<EntityEntry<IHasId<long>>> entries);

        void Save();
    }
}
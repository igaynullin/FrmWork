using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using FrmWork.Objects.Interfaces.General;

namespace FrmWork.Data.Logging
{
    public interface IAuditLogger : IDisposable
    {
        void Log(IEnumerable<EntityEntry<IHasId<long>>> entries);

        void Save();
    }
}
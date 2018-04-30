using System;
using System.ComponentModel.DataAnnotations.Schema;
using FrmWork.Objects.Interfaces.General;

namespace FrmWork.Objects.Entities
{
    public class AuditEntityLog : IHasCreate<string, DateTime>
    {
        public string Action { get; set; }

        public string EntityName { get; set; }

        public long EntityId { get; set; }

        public string Changes { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
using System;
using FrmWork.Objects.Interfaces.General;
using Microsoft.AspNetCore.Identity;

namespace FrmWork.Objects.Entities
{
    public class Role : IdentityRole<long>, IHasId<long>, IHasAudit<string, DateTime?>, IHasDescription
    {
        public string Creator { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Description { get; set; }
    }
}
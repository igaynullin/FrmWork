using System;
using FrmWork.Objects.Interfaces.General;
using Microsoft.AspNetCore.Identity;

namespace FrmWork.Objects.Entities
{
    public class UserClaim : IdentityUserClaim<long>
        , IHasId<long>
        , IHasAudit<string, DateTime?>
    {
        public string Creator { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public new long Id { get; set; }
    }
}
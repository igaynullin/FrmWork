using FrmWork.Objects.Interfaces.Entities;
using FrmWork.Objects.Interfaces.General;

namespace FrmWork.Objects.Entities
{
    public abstract class BaseEntity<TId, TUser, TDate> : IHasId<TId>, IHasAudit<TUser, TDate>, IHasLocalizationName, IEntity
    {
        public virtual TUser Creator { get; set; }
        public virtual TDate CreatedAt { get; set; }
        public virtual TUser Modifier { get; set; }
        public virtual TDate ModifiedAt { get; set; }
        public virtual TId Id { get; set; }

        public virtual string NameRu { get; set; }
        public virtual string NameEn { get; set; }
    }
}
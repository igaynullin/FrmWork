using System;
using System.Collections.Generic;
using System.Text;
using FrmWork.Objects.Interfaces.General;
using FrmWork.Objects.Interfaces.Models;

namespace FrmWork.Objects.Models
{
    public abstract class BaseModel<TId, TUser, TDate> : IHasId<TId>, IHasAudit<TUser, TDate>, IHasLocalizationName, IModel
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
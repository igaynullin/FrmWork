using FrmWork.Objects.Interfaces.General;

namespace FrmWork.Objects.Interfaces.Entities
{
    public interface IEntity<TId, TUser, TDate, TRowState, TLocalization, TOwner> :
          IHasId<TId>
        , IHasAudit<TUser, TDate>
        , IHasName
        , IHasRowState<TRowState>
        , IHasParent<TId>
        , IHasLocalizationId<TLocalization>
        , IHasOwner<TOwner>, IEntity
    {
    }

    public interface IEntity<TId, TUser, TDate> :
        IHasId<TId>
        , IHasAudit<TUser, TDate>
        , IHasName, IEntity
    {
    }

    public interface IEntity

    {
    }
}
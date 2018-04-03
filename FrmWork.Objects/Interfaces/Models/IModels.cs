using FrmWork.Objects.Interfaces.General;

namespace FrmWork.Objects.Interfaces.Models
{
    public interface IModel<TId, TUser, TDate, TRowState, TLocalization, TOwner> :
          IHasId<TId>
        , IHasAudit<TUser, TDate>
        , IHasName
        , IHasRowState<TRowState>
        , IHasParent<TId>
        , IHasLocalizationId<TLocalization>
        , IHasOwner<TOwner>, IModel
    {
    }

    public interface IModel<TId, TUser, TDate> :
        IHasId<TId>
        , IHasAudit<TUser, TDate>
        , IHasName, IModel
    {
    }

    public interface IModel

    {
    }
}
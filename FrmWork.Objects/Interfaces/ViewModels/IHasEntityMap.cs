using System;

namespace FrmWork.Objects.Interfaces.ViewModels
{
    public interface IHasEntityMap<TViewModel, TEntity>
    {
        Func<TViewModel, TEntity> EntityMapper { get; set; }

        TEntity GetModel(TViewModel model);
    }
}
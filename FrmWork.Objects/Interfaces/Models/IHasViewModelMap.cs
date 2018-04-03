using System;

namespace FrmWork.Objects.Interfaces.Models

{
    public interface IHasViewModelMap<TEntity, TViewModel>
    {
        Func<TEntity, TViewModel> Mapper { get; set; }

        TViewModel GetViewModel(TEntity entity);
    }
}
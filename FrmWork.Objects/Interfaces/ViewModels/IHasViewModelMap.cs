using System;

namespace FrmWork.Objects.Interfaces.ViewModels

{
    public interface IHasViewModelMap<TEntity, TViewModel>
    {
        Func<TEntity, TViewModel> ViewModelMapper { get; set; }

        TViewModel GetViewModel(TEntity entity);
    }
}
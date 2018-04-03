using FrmWork.Interfaces.ViewModelBases;
using System.Linq;

namespace FrmWork.Interfaces.Objects.ViewModelBases
{
    /// <summary>
    /// Use for search view models
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IHasFilterPanelViewModel<TModel> : IViewModel

    {
        IQueryable<TModel> GetWhereQuery(IQueryable<TModel> query);
    }
}
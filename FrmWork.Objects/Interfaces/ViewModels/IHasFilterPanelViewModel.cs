using System.Linq;

namespace FrmWork.Objects.Interfaces.ViewModels
{
    /// <summary>
    /// Use for search view models
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IHasFilterPanelViewModel<TEntity> : IViewModel

    {
        IQueryable<TEntity> GetWhereQuery(IQueryable<TEntity> query);
    }
}
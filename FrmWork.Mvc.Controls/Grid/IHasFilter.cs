using System.Linq;
using FrmWork.Objects.Interfaces.ViewModels;

namespace FrmWork.Mvc.Controls.Grid
{
    /// <summary>
    /// Use for search view models
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IHasFilter<TEntity>

    {
        IQueryable<TEntity> GetWhereQuery(IQueryable<TEntity> query);
    }
}
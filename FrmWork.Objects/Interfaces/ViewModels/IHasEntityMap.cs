namespace FrmWork.Objects.Interfaces.ViewModels
{
    public interface IHasEntityMap<in TViewModel, out TEntity>
    {
        TEntity GetModel(TViewModel model);
    }
}
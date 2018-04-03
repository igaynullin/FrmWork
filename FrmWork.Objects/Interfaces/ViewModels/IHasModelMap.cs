namespace FrmWork.Objects.Interfaces.ViewModels
{
    public interface IHasModelMap<in TViewModel, out TModel>
    {
        TModel GetModel(TViewModel model);
    }
}
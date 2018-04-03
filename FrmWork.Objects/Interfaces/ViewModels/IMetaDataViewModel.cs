using FrmWork.Interfaces.ViewModelBases;

namespace FrmWork.Objects.Interfaces.ViewModels
{
    public interface IMetaDataViewModel<TRecource>
    {
        string Title { get; set; }
        TRecource Resource { get; set; }
    }
}
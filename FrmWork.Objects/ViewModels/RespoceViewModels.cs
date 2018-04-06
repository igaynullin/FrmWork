using FrmWork.Objects.Models;

namespace FrmWork.Objects.ViewModels
{
    using Interfaces.ViewModels;

    public class ResponceViewModel<TViewModel, TResponceState, TResource> : IViewModel
    {
        public virtual TViewModel ViewModel { get; set; }
        public virtual TResponceState ProcessingResult { get; set; }
        public virtual IMetaDataViewModel<TResource> MetaDataViewModel { get; set; }
    }

    public class ResponceViewModel<TViewModel, TResource> : IViewModel
    {
        public virtual TViewModel ViewModel { get; set; }
        public virtual ProcessingResult ProcessingResult { get; set; }
        public virtual IMetaDataViewModel<TResource> MetaDataViewModel { get; set; }
    }
}
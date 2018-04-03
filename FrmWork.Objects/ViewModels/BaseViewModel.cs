using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using FrmWork.Interfaces.ViewModelBases;
using FrmWork.Objects.Interfaces.General;
using FrmWork.Objects.Interfaces.ViewModels;

namespace FrmWork.Objects.ViewModels
{
    public abstract class BaseViewModel : IHasName, IHasLocalizationName, IHasId<string>, IViewModel
    {
        public virtual string NameRu { get; set; }
        public virtual string NameEn { get; set; }
        public virtual string Id { get; set; }
        public virtual string Name { get => this.Name = nameof(IHasLocalizationName.NameEn).Contains(Thread.CurrentThread.CurrentUICulture.Name) ? this.NameEn : this.NameRu; set => throw new NotImplementedException(); }
    }

    public class ResponceViewModel<TViewModel, TResponceState, TResource> : IViewModel
    {
        public virtual TViewModel ViewModel { get; set; }
        public virtual TResponceState ResponceState { get; set; }
        public virtual IMetaDataViewModel<TResource> MetaDataViewModel { get; set; }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FrmWork.Objects.Interfaces.General;
using FrmWork.Objects.Interfaces.ViewModels;

namespace FrmWork.Objects.ViewModels
{
    public abstract class BaseViewModel : IHasName, IHasLocalizationName, IHasId<string>, IViewModel
    {
        [Display()]
        public virtual string NameRu { get; set; }

        public virtual string NameEn { get; set; }
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
    }
}
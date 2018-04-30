using System;
using System.Collections.Generic;
using System.Text;
using FrmWork.Mvc.Controls.Grid;
using FrmWork.Mvc.Controls.Grid.Core;
using FrmWork.Objects.Interfaces.ViewModels;

namespace FrmWork.ViewModels.Shared
{
    public class ListViewModel<TData, TFilterViewModel> : IViewModel
    {
        public TData Data { get; set; }
        public TFilterViewModel FilterViewModel { get; set; }
        public IPagedList PagedList { get; set; }
        public AjaxOptions AjaxOptions { get; set; }
    }
}
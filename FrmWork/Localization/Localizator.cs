using System;
using System.Collections.Generic;
using System.Text;
using FrmWork.Objects.ViewModels;
using System.Threading;
using FrmWork.Objects.Interfaces.General;

namespace FrmWork.Localization
{
    public static class LocalizatorName
    {
        private static readonly HashSet<string> _defaultLocalizations;

        static LocalizatorName()
        {
            _defaultLocalizations = new HashSet<string>
            {
                nameof(IHasLocalizationName.NameEn).Replace("Name",""),
                nameof(IHasLocalizationName.NameRu).Replace("Name",""),
            };
        }

        public static void LocalizateName(this BaseViewModel viewModel)
        {
            var currentLocalization = Thread.CurrentThread.CurrentUICulture.Name;

            viewModel.Name = nameof(IHasLocalizationName.NameEn).Contains(currentLocalization) ? viewModel.NameEn : viewModel.NameRu;
        }
    }

    //public static class LocalizatorId
    //{
    //    private static List<string> _defaultLocalizations;
    //    static LocalizatorId()
    //    {
    //        _defaultLocalizations = new List<string>
    //        {
    //            nameof(IHasLocalizationName.NameEn).Replace("Name",""),
    //            nameof(IHasLocalizationName.NameRu).Replace("Name",""),
    //        };
    //    }

    //    public static void LocalizateName(this BaseViewModel viewModel)
    //    {
    //        var curLocalization = Thread.CurrentThread.CurrentUICulture.
    //    }
    //}
}
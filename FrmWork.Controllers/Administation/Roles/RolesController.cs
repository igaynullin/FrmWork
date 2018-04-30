using System;
using System.Collections.Generic;
using System.Linq;
using FrmWork.Controllers.Constants;
using FrmWork.Data.Core;
using FrmWork.Mvc.Controls.Grid;
using FrmWork.Mvc.Controls.Grid.Core;
using FrmWork.Objects.Entities;
using FrmWork.Objects.Models;
using FrmWork.Objects.ViewModels;
using FrmWork.ViewModels.Administration.Roles;
using Microsoft.AspNetCore.Mvc;
using FrmWork.Resources.General;
using FrmWork.ViewModels.Shared;
using Microsoft.EntityFrameworkCore;

namespace FrmWork.Controllers.Administation.Roles
{
    [Area(nameof(Areas.Administration))]
    public class RolesController : Controller
    {
        private readonly DbCtx _db;

        public RolesController(DbCtx db)
        {
            _db = db;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var processingResult = new ProcessingResult { MessageType = MessageType.Error };
            try
            {
                var pagedList = new PagedList<Role>(_db.Roles.AsNoTracking());
                var metaData = pagedList.GetMetaData();
                metaData.Area = nameof(Areas.Administration);
                metaData.Controller = nameof(RolesController);
                metaData.Action = nameof(this._Index);
                var data = pagedList.Select(e => new IndexViewModel(e));
                var ajaxOptions = new AjaxOptions { UpdateTargetId = "roles", Url = $"/{metaData.Area}/{metaData.Controller}/{ metaData.Action}" };
                var viewModel = new ListViewModel<IEnumerable<IndexViewModel>, IndexFilterViewModel>
                {
                    Data = data,
                    AjaxOptions = ajaxOptions
                };
                processingResult.MessageType = MessageType.Success;
                var responce = new ResponceViewModel<ListViewModel<IEnumerable<IndexViewModel>, IndexFilterViewModel>, General>
                {
                    ViewModel = viewModel,
                    ProcessingResult = processingResult
                };
                return View(responce);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        [HttpGet]
        public ViewResult _Index(IndexFilterViewModel filter, IPagedList pagedList)
        {
            return null;
        }

        //[HttpGet]
        //public ViewResult Create()
        //{
        //    RoleView role = new RoleView();
        //    Service.SeedPermissions(role);

        //    return View(role);
        //}

        //[HttpPost]
        //public ActionResult Create([BindExcludeId] RoleView role)
        //{
        //    if (!Validator.CanCreate(role))
        //    {
        //        Service.SeedPermissions(role);

        //        return View(role);
        //    }

        //    Service.Create(role);

        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public ActionResult Details(int id)
        //{
        //    return NotEmptyView(Service.GetView(id));
        //}

        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    return NotEmptyView(Service.GetView(id));
        //}

        //[HttpPost]
        //public ActionResult Edit(RoleView role)
        //{
        //    if (!Validator.CanEdit(role))
        //    {
        //        Service.SeedPermissions(role);

        //        return View(role);
        //    }

        //    Service.Edit(role);

        //    Authorization?.Refresh();

        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public ActionResult Delete(int id)
        //{
        //    return NotEmptyView(Service.GetView(id));
        //}

        //[HttpPost]
        //[ActionName("Delete")]
        //public RedirectToActionResult DeleteConfirmed(int id)
        //{
        //    Service.Delete(id);

        //    Authorization?.Refresh();

        //    return RedirectToAction("Index");
        //}
    }
}
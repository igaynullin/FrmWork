using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FrmWork.Mvc.Controls.Grid;
using FrmWork.Objects.Entities;
using FrmWork.Objects.Interfaces.General;
using FrmWork.Objects.Interfaces.ViewModels;

namespace FrmWork.ViewModels.Administration.Roles
{
    public class IndexViewModel : IHasId<string>, IViewModel, IHasName, IHasDescription
    {
        public IndexViewModel(Role role)
        {
            this.Id = role.Id.ToString();
            this.Name = role.Name;
            this.Description = role.Description;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using FrmWork.Mvc.Controls.Grid;
using FrmWork.Mvc.Controls.Grid.Core;
using FrmWork.Objects.Entities;

namespace FrmWork.ViewModels.Administration.Roles
{
    public class IndexFilterViewModel : IHasFilter<Role>
    {
        public string Name { get; set; }

        [NotMapped]
        public AjaxOptions AjaxOptions { get; set; }

        public IQueryable<Role> GetWhereQuery(IQueryable<Role> query)
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                query = query.Where(e => (e.Name.Contains(this.Name)));
            }

            return query;
        }
    }
}
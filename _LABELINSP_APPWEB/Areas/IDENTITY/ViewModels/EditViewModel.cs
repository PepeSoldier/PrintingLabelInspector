using Microsoft.AspNet.Identity.EntityFramework;
using MDLX_CORE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _LABELINSP_APPWEB.Areas.IDENTITY.ViewModels
{
    public class EditViewModel
    {
        public bool EditUserMode { get; set; }
        public bool EditUserRoleMode { get; set; }

        public User User { get; set; }
        public List<User> Subordinates { get; set; }
        public List<ApplicationRole> UserRoles { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        [DisplayName("Dział")]
        public int SelectedDepartmentId { get; set; }
        [DisplayName("Wybierz Rolę")]
        public string SelectedRoleId { get; set; }
        public string DeputyId { get; set; }
        public string UserId { get; set; }
        public string DefaultRoleId { get; set; }

        public EditViewModel() { }
    }
}
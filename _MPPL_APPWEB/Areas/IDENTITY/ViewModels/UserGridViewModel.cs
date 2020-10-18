using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _LABELINSP_APPWEB.Areas.IDENTITY.ViewModels
{
    public class UserGridViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string Email { get; internal set; }

        public string Title { get; set; }
        public string Factory { get; set; }

        public int? DepartmentId { get; set; }
        public string RoleId { get; set; }
        public string SuperVisorUserId { get; set; }
    }
}
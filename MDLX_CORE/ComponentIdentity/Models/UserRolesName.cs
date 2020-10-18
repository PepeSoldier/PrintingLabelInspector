using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MDLX_CORE.Models.IDENTITY
{
    public class UserRolesName
    {
        public int id { get; set; }

        public UserRolesName()
        {
        }

        [DisplayName("Login")]
        public User user { get; set; }

        [DisplayName("Przypisana Rola")]
        public ApplicationRole role { get; set; }
    }
}
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MDLX_CORE.Models.IDENTITY
{
    public class UserRole : IdentityUserRole<string>
    {
        public UserRole() { Id = 0; }

        [Key]
        public int Id { get; set; }
    }
}
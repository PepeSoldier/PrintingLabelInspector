namespace _LABELINSP_APPWEB.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using XLIB_COMMON.Enums;
    using XLIB_COMMON.Repo.Base;
    using XLIB_COMMON.Repo.IDENTITY;

    public static class CnfigurationHelper
    {
        public static void Seed(DbContextAPP_Dev context)
        {
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            //RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);

            //string role1 = _roleManager.AddRole(DefRoles.ADMIN);
            //string adminId = _userManager.AddUser("Admin", DefRoles.ADMIN);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ADMIN);
        }
    }
}


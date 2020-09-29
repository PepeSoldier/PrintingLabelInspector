using MDLX_MASTERDATA.Enums;

namespace _MPPL_WEB_START.Migrations
{
    using MDL_BASE.ComponentBase.Entities;
    using MDL_BASE.ComponentBase.Repos;
    using MDL_BASE.Interfaces;
    using MDL_BASE.Models.IDENTITY;
    using MDL_BASE.Models.MasterData;
    using MDL_CORE.ComponentCore.Entities;
    using MDL_CORE.ComponentCore.Enums;
    using MDLX_CORE.ComponentCore.Entities;
    using MDLX_CORE.ComponentCore.Repos;
    using MDLX_MASTERDATA._Interfaces;
    using MDLX_MASTERDATA.Entities;
    using MDLX_MASTERDATA.Enums;
    using MDLX_MASTERDATA.Repos;
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
        public static void AddAccountAdminRoleAndAssignToAdmin(DbContextAPP_ context)
        {
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            //RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            //string accountAdminRoleId = _roleManager.AddRole(DefRoles.ACCOUNT_ADMIN);
            
            //List<string> adminRolesIds = context.Roles.Where(x => x.Name == "ADMIN" || x.Name == "Admin").Select(x => x.Id).ToList();
            //List<string> adminUserIds = context.UserRoles.Where(x => adminRolesIds.Contains(x.RoleId)).Select(x => x.UserId).ToList();

            //ApplicationRole role = context.Roles.Where(x => x.Name == DefRoles.ACCOUNT_ADMIN).FirstOrDefault();

            //foreach (var admUserId in adminUserIds)
            //{
            //    UserRole userRole = context.UserRoles.FirstOrDefault(x => x.RoleId == role.Id && x.UserId == admUserId);

            //    if (userRole == null)
            //    {
            //        userRole = new UserRole();
            //        userRole.RoleId = role.Id;
            //        userRole.UserId = admUserId;
            //        context.UserRoles.Add(userRole);
            //        context.SaveChanges();
            //    }
            //}
        }

        public static void Seed(DbContextAPP_Dev context)
        {
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            //RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            
            //string role1 = _roleManager.AddRole(DefRoles.ADMIN);
            //string role2 = _roleManager.AddRole(DefRoles.USER);
            //string role4 = _roleManager.AddRole(DefRoles.PRD_SCHEDULER);
            //string role5 = _roleManager.AddRole(DefRoles.PFEP_DEFPRINT_EDITOR);

            //string role6 = _roleManager.AddRole(DefRoles.ILOGIS_ADMIN);
            //string role7 = _roleManager.AddRole(DefRoles.ILOGIS_OPERATOR);
            //string role8 = _roleManager.AddRole(DefRoles.ILOGIS_VIEWER);

            //string role9 = _roleManager.AddRole(DefRoles.ONEPROD_VIEWER);
            //string role10 = _roleManager.AddRole(DefRoles.ONEPROD_ADMIN);
            //string role11 = _roleManager.AddRole(DefRoles.ONEPROD_MES_OPERATOR);
            //string role12 = _roleManager.AddRole(DefRoles.ONEPROD_MES_SUPEROPERATOR);
            //string role13 = _roleManager.AddRole(DefRoles.ILOGIS_WHDOC_APPROVER);
            //string role14 = _roleManager.AddRole(DefRoles.ILOGIS_WHDOC_EDITOR);
            //string role15 = _roleManager.AddRole(DefRoles.ILOGIS_WHDOC_VIEWER);
            //string role16 = _roleManager.AddRole(DefRoles.ACCOUNT_ADMIN);

            //string adminId = _userManager.AddUser("Admin", DefRoles.ADMIN);
            //string prdoperatorId = _userManager.AddUser("prd_operator", DefRoles.ONEPROD_MES_OPERATOR);
            //string prdadminId = _userManager.AddUser("prd_admin", DefRoles.ONEPROD_ADMIN);
            //string prdliderId = _userManager.AddUser("prd_lider", DefRoles.ONEPROD_MES_SUPEROPERATOR);
            //string whpickerId = _userManager.AddUser("wh_picker", DefRoles.ILOGIS_OPERATOR);
            //string whadminId = _userManager.AddUser("wh_admin", DefRoles.ILOGIS_ADMIN);

            //_userManager.AddUserToRoleName(adminId, DefRoles.ACCOUNT_ADMIN);
            //_userManager.AddUserToRoleName(adminId, DefRoles.PFEP_DEFPRINT_EDITOR);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_ADMIN);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_OPERATOR);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_VIEWER);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_VIEWER);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_ADMIN);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_MES_OPERATOR);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_MES_SUPEROPERATOR);

            //_userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_WHDOC_APPROVER);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_WHDOC_EDITOR);
            //_userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_WHDOC_VIEWER);
        }
    }
}


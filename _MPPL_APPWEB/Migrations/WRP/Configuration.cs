namespace _MPPL_WEB_START.Migrations.WRP
{
    using _MPPL_WEB_START.Migrations;
    using MDL_AP.Repo;
    using MDL_BASE.Models.IDENTITY;
    using XLIB_COMMON.Repo.IDENTITY;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.Identity.Owin;

    internal sealed class Configuration : DbMigrationsConfiguration<DbContextAPP_WRP_Prod>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\WRP";

            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_WRP -MigrationsDirectory:Migrations.WRP
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.WRP.Configuration 1K
            //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.WRP.Configuration
        }

        protected override void Seed(DbContextAPP_WRP_Prod context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            CnfigurationHelper.SeedDataNewRoles(context);
            
            UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);

            //UserRepo _userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserRepo>();
            //RoleRepo _roleManager = HttpContext.Current.GetOwinContext().GetUserManager<RoleRepo>();


            _roleManager.AddRole(DefRoles.USER);
            _roleManager.AddRole(DefRoles.ADMIN);
            _roleManager.AddRole(DefRoles.ACCOUNT_ADMIN);
            //_roleManager.AddRole(DefRoles.ProdLeader);
            //_roleManager.AddRole(DefRoles.ProdManager);
            //_roleManager.AddRole(DefRoles.Planner);
            //_roleManager.AddRole(DefRoles.WarehouseAdmin);
            _roleManager.AddRole(DefRoles.ONEPROD_VIEWER);
            _roleManager.AddRole(DefRoles.ONEPROD_MES_OPERATOR);
            _roleManager.AddRole(DefRoles.ONEPROD_ADMIN);
            _roleManager.AddRole(DefRoles.ONEPROD_MES_SUPEROPERATOR);

            //_roleManager.AddRole(DefRoles.iLogisUser);
            //_roleManager.AddRole(DefRoles.iLogisOperator);
            //_roleManager.AddRole(DefRoles.iLogisAdmin);
            //_roleManager.AddRole(DefRoles.iLogisPFEP_PRD_Editor);
            //_roleManager.AddRole(DefRoles.iLogisPFEP_WH_Editor);

            _userManager.AddUser("Admin", DefRoles.ADMIN, "admin123$");
            _userManager.AddUser("AccountAdmin", DefRoles.ACCOUNT_ADMIN, "admin345^");
            _userManager.AddUser("Operator1", DefRoles.ADMIN, "12345678");
            //_userManager.AddUser("Piotr", DefRoles.Admin);
            //_userManager.AddUser("Kamil", DefRoles.Admin);
            //_userManager.AddOperator("Operator");
            //_userManager.AddUser("Janusz", DefRoles.Admin);
            //_userManager.AddUser("Tomasz", DefRoles.Admin);

            //CnfigurationHelper.AddAccountAdminRoleAndAssignToAdmin(context);
        }
    }
}

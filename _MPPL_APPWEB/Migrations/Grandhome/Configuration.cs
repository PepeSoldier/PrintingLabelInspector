namespace _MPPL_WEB_START.Migrations.Grandhome
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

    internal sealed class Configuration : DbMigrationsConfiguration<DbContextAPP_Grandhome>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Grandhome";

            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Grandhome -MigrationsDirectory:Migrations.Grandhome
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.Grandhome.Configuration 1K
            //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.Grandhome.Configuration
        }

        protected override void Seed(DbContextAPP_Grandhome context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            CnfigurationHelper.SeedDataNewRoles(context);

            //RoleRepo _roleManager = new RoleRepo(new RoleStore<IdentityRole>(context), context);
            //UserRepo _userManager = new UserRepo(new UserStore<User>(context), context);
            UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            //UserRepo _userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserRepo>();
            //RoleRepo _roleManager = HttpContext.Current.GetOwinContext().GetUserManager<RoleRepo>();

            _roleManager.AddRole(DefRoles.ADMIN);
            _roleManager.AddRole(DefRoles.USER);
            //_roleManager.AddRole(DefRoles.ProdLeader);
            //_roleManager.AddRole(DefRoles.ProdManager);
            _roleManager.AddRole(DefRoles.PRD_SCHEDULER);
            _roleManager.AddRole(DefRoles.PFEP_DEFPRINT_EDITOR);

            _roleManager.AddRole(DefRoles.ONEPROD_VIEWER);
            _roleManager.AddRole(DefRoles.ONEPROD_MES_OPERATOR);
            _roleManager.AddRole(DefRoles.ONEPROD_ADMIN);
            _roleManager.AddRole(DefRoles.ONEPROD_MES_SUPEROPERATOR);
            _roleManager.AddRole(DefRoles.ACCOUNT_ADMIN);
            _roleManager.AddRole(DefRoles.ILOGIS_ADMIN);
            _roleManager.AddRole(DefRoles.ILOGIS_OPERATOR);
            _roleManager.AddRole(DefRoles.ILOGIS_OPERATOR_FEEDING);
            _roleManager.AddRole(DefRoles.ILOGIS_OPERATOR_PICKING);
            _roleManager.AddRole(DefRoles.ILOGIS_OPERATOR_INCOMING);
            _roleManager.AddRole(DefRoles.ILOGIS_CONFIG_EDITOR_WH);
            _roleManager.AddRole(DefRoles.ILOGIS_CONFIG_EDITOR_PRD);
            _roleManager.AddRole(DefRoles.ILOGIS_VIEWER);

            _userManager.AddUser("Admin", DefRoles.ADMIN, "Admin123$");
            _userManager.AddUser("Piotr", DefRoles.ADMIN);
            _userManager.AddUser("Kamil", DefRoles.ADMIN);
            _userManager.AddUser("Operator1", DefRoles.ILOGIS_OPERATOR);
            _userManager.AddUser("Operator2", DefRoles.ONEPROD_MES_OPERATOR);

            //_userManager.AddUser("Janusz", DefRoles.Admin);
            //_userManager.AddUser("Tomasz", DefRoles.Admin);
            CnfigurationHelper.AddAccountAdminRoleAndAssignToAdmin(context);
        }
    }
}

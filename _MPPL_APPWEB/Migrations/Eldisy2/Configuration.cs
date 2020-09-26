namespace _MPPL_WEB_START.Migrations.Eldisy2
{
    using MDL_BASE.Models.IDENTITY;
    using XLIB_COMMON.Repo.IDENTITY;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.Identity.Owin;

    internal sealed class Configuration : DbMigrationsConfiguration<_MPPL_WEB_START.Migrations.DbContextAPP_Eldisy2>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Eldisy2";
        }

        protected override void Seed(_MPPL_WEB_START.Migrations.DbContextAPP_Eldisy2 context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            CnfigurationHelper.SeedDataNewRoles(context);

            //RoleRepo _roleManager = new RoleRepo(new RoleStore<ApplicationRole>(context), context);
            //UserRepo _userManager = new UserRepo(new UserStore<ApplicationUser>(context), context);
            UserRepo _userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserRepo>();
            RoleRepo _roleManager = HttpContext.Current.GetOwinContext().GetUserManager<RoleRepo>();

            _roleManager.AddRole(DefRoles.ADMIN);
            _roleManager.AddRole(DefRoles.USER);
            //_roleManager.AddRole(DefRoles.ProdLeader);
            //_roleManager.AddRole(DefRoles.ProdManager);
            _roleManager.AddRole(DefRoles.PRD_SCHEDULER);
            _roleManager.AddRole(DefRoles.PFEP_DEFPRINT_EDITOR);

            _roleManager.AddRole(DefRoles.ONEPROD_VIEWER);
            _roleManager.AddRole(DefRoles.ONEPROD_ADMIN);
            _roleManager.AddRole(DefRoles.ONEPROD_MES_OPERATOR);
            _roleManager.AddRole(DefRoles.ONEPROD_MES_SUPEROPERATOR);


            _userManager.AddUser("Admin", DefRoles.ADMIN);
            _userManager.AddUser("Kamil", DefRoles.ADMIN);
            _userManager.AddUser("Piotr", DefRoles.ADMIN);
        }
    }
}

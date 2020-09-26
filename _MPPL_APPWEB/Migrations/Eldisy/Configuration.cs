namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using MDL_AP.Repo;
    using MDL_BASE.Models.IDENTITY;
    using XLIB_COMMON.Repo.IDENTITY;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.Identity.Owin;

    internal sealed class Configuration : DbMigrationsConfiguration<_MPPL_WEB_START.Migrations.DbContextAPP_Eldisy>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Eldisy";

            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_Eldisy -MigrationsDirectory:Migrations.Eldisy
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.Eldisy.Configuration 1K
            //Update-database -ConfigurationTypeName _MPPL_WEB_START.Migrations.Eldisy.Configuration
        }

        protected override void Seed(_MPPL_WEB_START.Migrations.DbContextAPP_Eldisy context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            CnfigurationHelper.SeedDataNewRoles(context);

            //RoleRepo _roleManager = new RoleRepo(new RoleStore<ApplicationRole>(context), context);
            //UserRepo _userManager = new UserRepo(new UserStore<User>(context), context);
            UserRepo _userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserRepo>();
            RoleRepo _roleManager = HttpContext.Current.GetOwinContext().GetUserManager<RoleRepo>();

            _roleManager.AddRole(DefRoles.ADMIN);
            _roleManager.AddRole(DefRoles.PFEP_PACKINGINSTR_CONFIRMER);
            _roleManager.AddRole(DefRoles.PFEP_PACKINGINSTR_EXAMINER);
            

            _userManager.AddUser("Admin", DefRoles.ADMIN);
        }
    }
}

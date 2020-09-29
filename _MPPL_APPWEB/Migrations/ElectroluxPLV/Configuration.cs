namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using _MPPL_WEB_START.Migrations;
    using MDL_BASE.Models.IDENTITY;
    using XLIB_COMMON.Repo.IDENTITY;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<_MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV_Prod>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ElectroluxPLV";

            //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV -MigrationsDirectory:Migrations.ElectroluxPLV
            //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration 1K
            //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLV.Configuration
        }

        protected override void Seed(_MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLV_Prod context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            //CnfigurationHelper.SeedDataNewRoles(context);
            CnfigurationHelper.AddAccountAdminRoleAndAssignToAdmin(context);
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            //RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            //RoleRepo _roleManager = new RoleRepo(new RoleStore<IdentityRole>(context), context);
            //UserRepo _userManager = new UserRepo(new UserStore<User>(context), context);


            //_roleManager.AddRole(DefRoles.Admin);
            //_roleManager.AddRole(DefRoles.User);
            //_roleManager.AddRole(DefRoles.ProdLeader);
            //_roleManager.AddRole(DefRoles.ProdManager);
            //_roleManager.AddRole(DefRoles.Engineer);
            //_roleManager.AddRole(DefRoles.Planner);
            //_roleManager.AddRole(DefRoles.WarehouseAdmin);
            //_roleManager.AddRole(DefRoles.TechnologyUser);
            //_roleManager.AddRole(DefRoles.TechnologyOperator);
            //_roleManager.AddRole(DefRoles.TechnologyAdmin);
            //_roleManager.AddRole(DefRoles.TechnologyLider);

            //_roleManager.AddRole(DefRoles.iLogisUser);
            //_roleManager.AddRole(DefRoles.iLogisOperator);
            //_roleManager.AddRole(DefRoles.iLogisAdmin);
            //_roleManager.AddRole(DefRoles.iLogisPFEP_PRD_Editor);
            //_roleManager.AddRole(DefRoles.iLogisPFEP_WH_Editor);

            //_userManager.AddUser("Admin", DefRoles.Admin);
            //_userManager.AddUser("Piotr", DefRoles.Admin);
            //_userManager.AddUser("Kamil", DefRoles.Admin);
            //_userManager.AddOperator("Operator");
            //_userManager.AddUser("Janusz", DefRoles.Admin);
            //_userManager.AddUser("Tomasz", DefRoles.Admin);

            //Seed_TechLocations(context);
            //Seed_TechPackageItem(context);
        }

    }
}

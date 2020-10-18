namespace _LABELINSP_APPWEB.Migrations.ElectroluxPLV
{
    using _LABELINSP_APPWEB.Migrations;
    using MDLX_CORE.Models.IDENTITY;
    using XLIB_COMMON.Repo.IDENTITY;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<_LABELINSP_APPWEB.Migrations.DbContextAPP_ElectroluxPLV>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ElectroluxPLV";

            //enable-migrations -ContextTypeName  _LABELINSP_APPWEB.Migrations.DbContextAPP_ElectroluxPLV -MigrationsDirectory:Migrations.ElectroluxPLV
            //Add-Migration -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.ElectroluxPLV.Configuration 1K
            //Update-Database -ConfigurationTypeName _LABELINSP_APPWEB.Migrations.ElectroluxPLV.Configuration
        }

        protected override void Seed(_LABELINSP_APPWEB.Migrations.DbContextAPP_ElectroluxPLV context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //CnfigurationHelper.SeedDataNewRoles(context);
            //CnfigurationHelper.AddAccountAdminRoleAndAssignToAdmin(context);
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            //RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            
            //_roleManager.AddRole(DefRoles.Admin);
            //_roleManager.AddRole(DefRoles.User);
            
            //_roleManager.AddRole(DefRoles.iLogisUser);
            //_roleManager.AddRole(DefRoles.iLogisOperator);
            
            //_userManager.AddUser("Admin", DefRoles.Admin);
        }

    }
}

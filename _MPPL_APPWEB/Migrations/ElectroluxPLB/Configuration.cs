namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using MDL_BASE.Models.IDENTITY;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Configuration.Internal;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using XLIB_COMMON.Repo.IDENTITY;

    internal sealed class Configuration : DbMigrationsConfiguration<DbContextAPP_ElectroluxPLB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ElectroluxPLB";
        }

        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLB -MigrationsDirectory:Migrations.ElectroluxPLB
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLB.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLB.Configuration

        protected override void Seed(_MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLB context)
        {
            //CnfigurationHelper.SeedDataNewRoles(context);
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            //RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            ////RoleRepo _roleManager = new RoleRepo(new RoleStore<IdentityRole>(context), context);
            ////UserRepo _userManager = new UserRepo(new UserStore<User>(context), context);
            //_roleManager.AddRole(DefRoles.ADMIN);
            //_roleManager.AddRole(DefRoles.USER);
            //_userManager.AddUser("Admin", DefRoles.Admin);
            //_roleManager.AddRole(DefRoles.ONEPROD_MES_SUPEROPERATOR);

            //CnfigurationHelper.SeedDataNewRoles(context);

            //CnfigurationHelper.Seed_PLB_MasterData_Users(context);
            //CnfigurationHelper.Seed_PLB_MasterData_ItemsPNC(context);
            ////CnfigurationHelper.Seed_PLB_SampleProdOrders(context);

            //CnfigurationHelper.Seed_PLB_Core(context);
            //CnfigurationHelper.Seed_PLB_MasterData(context);
            //CnfigurationHelper.Seed_PLB_iLOGIS(context);
            //CnfigurationHelper.Seed_PLB_MasterData_Items(context);
            //CnfigurationHelper.Seed_PLB_iLOGIS_PackageItem(context);
            CnfigurationHelper.AddAccountAdminRoleAndAssignToAdmin(context);
        }
    }
}

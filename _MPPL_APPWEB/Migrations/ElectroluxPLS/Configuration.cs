namespace _MPPL_WEB_START.Migrations.ElectroluxPLS
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
    using MDL_WMS.ComponentConfig.UnitOfWorks;
    using MDL_iLOGIS.ComponentConfig.Entities;
    using MDLX_MASTERDATA.Repos;
    using MDLX_MASTERDATA._Interfaces;
    using MDL_BASE.Models.MasterData;
    using MDLX_MASTERDATA.Enums;
    using MDL_BASE.Interfaces;
    using System.Collections.Generic;
    using MDLX_MASTERDATA.Entities;
    using MDL_iLOGIS.ComponentConfig.Repos;
    using MDL_iLOGIS.ComponentWMS.Enums;
    using MDL_iLOGIS.ComponentConfig._Interfaces;
    using System.Web;
    using Microsoft.AspNet.Identity.Owin;

    internal sealed class Configuration : DbMigrationsConfiguration<DbContextAPP_ElectroluxPLS>
    {
        //enable-migrations -ContextTypeName  _MPPL_WEB_START.Migrations.DbContextAPP_ElectroluxPLS -MigrationsDirectory:Migrations.ElectroluxPLS
        //Add-Migration -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLS.Configuration 1K
        //Update-Database -ConfigurationTypeName _MPPL_WEB_START.Migrations.ElectroluxPLS.Configuration

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ElectroluxPLS";
        }

        protected override void Seed(DbContextAPP_ElectroluxPLS context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //CnfigurationHelper.SeedDataNewRoles(context);

            //RoleRepo _roleManager = new RoleRepo(new RoleStore<ApplicationRole>(context), context);
            //UserRepo _userManager = new UserRepo(new ApplicationUserStore(context), context);
            //UserRepo _userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserRepo>();
            //RoleRepo _roleManager = HttpContext.Current.GetOwinContext().GetUserManager<RoleRepo>();

            //UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);

            //_roleManager.AddRole(DefRoles.ADMIN);
            //_roleManager.AddRole(DefRoles.USER);
            //_roleManager.AddRole(DefRoles.ILOGIS_VIEWER);
            //_roleManager.AddRole(DefRoles.ILOGIS_ADMIN);
            //_roleManager.AddRole(DefRoles.ILOGIS_OPERATOR);
            //_roleManager.AddRole(DefRoles.ILOGIS_WHDOC_VIEWER);
            //_roleManager.AddRole(DefRoles.ILOGIS_WHDOC_EDITOR);
            //_roleManager.AddRole(DefRoles.ILOGIS_WHDOC_APPROVER);
            _roleManager.AddRole(DefRoles.ILOGIS_WHDOC_SECURITY);

            //_userManager.AddUser("Admin", DefRoles.ADMIN, "admin123$");
            //_userManager.AddUser("User", DefRoles.ADMIN, "user123$");
            //_userManager.AddUser("iLogisUser", DefRoles.ADMIN, "iLogisUser123$");
            //_userManager.AddUser("iLogisWhDocApprover", DefRoles.ILOGIS_WHDOC_APPROVER, "iLogisWhDocApprover123$");
            //_userManager.AddUser("iLogisWhDocEditor", DefRoles.ILOGIS_WHDOC_EDITOR, "iLogisWhDocEditor123$");
            //_userManager.AddUser("iLogisWhDocViewer", DefRoles.ILOGIS_WHDOC_VIEWER, "iLogisWhDocViewer123$");

            ////SeedData_PLS(context);
            //CnfigurationHelper.AddAccountAdminRoleAndAssignToAdmin(context);
        }
        public void SeedData_PLS(DbContextAPP_ElectroluxPLS context)
        {
            Seed_MasterData(context);
            Seed_iLOGIS(context);
        }

        public static void Seed_MasterData(IDbContextMasterData db)
        {
            //SUPPLIERS ---------------------------------------------------------------------------------------------------------------------------------------
            RepoContractor contractorRepo = new RepoContractor(db);
            contractorRepo.Add(new Contractor() { Code = "100200", Country = "PL", Name = "IMPLEA Sp. z o.o.", NIP = "6004589901", ContactEmail = "email1@test987.text" });
            contractorRepo.Add(new Contractor() { Code = "100300", Country = "PL", Name = "ALBA Sp. z o.o.", NIP = "7005005500", ContactEmail = "email2@test987.text" });
            contractorRepo.Add(new Contractor() { Code = "100400", Country = "IT", Name = "ITALY RILL SPA", NIP = "6006006600", ContactEmail = "email3@test987.text" });
            contractorRepo.Add(new Contractor() { Code = "100500", Country = "RUS", Name = "GAZPROM", NIP = "1000001234", ContactEmail = "email4@test987.text" });
            contractorRepo.Add(new Contractor() { Code = "100600", Country = "GER", Name = "AEG Gmbh", NIP = "9000005678", ContactEmail = "email5@test987.text" });

            //********************************************************OBSZARY, ZASOBY, STANOWISKA, PROCESY********************************************************
            //****************************************************************************************************************************************************
            //****************************************************************************************************************************************************
            ProcessRepo processRepo = new ProcessRepo(db, null);
            RepoArea repoArea = new RepoArea(db);
            ItemRepo itemRepo = new ItemRepo(db);
            
            int? resrcGrp = 0;
            int? itmGrp = 0;

            
            itmGrp = CnfigurationHelper.Item_ADD(itemRepo, "GROUP", "900000", resrcGrp, null, ItemTypeEnum.ItemGroup, name: "GROUP CAR");
            CnfigurationHelper.Item_ADD(itemRepo, "900222100", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, name: "Ford Focus Sedan");
            CnfigurationHelper.Item_ADD(itemRepo, "900222101", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, name: "Ford Focus Hatchback");
            CnfigurationHelper.Item_ADD(itemRepo, "900222102", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, name: "Ford Focus Combi");
            CnfigurationHelper.Item_ADD(itemRepo, "900222103", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, name:  "Ford Focus RS");

            //PAINTING-AREA--------------------------------------------------------------------------------------------------------------------------------------------------

            
            itmGrp = CnfigurationHelper.Item_ADD(itemRepo, "GROUP", "600101", resrcGrp, null, ItemTypeEnum.ItemGroup, name: "GROUP BODY COLORED");
            CnfigurationHelper.Item_ADD(itemRepo, "600000000", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, name: "Farb Color WHITE (RAL 9003)");
            CnfigurationHelper.Item_ADD(itemRepo, "600000005", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, name: "Farb Color RED (RAL 3020)");
            CnfigurationHelper.Item_ADD(itemRepo, "600000010", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, name: "Farb Color BLUE (RAL 5005)");
            CnfigurationHelper.Item_ADD(itemRepo, "600000015", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, name: "Farb Color BLACK (RAL 9005)");
            CnfigurationHelper.Item_ADD(itemRepo, "600000020", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, name:  "Farb Color GREEN (RAL 6029)");

            //WELDING-AREA--------------------------------------------------------------------------------------------------------------------------------------------------
            itmGrp = CnfigurationHelper.Item_ADD(itemRepo, "GROUP", "600100", resrcGrp, null, ItemTypeEnum.ItemGroup, name: "GROUP BODY");
            CnfigurationHelper.Item_ADD(itemRepo, "600100200", "600100", resrcGrp, itmGrp, ItemTypeEnum.FinishedItem, name: "Body Sedan");
            CnfigurationHelper.Item_ADD(itemRepo, "600100201", "600100", resrcGrp, itmGrp, ItemTypeEnum.FinishedItem, name: "Body HatchBack");
            CnfigurationHelper.Item_ADD(itemRepo, "600100202", "600100", resrcGrp, itmGrp, ItemTypeEnum.FinishedItem, name: "Body Combi");
        }
        public static void Seed_iLOGIS(IDbContextiLOGIS db)
        {
            //Add ItemOP basing on Items from MasterData and PREFIX parameter
            ItemRepo itemRepo = new ItemRepo(db);
            ItemWMSRepo itemWMSRepo = new ItemWMSRepo(db);
            PackageRepo packageRepo = new PackageRepo(db);
            
            List<Item> items = itemRepo.GetList().ToList();
            foreach (Item itm in items)
            {
                itemWMSRepo.AddOrUpdate(new ItemWMS() { ItemId = itm.Id, PickerNo = 100, TrainNo = 100, ABC = ClassificationABCEnum.Undefined, XYZ = ClassificationXYZEnum.Undefined, Weight = 1, H = 0 });
            }

            CnfigurationHelper.Package_ADD(packageRepo, "0001", "Opakowanie Kartonowe", 25, 40, 60);
            CnfigurationHelper.Package_ADD(packageRepo, "P001", "Paleta zwyk³a", 0, 0, 0);
            CnfigurationHelper.Package_ADD(packageRepo, "P002", "Paleta EURO", 0, 0, 0);
            CnfigurationHelper.Package_ADD(packageRepo, "K001", "Kontener Niebieski", 0, 0, 0);
            CnfigurationHelper.Package_ADD(packageRepo, "K002", "Kontener Metalowy", 0, 0, 0);
            CnfigurationHelper.Package_ADD(packageRepo, "K003", "Kontener Plastikowy", 0, 0, 0);
            CnfigurationHelper.Package_ADD(packageRepo, "K004", "Przek³adki kartonowe", 0, 0, 0);
        }
    }
}

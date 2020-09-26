using MDL_iLOGIS.ComponentWMS.Enums;
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
    using MDL_iLOGIS.ComponentConfig._Interfaces;
    using MDL_iLOGIS.ComponentConfig.Entities;
    using MDL_iLOGIS.ComponentConfig.Repos;
    using MDL_iLOGIS.ComponentWMS.Entities;
    using MDL_iLOGIS.ComponentWMS.Enums;
    using MDL_iLOGIS.ComponentWMS.Repos;
    using MDL_ONEPROD.Interface;
    using MDL_ONEPROD.Model.OEEProd;
    using MDL_ONEPROD.Model.Scheduling;
    using MDL_ONEPROD.Repo.OEERepos;
    using MDL_ONEPROD.Repo.Scheduling;
    using MDL_PRD.Model;
    using MDL_WMS.ComponentConfig.Repos;
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
            RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            string accountAdminRoleId = _roleManager.AddRole(DefRoles.ACCOUNT_ADMIN);
            
            List<string> adminRolesIds = context.Roles.Where(x => x.Name == "ADMIN" || x.Name == "Admin").Select(x => x.Id).ToList();
            List<string> adminUserIds = context.UserRoles.Where(x => adminRolesIds.Contains(x.RoleId)).Select(x => x.UserId).ToList();

            ApplicationRole role = context.Roles.Where(x => x.Name == DefRoles.ACCOUNT_ADMIN).FirstOrDefault();

            foreach (var admUserId in adminUserIds)
            {
                UserRole userRole = context.UserRoles.FirstOrDefault(x => x.RoleId == role.Id && x.UserId == admUserId);

                if (userRole == null)
                {
                    userRole = new UserRole();
                    userRole.RoleId = role.Id;
                    userRole.UserId = admUserId;
                    context.UserRoles.Add(userRole);
                    context.SaveChanges();
                }
            }
        }

        public static void Seed(DbContextAPP_Dev context)
        {
            UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);
            //UserRepo _userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserRepo>();
            //RoleRepo _roleManager = HttpContext.Current.GetOwinContext().GetUserManager<RoleRepo>();

            //_roleManager.AddRole(DefRoles.ProdLeader);
            //_roleManager.AddRole(DefRoles.ProdManager);
            string role1 = _roleManager.AddRole(DefRoles.ADMIN);
            string role2 = _roleManager.AddRole(DefRoles.USER);
            string role4 = _roleManager.AddRole(DefRoles.PRD_SCHEDULER);
            string role5 = _roleManager.AddRole(DefRoles.PFEP_DEFPRINT_EDITOR);

            string role6 = _roleManager.AddRole(DefRoles.ILOGIS_ADMIN);
            string role7 = _roleManager.AddRole(DefRoles.ILOGIS_OPERATOR);
            string role8 = _roleManager.AddRole(DefRoles.ILOGIS_VIEWER);

            string role9 = _roleManager.AddRole(DefRoles.ONEPROD_VIEWER);
            string role10 = _roleManager.AddRole(DefRoles.ONEPROD_ADMIN);
            string role11 = _roleManager.AddRole(DefRoles.ONEPROD_MES_OPERATOR);
            string role12 = _roleManager.AddRole(DefRoles.ONEPROD_MES_SUPEROPERATOR);
            string role13 = _roleManager.AddRole(DefRoles.ILOGIS_WHDOC_APPROVER);
            string role14 = _roleManager.AddRole(DefRoles.ILOGIS_WHDOC_EDITOR);
            string role15 = _roleManager.AddRole(DefRoles.ILOGIS_WHDOC_VIEWER);
            string role16 = _roleManager.AddRole(DefRoles.ACCOUNT_ADMIN);

            string adminId = _userManager.AddUser("Admin", DefRoles.ADMIN);
            string prdoperatorId = _userManager.AddUser("prd_operator", DefRoles.ONEPROD_MES_OPERATOR);
            string prdadminId = _userManager.AddUser("prd_admin", DefRoles.ONEPROD_ADMIN);
            string prdliderId = _userManager.AddUser("prd_lider", DefRoles.ONEPROD_MES_SUPEROPERATOR);
            string whpickerId = _userManager.AddUser("wh_picker", DefRoles.ILOGIS_OPERATOR);
            string whadminId = _userManager.AddUser("wh_admin", DefRoles.ILOGIS_ADMIN);

            _userManager.AddUserToRoleName(adminId, DefRoles.ACCOUNT_ADMIN);
            _userManager.AddUserToRoleName(adminId, DefRoles.PFEP_DEFPRINT_EDITOR);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_ADMIN);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_OPERATOR);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_VIEWER);
            _userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_VIEWER);
            _userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_ADMIN);
            _userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_MES_OPERATOR);
            _userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_MES_SUPEROPERATOR);

            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_WHDOC_APPROVER);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_WHDOC_EDITOR);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_WHDOC_VIEWER);
        }
        public static void SeedData(DbContextAPP_Dev db)
        {
            //Seed_MasterData(db, db);
            //Seed_Core(db);
            //Seed_ONEPROD(db, db);
            //Seed_ONEPROD_OEE(db);
            Seed_iLOGIS(db);
        }

        public static void SeedDataNewRoles(DbContextAPP_ db)
        {
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ONEPROD_VIEWER + "' WHERE Name = 'TechnologyUser'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ONEPROD_ADMIN + "' WHERE Name = 'TechnologyAdmin'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ONEPROD_MES_OPERATOR + "' WHERE Name = 'TechnologyOperator'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ONEPROD_MES_SUPEROPERATOR + "' WHERE Name = 'TechnologyLider'");
            
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ILOGIS_ADMIN + "' WHERE Name = 'iLogisAdmin'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ILOGIS_VIEWER + "' WHERE Name = 'iLogisUser'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ILOGIS_OPERATOR + "' WHERE Name = 'iLogisOperator'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ILOGIS_CONFIG_EDITOR_WH + "' WHERE Name = 'iLogisPFEP_PRD_Editor'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ILOGIS_CONFIG_EDITOR_PRD + "' WHERE Name = 'iLogisPFEP_WH_Editor'");
            
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.PFEP_PACKINGINSTR_CONFIRMER + "' WHERE Name = 'Kierownik'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.PFEP_PACKINGINSTR_EXAMINER + "' WHERE Name = 'Inżynier Jakości'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.PFEP_DEFPRINT_EDITOR + "' WHERE Name = 'WarehouseAdmin'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.PRD_SCHEDULER + "' WHERE Name = 'Planner'");

            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.USER + "' WHERE Name = 'User'");
            db.Database.ExecuteSqlCommand("UPDATE [_MPPL].[IDENTITY_Role] SET Name = '" + DefRoles.ADMIN + "' WHERE Name = 'Admin'");           
        }

        //-----------------DEV-IMPLEA------------------------------------------------------------------
        public static void Seed_MasterData(IDbContextMasterData db, IDbContextOneprod dbOP)
        {
            //BRIGADES ---------------------------------------------------------------------------------------------------------------------------------------
            RepoLabourBrigade lbRepo = new RepoLabourBrigade(db);
            lbRepo.Add(new LabourBrigade() { Name = "Brygada A" });
            lbRepo.Add(new LabourBrigade() { Name = "Brygada B" });
            lbRepo.Add(new LabourBrigade() { Name = "Brygada C" });

            //SUPPLIERS ---------------------------------------------------------------------------------------------------------------------------------------
            RepoContractor contractorRepo = new RepoContractor(db);
            contractorRepo.Add(new Contractor() { Code = "100200", Country = "PL", Name = "Supplier1", NIP = "6004589901", ContactEmail = "email1@test987.text" });
            contractorRepo.Add(new Contractor() { Code = "100300", Country = "DE", Name = "Supplier2", NIP = "7005005500", ContactEmail = "email2@test987.text" });
            contractorRepo.Add(new Contractor() { Code = "100400", Country = "IT", Name = "Supplier3", NIP = "6006006600", ContactEmail = "email3@test987.text" });
            contractorRepo.Add(new Contractor() { Code = "100500", Country = "UK", Name = "Supplier4", NIP = "1000001234", ContactEmail = "email4@test987.text" });
            contractorRepo.Add(new Contractor() { Code = "100600", Country = "CH", Name = "Supplier5", NIP = "9000005678", ContactEmail = "email5@test987.text" });

            //********************************************************OBSZARY, ZASOBY, STANOWISKA, PROCESY********************************************************
            //****************************************************************************************************************************************************
            //****************************************************************************************************************************************************
            ProcessRepo processRepo = new ProcessRepo(db, null);
            RepoArea repoArea = new RepoArea(db);
            ItemOPRepo itemRepo = new ItemOPRepo(dbOP);
            //ItemRepo itemRepo = new ItemRepo(db);
            //ResourceRepo resourceRepo = new ResourceRepo(db);
            ResourceOPRepo resourceRepo = new ResourceOPRepo(dbOP, null);
            RepoWorkstation repoWorkstation = new RepoWorkstation(db);

            repoArea.Add(new Area() { Name = "Factory" });
            int virtualId = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.VirtualResource, 0, null, "Virual Line");

            int? resrcGrp = 0;
            int? itmGrp = 0;
            int lineId = 0;

            //ASSEMBLY-AREA---------------------------------------------------------------------------------------------------------------------------------------------------
            resrcGrp = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Group, 0, null, "Assembly Lines");
            lineId = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "Ass.Line-01");
            Workstation_ADD(repoWorkstation, lineId, 1, "ASS1.01 IN");
            Workstation_ADD(repoWorkstation, lineId, 1, "ASS1.03 F.Glass");
            Workstation_ADD(repoWorkstation, lineId, 2, "ASS1.05 Wheels");
            Workstation_ADD(repoWorkstation, lineId, 3, "ASS1.10 Interior");
            Workstation_ADD(repoWorkstation, lineId, 4, "ASS1.20 Engine");
            Workstation_ADD(repoWorkstation, lineId, 5, "ASS1.30 Exterior");

            int carAssId = Process_ADD(processRepo, "Car Assembly");
            itmGrp = ItemOP_ADD(itemRepo, "GROUP", "900000", resrcGrp, null, ItemTypeEnum.ItemGroup, "GROUP CAR", carAssId);
            ItemOP_ADD(itemRepo, "900222100", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, "Ford Focus Sedan");
            ItemOP_ADD(itemRepo, "900222101", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, "Ford Focus Hatchback");
            ItemOP_ADD(itemRepo, "900222102", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, "Ford Focus Combi");
            ItemOP_ADD(itemRepo, "900222103", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, "Ford Focus RS");

            //PAINTING-AREA--------------------------------------------------------------------------------------------------------------------------------------------------
            resrcGrp = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Group, 0, null, "Painting Lines");
            lineId = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "Painting Line 1");
            Workstation_ADD(repoWorkstation, lineId, 1, "PL1.01 IN");
            Workstation_ADD(repoWorkstation, lineId, 2, "PL1.01 OUT");

            int bodyPaintId = Process_ADD(processRepo, "Body Painting", carAssId);
            itmGrp = ItemOP_ADD(itemRepo, "GROUP", "600101", resrcGrp, null, ItemTypeEnum.ItemGroup, "GROUP BODY COLORED", bodyPaintId);
            ItemOP_ADD(itemRepo, "600000000", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Farb Color WHITE (RAL 9003)");
            ItemOP_ADD(itemRepo, "600000005", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Farb Color RED (RAL 3020)");
            ItemOP_ADD(itemRepo, "600000010", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Farb Color BLUE (RAL 5005)");
            ItemOP_ADD(itemRepo, "600000015", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Farb Color BLACK (RAL 9005)");
            ItemOP_ADD(itemRepo, "600000020", "600000", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Farb Color GREEN (RAL 6029)");

            //WELDING-AREA--------------------------------------------------------------------------------------------------------------------------------------------------
            resrcGrp = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Group, 0, null, "Body Welding Lines");
            lineId = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "BODY Welding Line 1");
            Workstation_ADD(repoWorkstation, lineId, 1, "BWL1.01 IN");
            Workstation_ADD(repoWorkstation, lineId, 2, "BWL1.01 OUT");

            int bodyWeldId = Process_ADD(processRepo, "Body Welding", bodyPaintId);
            itmGrp = ItemOP_ADD(itemRepo, "GROUP", "600100", resrcGrp, null, ItemTypeEnum.ItemGroup, "GROUP BODY", bodyWeldId);
            ItemOP_ADD(itemRepo, "600100200", "600100", resrcGrp, itmGrp, ItemTypeEnum.FinishedItem, "Body Sedan");
            ItemOP_ADD(itemRepo, "600100201", "600100", resrcGrp, itmGrp, ItemTypeEnum.FinishedItem, "Body HatchBack");
            ItemOP_ADD(itemRepo, "600100202", "600100", resrcGrp, itmGrp, ItemTypeEnum.FinishedItem, "Body Combi");

            resrcGrp = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Group, 0, null, "Welding Lines");
            lineId = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "Welding Line 1");
            Workstation_ADD(repoWorkstation, lineId, 1, "WL1.01 IN");
            Workstation_ADD(repoWorkstation, lineId, 2, "WL1.01 OUT");

            int rearDoorWeldID = Process_ADD(processRepo, "Rear Door Welding", bodyWeldId);
            itmGrp = ItemOP_ADD(itemRepo, "GROUP", "600200", resrcGrp, null, ItemTypeEnum.ItemGroup, "GROUP REAR DOOR", rearDoorWeldID);
            ItemOP_ADD(itemRepo, "600200100", "600200", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Sedan");
            ItemOP_ADD(itemRepo, "600200101", "600200", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Hatchback");
            ItemOP_ADD(itemRepo, "600200102", "600200", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Combi");

            //PRESSING-AREA--------------------------------------------------------------------------------------------------------------------------------------------------
            resrcGrp = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Group, 0, null, "Presses");
            lineId = ResourceOP_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "Press 1");

            Workstation_ADD(repoWorkstation, lineId, 1, "PR1.01 IN");
            Workstation_ADD(repoWorkstation, lineId, 2, "PR1.01 OUT");

            int processId = Process_ADD(processRepo, "Rear Door INNER pressing", rearDoorWeldID);
            itmGrp = ItemOP_ADD(itemRepo, "GROUP", "600210", resrcGrp, null, ItemTypeEnum.ItemGroup, "GROUP DOOR INNER", processId);
            ItemOP_ADD(itemRepo, "600200110", "600210", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Sedan INNER");
            ItemOP_ADD(itemRepo, "600200111", "600210", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Hatchback INNER");
            ItemOP_ADD(itemRepo, "600200112", "600210", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Combi INNER");

            processId = Process_ADD(processRepo, "Rear Door OUTER pressing", rearDoorWeldID);
            itmGrp = ItemOP_ADD(itemRepo, "GROUP", "600220", resrcGrp, null, ItemTypeEnum.ItemGroup, "GROUP DOOR OUTER", processId);
            ItemOP_ADD(itemRepo, "600200120", "600220", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Sedan OUTER");
            ItemOP_ADD(itemRepo, "600200121", "600220", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Hatchback OUTER");
            ItemOP_ADD(itemRepo, "600200122", "600220", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Rear Door Combi OUTER");

            processId = Process_ADD(processRepo, "Roof pressing", bodyWeldId);
            itmGrp = ItemOP_ADD(itemRepo, "GROUP", "600300", resrcGrp, null, ItemTypeEnum.ItemGroup, "GROUP ROOF", processId);
            ItemOP_ADD(itemRepo, "600200200", "600300", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Roof Sedan");
            ItemOP_ADD(itemRepo, "600200201", "600300", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Roof Hatchback");
            ItemOP_ADD(itemRepo, "600200202", "600300", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Roof Combi");

            processId = Process_ADD(processRepo, "Hood pressing", bodyWeldId);
            itmGrp = ItemOP_ADD(itemRepo, "GROUP", "600300", resrcGrp, null, ItemTypeEnum.ItemGroup, "GROUP HOOD", processId);
            ItemOP_ADD(itemRepo, "600200302", "600400", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Hood");
            ItemOP_ADD(itemRepo, "600200308", "600400", resrcGrp, itmGrp, ItemTypeEnum.IntermediateItem, "Hood RS");

            //BUYED ITEMS
            resrcGrp = null;
            itmGrp = null;
            //Silniki kody: 3xx xxx xxx -----------------------------------------------------------------------------------------------------------------------
            ItemOP_ADD(itemRepo, "300100100", "300100", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Benzin 1.0 95 KM");
            ItemOP_ADD(itemRepo, "300100140", "300100", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Benzin 1.4 140 KM");
            ItemOP_ADD(itemRepo, "300100180", "300100", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Benzin 1.8 260 KM");
            ItemOP_ADD(itemRepo, "300100260", "300100", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Diesel 1.6 112 KM");
            ItemOP_ADD(itemRepo, "300100220", "300100", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Diesel 2.0 150 KM");
            //Wyposażenie wewnętrzne kody: 4xx xxx xxx ---------------------------------------------------------------------------------------------------------
            ItemOP_ADD(itemRepo, "400100502", "400100", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Steering wheel");
            ItemOP_ADD(itemRepo, "400100505", "400100", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Steering wheel 3 arm");
            ItemOP_ADD(itemRepo, "400200605", "400200", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Seat Left");
            ItemOP_ADD(itemRepo, "400200606", "400200", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Seat Right");
            ItemOP_ADD(itemRepo, "400200615", "400200", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Seat Left Electric");
            ItemOP_ADD(itemRepo, "400200616", "400200", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Seat Right Electric");
            ItemOP_ADD(itemRepo, "400200705", "400207", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Display mono 5 inch");
            ItemOP_ADD(itemRepo, "400200708", "400207", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Display color 8inch");
            ItemOP_ADD(itemRepo, "400200801", "400208", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Instrument analog mono display DIESEL");
            ItemOP_ADD(itemRepo, "400200802", "400208", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Instrument analog big color display DIESEL");
            ItemOP_ADD(itemRepo, "400200811", "400208", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Instrument analog mono display BENZIN");
            ItemOP_ADD(itemRepo, "400200812", "400208", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Instrument analog big color display BENZIN");
            //Wyposażenie zewnętrzne kody: 5xx xxx xxx ---------------------------------------------------------------------------------------------------------
            //Lampy
            ItemOP_ADD(itemRepo, "500200101", "500210", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Left Lamp LED");
            ItemOP_ADD(itemRepo, "500200102", "500210", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Right Lamp LED");
            ItemOP_ADD(itemRepo, "500200104", "500210", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Left Lamp");
            ItemOP_ADD(itemRepo, "500200103", "500210", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Right Lamp");
            ItemOP_ADD(itemRepo, "500200105", "500220", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Rear Lamp Left");
            ItemOP_ADD(itemRepo, "500200106", "500220", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Rear Lamp Right");
            //Szyby
            ItemOP_ADD(itemRepo, "500200200", "500225", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Front Glass");
            ItemOP_ADD(itemRepo, "500200220", "500228", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Rear Glass Sedan");
            ItemOP_ADD(itemRepo, "500200221", "500228", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Rear Glass HatchBack");
            ItemOP_ADD(itemRepo, "500200222", "500228", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Rear Glass Combi");
            //koła
            ItemOP_ADD(itemRepo, "500200310", "500210", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Alu Rim 16");
            ItemOP_ADD(itemRepo, "500200311", "500210", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Alu Rim 17");
            ItemOP_ADD(itemRepo, "500200320", "500210", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Steel Rim 16");
            ItemOP_ADD(itemRepo, "500200321", "500210", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Steel Rim 17");
            ItemOP_ADD(itemRepo, "500200330", "500220", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Tire Continental 215/55/R16");
            ItemOP_ADD(itemRepo, "500200331", "500220", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Tire Continental 225/40/R17");
            ItemOP_ADD(itemRepo, "500200380", "500230", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Wheel Cap 16");
            ItemOP_ADD(itemRepo, "500200381", "500230", resrcGrp, itmGrp, ItemTypeEnum.BuyedItem, "Wheel Cap 17");
        }
        public static void Seed_Core(IDbContextCore db)
        {
            ResourceRepo resourceRepo = new ResourceRepo(db);
            ProductionOrderRepo poRepo = new ProductionOrderRepo(db);
            BomRepo bomRepo = new BomRepo(db);
            ItemRepo itemRepo = new ItemRepo(db);

            Resource2 assLine = resourceRepo.GetList().Where(x => x.Name == "Ass.Line-01").FirstOrDefault();
            Item sedan = itemRepo.GetByCode("900222100");                               // "Opel Astra Sedan" });
            Item hatchback = itemRepo.GetByCode("900222101");                           // "Opel Astra Hatchback" });
            Item combi = itemRepo.GetByCode("900222102");                               // "Opel Astra Combi" });
            Item rs = itemRepo.GetByCode("900222102");                                  // "Opel Astra RS" });

            WO_ADD(poRepo, "2020000011", 5, assLine.Id, sedan.Id);
            WO_ADD(poRepo, "2020000012", 20, assLine.Id, sedan.Id);
            WO_ADD(poRepo, "2020000013", 10, assLine.Id, sedan.Id);
            WO_ADD(poRepo, "2020000024", 2, assLine.Id, hatchback.Id);
            WO_ADD(poRepo, "2020000025", 4, assLine.Id, hatchback.Id);
            WO_ADD(poRepo, "2020000026", 12, assLine.Id, hatchback.Id);
            WO_ADD(poRepo, "2020000030", 5, assLine.Id, combi.Id);
            WO_ADD(poRepo, "2020000031", 1, assLine.Id, combi.Id);
            WO_ADD(poRepo, "2020000032", 8, assLine.Id, combi.Id);
            WO_ADD(poRepo, "2020000040", 5, assLine.Id, rs.Id);

            List<Resource2> resources = resourceRepo.GetList().Where(x => x.Type == ResourceTypeEnum.Resource).ToList();
            SystemVariableRepo systemVariableRepo = new SystemVariableRepo(db);

            foreach (Resource2 resource in resources)
            {
                systemVariableRepo.Add(new SystemVariable() { Type = EnumVariableType.Int, Value = "0", Name = "SerialNumber_resourceId_" + resource.Id });
            }

            //"900222100", "Ford Focus Sedan"
            Bom_ADD(bomRepo, itemRepo, "900222100", "600100200", 1, 1); //BODY sedan:
            Bom_ADD(bomRepo, itemRepo, "900222100", "600200100", 1, 2); //REAR DOOR sedan: RD
            Bom_ADD(bomRepo, itemRepo, "900222100", "600200110", 1, 3); //REAR DOOR sedan: RD inner
            Bom_ADD(bomRepo, itemRepo, "900222100", "600200120", 1, 3); //REAR DOOR sedan: RD outer
            Bom_ADD(bomRepo, itemRepo, "900222100", "500200220", 1, 3); //REAR DOOR sedan: Rear glass
            Bom_ADD(bomRepo, itemRepo, "900222100", "600200200", 1, 2); //ROOF combi
            Bom_ADD(bomRepo, itemRepo, "900222100", "600200302", 1, 2); //HOOD
            Bom_ADD(bomRepo, itemRepo, "900222100", "400100502", 1, 1); //Steering wheel
            Bom_ADD(bomRepo, itemRepo, "900222100", "400200605", 1, 1); //seat l
            Bom_ADD(bomRepo, itemRepo, "900222100", "400200606", 1, 1); //seat r
            Bom_ADD(bomRepo, itemRepo, "900222100", "400200708", 1, 1); //Display color 8inch
            Bom_ADD(bomRepo, itemRepo, "900222100", "400200802", 1, 1); //Instrument analog big color display DIESEL
            Bom_ADD(bomRepo, itemRepo, "900222100", "500200104", 1, 1); //Left Lamp
            Bom_ADD(bomRepo, itemRepo, "900222100", "500200103", 1, 1); //right Lamp
            Bom_ADD(bomRepo, itemRepo, "900222100", "500200105", 1, 1); //left Lamp rear
            Bom_ADD(bomRepo, itemRepo, "900222100", "500200106", 1, 1); //right Lamp rear
            Bom_ADD(bomRepo, itemRepo, "900222100", "500200200", 1, 1); //Front Glass
            Bom_ADD(bomRepo, itemRepo, "900222100", "500200310", 1, 1); //Alu 16
            Bom_ADD(bomRepo, itemRepo, "900222100", "500200330", 1, 1); //Tire Continental 215/55/R16
            Bom_ADD(bomRepo, itemRepo, "900222100", "300100260", 1, 1); //ENGINE Diesel 1.6 112 KM

            //"900222101", "Ford Focus Hatchback"
            Bom_ADD(bomRepo, itemRepo, "900222101", "600100201", 1, 1); //BODY hatch:
            Bom_ADD(bomRepo, itemRepo, "900222101", "600200101", 1, 2); //REAR DOOR hatch: RD
            Bom_ADD(bomRepo, itemRepo, "900222101", "600200111", 1, 3); //REAR DOOR hatch: RD inner
            Bom_ADD(bomRepo, itemRepo, "900222101", "600200121", 1, 3); //REAR DOOR hatch: RD outer
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200221", 1, 3); //REAR DOOR hatch: Rear glass
            Bom_ADD(bomRepo, itemRepo, "900222101", "600200201", 1, 2); //ROOF combi
            Bom_ADD(bomRepo, itemRepo, "900222101", "600200302", 1, 2); //HOOD
            Bom_ADD(bomRepo, itemRepo, "900222101", "400100502", 1, 1); //Steering wheel
            Bom_ADD(bomRepo, itemRepo, "900222101", "400200605", 1, 1); //seat l
            Bom_ADD(bomRepo, itemRepo, "900222101", "400200606", 1, 1); //seat r
            Bom_ADD(bomRepo, itemRepo, "900222101", "400200705", 1, 1); //Display mono 5 inch
            Bom_ADD(bomRepo, itemRepo, "900222101", "400200801", 1, 1); //Instrument analog mono display DIESEL
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200104", 1, 1); //Left Lamp
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200103", 1, 1); //right Lamp
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200105", 1, 1); //left Lamp rear
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200106", 1, 1); //right Lamp rear
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200200", 1, 1); //Front Glass
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200320", 1, 1); //steel 16
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200330", 1, 1); //Tire Continental 215/55/R16
            Bom_ADD(bomRepo, itemRepo, "900222101", "500200380", 1, 1); //Wheel Cap 16
            Bom_ADD(bomRepo, itemRepo, "900222101", "300100260", 1, 1); //ENGINE Diesel 1.6 112 KM

            //"900222102", "Ford Focus Combi"
            Bom_ADD(bomRepo, itemRepo, "900222102", "600100202", 1, 1); //BODY combi:
            Bom_ADD(bomRepo, itemRepo, "900222102", "600200102", 1, 2); //REAR DOOR combi: RD
            Bom_ADD(bomRepo, itemRepo, "900222102", "600200112", 1, 3); //REAR DOOR combi: RD inner
            Bom_ADD(bomRepo, itemRepo, "900222102", "600200122", 1, 3); //REAR DOOR combi: RD outer
            Bom_ADD(bomRepo, itemRepo, "900222102", "500200222", 1, 3); //REAR DOOR combi: Rear glass
            Bom_ADD(bomRepo, itemRepo, "900222102", "600200202", 1, 2); //ROOF combi
            Bom_ADD(bomRepo, itemRepo, "900222102", "600200302", 1, 2); //HOOD
            Bom_ADD(bomRepo, itemRepo, "900222102", "400100502", 1, 1); //Steering wheel
            Bom_ADD(bomRepo, itemRepo, "900222102", "400200605", 1, 1); //seat l
            Bom_ADD(bomRepo, itemRepo, "900222102", "400200606", 1, 1); //seat r
            Bom_ADD(bomRepo, itemRepo, "900222102", "400200705", 1, 1); //Display mono 5 inch
            Bom_ADD(bomRepo, itemRepo, "900222102", "400200801", 1, 1); //Instrument analog mono display DIESEL
            Bom_ADD(bomRepo, itemRepo, "900222102", "500200104", 1, 1); //Left Lamp
            Bom_ADD(bomRepo, itemRepo, "900222102", "500200103", 1, 1); //right Lamp
            Bom_ADD(bomRepo, itemRepo, "900222102", "500200105", 1, 1); //left Lamp rear
            Bom_ADD(bomRepo, itemRepo, "900222102", "500200106", 1, 1); //right Lamp rear
            Bom_ADD(bomRepo, itemRepo, "900222102", "500200200", 1, 1); //Front Glass
            Bom_ADD(bomRepo, itemRepo, "900222102", "500200310", 1, 1); //Alu 16
            Bom_ADD(bomRepo, itemRepo, "900222102", "500200330", 1, 1); //Tire Continental 215/55/R16
            Bom_ADD(bomRepo, itemRepo, "900222102", "300100220", 1, 1); //ENGINE Diesel 2.0 150 KM

            //"900222103", "Ford Focus RS"
            Bom_ADD(bomRepo, itemRepo, "900222103", "600100201", 1, 1); //BODY hatch: RD inner
            Bom_ADD(bomRepo, itemRepo, "900222103", "600200101", 1, 2); //REAR DOOR hatch: RD
            Bom_ADD(bomRepo, itemRepo, "900222103", "600200111", 1, 3); //REAR DOOR hatch: RD inner
            Bom_ADD(bomRepo, itemRepo, "900222103", "600200121", 1, 3); //REAR DOOR hatch: RD outer
            Bom_ADD(bomRepo, itemRepo, "900222103", "500200221", 1, 3); //REAR DOOR hatch: Rear glass
            Bom_ADD(bomRepo, itemRepo, "900222103", "600200201", 1, 2); //ROOF combi
            Bom_ADD(bomRepo, itemRepo, "900222103", "600200308", 1, 2); //HOOD RS
            Bom_ADD(bomRepo, itemRepo, "900222103", "400100505", 1, 1); //Steering wheel 3 arms
            Bom_ADD(bomRepo, itemRepo, "900222103", "400200605", 1, 1); //seat l
            Bom_ADD(bomRepo, itemRepo, "900222103", "400200606", 1, 1); //seat r
            Bom_ADD(bomRepo, itemRepo, "900222103", "400200708", 1, 1); //Display color 8inch
            Bom_ADD(bomRepo, itemRepo, "900222103", "400200812", 1, 1); //Instrument analog big color display BENZIN
            Bom_ADD(bomRepo, itemRepo, "900222103", "500200104", 1, 1); //Left Lamp
            Bom_ADD(bomRepo, itemRepo, "900222103", "500200103", 1, 1); //right Lamp
            Bom_ADD(bomRepo, itemRepo, "900222103", "500200105", 1, 1); //left Lamp rear
            Bom_ADD(bomRepo, itemRepo, "900222103", "500200106", 1, 1); //right Lamp rear
            Bom_ADD(bomRepo, itemRepo, "900222103", "500200200", 1, 1); //Front Glass
            Bom_ADD(bomRepo, itemRepo, "900222103", "500200311", 1, 1); //Alu 17
            Bom_ADD(bomRepo, itemRepo, "900222103", "500200331", 1, 1); //Tire Continental 225/40/R17
            Bom_ADD(bomRepo, itemRepo, "900222103", "300100180", 1, 1); //ENGINE Benzin 1.8 260 KM
        }
        public static void Seed_ONEPROD(IDbContextOneprod db, IDbContextOneprodMes dbMes)
        {
            //Create Workplaces from Workstations
            List<Workstation> workstations = new RepoWorkstation(db).GetList().ToList();
            WorkplaceRepo workplaceRepo = new WorkplaceRepo(dbMes, null);

            int i = workstations.Max(x => x.Id);
            foreach (Workstation w in workstations)
            {
                if (w.LineId != null && w.LineId > 0)
                {
                    workplaceRepo.Add(new Workplace()
                    {
                        MachineId = (int)w.LineId,
                        Name = w.Name,
                        SerialNumberType = SerialNumberType.YWWD5,
                        PrinterType = PrinterType.Zebra,
                        LabelLayoutNo = 0,
                        ComputerHostName = "#" + i,
                        PrintLabel = false,
                    });
                }
                i++;
            }

            //Add ItemGroups Basing on PREFIXES
            List<Param1> itemGroups = new List<Param1>();
            itemGroups.Add(new Param1() { Key = "900000", Name = "GROUP CAR", CycleTime = 40.0m, MinBatch = 1 });
            itemGroups.Add(new Param1() { Key = "600100", Name = "GROUP BODY", CycleTime = 34.3m, MinBatch = 1 });
            itemGroups.Add(new Param1() { Key = "600200", Name = "GROUP REARDOOR", CycleTime = 23.0m, MinBatch = 10 });
            itemGroups.Add(new Param1() { Key = "600210", Name = "GROUP REARDOOR OUTER", CycleTime = 12.0m, MinBatch = 10 });
            itemGroups.Add(new Param1() { Key = "600220", Name = "GROUP REARDOOR INNER", CycleTime = 11.5m, MinBatch = 10 });
            itemGroups.Add(new Param1() { Key = "600300", Name = "GROUP ROOF", CycleTime = 8.6m, MinBatch = 5 });
            itemGroups.Add(new Param1() { Key = "600400", Name = "GROUP HOOD", CycleTime = 9.8m, MinBatch = 20 });

            //Add ItemOP basing on Items from MasterData and PREFIX parameter
            ItemRepo itemRepo = new ItemRepo(db);
            ItemOPRepo itemOPRepo = new ItemOPRepo(db);
            CycleTimeRepo cycleTimeRepo = new CycleTimeRepo(db, null);

            int? itemGroupIdOfPrefixItem = null;
            foreach (Param1 itemGroup in itemGroups)
            {
                List<Item> itemsOfPrefix = itemRepo.GetList().Where(x => x.PREFIX == itemGroup.Key).ToList();

                foreach (Item itm in itemsOfPrefix)
                {
                    if (itm.Type == ItemTypeEnum.ItemGroup)
                    {
                        itemGroupIdOfPrefixItem = itm.Id;
                        //itemOPRepo.AddOrUpdate(new ItemOP() { Id = itm.Id, WorkOrderGenerator = true, MinBatch = itemGroup.MinBatch });

                        if (itm.ResourceGroupId != null)
                        {
                            cycleTimeRepo.AddOrUpdate(new MCycleTime() { Active = true, ItemGroupId = itm.Id, CycleTime = itemGroup.CycleTime, MachineId = (int)itm.ResourceGroupId, PiecesPerPallet = 1 });
                        }
                    }

                    //itemOPRepo.AddOrUpdate(new ItemOP() { Id = itm.Id, ItemGroupId = itemGroupIdOfPrefixItem, WorkOrderGenerator = true, MinBatch = itemGroup.MinBatch });
                    itemGroupIdOfPrefixItem = null;
                }
            }
        }
        public static void Seed_ONEPROD_OEE(IDbContextOneProdOEE dbOEE)
        {
            ResourceRepo resourceRepo = new ResourceRepo(dbOEE);
            ResourceOPRepo resourceOPRepo = new ResourceOPRepo(dbOEE, null);
            List<ResourceOP> resources = resourceOPRepo.GetList().OrderBy(x => x.ResourceGroupId).ToList();

            int count = resources.Count + 1;
            int groupCount = 0;
            foreach (ResourceOP r in resources)
            {
                r.StageNo = count - groupCount;
                r.SafetyTime = 60;
                r.ToolRequired = false;
                r.IsOEE = true;
                r.TargetOee = 0.8m;
                r.TargetAvailability = 0.9m;
                r.TargetPerformance = 0.9m;
                r.TargetQuality = 0.99m;
                resourceOPRepo.AddOrUpdate(r);

                groupCount++;
            }

            ReasonTypeRepo repo1 = new ReasonTypeRepo(dbOEE);
            OEEReasonType_ADD(repo1, "TimeAvailable", "TimeAvailable", 0, 0, "#6c6c6d", false);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "Production", "Production", 10, 10, "#14bd2c", false);
            OEEReasonType_ADD(repo1, "ScrapMaterial", "ScrapMaterial", 11, 11, "#ff0000", false);
            OEEReasonType_ADD(repo1, "ScrapProcess", "ScrapProcess", 12, 12, "#ff0000", false);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 13, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 14, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 15, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 16, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "ScrapLabel", "ScrapLabel", 19, 19, null, false);
            OEEReasonType_ADD(repo1, "Postój Planowany", "StopPlanned", 20, 20, "#0070ffa1", false);
            OEEReasonType_ADD(repo1, "Przezbrojenie planowane", "StopPlannedChangeOver", 20, 21, "#ae59d2", true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "Postój nieplanowany", "StopUnplanned", 30, 33, "#ff0000a1", false);
            OEEReasonType_ADD(repo1, "Awarie", "StopUnplannedBreakdown", 30, 30, "#98012f", false);
            OEEReasonType_ADD(repo1, "Mikroprzestoje", "StopUnplannedPreformance", 32, 32, "#95b5b5a1", false);
            OEEReasonType_ADD(repo1, "Przezbrojenie", "StopUnplannedChangeOver", 30, 31, "#ae59d2", false);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
            OEEReasonType_ADD(repo1, "unused", "unused", -1, 0, null, true);
        }
        public static void Seed_iLOGIS(IDbContextiLOGIS db)
        {
            //Add ItemOP basing on Items from MasterData and PREFIX parameter
            ItemRepo itemRepo = new ItemRepo(db);
            ItemWMSRepo itemWMSRepo = new ItemWMSRepo(db);
            PackageRepo packageRepo = new PackageRepo(db);
            PackageItemRepo packageItemRepo = new PackageItemRepo(db);
            StockUnitRepo stockUnitRepo = new StockUnitRepo(db);
            WarehouseRepo whRepo = new WarehouseRepo(db);
            WarehouseLocationRepo whlRepo = new WarehouseLocationRepo(db);
            WarehouseLocationTypeRepo warehouseLocationTypeRepo = new WarehouseLocationTypeRepo(db);
            RepoWorkstation repoWorkstation = new RepoWorkstation(db);
            WorkstationItemRepo wiRepo = new WorkstationItemRepo(db);

            ////INSERT INTO[EPSS_2_fixtures].[iLOGIS].[CONFIG_Item] ([Id],[Weight],[PickerNo],[TrainNo],[H],[ABC],[XYZ]) SELECT ID, 0, 100, 100, 0, 0 ,0 FROM[_MPPL].[MASTERDATA_Item]
            ////itemWMSRepo.ExecuteSqlCommand("INSERT INTO[EPSS_2_fixtures].[iLOGIS].[CONFIG_Item] ([Id],[Weight],[PickerNo],[TrainNo],[H],[ABC],[XYZ]) SELECT ID, 0, 100, 100, 0, 0 ,0 FROM[_MPPL].[MASTERDATA_Item]");

            List<Item> items = itemRepo.GetList().ToList();
            foreach (Item itm in items)
            {
                itemWMSRepo.AddOrUpdate(new ItemWMS() { ItemId = itm.Id, PickerNo = 100, TrainNo = 100, ABC = ClassificationABCEnum.Undefined, XYZ = ClassificationXYZEnum.Undefined, Weight = 1, H = 0 });
            }

            int whlocTypeSTD = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Shelf, "STD");
            int whlocTypeTro = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Trolley, "Trolley");
            int whlocTypeTrB = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Trolley, "Trolley BODY");
            int whlocTypeTrD = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Trolley, "Trolley DOOR");
            int whlocTypeTDp = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Trolley, "Trolley DOOR I/O");
            int whlocTypeTrH = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Trolley, "Trolley HOOD");
            int whlocTypeTrR = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Trolley, "Trolley ROOF");
            int whlocTypeFrk = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.FlowRack, "FlowRack");
            int whl1ocTypeFee =WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Feeder, "Feeder");

            Package_ADD(packageRepo, "0001", "Cartoon Package", 25, 40, 60);
            Package_ADD(packageRepo, "0001", "Cartoon Package Big", 50, 40, 60);
            Package_ADD(packageRepo, "0001", "No Package", 0, 0, 0);
            Package_ADD(packageRepo, "T001", "BODY Trolley", 0, 0, 0);
            Package_ADD(packageRepo, "T002", "DOOR Trolley", 0, 0, 0);
            Package_ADD(packageRepo, "T003", "DOOR I/O Trolley", 0, 0, 0);
            Package_ADD(packageRepo, "T003", "HOOD Trolley", 0, 0, 0);
            Package_ADD(packageRepo, "T003", "ROOF Trolley", 0, 0, 0);

            int warehouseId;
            warehouseId = Warehouse_ADD(whRepo, "90800", null, null, WarehouseTypeEnum.AccountingWarehouse, "FGW");
            warehouseId = Warehouse_ADD(whRepo, "90120", null, null, WarehouseTypeEnum.AccountingWarehouse, "WH");
            WarehouseLocation_ADD(whlRepo, "010101", "01", 1, whlocTypeSTD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010102", "01", 1, whlocTypeSTD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010103", "01", 1, whlocTypeSTD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010104", "01", 1, whlocTypeSTD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010201", "01", 2, whlocTypeSTD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010202", "01", 2, whlocTypeSTD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010203", "01", 2, whlocTypeSTD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010204", "01", 2, whlocTypeSTD, warehouseId);

            warehouseId = Warehouse_ADD(whRepo, "90130", null, null, WarehouseTypeEnum.AccountingWarehouse, "BODY Warehouse");
            WarehouseLocation_ADD(whlRepo, "010102", "01", 1, whlocTypeTrB, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010101", "01", 1, whlocTypeTrB, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010201", "01", 2, whlocTypeTrB, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010202", "01", 2, whlocTypeTrB, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010301", "01", 3, whlocTypeTrB, warehouseId);
            WarehouseLocation_ADD(whlRepo, "010302", "01", 4, whlocTypeTrB, warehouseId);

            warehouseId = Warehouse_ADD(whRepo, "90140", null, null, WarehouseTypeEnum.AccountingWarehouse, "Welding Warehouse");
            WarehouseLocation_ADD(whlRepo, "T1001", "T", 1, whlocTypeTrD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T1002", "T", 1, whlocTypeTrD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T1003", "T", 1, whlocTypeTrD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T1004", "T", 1, whlocTypeTrD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T1005", "T", 1, whlocTypeTrD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T1006", "T", 1, whlocTypeTrD, warehouseId);

            warehouseId = Warehouse_ADD(whRepo, "90150", null, null, WarehouseTypeEnum.AccountingWarehouse, "PRESS Warehouse");
            WarehouseLocation_ADD(whlRepo, "T0001", "T", 1, whlocTypeTDp, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0002", "T", 1, whlocTypeTDp, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0003", "T", 1, whlocTypeTDp, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0004", "T", 1, whlocTypeTDp, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0005", "T", 1, whlocTypeTrH, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0006", "T", 1, whlocTypeTrH, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0007", "T", 1, whlocTypeTrH, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0008", "T", 1, whlocTypeTrR, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0009", "T", 1, whlocTypeTrR, warehouseId);
            WarehouseLocation_ADD(whlRepo, "T0010", "T", 1, whlocTypeTrR, warehouseId);

            TransporterRepo transporterRepo = new TransporterRepo(db);
            transporterRepo.Add(new Transporter() { Name = "Picker 1", Code = "100", DedicatedResources = "Assembly Line", Type = EnumTransporterType.Picker, LoopQty = 5 });
            transporterRepo.Add(new Transporter() { Name = "Train 1", Code = "100", DedicatedResources = "Assembly Line", Type = EnumTransporterType.Train, LoopQty = 5 });

            List<Param1> itemGroups = new List<Param1>();
            itemGroups.Add(new Param1() { Key = "900000", Name = "GROUP CAR-------", Type = ItemTypeEnum.ItemGroup, WhName = "FGW", WhLocType = "STD", PckgName = "No Package", PckgPerPallet = 1, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.FullPackage, });
            itemGroups.Add(new Param1() { Key = "600100", Name = "GROUP BODY------", Type = ItemTypeEnum.ItemGroup, WhName = "BODY Warehouse", WhLocType = "Trolley BODY", PckgName = "BODY Trolley", PckgPerPallet = 1, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "600200", Name = "GROUP REARDOOR--", Type = ItemTypeEnum.ItemGroup, WhName = "Welding Warehouse", WhLocType = "Trolley DOOR", PckgName = "DOOR Trolle", PckgPerPallet = 1, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "600210", Name = "GROUP REARDOOR O", Type = ItemTypeEnum.ItemGroup, WhName = "PRESS Warehouse", WhLocType = "Trolley DOOR I/O", PckgName = "DOOR I/O Trolley", PckgPerPallet = 1, QtyPerPckg = 40, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "600220", Name = "GROUP REARDOOR I", Type = ItemTypeEnum.ItemGroup, WhName = "PRESS Warehouse", WhLocType = "Trolley DOOR I/O", PckgName = "DOOR I/O Trolley", PckgPerPallet = 1, QtyPerPckg = 40, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "600300", Name = "GROUP ROOF------", Type = ItemTypeEnum.ItemGroup, WhName = "PRESS Warehouse", WhLocType = "Trolley ROOF", PckgName = "ROOF Trolley", PckgPerPallet = 1, QtyPerPckg = 25, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "600400", Name = "GROUP HOOD------", Type = ItemTypeEnum.ItemGroup, WhName = "PRESS Warehouse", WhLocType = "Trolley HOOD", PckgName = "HOOD Trolley", PckgPerPallet = 1, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });

            itemGroups.Add(new Param1() { Key = "300100", Name = "Engines-", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "No Package", PckgPerPallet = 1, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "400100", Name = "Steer.Wh", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 1, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "400200", Name = "Seat----", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 2, QtyPerPckg = 2, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "400207", Name = "Display--", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 16, QtyPerPckg = 16, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "400208", Name = "Instrmnt.", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 16, QtyPerPckg = 4, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "500210", Name = "f.Lamp---", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 20, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "500220", Name = "r.Lamp---", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 20, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "500225", Name = "f.glass--", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "No Package", PckgPerPallet = 6, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "500228", Name = "r.glass--", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "No Package", PckgPerPallet = 6, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "500210", Name = "rims-----", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 6, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "500220", Name = "tires----", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 6, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "500230", Name = "wheel-cap", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 6, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "500210", Name = "farb-----", Type = ItemTypeEnum.BuyedItem, WhName = "WH", WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 6, QtyPerPckg = 1, PickStrat = PickingStrategyEnum.UpToOrderQty });

            foreach (var itmgroup in itemGroups)
            {
                List<int> itemIds = itemRepo.GetList().Where(x => x.PREFIX == itmgroup.Key && x.Type == itmgroup.Type).Select(x => x.Id).ToList();
                Package package = packageRepo.GetList().Where(x => x.Name == itmgroup.PckgName).FirstOrDefault();
                WarehouseLocationType whLocType = warehouseLocationTypeRepo.GetList().Where(x => x.Name == itmgroup.WhLocType).FirstOrDefault();
                Warehouse wh = whRepo.GetList().Where(x => x.Name == itmgroup.WhName).FirstOrDefault();

                if (package != null && whLocType != null && wh != null)
                {
                    foreach (int itemId in itemIds)
                    {
                        PackageItem_ADD(packageItemRepo, itemId, package.Id, itmgroup.QtyPerPckg, whLocType.Id, itmgroup.PckgPerPallet, itmgroup.PickStrat, wh.Id);
                    }
                }
            }

            List<Param1> itemW = new List<Param1>();
            itemW.Add(new Param1() { Key = "600100", Name = "GROUP BODY------", Type = ItemTypeEnum.ItemGroup, WorkstationName = "ASS1.01 IN" });
            itemW.Add(new Param1() { Key = "600200", Name = "GROUP REARDOOR--", Type = ItemTypeEnum.ItemGroup, WorkstationName = "BWL1.01 IN" });
            itemW.Add(new Param1() { Key = "600300", Name = "GROUP ROOF------", Type = ItemTypeEnum.ItemGroup, WorkstationName = "BWL1.01 IN" });
            itemW.Add(new Param1() { Key = "600400", Name = "GROUP HOOD------", Type = ItemTypeEnum.ItemGroup, WorkstationName = "BWL1.01 IN" });
            itemW.Add(new Param1() { Key = "600210", Name = "GROUP REARDOOR O", Type = ItemTypeEnum.ItemGroup, WorkstationName = "WL1.01 IN" });
            itemW.Add(new Param1() { Key = "600220", Name = "GROUP REARDOOR I", Type = ItemTypeEnum.ItemGroup, WorkstationName = "WL1.01 IN" });

            itemW.Add(new Param1() { Key = "300100", Name = "Engines-", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.20 Engine" });
            itemW.Add(new Param1() { Key = "400100", Name = "Steer.Wh", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.10 Interior" });
            itemW.Add(new Param1() { Key = "400200", Name = "Seat----", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.10 Interior" });
            itemW.Add(new Param1() { Key = "400207", Name = "Display--", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.10 Interior" });
            itemW.Add(new Param1() { Key = "400208", Name = "Instrmnt.", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.10 Interior" });
            itemW.Add(new Param1() { Key = "500210", Name = "f.Lamp---", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.30 Exterior" });
            itemW.Add(new Param1() { Key = "500220", Name = "r.Lamp---", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.30 Exterior" });
            itemW.Add(new Param1() { Key = "500225", Name = "f.glass--", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.03 F.Glass" });
            itemW.Add(new Param1() { Key = "500228", Name = "r.glass--", Type = ItemTypeEnum.BuyedItem, WorkstationName = "WL1.01 IN" });
            itemW.Add(new Param1() { Key = "500210", Name = "rims-----", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.30 Exterior" });
            itemW.Add(new Param1() { Key = "500220", Name = "tires----", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.30 Exterior" });
            itemW.Add(new Param1() { Key = "500230", Name = "wheel-cap", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.30 Exterior" });
            itemW.Add(new Param1() { Key = "600000", Name = "farb-----", Type = ItemTypeEnum.BuyedItem, WorkstationName = "PL1.01 IN" });

            foreach (var itmgroup in itemW)
            {
                List<int> itemIds = itemRepo.GetList().Where(x => x.PREFIX == itmgroup.Key && x.Type == itmgroup.Type).Select(x => x.Id).ToList();
                Workstation workstation = repoWorkstation.GetList().Where(x => x.Name == itmgroup.WorkstationName).FirstOrDefault();

                if (workstation != null)
                {
                    foreach (int itemId in itemIds)
                    {
                        WorkstationItem_ADD(wiRepo, itemId, workstation.Id);
                    }
                }
            }

            List<string> itemCodes = itemRepo.GetList().Where(x => x.Type == ItemTypeEnum.BuyedItem).Select(x => x.Code).ToList();
            foreach (string itemcode in itemCodes)
            {
                Stock_ADD(stockUnitRepo, itemRepo, itemcode, 1000);
            }
        }

        //-----------------PLB-ELUX-ZABRZ---------------------------------------------------------------
        public static void Seed_PLB_MasterData_Users(IDbContextCore db)
        {
            DbContextAPP_ context = (DbContextAPP_)db;
            
            UserRepo _userManager = new UserRepo(new ApplicationUserStore<User>(context), context);
            RoleRepo _roleManager = new RoleRepo(new ApplicationRoleStore(context), context);

            string role1 = _roleManager.AddRole(DefRoles.ADMIN);
            string role2 = _roleManager.AddRole(DefRoles.USER);
            string role6 = _roleManager.AddRole(DefRoles.ILOGIS_ADMIN);
            string role7 = _roleManager.AddRole(DefRoles.ILOGIS_OPERATOR);
            string role8 = _roleManager.AddRole(DefRoles.ILOGIS_VIEWER);

            string adminId = _userManager.AddUser("Admin", DefRoles.ADMIN);
            string whadminId = _userManager.AddUser("wh_admin", DefRoles.ILOGIS_ADMIN);
            string whoperatorId = _userManager.AddUser("wh_operator", DefRoles.ILOGIS_OPERATOR);
            string whuserId = _userManager.AddUser("wh_user", DefRoles.ILOGIS_VIEWER);

            _userManager.AddUserToRoleName(adminId, DefRoles.PFEP_DEFPRINT_EDITOR);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_ADMIN);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_OPERATOR);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_VIEWER);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_CONFIG_EDITOR_PRD);
            _userManager.AddUserToRoleName(adminId, DefRoles.ILOGIS_CONFIG_EDITOR_WH);
            _userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_VIEWER);
            _userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_ADMIN);
            _userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_MES_OPERATOR);
            _userManager.AddUserToRoleName(adminId, DefRoles.ONEPROD_MES_SUPEROPERATOR);
        }
        public static void Seed_PLB_MasterData_Items(IDbContextMasterData db)
        {
            ItemRepo itemRepo = new ItemRepo(db);

            //GROUP ZASYP");
            //GROUP KARTONY");
            //GROUP ELEKTRYKA");
            //GROUP BUFOR");
            //GROUP PLASTIKI");
            //GROUP INSTRUKCJE");
            //GROUP LINE OF BLOWERS");
            //GROUP CENTRAL SITO TUBY");
            //GROUP SILNIKI");
            //GROUP ? ");

            //int hoodAssemblyProcessId = Process_ADD(processRepo, "Hood Assembly");

            List<Item> groupList = itemRepo.GetList().Where(x => x.Type == ItemTypeEnum.ItemGroup).ToList();
            //groupList.Add(new Item() { Id = null, Name = "GROUP " });  //

            //Item_ADD(itemRepo, "GROUP", "9010", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD Standard Chimney");
            //Item_ADD(itemRepo, "", "", null, groupList.FirstOrDefault(x=>x.Name == "").Id, ItemTypeEnum.BuyedItem, null, "");
            Item_ADD(itemRepo, "003200588", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO LUCE ES5 ELX");
            Item_ADD(itemRepo, "003200589", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO ON/OFF+1V MOTORE ES5 ELX");
            Item_ADD(itemRepo, "003200590", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO 2V MOTORE ES5 ELX");
            Item_ADD(itemRepo, "003200591", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO 3V MOTORE ES5 ELX");
            Item_ADD(itemRepo, "02000031", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X5 TSP TC N");
            Item_ADD(itemRepo, "02000071T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.4,2X32 TL Z D12MM TORX T20");
            Item_ADD(itemRepo, "02000100", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.2,9X13TSPTC2PRZINC");
            Item_ADD(itemRepo, "02000112", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.2,9X9,5TSPTCZIN-SP.S.PF");
            Item_ADD(itemRepo, "02000118T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X9,5 TL ZIN SP.SPE TORX T20");
            Item_ADD(itemRepo, "02000124", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X13TC2PRZINC");
            Item_ADD(itemRepo, "02000129", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X9,5TCTCZINC-SP.SP");
            Item_ADD(itemRepo, "02000142", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X13-TPZ  PA.TRU.19050");
            Item_ADD(itemRepo, "02000166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X6 TL TC Z PUNT.SP");
            Item_ADD(itemRepo, "02000196", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACC.PVC 35X35 FILT.RED.SP0,02");
            Item_ADD(itemRepo, "02000209", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACC.POLYETHYLENE \"K\" 07070 SP0,04");
            Item_ADD(itemRepo, "02000212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCH.DOCUM.19X27 SP 0,07");
            Item_ADD(itemRepo, "02000222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "RIVETTO 3,2X9,5 TS ALLUMINIO");
            Item_ADD(itemRepo, "02000287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCHETTO PVC 70X100 SP.0,07");
            Item_ADD(itemRepo, "02000311", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GUIDA A SFERE \"S3\" 03024");
            Item_ADD(itemRepo, "02000509", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFILO LD33/ADESIVO 7X3 GRIGIO");
            Item_ADD(itemRepo, "02000712", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FASCETTA RAPIDA NATURAL 98mm RG203");
            Item_ADD(itemRepo, "02000886", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "VETRO CARR.TR.97/413 55 145315");
            Item_ADD(itemRepo, "02001627", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "LATERAL SIDE PACKAGING TILIA");
            Item_ADD(itemRepo, "02011399", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFILO MAGNETICO+BIAD.650X15X2");
            Item_ADD(itemRepo, "02011986T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE 3,9X13 TORX T20");
            Item_ADD(itemRepo, "02011989", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SCREW AFL4,20x8TaBMg 6L-T Dent BLACK");
            Item_ADD(itemRepo, "02012003", "9103", null, null, ItemTypeEnum.BuyedItem, null, "LEVA MICRO 00568512");
            Item_ADD(itemRepo, "02300155", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT.1,5 SS CLII");
            Item_ADD(itemRepo, "02300187", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT2 SPINA-2P CLII");
            Item_ADD(itemRepo, "02300255", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT.1,5 SSV  CLII");
            Item_ADD(itemRepo, "02300256", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT2 SS -2P CLII");
            Item_ADD(itemRepo, "02300260", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT.1,5 SE CLII");
            Item_ADD(itemRepo, "02320387", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "REFLEKTOR LED BEST 3V 700MA 2,1W");
            Item_ADD(itemRepo, "02320389", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LED CANDLE E14 4W 230-240V");
            Item_ADD(itemRepo, "02502721", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BASE INFERIORE ES5 60");
            Item_ADD(itemRepo, "02502722", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "LATERALE ES5");
            Item_ADD(itemRepo, "02502734", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BASE INFERIORE ES5 90");
            Item_ADD(itemRepo, "02502736", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "DISTANZIALE LATERALE DX-SX ES5 50");
            Item_ADD(itemRepo, "02502927", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA IMBALLO ES5 60 ELUX");
            Item_ADD(itemRepo, "02502928", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA IMBALLO ES5 90 ELUX");
            Item_ADD(itemRepo, "02502940", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.TILIA CM.52 ELUX");
            Item_ADD(itemRepo, "02502949", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.TILIA CM.70 ELUX");
            Item_ADD(itemRepo, "03118258", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA LAMPADE MET 90");
            Item_ADD(itemRepo, "03118259", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SUPP.FILTRO ANT.PULL-OUT MET 90");
            Item_ADD(itemRepo, "03118297", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA LAMPADE MET 50 (F)");
            Item_ADD(itemRepo, "03118298", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARRELLO MET 50 (F)");
            Item_ADD(itemRepo, "03119115", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "FRONTALE ES5 W2 60");
            Item_ADD(itemRepo, "03127928", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARRELLO MET 60 (F)");
            Item_ADD(itemRepo, "03127929", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA LAMPADE MET 60 (F)");
            Item_ADD(itemRepo, "03127930", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "TESTATA DX-SX ES5 MET (F)");
            Item_ADD(itemRepo, "03127931", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARRELLO MET 90");
            Item_ADD(itemRepo, "03127934", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA SUPP.ALOGENE/LED MET 60 (F)");
            Item_ADD(itemRepo, "03127935", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARRELLO GLASS+FILTRO MET 60 (F)");
            Item_ADD(itemRepo, "03127937", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARRELLO 1 GFA MET 60 (F)");
            Item_ADD(itemRepo, "03127938", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "STAFFA VETRO VERSA DX MET (F)");
            Item_ADD(itemRepo, "03127939", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "STAFFA VETRO VERSA SX MET (F)");
            Item_ADD(itemRepo, "03127940", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "STAFFA REGOLAZIONE PORF.CAPPA MET");
            Item_ADD(itemRepo, "03145045", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "FRAME TILIA MET.70");
            Item_ADD(itemRepo, "03145051", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "FRAME TILIA MET.52");
            Item_ADD(itemRepo, "03292199", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SCAT.COLL.ELETT.413 AE  145025");
            Item_ADD(itemRepo, "03292200", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FISSAC.SC.CO.EL.413 AE  145027");
            Item_ADD(itemRepo, "03292201", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "COP.SCAT.COLL.ELET. 413 145026");
            Item_ADD(itemRepo, "03292232", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "DISTANZI.413 GR360AEPVC 145148");
            Item_ADD(itemRepo, "03292388", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "DISTANZI.413 GR390AEPVC 145152");
            Item_ADD(itemRepo, "03292499", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "GUIDACAVO A SCATTO AE   132171");
            Item_ADD(itemRepo, "03293051", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA P195  POLIC.UL V2      163268");
            Item_ADD(itemRepo, "03293227", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOR.MESKA. ANTINTR.V0");
            Item_ADD(itemRepo, "03293328", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "GRIGLIA DEFL.FILTR.VERSA");
            Item_ADD(itemRepo, "03293332", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "DISTANZ.413 GR3 50 AE 145150");
            Item_ADD(itemRepo, "03293339", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CEILING LAMP 110X62 NO PIN");
            Item_ADD(itemRepo, "03293341", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CLIP BLOCCACAVO COMANDI VERSA");
            Item_ADD(itemRepo, "03293345", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COVER BRACKET PLASTIC TILIA");
            Item_ADD(itemRepo, "03293354", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "REDUCTION D.120-100 PA");
            Item_ADD(itemRepo, "03293446", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CHIUSURA TEST.ES5");
            Item_ADD(itemRepo, "03293447", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "MOSTRINA AGGANCIO FRONTALE ES5");
            Item_ADD(itemRepo, "03293449", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MOSTRINA COMANDO ES5");
            Item_ADD(itemRepo, "03293452", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "MOSTRINA LATO DX CHIUSA ES5 R7004");
            Item_ADD(itemRepo, "03295083", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SUWAK ZAMYKAJACY GR3 NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "03295084", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "HAK ZAMYKAJACY GR3 NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "03300551", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "NR.2 CA D.125 MINIBLOWER+TERM");
            Item_ADD(itemRepo, "04302172", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "MASCHERA FORATURA VERSA");
            Item_ADD(itemRepo, "04302173", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "MASCHERA FORATURA BI-GROUP");
            Item_ADD(itemRepo, "04308400", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA BEST RUSSIA");
            Item_ADD(itemRepo, "04308759", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA AEG");
            Item_ADD(itemRepo, "04308761", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA JUNO");
            Item_ADD(itemRepo, "04308763", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA ELECTROLUX");
            Item_ADD(itemRepo, "04308765", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA ZANUSSI");
            Item_ADD(itemRepo, "04308767", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA FAURE");
            Item_ADD(itemRepo, "04308768", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA PROGRESS");
            Item_ADD(itemRepo, "04308769", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA ZANKER");
            Item_ADD(itemRepo, "04308772", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO A4");
            Item_ADD(itemRepo, "04308775", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS AEG 801320720");
            Item_ADD(itemRepo, "04308776", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ELECTROLUX 801320634");
            Item_ADD(itemRepo, "04308777", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ZANUSSI 801320510");
            Item_ADD(itemRepo, "04308778", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ZANKER 801320602");
            Item_ADD(itemRepo, "04308779", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD AEG 801320721");
            Item_ADD(itemRepo, "04308780", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ELECTROLUX 801320635");
            Item_ADD(itemRepo, "04308781", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ZANUSSI 801320644");
            Item_ADD(itemRepo, "04308782", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY AEG 801320722");
            Item_ADD(itemRepo, "04308783", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ELECTROLUX 801320636");
            Item_ADD(itemRepo, "04308784", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ZANUSSI 801320650");
            Item_ADD(itemRepo, "04308785", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ELECTROLUX 801320578  R");
            Item_ADD(itemRepo, "04308786", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK AEG 801320684  O");
            Item_ADD(itemRepo, "04308787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ZANUSSI 801320243  X");
            Item_ADD(itemRepo, "04308788", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON 801320204  S");
            Item_ADD(itemRepo, "04308830", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.VERSA ELECTROLUX NEW RISER");
            Item_ADD(itemRepo, "04308831", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA ZANUSSI NEW RISER");
            Item_ADD(itemRepo, "04308832", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA FAURE NEW RISER");
            Item_ADD(itemRepo, "04308833", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA AEG NEW RISER");
            Item_ADD(itemRepo, "04308834", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA JUNO NEW RISER");
            Item_ADD(itemRepo, "04308841", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "WARRANTY RUSSO AEG ELEC.ZANU.801320556 D");
            Item_ADD(itemRepo, "04308844", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERTIFICATO RUSSO ELECTR.867351004-5189");
            Item_ADD(itemRepo, "04308848", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON OEM-GB 801320605 P");
            Item_ADD(itemRepo, "04308849", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI TILIA ELECTROLUX");
            Item_ADD(itemRepo, "04308850", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO TILIA ELECTROLUX");
            Item_ADD(itemRepo, "04308855", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA BEST");
            Item_ADD(itemRepo, "04308856", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA BEST");
            Item_ADD(itemRepo, "04308857", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI TILIA ZANUSSI");
            Item_ADD(itemRepo, "04308858", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO TILIA ZANUSSI");
            Item_ADD(itemRepo, "04308860", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI TILIA AEG");
            Item_ADD(itemRepo, "04308861", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO TILIA AEG");
            Item_ADD(itemRepo, "04308866", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI TILIA FAURE");
            Item_ADD(itemRepo, "04308867", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO TILIA FAURE");
            Item_ADD(itemRepo, "04308868", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ELECTR. 2Y 801320639");
            Item_ADD(itemRepo, "04308869", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ELECTR. UK 801320640");
            Item_ADD(itemRepo, "04308870", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ELECTROLUX 2Y  801320638");
            Item_ADD(itemRepo, "04308921", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERT.RUSSO ELECTR.RUC-SE.HA21.B.00612/19");
            Item_ADD(itemRepo, "04309965", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AD.MET.74X74 (1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04309976", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.ADES.POLIESTER AD.PER 5x2,5");
            Item_ADD(itemRepo, "04309982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA BIANC.100X300(1)");
            Item_ADD(itemRepo, "04309989", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA BIANCA 76X40 (1)");
            Item_ADD(itemRepo, "04404512", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING TURCHIA NEUTRA");
            Item_ADD(itemRepo, "04405212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING UKRAINA");
            Item_ADD(itemRepo, "04405395", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING A+++ NEUTRA");
            Item_ADD(itemRepo, "04500012", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NASTRO CARTA M50X19");
            Item_ADD(itemRepo, "04500037", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAST.ADES.AVVERT.PERICOLO 54m");
            Item_ADD(itemRepo, "04808159", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8159");
            Item_ADD(itemRepo, "04808160", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8160");
            Item_ADD(itemRepo, "04808166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8166");
            Item_ADD(itemRepo, "04808169", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8169");
            Item_ADD(itemRepo, "06002290", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ASS.MINI BLOWER EMC HEI-SK300");
            Item_ADD(itemRepo, "06002302", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ASS.MINI BLOWER EMC HEI-SL400");
            Item_ADD(itemRepo, "06102769", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COM.COMPLETO ES5 BASCULANTE ELX");
            Item_ADD(itemRepo, "06102770", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LED ES5 60 CM");
            Item_ADD(itemRepo, "06102977", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS. COMANDO COMPLETO ES5 BASIC");
            Item_ADD(itemRepo, "06103004", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LAMPADE 4W ES5 60 CM");
            Item_ADD(itemRepo, "06103011", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LAMPADE 4W ES5 90 CM");
            Item_ADD(itemRepo, "06103012", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LAMPADE 4W ES5 60 CM X RICAMBI");
            Item_ADD(itemRepo, "06103013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LAMPADE 4W ES5 90 CM X RICAMBI");
            Item_ADD(itemRepo, "06142999", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 52 GR3 ELECTROLUX");
            Item_ADD(itemRepo, "06143000", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 70 GR3 ZANUSSI");
            Item_ADD(itemRepo, "06143193", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 70 GR3 ELECTROLUX");
            Item_ADD(itemRepo, "06143524", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 52 GR3 AEG");
            Item_ADD(itemRepo, "06143525", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 52 GR3 FAURE");
            Item_ADD(itemRepo, "06143526", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 52 GR3 ZANUSSI");
            Item_ADD(itemRepo, "08087718", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR0 ANTIGR.377X203,5 ES5 50");
            Item_ADD(itemRepo, "08087757", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR0 ANTIGR.477X203,5 ES5 60");
            Item_ADD(itemRepo, "08087770", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR0 ANTIGR.388X193 ES5 90");
            Item_ADD(itemRepo, "08087773", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO C/MANIGLIA TILIA 1F 488,5x183");
            Item_ADD(itemRepo, "08087775", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO C/MANIGLIA TILIA 222,5x183");
            Item_ADD(itemRepo, "08094297", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CASSONE ES4 MINI BLOWER ELZ 90");
            Item_ADD(itemRepo, "08094298", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CASSONE ES4 MINI BLOWER ELZ 50");
            Item_ADD(itemRepo, "08094365", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.FAST INSTALL.HOOD TORX");
            Item_ADD(itemRepo, "08094367", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.COVER BOX BLOWER DX51D 52 TILIA");
            Item_ADD(itemRepo, "08094371", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.COVER BOX BLOWER DX51D 70 TILIA");
            Item_ADD(itemRepo, "08094378", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.FRONT.VERSA+GLASS WHITE 60+SER. ELUX");
            Item_ADD(itemRepo, "08094379", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.FRONT.VERSA+GLASS BLACK 60+SER. ELUX");
            Item_ADD(itemRepo, "E3344909", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.STAFFA AGGANCIO SUPP.P ELZ");
            Item_ADD(itemRepo, "E3350829", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI. STAFFA TRASF.EL ES");
            Item_ADD(itemRepo, "E3352962", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.FRONTALE ES5 XS 60");
            Item_ADD(itemRepo, "E3353451", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.FRONTALE ES5 XS 90");
            Item_ADD(itemRepo, "E3353478", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.FRONTALE ES5 XS 50");
            Item_ADD(itemRepo, "E3353479", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.BATTUTA FILTRO PULL-OUT XS");
            Item_ADD(itemRepo, "E3353603", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.SUPP.LAMPADE PULL-OUT ELZ");
            Item_ADD(itemRepo, "E3353718", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.BRACKET CONTROL TILIA");
            Item_ADD(itemRepo, "E3405623", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD.TOX CASS.ES5 EL.LIGHT DX51D 60");
            Item_ADD(itemRepo, "003200314", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "QUAD.COM.GI198 BL1 P431C");
            Item_ADD(itemRepo, "004203896", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "GROUND WIRE CHASSIS WIN DIS. 2");
            Item_ADD(itemRepo, "02000069", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.4,8X38TL D.12 TCZ");
            Item_ADD(itemRepo, "02000069T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.4,8X38TL D.12 Z TORX T20");
            Item_ADD(itemRepo, "02000100", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.2,9X13TSPTC2PRZINC");
            Item_ADD(itemRepo, "02000115", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X10 TCTC ZIGRIN.ZINC.");
            Item_ADD(itemRepo, "02000118", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X9,5TLTCZIN-SP.SPE");
            Item_ADD(itemRepo, "02000124", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X13TC2PRZINC");
            Item_ADD(itemRepo, "02000135", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.2,9X9,5FRTCZINSP-PA.FE");
            Item_ADD(itemRepo, "02000140", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X9,5 TC TC N");
            Item_ADD(itemRepo, "02000142", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X13-TPZ  PA.TRU.19050");
            Item_ADD(itemRepo, "02000154", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X16TCTC ZINC.2PR");
            Item_ADD(itemRepo, "02000166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X6 TL TC Z PUNT.SP");
            Item_ADD(itemRepo, "02000196", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACC.PVC 35X35 FILT.RED.SP0,02");
            Item_ADD(itemRepo, "02000209", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACC.POLYETHYLENE \"K\" 07070 SP0,04");
            Item_ADD(itemRepo, "02000212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCH.DOCUM.19X27 SP 0,07");
            Item_ADD(itemRepo, "02000287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCHETTO PVC 70X100 SP.0,07");
            Item_ADD(itemRepo, "02000354", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "RONDELLA DOPPIA DENT. 4X15 N");
            Item_ADD(itemRepo, "02000605", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FERMACAVO ADES. 72-0256-9200");
            Item_ADD(itemRepo, "02000609", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POLIETIL.BOLLATO  900");
            Item_ADD(itemRepo, "02000700", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASSELLI A MURO D. 8");
            Item_ADD(itemRepo, "02000712", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FASCETTA RAPIDA NATURAL 98mm RG203");
            Item_ADD(itemRepo, "02000726", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "DADO 4MX7 OTTONE");
            Item_ADD(itemRepo, "02000731", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ROND.DENTELLATA D.4,3");
            Item_ADD(itemRepo, "02001106", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESP.LA.AGG.PZ      90X60X32");
            Item_ADD(itemRepo, "02001331", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESPANSO IMBALLO KK35    154028");
            Item_ADD(itemRepo, "02001387", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESPANSO KK UNIFICATO SX 163064");
            Item_ADD(itemRepo, "02001388", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESPANSO KK UNIFICATO DX 163065");
            Item_ADD(itemRepo, "02009348", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PORTAMESTOLI  K224-R ECO OTT.SAT. 60");
            Item_ADD(itemRepo, "02009349", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PORTAMESTOLI  K224-R ECO OTT.SAT. 90");
            Item_ADD(itemRepo, "02009356", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PORTAMESTOLI GORENJE K24");
            Item_ADD(itemRepo, "02010004", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POLIETIL.BOLLATO  450");
            Item_ADD(itemRepo, "02011020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.UNIV.SPAX 4,2X15 TCTC ZN");
            Item_ADD(itemRepo, "02011110", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X12 TCTC ZIGR. OTTONE");
            Item_ADD(itemRepo, "02011113", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "RIFLETTORE K2000 ELZ  163107");
            Item_ADD(itemRepo, "02011358", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCH.PVC  SP0,015  1000X600 FS340");
            Item_ADD(itemRepo, "02011359", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCH.PVC  SP0,015  1200X700 FS340");
            Item_ADD(itemRepo, "02011558", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO ALLUMINIO COLORE OTTONE");
            Item_ADD(itemRepo, "02011806", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE AF 3,9x16 TBL TC N ZINC.");
            Item_ADD(itemRepo, "02011957", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "HAND SCREW M5X12");
            Item_ADD(itemRepo, "02011983T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X6 TL Z PUNT.SP TORX T20");
            Item_ADD(itemRepo, "02011986T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE 3,9X13 TORX T20");
            Item_ADD(itemRepo, "02011988", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TIMESTRIP");
            Item_ADD(itemRepo, "02300155", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT.1,5 SS CLII");
            Item_ADD(itemRepo, "02300187", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT2 SPINA-2P CLII");
            Item_ADD(itemRepo, "02300230", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75MT1,5 SP.ING.H05VV-F 2343");
            Item_ADD(itemRepo, "02300236", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.3XO,75MT1,5O SE HO5+OCCH.D4,2");
            Item_ADD(itemRepo, "02300256", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT2 SS -2P CLII");
            Item_ADD(itemRepo, "02300260", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT.1,5 SE CLII");
            Item_ADD(itemRepo, "02300798", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FAR.AL.F&F ZIK7+CONN.6,3 L160 SAT.OSRAM");
            Item_ADD(itemRepo, "02300981", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "COPRICONNETTORE  POLIPROP V2 AE");
            Item_ADD(itemRepo, "02320346", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LAMP.E14 CANDLE 230-240V 28W");
            Item_ADD(itemRepo, "02320387", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "REFLEKTOR LED BEST 3V 700MA 2,1W");
            Item_ADD(itemRepo, "02320389", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LED CANDLE E14 4W 230-240V");
            Item_ADD(itemRepo, "02501803", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SOST.TUBO KK UNIFIC.90  163060");
            Item_ADD(itemRepo, "02501804", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SOST.TUBO KK UNIFIC.60  163059");
            Item_ADD(itemRepo, "02502711", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ANGOLARE WIN");
            Item_ADD(itemRepo, "02502712", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "AVVOLGENTE TUBO WIN");
            Item_ADD(itemRepo, "02502713", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BASE IMBALLO WIN");
            Item_ADD(itemRepo, "02502719", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "DISTANZIALE IMBALLO WIN");
            Item_ADD(itemRepo, "02502929", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.KK UNIF.60 FALDE COMB.ELUX");
            Item_ADD(itemRepo, "02502930", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.KK UNIF.90 FALDE COMB.ELUX");
            Item_ADD(itemRepo, "02502936", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA IMBALLO WIN 50/60 ELUX");
            Item_ADD(itemRepo, "02502937", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA IMBALLO WIN 70/80/90 ELUX");
            Item_ADD(itemRepo, "03119097", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN C2 PU W2 60");
            Item_ADD(itemRepo, "03119098", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN C2 PU BL1 60");
            Item_ADD(itemRepo, "03119100", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN C2 PU BL1 90");
            Item_ADD(itemRepo, "03119105", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN A2 ELER W2 60");
            Item_ADD(itemRepo, "03119111", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN A2 ELER W2 50");
            Item_ADD(itemRepo, "03119137", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN C2 PU W2 60 (F)");
            Item_ADD(itemRepo, "03119138", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN C2 PU W2 90 (F)");
            Item_ADD(itemRepo, "03119139", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN C2 PU BL1 60 (F)");
            Item_ADD(itemRepo, "03119140", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN C2 PU BL1 90 (F)");
            Item_ADD(itemRepo, "03119155", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS ROCOCO BL262 60 (S)");
            Item_ADD(itemRepo, "03119158", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS ROCOCO ANCIENT WHITE 60 (S)");
            Item_ADD(itemRepo, "03119176", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA WIN A2 ELER ANTRANERO 60");
            Item_ADD(itemRepo, "031469686", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "RAL9005. CARCASSA K24RL L1 A2 PU 60");
            Item_ADD(itemRepo, "031469687", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "AVORIO. CARCASSA K24RL L1 A2 PU 60");
            Item_ADD(itemRepo, "03147147", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K24 PC RETRO' 2005 SLS R1015 60");
            Item_ADD(itemRepo, "03147148", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K24 PC RETRO' 2005 SLS R1015 90");
            Item_ADD(itemRepo, "03156463", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K24 PC RETRO' 2005 SLS ANTRANERO 60");
            Item_ADD(itemRepo, "03156464", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K24 PC RETRO' 2005 SLS ANTRANERO 90");
            Item_ADD(itemRepo, "03201014", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GEMMA SPIA ROSSA 186PK 86129");
            Item_ADD(itemRepo, "03202322", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GOMMINO SUPP.MOT. EPDM K135 BLUE");
            Item_ADD(itemRepo, "03202442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "BLOCCACAVO ADIMPEX SR/SP1 SRB-R-3");
            Item_ADD(itemRepo, "03292631", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "RIDUZIONE 150/125 K2000 163094");
            Item_ADD(itemRepo, "03293222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "COPRICONNETTORE FEMMINA NILAMID");
            Item_ADD(itemRepo, "03293227", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOR.MESKA. ANTINTR.V0");
            Item_ADD(itemRepo, "03293294", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "STAFFA FISSAGGIO SCATOLA SMART PP GR");
            Item_ADD(itemRepo, "03293295", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO CAVO ALIMENTAZIONE SMART PP GR");
            Item_ADD(itemRepo, "03293297", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA PC 110X62");
            Item_ADD(itemRepo, "03293302", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO CAVO ALIMENTAZIONE SMART PA");
            Item_ADD(itemRepo, "03293303", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "STAFFA FISSAGGIO SCATOLA SMART PA");
            Item_ADD(itemRepo, "03293320", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "DEFL.FILTRANTE WIN AE");
            Item_ADD(itemRepo, "03293370", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO CAVO ALIMENT.SMART PP BLACK");
            Item_ADD(itemRepo, "03293371", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "STAFFA FISSAGGIO SCATOLA SMART PP BLACK");
            Item_ADD(itemRepo, "03294017", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO LUCE  K195 OV BL1 132127");
            Item_ADD(itemRepo, "03294018", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO MOT.OV K195  BL1 132128");
            Item_ADD(itemRepo, "03294757", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA SP NEW 07");
            Item_ADD(itemRepo, "03295264", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "FLANGIA D150 K2000");
            Item_ADD(itemRepo, "03300484", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO CA D.190 K2000 ALTE AE 163630");
            Item_ADD(itemRepo, "03300489", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "NR.2 CA D.190 K2000 ALTE+TERMORETRAIBILE");
            Item_ADD(itemRepo, "04302103", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "MASCHERA FORATURA K24   250016");
            Item_ADD(itemRepo, "04302174", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "MASCHERA FORATURA WIN");
            Item_ADD(itemRepo, "04306466", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "VOLANTINO ISTRUZIONE NORME CLI/CLII");
            Item_ADD(itemRepo, "04306513", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.K24-K25 NEUTRO PIATTAFORMA");
            Item_ADD(itemRepo, "04306513R", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.K24 PIATT.RUMENO");
            Item_ADD(itemRepo, "04306517", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO CAVO SPECIALE");
            Item_ADD(itemRepo, "04307442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FO_SMALTIMENTO WEEE DIRECTIVE CEE");
            Item_ADD(itemRepo, "04308215", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA GLEM/AIRLUX  ITALIA");
            Item_ADD(itemRepo, "04308352", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA GORENJE 573380 (06-16)");
            Item_ADD(itemRepo, "04308387", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FO.CENTRI ASSISTENZA GORENJE");
            Item_ADD(itemRepo, "04308393", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "VOL.ISTRUZIONE NORME CLI/CLII GORENJE");
            Item_ADD(itemRepo, "04308400", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA BEST RUSSIA");
            Item_ADD(itemRepo, "04308434", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA CONVENZIONALE GLEM");
            Item_ADD(itemRepo, "04308564", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.ISTR K24R L1 A2 PU GORENJE");
            Item_ADD(itemRepo, "04308565", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA GORENJE SERBIA 242491");
            Item_ADD(itemRepo, "04308574", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA GORENJE KAZAKHSTAN 150565");
            Item_ADD(itemRepo, "04308575", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA GORENJE 150566");
            Item_ADD(itemRepo, "04308589", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "VOLANTINO NORME ARABO");
            Item_ADD(itemRepo, "04308632", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.WIN");
            Item_ADD(itemRepo, "04308691", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.WIN GLEM+ARABO");
            Item_ADD(itemRepo, "04308692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO MATTONCINO POLISTIR.MOTORE HF800");
            Item_ADD(itemRepo, "04308694", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.WIN M-SYSTEM");
            Item_ADD(itemRepo, "04308696", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA GLEM PORTOGALLO");
            Item_ADD(itemRepo, "04308726", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.SAVO WIN E2 PU FINLANDESE");
            Item_ADD(itemRepo, "04308727", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.SAVO WIN D2 ELER FINLANDESE");
            Item_ADD(itemRepo, "04308772", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO A4");
            Item_ADD(itemRepo, "04308776", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ELECTROLUX 801320634");
            Item_ADD(itemRepo, "04308778", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ZANKER 801320602");
            Item_ADD(itemRepo, "04308780", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ELECTROLUX 801320635");
            Item_ADD(itemRepo, "04308783", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ELECTROLUX 801320636");
            Item_ADD(itemRepo, "04308785", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ELECTROLUX 801320578  R");
            Item_ADD(itemRepo, "04308786", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK AEG 801320684  O");
            Item_ADD(itemRepo, "04308787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ZANUSSI 801320243  X");
            Item_ADD(itemRepo, "04308788", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON 801320204  S");
            Item_ADD(itemRepo, "04308792", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO WIN AEG");
            Item_ADD(itemRepo, "04308794", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO WIN ELECTROLUX");
            Item_ADD(itemRepo, "04308796", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO WIN FAURE");
            Item_ADD(itemRepo, "04308797", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI WIN NEUE");
            Item_ADD(itemRepo, "04308798", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI WIN PROGRESS");
            Item_ADD(itemRepo, "04308799", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI WIN ZANKER");
            Item_ADD(itemRepo, "04308801", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO WIN ZANUSSI");
            Item_ADD(itemRepo, "04308810", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO ERRATA CORRIGE NEW TUBI K24");
            Item_ADD(itemRepo, "04308824", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA UN.GORENJE 143834 (07-18)");
            Item_ADD(itemRepo, "04308826", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI WIN ELECTROLUX NEW RISER");
            Item_ADD(itemRepo, "04308827", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI WIN FAURE NEW RISER");
            Item_ADD(itemRepo, "04308828", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI WIN ZANUSSI NEW RISER");
            Item_ADD(itemRepo, "04308829", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI WIN AEG NEW RISER");
            Item_ADD(itemRepo, "04308841", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "WARRANTY RUSSO AEG ELEC.ZANU.801320556 D");
            Item_ADD(itemRepo, "04308845", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERTIFICATO RUSSO ELECTR. 867351005-5190");
            Item_ADD(itemRepo, "04308846", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI WIN ELECTROLUX RUSSIA");
            Item_ADD(itemRepo, "04308847", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO WIN ELECTROLUX RUSSIA");
            Item_ADD(itemRepo, "04308848", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON OEM-GB 801320605 P");
            Item_ADD(itemRepo, "04308859", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.TIMESTRIP FAURE");
            Item_ADD(itemRepo, "04308883", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL ROCOCO ELECTROLUX");
            Item_ADD(itemRepo, "04308921", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERT.RUSSO ELECTR.RUC-SE.HA21.B.00612/19");
            Item_ADD(itemRepo, "04344583", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.IMB.WIN WICKES 07000131A");
            Item_ADD(itemRepo, "04344584", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ANTITACCHEGGIO WICKES");
            Item_ADD(itemRepo, "04399901", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CARTA PROT.K60   1200X800");
            Item_ADD(itemRepo, "04399902", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CARTA PROT.K90   1200X1200");
            Item_ADD(itemRepo, "04404512", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING TURCHIA NEUTRA");
            Item_ADD(itemRepo, "04405212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING UKRAINA");
            Item_ADD(itemRepo, "04405395", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING A+++ NEUTRA");
            Item_ADD(itemRepo, "04500000", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NASTRO ADESIVO  60X48");
            Item_ADD(itemRepo, "04500012", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NASTRO CARTA M50X19");
            Item_ADD(itemRepo, "04500037", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAST.ADES.AVVERT.PERICOLO 54m");
            Item_ADD(itemRepo, "04808144", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8144");
            Item_ADD(itemRepo, "06002213U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 PLAS. BEST EA27 P-C");
            Item_ADD(itemRepo, "06002240U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 BEST SC.COND.EB40 PIAT.C");
            Item_ADD(itemRepo, "06002248U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 BEST SC.COND.EB20 P-C");
            Item_ADD(itemRepo, "06002284", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "HF 600 Conv.Plast.S2011cond.4mF + Cutoff");
            Item_ADD(itemRepo, "06002294U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011PL.BEST EA27 220V 60HZ P-C");
            Item_ADD(itemRepo, "06002314", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "HF 800.1 Conv.Plast.S2011cond.5mF+Cutoff");
            Item_ADD(itemRepo, "06102684", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO 0238-AC CLII SS");
            Item_ADD(itemRepo, "06102782", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO IS ELUX SE 02300244");
            Item_ADD(itemRepo, "06102795", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.LED DRIVER K24 2018");
            Item_ADD(itemRepo, "06102900", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.SMART ELER D2");
            Item_ADD(itemRepo, "06102911", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ASS.TRASF.K24 EB L450  (TRASF.02300821)");
            Item_ADD(itemRepo, "06102969", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.SMART PU C2");
            Item_ADD(itemRepo, "06102973", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.SMART PU LED");
            Item_ADD(itemRepo, "06103022", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.SMART PU C2 BRASSED BLACK");
            Item_ADD(itemRepo, "06103023", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.SMART PU C2 CHROMED BLACK");
            Item_ADD(itemRepo, "06103049", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS. LAMPADE K24 EA P-C 4W");
            Item_ADD(itemRepo, "06106749", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.SMART PU C2 BRASSED GR");
            Item_ADD(itemRepo, "06106750", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.SMART PU C2 CHROMED GR");
            Item_ADD(itemRepo, "08087539", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ALL.MAN.C/MAN.K24 2011 239X298");
            Item_ADD(itemRepo, "08087705", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ALL.MAN.WIN UNICO 60 570x343,5");
            Item_ADD(itemRepo, "08087709", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ALL.MAN.WIN UNICO 50 456,5x343,5");
            Item_ADD(itemRepo, "08087766", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO WIN 343,5X285");
            Item_ADD(itemRepo, "08087767", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO WIN 343,5x250");
            Item_ADD(itemRepo, "08088362", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ASS.FLANG.VR.D150 K2000 163610");
            Item_ADD(itemRepo, "08094167", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2PU XS 70");
            Item_ADD(itemRepo, "08094168", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2PU XS 80");
            Item_ADD(itemRepo, "08094170", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2PU XS 90");
            Item_ADD(itemRepo, "08094182", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2 PU XS 90 W");
            Item_ADD(itemRepo, "08094183", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2 PU XS 70 W");
            Item_ADD(itemRepo, "08094186", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN A2 PU XS 60 W");
            Item_ADD(itemRepo, "08094188", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN A2 PU XS 90 W");
            Item_ADD(itemRepo, "08094197", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN ELER A2 XS 60 W");
            Item_ADD(itemRepo, "08094200", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2 PU XS 50 W");
            Item_ADD(itemRepo, "08094201", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN ELER A2 XS 50 W");
            Item_ADD(itemRepo, "08094202", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2 PU XS 60");
            Item_ADD(itemRepo, "08094203", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2 PU XS 70");
            Item_ADD(itemRepo, "08094204", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.WIN C2PU XS 90 2018");
            Item_ADD(itemRepo, "A15334501", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SPOON HOLDER BRASSED ROCOCO");
            Item_ADD(itemRepo, "A15334502", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SPOON HOLDER CHROMED ROCOCO");
            Item_ADD(itemRepo, "E3251829", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "TR.PIASTR.FISS.TERRA");
            Item_ADD(itemRepo, "E3344399", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.SQUADRETTA COM.K24 PU EA ELZ   250051");
            Item_ADD(itemRepo, "E3351830", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.STAFFA FISS.CONV. PIATT.A");
            Item_ADD(itemRepo, "E3353597", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA CAMINI WIN SKINPASSATO");
            Item_ADD(itemRepo, "08010556", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 TEL.XS SENZA DEFLETTORE");
            Item_ADD(itemRepo, "08010575", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 ANTRANERO");
            Item_ADD(itemRepo, "08095944", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 XS AEG+DIV");
            Item_ADD(itemRepo, "08095945", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 XS ELECTROLUX+DIV");
            Item_ADD(itemRepo, "08095946", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 XS FAURE+DIV+F.WEG.");
            Item_ADD(itemRepo, "08095947", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H350 XS NEUE+DIV");
            Item_ADD(itemRepo, "08095948", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 XS ZANKER+DIV");
            Item_ADD(itemRepo, "08095949", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 XS ZANUSSI+DIV");
            Item_ADD(itemRepo, "08095950", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 XS PROGRESS+DIV");
            Item_ADD(itemRepo, "08095951", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H350 XS ZANUSSI+DIV");
            Item_ADD(itemRepo, "08095952", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H350 XS +DIV+F.WEG.");
            Item_ADD(itemRepo, "08095953", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H350 XS +DIV");
            Item_ADD(itemRepo, "08095960", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD XS + DIV+ F.WEG.");
            Item_ADD(itemRepo, "08095961", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB  K24 OLD W2 + DIV + F.WEG.");
            Item_ADD(itemRepo, "08095963", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 BL1 FAURE+DIV+F.WEG.");
            Item_ADD(itemRepo, "08095964", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H350 BL1 + DIV");
            Item_ADD(itemRepo, "08095965", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 W2 FAURE+DIV+F.WEG.");
            Item_ADD(itemRepo, "08095966", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 BL262 GOLD ELUX +DIV");
            Item_ADD(itemRepo, "08095967", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 BL262 SILVER ELUX +DIV");
            Item_ADD(itemRepo, "08095968", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 WH GOLD ELUX +DIV");
            Item_ADD(itemRepo, "08095969", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB WIN H460 WH SILVER ELUX +DIV");
            Item_ADD(itemRepo, "08095970", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD IVORY + DIV + UCHWYT");
            Item_ADD(itemRepo, "08095971", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD ANTRANERO BEST+DIV+PORTAME");
            Item_ADD(itemRepo, "08095972", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD BL1 + DIV + F.WEG.");
            Item_ADD(itemRepo, "08095973", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD AVORIO BEST + DIV + UCHW60");
            Item_ADD(itemRepo, "08095974", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD R9005+DIV + UCHWYT");
            Item_ADD(itemRepo, "08095975", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD ANTRANER BEST+DIV+UCHW90");
            Item_ADD(itemRepo, "08095976", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD AVORIO BEST+DIV+UCHWYT 90");
            Item_ADD(itemRepo, "08095977", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD W2 + DIV");
            Item_ADD(itemRepo, "08095980", "9106", null, groupList.FirstOrDefault(x => x.Name == "GROUP CENTRAL SITO TUBY").Id, ItemTypeEnum.BuyedItem, null, "ZESPÓŁ TUB K24 OLD XS + DEFLEKTOR");
            Item_ADD(itemRepo, "004202557", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILERIA EC L=630MM DIS. N 10");
            Item_ADD(itemRepo, "004203524", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVI  PONTE MOTORE PIAT.LIGHT DIS.N 6");
            Item_ADD(itemRepo, "004203807", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILERIA LED WITCH DIS. 2");
            Item_ADD(itemRepo, "004203986", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "WIRES CONN.2 LEDS ISL ELX100 DIS.28");
            Item_ADD(itemRepo, "004204022", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "WIRING 2 LEDS ZETA DIS.38");
            Item_ADD(itemRepo, "02000039", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,4X15TSPTC ZINC.PHILLIPS");
            Item_ADD(itemRepo, "02000117", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X9,5TLTCN-SP.SPE");
            Item_ADD(itemRepo, "02000137", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X15 TC TC ZINC.");
            Item_ADD(itemRepo, "02000469", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "STAFFA FISS.RACC.OLIO ZN + VITE 1/4 BSW");
            Item_ADD(itemRepo, "02000669", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "DADO M6X10 ZINCATO");
            Item_ADD(itemRepo, "020007050", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFILO AERSTOP CN24/A 15X6");
            Item_ADD(itemRepo, "02001315", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "TAMPONE  ESPANSO K10     47053");
            Item_ADD(itemRepo, "02001355", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BASE POLISTIR. K-DRIADE 169016");
            Item_ADD(itemRepo, "02001403", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESPANSO BASE CK - PK    151106");
            Item_ADD(itemRepo, "02001630", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PACKAGING BASE 440X330X30");
            Item_ADD(itemRepo, "02001631", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PACKAGING ANGULAR 150X150X600");
            Item_ADD(itemRepo, "02005173", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "VETRO  KB300 90");
            Item_ADD(itemRepo, "02005189", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "VETRO  KB300 60");
            Item_ADD(itemRepo, "02005215", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "VETRO  K-FLAT 90 SP.6 mm");
            Item_ADD(itemRepo, "02005216", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "VETRO K-FLAT 60 SP.6 mm");
            Item_ADD(itemRepo, "02011162", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "RONDELLA NYLON D15X5,5X1");
            Item_ADD(itemRepo, "02011212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "DADO IN GABBIA M4 H8,3 C70");
            Item_ADD(itemRepo, "02011235", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO FE NI/SATINATO");
            Item_ADD(itemRepo, "02011239", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO FE NI/SATINATO 8x11,5");
            Item_ADD(itemRepo, "02011316", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4 X20 TESTA D14  TORX");
            Item_ADD(itemRepo, "02011560", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4 X23 TESTA D14  TORX");
            Item_ADD(itemRepo, "02011938", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "SCREW MALE FIXING LATERAL EXTENSION");
            Item_ADD(itemRepo, "02011939", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "SCREW FEMALE FIXING LATERAL EXTENSION");
            Item_ADD(itemRepo, "02300118", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LAMP.TUBOL. 125-130V 40W");
            Item_ADD(itemRepo, "02300126", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO ALIM.3X0,75 MT.1,5 S.E.");
            Item_ADD(itemRepo, "02300128", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA AUSTRALIA   3x0,75 160cm");
            Item_ADD(itemRepo, "02300130", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.3X0,75MT1,50 SS H05");
            Item_ADD(itemRepo, "02300422", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "INT.BASCULANTE K BETA ELECTROLUX");
            Item_ADD(itemRepo, "02300872", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FERRITE DI19 DE29 H75");
            Item_ADD(itemRepo, "02301035", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FLAT CABLE CONTROL  L500 DIS:9");
            Item_ADD(itemRepo, "02301058", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PIATTINA COMANDO WITCH  DIS.4");
            Item_ADD(itemRepo, "02301075", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FLAT WIRING CONTROL GLOW DIS.29");
            Item_ADD(itemRepo, "02301076", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FLAT WIRING H2H GLOW DIS.30");
            Item_ADD(itemRepo, "02301084", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FLAT WIRING CONTROL ER DIS.2");
            Item_ADD(itemRepo, "02320358", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO ALIM .SPINA EU /IEC CLI 1,5 M");
            Item_ADD(itemRepo, "02320408", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FERRITE RRC-14-7-28-M");
            Item_ADD(itemRepo, "02501807", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SUPPORTO TUBO KK034 120 152117");
            Item_ADD(itemRepo, "02501878", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SOSTEGNO TUBO K2000 SLIM  60 163194");
            Item_ADD(itemRepo, "02501879", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SOSTEGNO TUBO K2000 SLIM 90 163195");
            Item_ADD(itemRepo, "02501893", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SOSTEGNO TUBO K2000 SLIM 80 163207");
            Item_ADD(itemRepo, "02501936", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SOSTEGNO TUBO K2000 SLIM 70");
            Item_ADD(itemRepo, "02501995", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.K UNIFICA.120 FALDE COMB.");
            Item_ADD(itemRepo, "02502094", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.KK UNIFICATA FALDE COMB.CM70");
            Item_ADD(itemRepo, "02502870", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.CM90 VETRO PIANO-CURVO");
            Item_ADD(itemRepo, "02502871", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.CM60 VETRO PIANO-CURVO");
            Item_ADD(itemRepo, "02502872", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BASE IMBALLO CM90 VETRO PIANO-CURVO");
            Item_ADD(itemRepo, "02502873", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PROTEZIONE ANTERIORE IMBALLO CM90 VETRO");
            Item_ADD(itemRepo, "02502874", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PROTEZ. ANT. IMB. CM60 VETRO PIANO-CURVO");
            Item_ADD(itemRepo, "02502875", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PROT.LATERALE IMB.CM90 VETRO PIANO-CURVO");
            Item_ADD(itemRepo, "02502876", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PROTEZIONE CAMINI IMBALLO CM90");
            Item_ADD(itemRepo, "02502877", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BASE IMBALLO CM60 VETRO PIANO-CURVO");
            Item_ADD(itemRepo, "02502878", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PROT.LATERALE IMB.CM60 VETRO PIANO-CURVO");
            Item_ADD(itemRepo, "02502879", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PROTEZIONE CAMINI IMBALLO CM60");
            Item_ADD(itemRepo, "02502952", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PACKAGING LATERAL PART DELTA-RHO 60");
            Item_ADD(itemRepo, "02502953", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PACKAGING REAR PROTECTION DELTA-RHO");
            Item_ADD(itemRepo, "02502959", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PACKAGING BOX DELTA-RHO 90");
            Item_ADD(itemRepo, "02502960", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PACKAGING LATERAL PART DELTA-RHO 90");
            Item_ADD(itemRepo, "03202108", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFILO PVC \"U\"");
            Item_ADD(itemRepo, "03202135", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFILO PVC MORBIDO  M126");
            Item_ADD(itemRepo, "03202286", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PASSACAVO  OCB-500 DIAM.8 3030813C");
            Item_ADD(itemRepo, "03202287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PASSACAVO DIAM.13 PGSB-1519A");
            Item_ADD(itemRepo, "03202288", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PASSACAVO DIAM.20 PGSB-9A");
            Item_ADD(itemRepo, "03290499", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA KABLA  12061         AE");
            Item_ADD(itemRepo, "03291043", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "STAF.FERMACAVO K186 86163 AE");
            Item_ADD(itemRepo, "03292017", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA KABLA ZASILANIA AE  US032");
            Item_ADD(itemRepo, "03292018", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI KABLA ZASILANIA  AE US033");
            Item_ADD(itemRepo, "03292020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FERMAC.SCATOLA IMP. AE US022");
            Item_ADD(itemRepo, "03292099", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "RIDUZIONE D.150-125 AE 09048");
            Item_ADD(itemRepo, "03292495", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SCAT.PROT.COND.425  AE  163051");
            Item_ADD(itemRepo, "03292668", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PROLUNGA DEFL.FIL.K2000 SLIM AE  163215");
            Item_ADD(itemRepo, "03293091", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA PC 170X70");
            Item_ADD(itemRepo, "03293141", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "RACCORDO D150 SERVICE");
            Item_ADD(itemRepo, "03293220", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SUPP.FI.P-.A PP COPO+20%TALCO V0 NERO");
            Item_ADD(itemRepo, "03293229", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COVER BOX P.A NILAMID");
            Item_ADD(itemRepo, "03293234", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO AGGIUNTIVO BUSS.NILAMID");
            Item_ADD(itemRepo, "03293349", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BUTTON GUIDE NEW PUSH BOTTON EVEREL");
            Item_ADD(itemRepo, "03293357", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BUTTON NEW PUSH BUTTON EVEREL");
            Item_ADD(itemRepo, "03293360", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BRACKET NEW PU EVEREL GLASS HOOD");
            Item_ADD(itemRepo, "03293366", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COVER FRONT BETA PLUS");
            Item_ADD(itemRepo, "03293386", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "UI VISION HOUSING BOX NP");
            Item_ADD(itemRepo, "03293393", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COVER FRONT BETA PLUS ZANUSSI");
            Item_ADD(itemRepo, "03293455", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "TWIST LOCK TL-600");
            Item_ADD(itemRepo, "03295264", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "FLANGIA D150 K2000");
            Item_ADD(itemRepo, "03295567", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "DEFLET.FILTRANTE 2000 APERTO");
            Item_ADD(itemRepo, "03295619", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BUTTON GUIDE  PP V2");
            Item_ADD(itemRepo, "03300529", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO CA RE 240X193X10 (141 TYPE5X4A)");
            Item_ADD(itemRepo, "03300552", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "N.1 FILTRO CA RE 240X193X10+TERMORET.");
            Item_ADD(itemRepo, "04000245C", "9103", null, null, ItemTypeEnum.BuyedItem, null, "PAINT RAL7022  EPOXER NEW CAT.");
            Item_ADD(itemRepo, "04011584", "9103", null, null, ItemTypeEnum.BuyedItem, null, "PRIMER 3M(TM) Adhesion Promoter 111");
            Item_ADD(itemRepo, "04302175", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "MASCHERA FORATURA BETA");
            Item_ADD(itemRepo, "04302181", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "DRILLING JIG T-GLASS");
            Item_ADD(itemRepo, "04307183", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.K18-K19 NEUTRO SENZA NORME");
            Item_ADD(itemRepo, "04308296", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA SMEG AUSTRALIA");
            Item_ADD(itemRepo, "04308297", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO NORME SMEG AUSTRALIA");
            Item_ADD(itemRepo, "04308383", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO .ISTR .K-BASSE  PIATT.A-C-D");
            Item_ADD(itemRepo, "04308458", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA M-SYSTEM");
            Item_ADD(itemRepo, "03115184", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PORTAPLAF.K22/224 V.2009 PC  90 BL1");
            Item_ADD(itemRepo, "03115191", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARCASSA K19L  V.2009 BL1  90");
            Item_ADD(itemRepo, "03115198", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PANNELLO ANTER.V2009  NEUTRO  90 BL1");
            Item_ADD(itemRepo, "03115567", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K SIGMA P-A COM. S 2000 W2");
            Item_ADD(itemRepo, "03119116", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K BETA A2 PU W2 90 (F)");
            Item_ADD(itemRepo, "03119117", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K BETA A2 PU BL1 90 (F)");
            Item_ADD(itemRepo, "03119123", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "STAFFA COMANDI BETA T BA/PU W2 (F)");
            Item_ADD(itemRepo, "03119124", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "STAFFA COMANDI BETA T BA/PU BL1 (F)");
            Item_ADD(itemRepo, "03119146", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K BETA T BA C2 H6 BL1 90 (F)");
            Item_ADD(itemRepo, "03119161", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA PLUS PU W2 60 (SUPPLIER)");
            Item_ADD(itemRepo, "03119162", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA PLUS PU BL1 60 (SUPPLIER)");
            Item_ADD(itemRepo, "03119199", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.THETA ANTRANERO 90");
            Item_ADD(itemRepo, "03119200", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SQUADRETTA COM. THETA ANTRANERO");
            Item_ADD(itemRepo, "03119203", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.THETA ANTRANERO 60");
            Item_ADD(itemRepo, "03119217", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BETA PLUS CONTROL BRACKET W2 (SUPPLIER)");
            Item_ADD(itemRepo, "03119218", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BETA PLUS CONTROL BRACKET BL1 (SUPPLIER)");
            Item_ADD(itemRepo, "03119219", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA PLUS PU BL1 90 (SUPPLIER)");
            Item_ADD(itemRepo, "03145601", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PANNELLO ANTERIORE NEUTRO  120 MET.");
            Item_ADD(itemRepo, "03145602", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PANN.ANT.PORTAPLAF K19  120 MET.");
            Item_ADD(itemRepo, "03145737", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. PORTAPLAF.K22/224 PC 90 V.2009");
            Item_ADD(itemRepo, "03145755", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. PANNELLO ANTER. NEUT.V2009 90");
            Item_ADD(itemRepo, "03147109", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K BETA C2 PU ANTRANERO 60");
            Item_ADD(itemRepo, "03147183", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CARC.K BETA C2 PU ANTRANERO 90");
            Item_ADD(itemRepo, "03147184", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "STAFFA COMANDI K BETA C2 PU ANTRANERO");
            Item_ADD(itemRepo, "04308461", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR .M-SYSTEM K-BASSE  PIATT.A-C-D");
            Item_ADD(itemRepo, "04308613", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO ISTR.SAVO K SIGMA ASC EC FINL");
            Item_ADD(itemRepo, "04308624", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO ISTR.SAVO K SIGMA ASC EC SVED");
            Item_ADD(itemRepo, "04308645", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO SIGMA FINL.");
            Item_ADD(itemRepo, "04308647", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO TAU FINL.");
            Item_ADD(itemRepo, "04308680", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO GARANZIA FRANCIA NEUTRA");
            Item_ADD(itemRepo, "04308702", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO ZETA FINL.");
            Item_ADD(itemRepo, "04308707", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO RHO FINLANDESE");
            Item_ADD(itemRepo, "04308709", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO DELTA FINLANDESE");
            Item_ADD(itemRepo, "04308710", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO BETA FINLANDESE");
            Item_ADD(itemRepo, "04308717", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO MORA K-BASSE");
            Item_ADD(itemRepo, "04308730", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA MORA");
            Item_ADD(itemRepo, "04308731", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO CENTRI ASSISTENZA MORA");
            Item_ADD(itemRepo, "04308812", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI BETA JUNO");
            Item_ADD(itemRepo, "04308813", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO BETA JUNO");
            Item_ADD(itemRepo, "04308814", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI BETA ELECTROLUX");
            Item_ADD(itemRepo, "04308815", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO BETA ELECTROLUX");
            Item_ADD(itemRepo, "04308816", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI BETA FAURE");
            Item_ADD(itemRepo, "04308817", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO BETA FAURE");
            Item_ADD(itemRepo, "04308818", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI BETA ZANUSSI");
            Item_ADD(itemRepo, "04308819", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO BETA ZANUSSI");
            Item_ADD(itemRepo, "04308820", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI BETA PROGRESS");
            Item_ADD(itemRepo, "04308821", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI BETA ZANKER");
            Item_ADD(itemRepo, "04308873", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL T-GLASS ELECTROLUX");
            Item_ADD(itemRepo, "04308874", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL T-GLASS ELECTROLUX");
            Item_ADD(itemRepo, "04308879", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL T-GLASS ZANUSSI");
            Item_ADD(itemRepo, "04308880", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL T-GLASS ZANUSSI");
            Item_ADD(itemRepo, "04308882", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO ERRATA CORRIGE NEW TUBI K-BASSE");
            Item_ADD(itemRepo, "04308886", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL BETA GLASS ELECT.");
            Item_ADD(itemRepo, "04308887", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA GLASS ELECTROLUX");
            Item_ADD(itemRepo, "03293391", "", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO AGGIUNTIVO BUSS.NILAMID+FORO");
            Item_ADD(itemRepo, "04308894", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL T-GLASS FAURE");
            Item_ADD(itemRepo, "04308895", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL T-GLASS FAURE");
            Item_ADD(itemRepo, "04308896", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL T-GLASS AEG");
            Item_ADD(itemRepo, "04308897", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL T-GLASS AEG");
            Item_ADD(itemRepo, "04308904", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.SAVO THETA FINLANDESE");
            Item_ADD(itemRepo, "04308905", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.SAVO SIGMA FINLANDESE");
            Item_ADD(itemRepo, "04308911", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.SAVO THETA AAVA FINLANDESE");
            Item_ADD(itemRepo, "04308917", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL BETA PLUS ELECT.");
            Item_ADD(itemRepo, "04308927", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK DIETER KNOLL 801320966");
            Item_ADD(itemRepo, "04309980", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AD.MET.110X150 (1) 3M 7818 UL (SP-330");
            Item_ADD(itemRepo, "04309981", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AD.MET.28X15 (3) 3M 7872 UL (SP-330)");
            Item_ADD(itemRepo, "04309984", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA BIANCA  72X23 (1)");
            Item_ADD(itemRepo, "04309987", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA BIANCA 34X10 4(3)");
            Item_ADD(itemRepo, "04309990", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AD.MET.40X76 (1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04309991", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AD.MET.34X10 (2) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04310226", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA BOLLINO TERRA");
            Item_ADD(itemRepo, "04310889", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA POLYESTERE 300X80-GR250");
            Item_ADD(itemRepo, "04344114", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA REGISTER MY APPLIANCE LAMONA");
            Item_ADD(itemRepo, "04344888", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.IMB.RHO WICKES 942022088");
            Item_ADD(itemRepo, "04345423", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.IMB.DELTA WICKES 942022089");
            Item_ADD(itemRepo, "04345536", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TRASPARENT LABEL 80X59,7 mm");
            Item_ADD(itemRepo, "04399998", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CARTA PROTETTIVA K 1200(PRESTRAPPO 40CM)");
            Item_ADD(itemRepo, "04400531", "9103", null, null, ItemTypeEnum.BuyedItem, null, "TARGA DATI TECNICI SHI LEI K19(REF.K181)");
            Item_ADD(itemRepo, "04404110", "9103", null, null, ItemTypeEnum.BuyedItem, null, "TARGA DATI TECNICI SHI LEI BETA EP45");
            Item_ADD(itemRepo, "04808024", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8024");
            Item_ADD(itemRepo, "04808051", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8051");
            Item_ADD(itemRepo, "04808164", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8164");
            Item_ADD(itemRepo, "04808170", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING SHEET 8170");
            Item_ADD(itemRepo, "04808171", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING SHEET 8171");
            Item_ADD(itemRepo, "04808185", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8185");
            Item_ADD(itemRepo, "06102575", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS. COMANDO K7088 P-A EL");
            Item_ADD(itemRepo, "06102588", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO 0301-AC CLI SE 02300236");
            Item_ADD(itemRepo, "06102613", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO 0301-AC SE CLII");
            Item_ADD(itemRepo, "06102629", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.WIRES CONNEC.2 LEDS P.A JOINT N 22");
            Item_ADD(itemRepo, "06102645", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS. LAMPADE K24 EA P-C 28W");
            Item_ADD(itemRepo, "06102654", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.TRASF.KB140 L100 (02300821)");
            Item_ADD(itemRepo, "06102676", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.WIRES CONNEC.2 LEDS PHANT. N 2");
            Item_ADD(itemRepo, "06102686", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO 0301-AC CLI S.TAIWAN");
            Item_ADD(itemRepo, "06102691", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.TRASF.BETA EB (TRASF.02300861)");
            Item_ADD(itemRepo, "06102727", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS. COMANDO SERIE 2000 P-A EL BLU");
            Item_ADD(itemRepo, "06102772", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.LAMPADE K3139 PC 4W");
            Item_ADD(itemRepo, "06102783", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO IS ELUX EB25");
            Item_ADD(itemRepo, "06102784", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO IS BETA ELUX");
            Item_ADD(itemRepo, "06102787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LED K BETA");
            Item_ADD(itemRepo, "06102791", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO IS BETA EB MOTOR");
            Item_ADD(itemRepo, "06102793", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDI touch RED LIGHT");
            Item_ADD(itemRepo, "06102794", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO 1-2-3 VEL EB MOT");
            Item_ADD(itemRepo, "06102803", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.H2H PASSIVE");
            Item_ADD(itemRepo, "06102815", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.INT.BASC.ELX 2019");
            Item_ADD(itemRepo, "06102934", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDO K7088 NE1 BLEU P-A EL");
            Item_ADD(itemRepo, "06102956", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.PIATT. LIGHT EB");
            Item_ADD(itemRepo, "06102959", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDI touch BIANCO LIGHT");
            Item_ADD(itemRepo, "06102960", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.PIATT. LIGHT HF8");
            Item_ADD(itemRepo, "06102993", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE NEW SENSORE P-A");
            Item_ADD(itemRepo, "06103008", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDI STEEL TOUCH RoHS");
            Item_ADD(itemRepo, "06143527", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING ASS.GLASS EA-EC MOTOR");
            Item_ADD(itemRepo, "06143528", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING ASS.GLASS EB MOTOR");
            Item_ADD(itemRepo, "06143530", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING BETA GLASS");
            Item_ADD(itemRepo, "06143549", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING BETA GLASS 08080905");
            Item_ADD(itemRepo, "06145229", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E. PIATT.A SCHEDA STD LED,EB/EP");
            Item_ADD(itemRepo, "06145264", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E PIATT.A SCHEDA STD LED HF 800");
            Item_ADD(itemRepo, "08080892", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "AUX DRIVER 0-10V EXT-MOTOR");
            Item_ADD(itemRepo, "08080C012", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ACT CO 12TCH 12LWH 3+1V ON/OFF");
            Item_ADD(itemRepo, "08080C013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ACT CO 08TCH 8LWH 3+1V ON/OFF");
            Item_ADD(itemRepo, "08080C019", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ACT CO 12TCH 12LWH 3+1V ON/OFF BUZZER ZY");
            Item_ADD(itemRepo, "08087067", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.FILTR0 ANTIGR.ALL C/CA K2456 90");
            Item_ADD(itemRepo, "08087140", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.FILTRO ANTIGRASSO ALL.SP2000 90");
            Item_ADD(itemRepo, "08087519", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO MET. K2000L INOX 239X298 V2011");
            Item_ADD(itemRepo, "08087549", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.FILTRO ALL SP2000+PANN.CA 90 2011");
            Item_ADD(itemRepo, "08087679", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO 343,5X285");
            Item_ADD(itemRepo, "08087741", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO 343,5x250");
            Item_ADD(itemRepo, "08087749", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO INOX 343,5X285");
            Item_ADD(itemRepo, "08094395", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "FRONTAL BETA GLASS 60 ASS.02005464");
            Item_ADD(itemRepo, "08094397", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "FRONTAL BETA GLASS 90 ASS.02005463");
            Item_ADD(itemRepo, "08094438", "9103", null, null, ItemTypeEnum.BuyedItem, null, "FRONTAL BETA GLASS 60 ASS.02005482");
            Item_ADD(itemRepo, "08094440", "9103", null, null, ItemTypeEnum.BuyedItem, null, "FRONTAL BETA GLASS 90 ASS.02005483");
            Item_ADD(itemRepo, "08094477", "9103", null, null, ItemTypeEnum.BuyedItem, null, "FRONTAL BETA GLASS 90 ASS.02005501");
            Item_ADD(itemRepo, "08094478", "9103", null, null, ItemTypeEnum.BuyedItem, null, "FRONTAL BETA GLASS 60 ASS.02005500");
            Item_ADD(itemRepo, "08094538", "9103", null, null, ItemTypeEnum.BuyedItem, null, "FRONTAL BETA GLASS 90 ASS.02005505 ZANUS");
            Item_ADD(itemRepo, "12010951", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "FIR WOOD BOARD 30X30X965");
            Item_ADD(itemRepo, "801320244", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ZANUSSI");
            Item_ADD(itemRepo, "801320557", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "WARRANTY CARD EXTENDED 3Y ELEC.AUSTRALIA");
            Item_ADD(itemRepo, "867351149", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MAN.BETA GLASS LAMONA LAM2875-2876");
            Item_ADD(itemRepo, "867351150", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INST.MAN.BETA GLASS LAMONA LAM2875-2876");
            Item_ADD(itemRepo, "867351177", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL BETA PLUS ELECTROLUX");
            Item_ADD(itemRepo, "867351179", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL BETA GLASS AR.MARTIN");
            Item_ADD(itemRepo, "867351182", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA PLUS ELECTROLUX");
            Item_ADD(itemRepo, "867351183", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA GLASS ARTHUR MARTIN");
            Item_ADD(itemRepo, "867351192", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL BETA GLASS D.KNOLL");
            Item_ADD(itemRepo, "867351193", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA GLASS D.KNOLL");
            Item_ADD(itemRepo, "867351204", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA PLUS AEG");
            Item_ADD(itemRepo, "867351205", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL BETA PLUS AEG");
            Item_ADD(itemRepo, "867351251", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA X-PLUS FAURE");
            Item_ADD(itemRepo, "867351254", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA X-PLUS ZANUSSI");
            Item_ADD(itemRepo, "867351258", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA PLUS ZANUSSI");
            Item_ADD(itemRepo, "867351260", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA PLUS FAURE");
            Item_ADD(itemRepo, "867351267", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL BETA X-PLUS");
            Item_ADD(itemRepo, "867351269", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL BETA PLUS");
            Item_ADD(itemRepo, "867351272", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTAL.MANUAL BETA GLASS WESTINGHOUSE");
            Item_ADD(itemRepo, "867351273", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL BETA GLASS WESTINGHOUSE");
            Item_ADD(itemRepo, "R2300132", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KOSTKA ZACISKOWA 6X3 ELEM");
            Item_ADD(itemRepo, "02000069", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.4,8X38TL D.12 TCZ");
            Item_ADD(itemRepo, "02000069T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.4,8X38TL D.12 Z TORX T20");
            Item_ADD(itemRepo, "02000100", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.2,9X13TSPTC2PRZINC");
            Item_ADD(itemRepo, "02000118", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X9,5TLTCZIN-SP.SPE");
            Item_ADD(itemRepo, "02000118T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X9,5 TL ZIN SP.SPE TORX T20");
            Item_ADD(itemRepo, "02000124", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X13TC2PRZINC");
            Item_ADD(itemRepo, "02000129", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X9,5TCTCZINC-SP.SP");
            Item_ADD(itemRepo, "02000135", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.2,9X9,5FRTCZINSP-PA.FE");
            Item_ADD(itemRepo, "02000142", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X13-TPZ  PA.TRU.19050");
            Item_ADD(itemRepo, "02000166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X6 TL TC Z PUNT.SP");
            Item_ADD(itemRepo, "02000196", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACC.PVC 35X35 FILT.RED.SP0,02");
            Item_ADD(itemRepo, "02000209", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACC.POLYETHYLENE \"K\" 07070 SP0,04");
            Item_ADD(itemRepo, "02000212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCH.DOCUM.19X27 SP 0,07");
            Item_ADD(itemRepo, "02000222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "RIVETTO 3,2X9,5 TS ALLUMINIO");
            Item_ADD(itemRepo, "02000287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCHETTO PVC 70X100 SP.0,07");
            Item_ADD(itemRepo, "02000509", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFILO LD33/ADESIVO 7X3 GRIGIO");
            Item_ADD(itemRepo, "06002202U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 PLAS.EB40 1559.87");
            Item_ADD(itemRepo, "06002213U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 PLAS. BEST EA27 P-C");
            Item_ADD(itemRepo, "06002239U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2013 ALL  EB40 SCAT.COND.P-C");
            Item_ADD(itemRepo, "06002240U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 BEST SC.COND.EB40 PIAT.C");
            Item_ADD(itemRepo, "06002271U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CO.S2013 ALL EP40 SC CO OIL 120V PC");
            Item_ADD(itemRepo, "06002283", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "HF 800 Conv.Plast.Caricato S2011+Cutoff");
            Item_ADD(itemRepo, "06002284", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "HF 600 Conv.Plast.S2011cond.4mF + Cutoff");
            Item_ADD(itemRepo, "06002298U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CO.S2013ALL EP450003.2 SC CO OIL120V");
            Item_ADD(itemRepo, "06002303", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 PLAS. BEST EB25+COND.");
            Item_ADD(itemRepo, "06002312", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 PLAS.BEST EB25+COND+C.OFF");
            Item_ADD(itemRepo, "06002313U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.S2011 PLAS.EC20 2.3.1.05+COND");
            Item_ADD(itemRepo, "06002314", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "HF 800.1 Conv.Plast.S2011cond.5mF+Cutoff");
            Item_ADD(itemRepo, "02000605", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FERMACAVO ADES. 72-0256-9200");
            Item_ADD(itemRepo, "02000700", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASSELLI A MURO D. 8");
            Item_ADD(itemRepo, "02000712", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FASCETTA RAPIDA NATURAL 98mm RG203");
            Item_ADD(itemRepo, "02000726", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "DADO 4MX7 OTTONE");
            Item_ADD(itemRepo, "02000731", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ROND.DENTELLATA D.4,3");
            Item_ADD(itemRepo, "02001106", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESP.LA.AGG.PZ      90X60X32");
            Item_ADD(itemRepo, "02001331", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESPANSO IMBALLO KK35    154028");
            Item_ADD(itemRepo, "02001387", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESPANSO KK UNIFICATO SX 163064");
            Item_ADD(itemRepo, "02001388", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESPANSO KK UNIFICATO DX 163065");
            Item_ADD(itemRepo, "02011020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.UNIV.SPAX 4,2X15 TCTC ZN");
            Item_ADD(itemRepo, "02011110", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X12 TCTC ZIGR. OTTONE");
            Item_ADD(itemRepo, "02011358", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SACCH.PVC  SP0,015  1000X600 FS340");
            Item_ADD(itemRepo, "02011983T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X6 TL Z PUNT.SP TORX T20");
            Item_ADD(itemRepo, "02011986T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE 3,9X13 TORX T20");
            Item_ADD(itemRepo, "02011988", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TIMESTRIP");
            Item_ADD(itemRepo, "02300155", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT.1,5 SS CLII");
            Item_ADD(itemRepo, "02300260", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.2X0,75 MT.1,5 SE CLII");
            Item_ADD(itemRepo, "02300798", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FAR.AL.F&F ZIK7+CONN.6,3 L160 SAT.OSRAM");
            Item_ADD(itemRepo, "02300981", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "COPRICONNETTORE  POLIPROP V2 AE");
            Item_ADD(itemRepo, "02320387", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "REFLEKTOR LED BEST 3V 700MA 2,1W");
            Item_ADD(itemRepo, "02320389", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LED CANDLE E14 4W 230-240V");
            Item_ADD(itemRepo, "02502929", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.KK UNIF.60 FALDE COMB.ELUX");
            Item_ADD(itemRepo, "02502930", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.KK UNIF.90 FALDE COMB.ELUX");
            Item_ADD(itemRepo, "03201014", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GEMMA SPIA ROSSA 186PK 86129");
            Item_ADD(itemRepo, "03202442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "BLOCCACAVO ADIMPEX SR/SP1 SRB-R-3");
            Item_ADD(itemRepo, "03292499", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "GUIDACAVO A SCATTO AE   132171");
            Item_ADD(itemRepo, "03292631", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "RIDUZIONE 150/125 K2000 163094");
            Item_ADD(itemRepo, "03293222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "COPRICONNETTORE FEMMINA NILAMID");
            Item_ADD(itemRepo, "03293227", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOR.MESKA. ANTINTR.V0");
            Item_ADD(itemRepo, "03293297", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA PC 110X62");
            Item_ADD(itemRepo, "03294757", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA SP NEW 07");
            Item_ADD(itemRepo, "04306466", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "VOLANTINO ISTRUZIONE NORME CLI/CLII");
            Item_ADD(itemRepo, "08015789", "9176", null, null, ItemTypeEnum.BuyedItem, null, "K25 ASS.TUBO TEL.BL1");
            Item_ADD(itemRepo, "08016308", "9176", null, null, ItemTypeEnum.BuyedItem, null, "K25 ASS.TUBO TEL.XS304");
            Item_ADD(itemRepo, "04307442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FO_SMALTIMENTO WEEE DIRECTIVE CEE");
            Item_ADD(itemRepo, "04308400", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA BEST RUSSIA");
            Item_ADD(itemRepo, "04308692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO MATTONCINO POLISTIR.MOTORE HF800");
            Item_ADD(itemRepo, "04308772", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO A4");
            Item_ADD(itemRepo, "04308775", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS AEG 801320720");
            Item_ADD(itemRepo, "04308776", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ELECTROLUX 801320634");
            Item_ADD(itemRepo, "04308777", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ZANUSSI 801320510");
            Item_ADD(itemRepo, "04308778", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ZANKER 801320602");
            Item_ADD(itemRepo, "04308779", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD AEG 801320721");
            Item_ADD(itemRepo, "04308780", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ELECTROLUX 801320635");
            Item_ADD(itemRepo, "04308781", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ZANUSSI 801320644");
            Item_ADD(itemRepo, "04308782", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY AEG 801320722");
            Item_ADD(itemRepo, "04308783", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ELECTROLUX 801320636");
            Item_ADD(itemRepo, "04308784", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ZANUSSI 801320650");
            Item_ADD(itemRepo, "08092831", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARCASSA K19L v.2009  PU XS304 90");
            Item_ADD(itemRepo, "08093234", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARCASSA K19L  PU XS304 120");
            Item_ADD(itemRepo, "08093795", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.ZETA+VETRO SOLO SIMB. 60 XS/BL");
            Item_ADD(itemRepo, "08093926", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.BETA PU H.6 A2 XS 90");
            Item_ADD(itemRepo, "08093929", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.BETA PU H.6 A2 XS 60");
            Item_ADD(itemRepo, "08093979", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CARC. BETA PU H.6 XS 80");
            Item_ADD(itemRepo, "08093985", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.ZETA+VETRO BEST+SIMB. 90 XS/BL");
            Item_ADD(itemRepo, "08093987", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.ZETA+VETRO BEST+SIMB 60 XS/BL");
            Item_ADD(itemRepo, "08093988", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CARC.  THETA  XS 60   468046");
            Item_ADD(itemRepo, "08093989", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CARC. THETA  XS 80   468047");
            Item_ADD(itemRepo, "08093990", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CARC. THETA  XS 90   468048");
            Item_ADD(itemRepo, "08094011", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CARC. BETA PU H.10 XS 90 468011");
            Item_ADD(itemRepo, "08094022", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.ZETA+VETRO SOLO SIMB. 90 XS/BL");
            Item_ADD(itemRepo, "08094023", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.ZETA+VETRO SOLO SIMB. 90 XS/WH");
            Item_ADD(itemRepo, "08094027", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CARC. BETA PU C2 H.6 XS 60");
            Item_ADD(itemRepo, "08094029", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CARC. BETA PU C2 H.6 XS 90");
            Item_ADD(itemRepo, "08094056", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CARC. BETA SALD. PU XS 60");
            Item_ADD(itemRepo, "08094093", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.THETA NE1 XS 60");
            Item_ADD(itemRepo, "08094094", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.THETA NE1 XS 70");
            Item_ADD(itemRepo, "08094095", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.THETA NE1 XS 90");
            Item_ADD(itemRepo, "08094102", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.DELTA PU XS");
            Item_ADD(itemRepo, "08094103", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.SIGMA ST XS");
            Item_ADD(itemRepo, "08094104", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.RHO PU XS");
            Item_ADD(itemRepo, "08094123", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.BETA PU A2 H.6 XS304 90");
            Item_ADD(itemRepo, "08094134", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.TAU EL XS");
            Item_ADD(itemRepo, "08094135", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.SIGMA EL XS");
            Item_ADD(itemRepo, "08094212", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.STAFFA COMANDI BASCULANTI ELX");
            Item_ADD(itemRepo, "08094230", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES BETA ASSEMBLY+ DIVERTER XS");
            Item_ADD(itemRepo, "08094253", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.ZETA+VETRO MORA+SIMB.60 XS/BL");
            Item_ADD(itemRepo, "08094254", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.ZETA+VETRO MORA+SIMB.90 XS/BL");
            Item_ADD(itemRepo, "08094331", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.BETA T PU H.6 A2 XS 60");
            Item_ADD(itemRepo, "08094332", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.BETA T PU H.6 A2 XS 90");
            Item_ADD(itemRepo, "08094334", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.BETA T BA C2 H.6 XS 60");
            Item_ADD(itemRepo, "08094335", "9140", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CARC.BETA T BA C2 H.6 XS 90");
            Item_ADD(itemRepo, "08094373", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS DELTA PU XS ASSEMBLY");
            Item_ADD(itemRepo, "08094375", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS RHO PU XS ASSEMBLY");
            Item_ADD(itemRepo, "08094391", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA PLUS PU XS 90 ASSEMBLY");
            Item_ADD(itemRepo, "08094393", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA PLUS PU XS 60 ASSEMBLY");
            Item_ADD(itemRepo, "04308785", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ELECTROLUX 801320578  S");
            Item_ADD(itemRepo, "04308786", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK AEG 801320684  P");
            Item_ADD(itemRepo, "08094411", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA GLASS XS 60 ASSEMBLY");
            Item_ADD(itemRepo, "08094413", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA GLASS XS 90 ASSEMBLY");
            Item_ADD(itemRepo, "04308787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ZANUSSI 801320243  Y");
            Item_ADD(itemRepo, "04308788", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON 801320204  T");
            Item_ADD(itemRepo, "08094441", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA GLASS XS 60 ASSEMBLY");
            Item_ADD(itemRepo, "08094442", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA GLASS XS 90 ASSEMBLY");
            Item_ADD(itemRepo, "04308841", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "WARRANTY RUSSO AEG ELEC.ZANU.801320556 D");
            Item_ADD(itemRepo, "04308844", "9103", null, null, ItemTypeEnum.BuyedItem, null, "CERTIFICATO RUSSO ELECTR.867351004-5189");
            Item_ADD(itemRepo, "08094529", "9140", null, null, ItemTypeEnum.BuyedItem, null, "LATERAL SIDE XS 85mm+BUBBLE FOILE");
            Item_ADD(itemRepo, "08094530", "9140", null, null, ItemTypeEnum.BuyedItem, null, "LATERAL SIDE XS 135mm+BUBBLE FOILE");
            Item_ADD(itemRepo, "04308845", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERTIFICATO RUSSO ELECTR. 867351005-5190");
            Item_ADD(itemRepo, "08094539", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA GLASS XS 90 ASS.ZANUSSI");
            Item_ADD(itemRepo, "08094567", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA PLUS PU XS 90 ASS.ZANUSSI");
            Item_ADD(itemRepo, "08094572", "9140", null, null, ItemTypeEnum.BuyedItem, null, "CHASSIS BETA PLUS PU XS 60 ASS.ZANUSSI");
            Item_ADD(itemRepo, "08095924", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS ZANUSSI+DIV+KART 60");
            Item_ADD(itemRepo, "08095925", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS ZANUSSI+DIV+KART 90");
            Item_ADD(itemRepo, "08095926", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS ELECTR.+DIV+KART 90");
            Item_ADD(itemRepo, "08095927", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES TUB T-SHAPE XS ELECTROLUX+DIV");
            Item_ADD(itemRepo, "08095928", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS FAURE+DIV+KART 90");
            Item_ADD(itemRepo, "08095929", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS AEG+DIV+KART 60");
            Item_ADD(itemRepo, "08095930", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE XS AEG+DIV");
            Item_ADD(itemRepo, "08095931", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE XS A.MART+DIV");
            Item_ADD(itemRepo, "08095932", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE XS D.KNOLL+DIV");
            Item_ADD(itemRepo, "08095933", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS+DIV+KARTON 60");
            Item_ADD(itemRepo, "08095934", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS+DIV+KARTON 90");
            Item_ADD(itemRepo, "08095935", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE XS+DIV");
            Item_ADD(itemRepo, "08095936", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE XS JUNO OLD+DIV");
            Item_ADD(itemRepo, "08095937", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD XS ELECTROLUX+DIV");
            Item_ADD(itemRepo, "08095938", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD XS FAURE+DIV");
            Item_ADD(itemRepo, "08095939", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD XS ZANUSSI+DIV");
            Item_ADD(itemRepo, "08095940", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD XS PROGRESS+DIV");
            Item_ADD(itemRepo, "08095941", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD XS ZANKER+DIV");
            Item_ADD(itemRepo, "08095943", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD XS304+DEFLEKTOR");
            Item_ADD(itemRepo, "08095954", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD W2 ELECTROLUX+DIV");
            Item_ADD(itemRepo, "08095955", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE W2 ELECTROLUX+DIV");
            Item_ADD(itemRepo, "08095956", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD BL1 ELECTROLUX+DIV");
            Item_ADD(itemRepo, "08095957", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE BL1 ELECTROLUX+DIV");
            Item_ADD(itemRepo, "08095958", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD W2");
            Item_ADD(itemRepo, "08095959", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-SHAPE OLD ANTRANERO+DIV");
            Item_ADD(itemRepo, "08095962", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES K24 OLD XS + DIV+ F.WEG.");
            Item_ADD(itemRepo, "08095978", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES BETA ASSEMBLY XS");
            Item_ADD(itemRepo, "08095981", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS FAURE+DIV");
            Item_ADD(itemRepo, "08095982", "9140", null, null, ItemTypeEnum.BuyedItem, null, "FLUES T-GLASS XS WESTINGHOUSE+DIVERTER");
            Item_ADD(itemRepo, "04308848", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON OEM-GB 801320605 Q");
            Item_ADD(itemRepo, "04308859", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.TIMESTRIP FAURE");
            Item_ADD(itemRepo, "04308868", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ELECTR. 2Y 801320639");
            Item_ADD(itemRepo, "04308869", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ELECTR. UK 801320640");
            Item_ADD(itemRepo, "04308870", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ELECTROLUX 2Y  801320638");
            Item_ADD(itemRepo, "04308921", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERT.RUSSO ELECTR.RUC-SE.HA21.B.00612/19");
            Item_ADD(itemRepo, "04309965", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AD.MET.74X74 (1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04309976", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.ADES.POLIESTER AD.PER 5x2,5");
            Item_ADD(itemRepo, "04309982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA BIANC.100X300(1)");
            Item_ADD(itemRepo, "04309989", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA BIANCA 76X40 (1)");
            Item_ADD(itemRepo, "04344584", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ?").Id, ItemTypeEnum.BuyedItem, null, "TARGA ANTITACCHEGGIO WICKES");
            Item_ADD(itemRepo, "04399901", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CARTA PROT.K60   1200X800");
            Item_ADD(itemRepo, "04399902", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CARTA PROT.K90   1200X1200");
            Item_ADD(itemRepo, "04404512", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING TURCHIA NEUTRA");
            Item_ADD(itemRepo, "04405212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING UKRAINA");
            Item_ADD(itemRepo, "04405395", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ENERGY LABELLING A+++ NEUTRA");
            Item_ADD(itemRepo, "04500000", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NASTRO ADESIVO  60X48");
            Item_ADD(itemRepo, "04500037", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAST.ADES.AVVERT.PERICOLO 54m");
            Item_ADD(itemRepo, "06103049", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS. LAMPADE K24 EA P-C 4W");
            Item_ADD(itemRepo, "08087539", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ALL.MAN.C/MAN.K24 2011 239X298");
            Item_ADD(itemRepo, "08087766", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO WIN 343,5X285");
            Item_ADD(itemRepo, "E3251953", "9140", null, null, ItemTypeEnum.BuyedItem, null, "TR.DISTAN.SUPP.TUBO P.A");
            Item_ADD(itemRepo, "E3252124", "9140", null, null, ItemTypeEnum.BuyedItem, null, "TR.STAFFA VERS.MAKE UP AIR DAMPER");
            Item_ADD(itemRepo, "E3343147", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SQUADR.SUPP.TUBO ELZ 163069");
            Item_ADD(itemRepo, "E3346574", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SQUADRETTA COM.K BETA H.10 PU  468026");
            Item_ADD(itemRepo, "E3346606", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SQUADRETTA COM.K BETA H.6 PUC2 468010");
            Item_ADD(itemRepo, "E3346692", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.COP.BUSSOLOTTO .K18 ECO   ELZ");
            Item_ADD(itemRepo, "E3347866", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SQUADRETTA COM.K BETA H.6 PU  468025");
            Item_ADD(itemRepo, "E3347869", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA COM. DELTA - RHO  PU 471004");
            Item_ADD(itemRepo, "E3347871", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SQUADRETTA COM.ZETA  468035");
            Item_ADD(itemRepo, "E3347872", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SQUADRETTA COM. THETA  468034");
            Item_ADD(itemRepo, "E3347873", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA COM. SIGMA PU 471006");
            Item_ADD(itemRepo, "E3347877", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA COM. K7388-LUNA P-A PU  471010");
            Item_ADD(itemRepo, "E3351779", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SUPP.BRACKET BOX BLOWER KK ELZ");
            Item_ADD(itemRepo, "E3351827", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SCHIEN.BUSS. PIATTAFORMA A ELZ");
            Item_ADD(itemRepo, "E3351828", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.FASCIA BUSS.PIATT. A ELZ");
            Item_ADD(itemRepo, "E3351830", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA FISS.CONV. PIATT.A");
            Item_ADD(itemRepo, "E3351831", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA A MURO. PIATT.A");
            Item_ADD(itemRepo, "E3352125", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA FISS.SCAT. I.E. P.A.");
            Item_ADD(itemRepo, "E3352327", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.NEW BUSSOLOTTO .K19 S-2000   ELZ");
            Item_ADD(itemRepo, "E3352755", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA COMANDI S2000");
            Item_ADD(itemRepo, "E3352808", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SUPP.PIASTRA COPRI CONNETTORI P-A");
            Item_ADD(itemRepo, "E3352809", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.PIASTRA COPRI CONNETTORI P-A");
            Item_ADD(itemRepo, "E3353159", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.COVER BOX BLOWER P-A EC ELZ");
            Item_ADD(itemRepo, "E3353160", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.BRACKET BLOWER/SCAT.COND P-A ELZ");
            Item_ADD(itemRepo, "E3353185", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAMPO PARTE INTERNA CAPPE VETRO ELZ");
            Item_ADD(itemRepo, "E3353245", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SQUADRETTA COM.K BETA H.6 PU XS304");
            Item_ADD(itemRepo, "E3353600", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.PROTEZIONE INT.BA");
            Item_ADD(itemRepo, "E3353602", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA CAMINI T-SHAPE SKINPASSATO");
            Item_ADD(itemRepo, "E3353615", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.STAFFA COMANDI BETA T BA/PU XS");
            Item_ADD(itemRepo, "E3353719", "9140", null, null, ItemTypeEnum.BuyedItem, null, "BEND.LOWER FLUE BETA XS");
            Item_ADD(itemRepo, "E3353720", "9140", null, null, ItemTypeEnum.BuyedItem, null, "BEND.UPPER FLUE BETA XS");
            Item_ADD(itemRepo, "E3353752", "9140", null, null, ItemTypeEnum.BuyedItem, null, "BEND.ELLE BG");
            Item_ADD(itemRepo, "E3353753", "9140", null, null, ItemTypeEnum.BuyedItem, null, "BEND.MOTOR BOX BRACKET BG");
            Item_ADD(itemRepo, "E3353754", "9140", null, null, ItemTypeEnum.BuyedItem, null, "BEND.CONTROL BRACKET BG");
            Item_ADD(itemRepo, "E3353757", "9140", null, null, ItemTypeEnum.BuyedItem, null, "BEND.BETA PLUS CONTROL BRACKET XS");
            Item_ADD(itemRepo, "E3353964", "9140", null, null, ItemTypeEnum.BuyedItem, null, "BEND.BETA PLUS CONTROL BRACKET XS ZAN");
            Item_ADD(itemRepo, "E3354722", "9140", null, null, ItemTypeEnum.BuyedItem, null, "PI.SQUADRETTA COM.THETA NE1");
            Item_ADD(itemRepo, "08088362", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ASS.FLANG.VR.D150 K2000 163610");
            Item_ADD(itemRepo, "003200588", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO LUCE ES5 ELX");
            Item_ADD(itemRepo, "003200589", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO ON/OFF+1V MOTORE ES5 ELX");
            Item_ADD(itemRepo, "003200590", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO 2V MOTORE ES5 ELX");
            Item_ADD(itemRepo, "003200591", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASTO 3V MOTORE ES5 ELX");
            Item_ADD(itemRepo, "003200592", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ROTATIVE CONTROL PUSH PULL WITH SIMB.ITR");
            Item_ADD(itemRepo, "004202547", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD LED SUPERSILENT DIS. N 2");
            Item_ADD(itemRepo, "004202554", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD UZIEMIENIA DIS. N 4");
            Item_ADD(itemRepo, "004203453", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD LAMP HALOGEN. N 2 DIS.N 15");
            Item_ADD(itemRepo, "004203524", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD SILNIKA PIAT.LIGHT DIS.N 6");
            Item_ADD(itemRepo, "004203807", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD LED WITCH DIS. 2");
            Item_ADD(itemRepo, "004203808", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD MICROSWITCH  WITCH DIS. 3");
            Item_ADD(itemRepo, "004203860", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD LAMP.2 HALOGENY VISOR DIS N 9");
            Item_ADD(itemRepo, "004203861", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZEDLUZKA. PRZEW. MICRO. VISOR DIS N 10");
            Item_ADD(itemRepo, "004203978", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "WIRING CONN.BLOWER WITCH ECOD.DIS.35");
            Item_ADD(itemRepo, "004260287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD SILNIKA 413 1M 55/60CM");
            Item_ADD(itemRepo, "004260295", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD SILNIKA 413 1M 90");
            Item_ADD(itemRepo, "02000025", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,9X12TSPTCN-SP.SPE");
            Item_ADD(itemRepo, "02000031", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X5 TSP TC N");
            Item_ADD(itemRepo, "02000034", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 3,9X16TSPTCN2PR");
            Item_ADD(itemRepo, "02000035", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X6 TC TC ZIG N");
            Item_ADD(itemRepo, "02000039", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 3,4X15TSPTC OCYNK.PHILLIPS");
            Item_ADD(itemRepo, "02000069", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 4,8X38 TLTC Z");
            Item_ADD(itemRepo, "02000070", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 4,2X32TLTCTP/Z");
            Item_ADD(itemRepo, "02000071", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET AU.4,2X32TLTCZD12MM");
            Item_ADD(itemRepo, "02000071T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.4,2X32 TL Z D12MM TORX T20");
            Item_ADD(itemRepo, "02000097", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 3,5X13TLTCZIN-SP.SPEC");
            Item_ADD(itemRepo, "02000100", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X13TSPTC2PRZINC");
            Item_ADD(itemRepo, "02000112", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X9,5TSPTCZIN-SP.S.PF");
            Item_ADD(itemRepo, "02000117", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,9X9,5 TSP TC N S");
            Item_ADD(itemRepo, "02000118", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA 3,9x9,5 TSP TC N SP");
            Item_ADD(itemRepo, "02000118T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X9,5 TL ZIN SP.SPE TORX T20");
            Item_ADD(itemRepo, "02000124", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,2X13TC2PRZINC");
            Item_ADD(itemRepo, "02000129", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.3,2X9,5TCTCZINC-SP.SP");
            Item_ADD(itemRepo, "02000135", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT. 2,9X9,5FRTCZINSP-PA.FE");
            Item_ADD(itemRepo, "02000137", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X15 TC TC ZINC.");
            Item_ADD(itemRepo, "02000140", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,2X9,5 TC TC N");
            Item_ADD(itemRepo, "02000142", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.3,2X13-TPZ  PA.TRU.19050");
            Item_ADD(itemRepo, "02000146", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M5X50 TLTC ZINC.");
            Item_ADD(itemRepo, "02000153", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 2,9X6,5TSPTC ZIN.SP.S-PF");
            Item_ADD(itemRepo, "02000154", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,2X16TCTC ZINC.2PR");
            Item_ADD(itemRepo, "02000164", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X4 TSPEC TC ZIG.N US597");
            Item_ADD(itemRepo, "02000166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT. 3,9X6 TL TC Z PUNT.SP");
            Item_ADD(itemRepo, "02000167", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4x8 TSTC OCYNK");
            Item_ADD(itemRepo, "02000178", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X20 TL TC ZN");
            Item_ADD(itemRepo, "02000196", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC 30x35 FILTR.RED. GRUB 0,02");
            Item_ADD(itemRepo, "02000202", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK 16x19 SP 0,07 12056");
            Item_ADD(itemRepo, "02000209", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK FOLIOWY \"K\" 07070 SP0,04");
            Item_ADD(itemRepo, "02000212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WORECZEK FOLIOWY 19X27 GRUB. 0,07");
            Item_ADD(itemRepo, "02000222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NIT 3,2X9,5 TS ALUMINIUM");
            Item_ADD(itemRepo, "02000287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC 70x100 GRUB.0,07");
            Item_ADD(itemRepo, "02000311", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LOZYSKO KULKOWE \"S3\" 03024");
            Item_ADD(itemRepo, "02000354", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA PODWOJNA ZEBATA . 4X15 N");
            Item_ADD(itemRepo, "02000509", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFIL LD33/PRZYLEPNY 7X3 SZARY");
            Item_ADD(itemRepo, "02000605", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "UCHWYT KABLA PRZYLEPNY WCK-460-01A4-RT");
            Item_ADD(itemRepo, "02000609", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FOLIA BABELKOWA  900");
            Item_ADD(itemRepo, "02000700", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "KOLEK ROZPOROWY D.8");
            Item_ADD(itemRepo, "020007050", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GUM. IZOL. PRZEPL.POWIET.CN24/A15X6 SE30");
            Item_ADD(itemRepo, "02000707", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFIL SAMOPRZYLEPNY K630/A BIALY  5X5");
            Item_ADD(itemRepo, "02000712", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OPASKA ZACISKOWA NATURALNA 98mm RG-203");
            Item_ADD(itemRepo, "02000715", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PIANKA SAMOPRZYLEPNA OKRAGLA FI 13MM.");
            Item_ADD(itemRepo, "02000721", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA M4X7 ZELAZO Z.");
            Item_ADD(itemRepo, "02000726", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA  4MX7 MOSIADZ");
            Item_ADD(itemRepo, "02000730", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA POD MNOTAZ DMUCHAWY. US");
            Item_ADD(itemRepo, "02000731", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA ZEBATA Sred.4,3");
            Item_ADD(itemRepo, "02000736", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA PLASKA  SRED.D.5,2X15");
            Item_ADD(itemRepo, "02000749", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZAWÓR ZWROTNY ALUM.  123");
            Item_ADD(itemRepo, "02000756", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZAWOR ZWROTNY ALUMINIUM SRED.150");
            Item_ADD(itemRepo, "02000776", "9103", null, null, ItemTypeEnum.BuyedItem, null, "SZYBA PLAFON.413        145038");
            Item_ADD(itemRepo, "02000779", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SZYBA PLAFON.413 45/50  145068");
            Item_ADD(itemRepo, "02000789", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SZYBA CARR.TRASP.415 60 145048");
            Item_ADD(itemRepo, "02000790", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SZYBA 415 90 145142");
            Item_ADD(itemRepo, "02000811", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OGRANICZNIK FILTRA PAN.C.A.413 145385");
            Item_ADD(itemRepo, "02000886", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SZYBA CARR.TR.97/413 55 145315");
            Item_ADD(itemRepo, "02000887", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SZYBA CARR.TR.97/413 60 145316");
            Item_ADD(itemRepo, "02001150", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ANGOLARE ESPANSO IS3 31033");
            Item_ADD(itemRepo, "02001315", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SEPARATOR STYROP. 580X400X50     47053");
            Item_ADD(itemRepo, "02001344", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BOK STYROPIANOWY 413  PRAWY   145104");
            Item_ADD(itemRepo, "02001345", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BOK STYROPIANOWY 413  LEWY   145103");
            Item_ADD(itemRepo, "02001355", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PODSTAWA STYROPIANOWA K-DRIADE");
            Item_ADD(itemRepo, "02001375", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "STYROPIAN OPAK.425          30083");
            Item_ADD(itemRepo, "02001409", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "NAROZNIK STYROPIANOWY");
            Item_ADD(itemRepo, "02001416", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ESPANSO BASE K7588");
            Item_ADD(itemRepo, "02001419", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PODSTAWA STYROPIANOWA P195 2M USA CM.72");
            Item_ADD(itemRepo, "02001483", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "STYROPIAN POD SZYBE K9087");
            Item_ADD(itemRepo, "02001540", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "STYROPIAN WYPELNIAJACY ES23  50");
            Item_ADD(itemRepo, "02001628", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "LATERAL STYROFOAM SKLOK");
            Item_ADD(itemRepo, "02005319", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SZYBA VISIERA GHOST 90 (L=815)");
            Item_ADD(itemRepo, "02005320", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SZYBA VISIERA GHOST 60 (L=515)");
            Item_ADD(itemRepo, "02005321", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SZYBA VISIERA GHOST 120 (L=1115)");
            Item_ADD(itemRepo, "02005456", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "GLASS DRAWER SKLOK 60");
            Item_ADD(itemRepo, "02005457", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "GLASS DRAWER SKLOK 90");
            Item_ADD(itemRepo, "02010004", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POLIETIL.BOLLATO  450");
            Item_ADD(itemRepo, "02010531", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "IZOLACJA PIAN.SAMOPR.+FILM 400x280x15");
            Item_ADD(itemRepo, "02011019", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA Z PODKLADKA ZEBATA M4X9,8X4,4");
            Item_ADD(itemRepo, "02011020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.UNIW.SPAX 4,2X15 TCTC ZN");
            Item_ADD(itemRepo, "02011021", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 2,9X19 TSP TC ZN SP");
            Item_ADD(itemRepo, "02011097", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X30 TC TC ZINC.");
            Item_ADD(itemRepo, "02011110", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X12 TCTC RADELK.MOSIADZ");
            Item_ADD(itemRepo, "02011114", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA AD ALETTE M5");
            Item_ADD(itemRepo, "02011115", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M5X20 TC T ZINC.");
            Item_ADD(itemRepo, "02011117", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA PODWOJNA ZEBATA 5,5x16 n");
            Item_ADD(itemRepo, "02011139", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SPREZYNA.RUCH.SZYBY SP2000   163272");
            Item_ADD(itemRepo, "02011162", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA NYLON D15X5,5X1");
            Item_ADD(itemRepo, "020111650", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LOZYSKO KULKOWE H.270 FC (FORATA)");
            Item_ADD(itemRepo, "02011203", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWIN.M4X15,6 SPIRALFORM 050015");
            Item_ADD(itemRepo, "02011225", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OGRANICZNIK FILTRA PAN.C.A.412");
            Item_ADD(itemRepo, "02011301", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA KLATKOWA M5   USA RoHS");
            Item_ADD(itemRepo, "02011303", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WTYK M4 SZESCIOKATNY  09688-00413 RoHS");
            Item_ADD(itemRepo, "02011314", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LOGO SAMOPRZYL.BROAN ELITE CR/LUC.06750");
            Item_ADD(itemRepo, "02011358", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC SP0,015 1000x600 FS340");
            Item_ADD(itemRepo, "02011359", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC SP0,015 1200x700 FS340");
            Item_ADD(itemRepo, "02011492", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LAT. DX VISIERA MOBILE IN ZAMA NICH.");
            Item_ADD(itemRepo, "02011493", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LAT. SX VISIERA MOBILE IN ZAMA NICH.");
            Item_ADD(itemRepo, "02011494", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GUSCIO VISIERA MOBILE DX  NICHELATO");
            Item_ADD(itemRepo, "02011495", "9103", null, null, ItemTypeEnum.BuyedItem, null, "GUSCIO VISIERA MOBILE SX  NICHELATO");
            Item_ADD(itemRepo, "02011532", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "FRONT.CARR.415 GALV.INOX 90");
            Item_ADD(itemRepo, "02011580", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FRONTAL SKLOK GALV.INOX 60");
            Item_ADD(itemRepo, "02011581", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FRONTAL SKLOK GALV.INOX 90");
            Item_ADD(itemRepo, "02011801", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET AF 3,9X13 TCTC ZN KARBOWANY");
            Item_ADD(itemRepo, "02011976", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "MOTOR VALVE KSOM-125-0");
            Item_ADD(itemRepo, "02011977", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRÊTKA M 5  DIN 985 A2");
            Item_ADD(itemRepo, "02011979", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "RACCORDO VALVOLA SAVO BDM-1-D-012");
            Item_ADD(itemRepo, "020119791", "", null, null, ItemTypeEnum.BuyedItem, null, "105 VALV.SAVO - SCANTONATURA RACCORDO");
            Item_ADD(itemRepo, "02011980", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FLANGIA AGGANCIO VALVOLA SAVO KKT-125");
            Item_ADD(itemRepo, "020119801", "", null, null, ItemTypeEnum.BuyedItem, null, "105 VALV.SAVO - FORATURAFLANGIA AGGANCIO");
            Item_ADD(itemRepo, "02011982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZAWÓR MANUALNY SAVO KSO-100");
            Item_ADD(itemRepo, "02011983T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,9X6 TL Z PUNT.SP TORX T20");
            Item_ADD(itemRepo, "02011985T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X8 TB ZINC.TORX T20");
            Item_ADD(itemRepo, "02011986T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE 3,9X13 TORX T20");
            Item_ADD(itemRepo, "02011987", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "MUFA WKLEJ. NYL. M4 B=8/10MM 039.33.042");
            Item_ADD(itemRepo, "02011989", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VI.AU.3,2X13TC2PRZINC");
            Item_ADD(itemRepo, "02012003", "9103", null, null, ItemTypeEnum.BuyedItem, null, "LEVA MICRO 00568512");
            Item_ADD(itemRepo, "02300093", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZARÓWKA RUROWA OPAKOW. 230-240V 40W");
            Item_ADD(itemRepo, "02300126", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA             3x0,75 150cm");
            Item_ADD(itemRepo, "02300128", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA AUSTRALIA   3x0,75 160cm");
            Item_ADD(itemRepo, "02300130", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL ZASILANIA 3X0,75MT1,50 SS H05");
            Item_ADD(itemRepo, "02300136", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL BEZ WTYCZKI HO5       3x0,75 100cm");
            Item_ADD(itemRepo, "02300137", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA             3x0,75 100cm");
            Item_ADD(itemRepo, "02300155", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL BEZ WTYCZKI KLII      2x0,75 150cm");
            Item_ADD(itemRepo, "02300156", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL BEZ WTYCZKI CII       2x0,75 100cm");
            Item_ADD(itemRepo, "02300160", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA 2PCII       2x0,75 100cm");
            Item_ADD(itemRepo, "02300162", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "CAVO AL.3X0,75 SE MT.2,2");
            Item_ADD(itemRepo, "02300187", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEW.ZASI.2X0,75 M 2 WTYCZKA-2BIEG.KLII");
            Item_ADD(itemRepo, "02300249", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL SJT 3X18AWG+WTY.UL C.N 730 L1200");
            Item_ADD(itemRepo, "02300254", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD ZASIL.3X2  MT1,5O WTYCZ.TAIWAN");
            Item_ADD(itemRepo, "02300256", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL ZASIL. BEZ WTYCZKI 2X0,75 MT2 CLII");
            Item_ADD(itemRepo, "02300260", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA EU KLII     2x0,75 150cm");
            Item_ADD(itemRepo, "02300262", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PLASKI PRZEWOD PAN. STER.DIS.N 1");
            Item_ADD(itemRepo, "02300525", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZAROWKA RUROWA E14\" 230-240V 40W");
            Item_ADD(itemRepo, "02300789", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FAR.AL.F&F+CONN G.P.SAT.LAM.12V20W OSRAM");
            Item_ADD(itemRepo, "02300798", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "HALOGEN.F&F ZIK7+ZLACZ.6,3L160 SAT.OSRAM");
            Item_ADD(itemRepo, "02300806", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LAMP.HALOGEN.CR S1000 20WG.E. ZV800");
            Item_ADD(itemRepo, "02300891", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZARÓW.HALOG.DWUSTYK.12V 20W TRAS OSRAM");
            Item_ADD(itemRepo, "02300981", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOROW  POLIPROP V2 AE");
            Item_ADD(itemRepo, "02301058", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PLASKI PRZEWOD PAN. STER. WITCH  DIS.4");
            Item_ADD(itemRepo, "02301068", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEDLUZKA PRZEW.STER. VISOR DIS.11");
            Item_ADD(itemRepo, "02310103A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA27 53.33C CL.F 230-240V~50Hz 100W CCW");
            Item_ADD(itemRepo, "02320386", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "COND.5MF 425/475V 10000H C878BE24500AA5J");
            Item_ADD(itemRepo, "02320387", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "REFLEKTOR LED BEST 3V 700MA 2,1W");
            Item_ADD(itemRepo, "02320389", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZAROWKA SWIECZKOWA LED E14 4W 230-240V");
            Item_ADD(itemRepo, "02500811", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.\"90USA-325\" 30\"30010");
            Item_ADD(itemRepo, "02500814", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.\"90USA-325\" 60 30021");
            Item_ADD(itemRepo, "02501547", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.95-413 90      145226");
            Item_ADD(itemRepo, "02501548", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.95-413 60      145225");
            Item_ADD(itemRepo, "02501550", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.413 50         145223");
            Item_ADD(itemRepo, "02501674", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.340 60 KE ST 343 01010");
            Item_ADD(itemRepo, "02501679", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.325 60 KE ST 343 30021");
            Item_ADD(itemRepo, "02501680", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.325 90 KE ST 343 30022");
            Item_ADD(itemRepo, "02502231", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART.  ES23 50");
            Item_ADD(itemRepo, "02502588", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB. GHOST RINF.90");
            Item_ADD(itemRepo, "02502589", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB. GHOST RINF.90");
            Item_ADD(itemRepo, "02502590", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB. GHOST RINF.90");
            Item_ADD(itemRepo, "02502591", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "DISTANZ. LAT.GHOST/PHANTOM  60");
            Item_ADD(itemRepo, "02502592", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "DISTANZ. LAT.GHOST/PHANTOM  90");
            Item_ADD(itemRepo, "02502593", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB. GHOST RINF.90");
            Item_ADD(itemRepo, "02502594", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PROT.SUPERIORE GHOST/PHANTOM");
            Item_ADD(itemRepo, "02502721", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PODSTAWA KARTONOWA ES5 60");
            Item_ADD(itemRepo, "02502722", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BOK KARTONOWY ES5");
            Item_ADD(itemRepo, "02502734", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PODSTAWA KART. ES5 90");
            Item_ADD(itemRepo, "02502736", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "WKLADKA DYSTANSOWA KART. ES5 50");
            Item_ADD(itemRepo, "02502882", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "WKLADKA KART. SUPER SILENT");
            Item_ADD(itemRepo, "02502883", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KART. SUPER SILENT 600");
            Item_ADD(itemRepo, "02502927", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAK. KARTONOWE ES5 60 ELUX");
            Item_ADD(itemRepo, "02502928", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA IMBALLO ES5 90 ELUX");
            Item_ADD(itemRepo, "02502941", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PACKAGING BOX SKLOK 90 ELUX");
            Item_ADD(itemRepo, "02502942", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ROLL BOX BLOWER SKLOK 90 ELUX");
            Item_ADD(itemRepo, "02502943", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "TOP BASE MOTOR BOX SKLOK ELUX");
            Item_ADD(itemRepo, "02502944", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BOTTOM BASE MOTOR BOX SKLOK ELUX");
            Item_ADD(itemRepo, "02502945", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BASE PACKAGING SKLOK 90 ELUX");
            Item_ADD(itemRepo, "02502946", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "PACKAGING BOX SKLOK 60 ELUX");
            Item_ADD(itemRepo, "02502947", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BASE PACKAGING SKLOK 60 ELUX");
            Item_ADD(itemRepo, "02502948", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ROLL BOX BLOWER SKLOK 60 ELUX");
            Item_ADD(itemRepo, "03104821", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. S.SILENT 60CM - MANTELLO INF.");
            Item_ADD(itemRepo, "03104822", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. S.SILENT 60CM - ELS FRONT.");
            Item_ADD(itemRepo, "03104823", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. S.SILNET 60CM - CORNICE LED");
            Item_ADD(itemRepo, "03117438", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 60CM - PANN.ALOG-LED ES SL");
            Item_ADD(itemRepo, "03117440", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 50CM - PANN.ALOG-LED ES SL");
            Item_ADD(itemRepo, "03117507", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.3 60CM - CARC.413 - 2M");
            Item_ADD(itemRepo, "03117516", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 50CM - PANN.LAM.NEON 413");
            Item_ADD(itemRepo, "03117671", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 60CM - CARC.412 - 2M");
            Item_ADD(itemRepo, "03117675", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.3 60CM - PANN.FILT.412");
            Item_ADD(itemRepo, "03117725", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.3 60CM - CARRELLO 97/413");
            Item_ADD(itemRepo, "03117727", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 60CM - CARRELLO 414");
            Item_ADD(itemRepo, "03117730", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. SUPP.FIL.SU TEST.414");
            Item_ADD(itemRepo, "03118112", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 60CM - PANN.LAM.NEON PC - 413");
            Item_ADD(itemRepo, "03118258", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L5. ES5 90CM - PLAFONIERA LAMPADE");
            Item_ADD(itemRepo, "03118259", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 90CM - SUPP.FILTRO ANT.");
            Item_ADD(itemRepo, "03118297", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L5. ES5 50CM - PLAF.LAMPADE (F)");
            Item_ADD(itemRepo, "03118298", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 ES5 50CM - CARRELLO (F)");
            Item_ADD(itemRepo, "03118643", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L5. ES23 60CM - COPERCHIO COM.");
            Item_ADD(itemRepo, "03118714", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L5. ES23 60CM - CARC.");
            Item_ADD(itemRepo, "03118716", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.3 ES23 60CM - PANNELLO LAMPADE");
            Item_ADD(itemRepo, "03118769", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.3 ES23 50CM - CARC.");
            Item_ADD(itemRepo, "03118771", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.3 ES23 50CM - COPERCHIO COM. CARR.");
            Item_ADD(itemRepo, "03118772", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.3 ES23 50CM - PANNELLO LAMPADE");
            Item_ADD(itemRepo, "03118881", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 50CM - CARRELLO 97/414");
            Item_ADD(itemRepo, "03119029", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 50CM - CARC.413-415 - CE v.2015");
            Item_ADD(itemRepo, "03119030", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. 60CM - CARC.413 CE v.2015");
            Item_ADD(itemRepo, "03119038", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. VMOT - BRACKET CALIBR. BUTTON");
            Item_ADD(itemRepo, "03119039", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. VMOT - 60CM - CARC.414");
            Item_ADD(itemRepo, "03119041", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. VMOT - 50CM - CARC.414");
            Item_ADD(itemRepo, "03119044", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. VMOT - COVER_1");
            Item_ADD(itemRepo, "03119045", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. VMOT - COVER_2");
            Item_ADD(itemRepo, "03119115", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L5. ES5 60CM - FRONTALE");
            Item_ADD(itemRepo, "03119143", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.3 ES23 50CM - CARR.2F SL V.2018");
            Item_ADD(itemRepo, "03119144", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. ES23 60CM CARR. - 2F SL V.2018");
            Item_ADD(itemRepo, "03119145", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L3. VMOT - BRACKET STOP CARR. DX");
            Item_ADD(itemRepo, "03127525", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 60CM - PANN.LAM.AL.413");
            Item_ADD(itemRepo, "03127526", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 60CM - CARRELLO 414");
            Item_ADD(itemRepo, "03127527", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 SUPP.FI.SU TE. - 414");
            Item_ADD(itemRepo, "03127540", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 60CM - CARRELLO 97/413");
            Item_ADD(itemRepo, "03127554", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 90CM - CARRELLO 414");
            Item_ADD(itemRepo, "03127555", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 90CM - LIS.FIL.414");
            Item_ADD(itemRepo, "03127559", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 90CM - PANN.LAM.AL.413");
            Item_ADD(itemRepo, "03127656", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 60CM - PANN.LAMP.NEON PC - 413");
            Item_ADD(itemRepo, "03127659", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 60CM - PANN.LAMP.NEON PC - 415");
            Item_ADD(itemRepo, "03127709", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 60CM - CARC.412 - 1M");
            Item_ADD(itemRepo, "03127711", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 60CM - PANN.FILT.412");
            Item_ADD(itemRepo, "03127712", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 90CM - PANN.FILT.412");
            Item_ADD(itemRepo, "03127713", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 90CM - CARC. 412 - 1M");
            Item_ADD(itemRepo, "03127925", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GR3. M.3 417 60CM - PANN.ALOG-LED PC");
            Item_ADD(itemRepo, "03127926", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GR3. M.3 417 90CM - PANN.ALOG-LED PC");
            Item_ADD(itemRepo, "03127928", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 60CM CARRELLO");
            Item_ADD(itemRepo, "03127929", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 60CM - PLAF.LAMPADE (F)");
            Item_ADD(itemRepo, "03127930", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 - TESTATA DX-SX (F)");
            Item_ADD(itemRepo, "03127931", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 90CM - CARRELLO");
            Item_ADD(itemRepo, "03127932", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 60CM - CARC. model CL");
            Item_ADD(itemRepo, "03127933", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 90CM - CARC. CL");
            Item_ADD(itemRepo, "03127934", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 60CM - PLAF.ALOG/LED (F)");
            Item_ADD(itemRepo, "03127935", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES% 60CM CARRELLO GLASS FILTRO");
            Item_ADD(itemRepo, "03127937", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 60CM CARRELLO GFA");
            Item_ADD(itemRepo, "03127938", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 - STAFFA VETRO DX (F)");
            Item_ADD(itemRepo, "03127939", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 - STAFFA VETRO SX (F)");
            Item_ADD(itemRepo, "03127940", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L2. ES5 - STAFFA REGOLAZ.PORF.CAPPA");
            Item_ADD(itemRepo, "03145024", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. CARC.ALTA VISOR 60");
            Item_ADD(itemRepo, "03145025", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L5. CARC.ALTA VISOR 30\"");
            Item_ADD(itemRepo, "03145147", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 60CM - FRONT.CARR. - 415");
            Item_ADD(itemRepo, "03145149", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 60CM - CARC.415 - 2M");
            Item_ADD(itemRepo, "03145156", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 90CM - FRONT.CARR. - 415");
            Item_ADD(itemRepo, "03145157", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 90CM - PANN.LAM.NEON - 415");
            Item_ADD(itemRepo, "03145158", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 90CM - CARC. 415 - 2M");
            Item_ADD(itemRepo, "03145194", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 60CM - CARC.413 - 2M");
            Item_ADD(itemRepo, "03145283", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 90CM - CARRELLO 413/97");
            Item_ADD(itemRepo, "03145360", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 60CM - CARC.425 - PC");
            Item_ADD(itemRepo, "03145361", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 90CM - CARC. 425 - PC");
            Item_ADD(itemRepo, "03145362", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 60CM - PANN.LAMP.ALOG. - 425");
            Item_ADD(itemRepo, "03145363", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. 90CM - PANN.LAMP.ALOG. - 425");
            Item_ADD(itemRepo, "03145537", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GR3. M.3 417 90CM - CARC. 1M");
            Item_ADD(itemRepo, "03145539", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GR3. M.3 417 60CM - CARC. 1M");
            Item_ADD(itemRepo, "03146992", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.3 WITCH 60CM - CARC. model stary");
            Item_ADD(itemRepo, "03146993", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 60CM - PANNELLO COM.");
            Item_ADD(itemRepo, "03146994", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 60CM - DISTANZIALE");
            Item_ADD(itemRepo, "03146995", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 60CM - PROLUNGA DISTANZ.");
            Item_ADD(itemRepo, "03146996", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 90CM - CARC. stary model");
            Item_ADD(itemRepo, "03146997", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 90CM - PANNELLO COM.");
            Item_ADD(itemRepo, "03146998", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 90CM - DISTANZIALE");
            Item_ADD(itemRepo, "03146999", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L3. WITCH 90CM - PROLUNGA DISTANZ.");
            Item_ADD(itemRepo, "03202108", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFIL PVC W KSZTACIE \"U\" CZERWONY");
            Item_ADD(itemRepo, "03202129", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFIL PVC \"U\" M206 CZARNY");
            Item_ADD(itemRepo, "03202286", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZELOTKA KABLOWA  OCB-500 SRED.8");
            Item_ADD(itemRepo, "03202287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZELOTKA KABLOWA SRED.13 PGSB-1519A");
            Item_ADD(itemRepo, "03202288", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZEPUST KABLOWY SRED.20 PGSB-9A");
            Item_ADD(itemRepo, "03202315", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GUMA ANTYWIBRACYJNA K2000  163108");
            Item_ADD(itemRepo, "03202442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZACISK KABLA CZARNY SR/SP1 SRB-R-3");
            Item_ADD(itemRepo, "03202454", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TULEJKA ZACISK.DO PRZEW.SR.22,2 SR-7W-2");
            Item_ADD(itemRepo, "03290016", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZACISK KABLA 12052 AE");
            Item_ADD(itemRepo, "03290068", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZASLEPKA DO SP Sr.123 AE 132049");
            Item_ADD(itemRepo, "03290499", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA KABLA  12061         AE");
            Item_ADD(itemRepo, "032904990", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA KABLA  12061         AU");
            Item_ADD(itemRepo, "03291013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KABLA SILNIKA K86 86019 AE");
            Item_ADD(itemRepo, "03291044", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PRZELOTKA KABLA  86086 AE");
            Item_ADD(itemRepo, "03292018", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI KABLA ZASILANIA  AE US033");
            Item_ADD(itemRepo, "03292020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KABLA PUSZKI INST.ELE.AE US022");
            Item_ADD(itemRepo, "03292029", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRYWKA ZASILANIA.USC.86-313 US324 AE");
            Item_ADD(itemRepo, "03292082", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA ZASILANIA USC.86-313 AE US348");
            Item_ADD(itemRepo, "03292083", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WEWN.PUSZ.ZAS.USC.86-313 AE US349");
            Item_ADD(itemRepo, "03292099", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "REDUKCJA SRED.150-125 AE 09048");
            Item_ADD(itemRepo, "03292114", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KOLNIERZ 88-313CE - (AE) US411");
            Item_ADD(itemRepo, "03292192", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LOZYSKO SILNIKA  K196  AE   132014");
            Item_ADD(itemRepo, "03292193", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D190 16 PALE AE 163027");
            Item_ADD(itemRepo, "03292194", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSL.NAKRET.WIRNIKA AE    145080");
            Item_ADD(itemRepo, "03292197", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PRZYCISK SUWANY 413 W2            145041");
            Item_ADD(itemRepo, "03292198", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI PAN.STER.413 W2  145035");
            Item_ADD(itemRepo, "03292199", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA ZASILANIA 413 AE  145025");
            Item_ADD(itemRepo, "03292200", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KAB.PUSZ.STER.ELE.413 AE145027");
            Item_ADD(itemRepo, "03292201", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI ZASILANIA 413 145026");
            Item_ADD(itemRepo, "03292203", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "DYSTANS PANELU 413      AE  145028");
            Item_ADD(itemRepo, "03292204", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZASLEPKA BOKU 413 AE W2 145036");
            Item_ADD(itemRepo, "03292205", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WSP. SZYBY PLAF.413 W2 SX AE 145055");
            Item_ADD(itemRepo, "03292206", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WSP. SZYBY PLAF. 413 W2 DX AE 145056");
            Item_ADD(itemRepo, "03292209", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 413 W2 60AEPVC 145148");
            Item_ADD(itemRepo, "03292210", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PIERSCIEN PRZYL.Sr.123 413   AE  145037");
            Item_ADD(itemRepo, "03292224", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA KOSTKI ZACISKOWEJ 413 2MAE 145079");
            Item_ADD(itemRepo, "03292230", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCZEK FRON.413 CR  DX AE 145024");
            Item_ADD(itemRepo, "03292231", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCZEK FRONT.413 CR SX AE 145023");
            Item_ADD(itemRepo, "03292232", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 413 GR3 60 AEPVC 145148");
            Item_ADD(itemRepo, "03292249", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 413 W2 50 AE 145150");
            Item_ADD(itemRepo, "03292250", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZASLEP.OSWIETLENIA  413 POLIPR.  145093");
            Item_ADD(itemRepo, "03292251", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SEPARATOR 413 2M POLIP.145012");
            Item_ADD(itemRepo, "03292259", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA PRZYCISKU 413 BL1 AE   145040");
            Item_ADD(itemRepo, "03292260", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZYCISK SUWANY 413 BL1   145041");
            Item_ADD(itemRepo, "03292261", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRY.PUSZKI PAN. STER.  413 BL1 145035");
            Item_ADD(itemRepo, "03292262", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZASLEPKA BOKU .413 AE BL1   145036");
            Item_ADD(itemRepo, "03292263", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WSP. SZYBY PLAF.413 BL1 SX AE  145055");
            Item_ADD(itemRepo, "03292264", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WSP. SZYBY PLAF.413 BL1 DX AE  145056");
            Item_ADD(itemRepo, "03292270", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA NAKRETKI SILNIKA AE");
            Item_ADD(itemRepo, "03292290", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO PRZEWODU REFLEKTORA K AU");
            Item_ADD(itemRepo, "03292368", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 413 BL1 90  AE 145152");
            Item_ADD(itemRepo, "03292369", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 413 BL160 AE PVC 145148");
            Item_ADD(itemRepo, "03292382", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA PRZYCISKU 413 GR3 AE   145040");
            Item_ADD(itemRepo, "03292383", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PRZYCISK SUWANY 413 GR3           145041");
            Item_ADD(itemRepo, "03292384", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI PAN. STER. 413 GR3 145035");
            Item_ADD(itemRepo, "03292385", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZASLEPKA BOKU 413 AEGR3 145036");
            Item_ADD(itemRepo, "03292388", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 413 GR390AEPVC 145152");
            Item_ADD(itemRepo, "03292457", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 415 GR360 AE PVC 145153");
            Item_ADD(itemRepo, "03292459", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 415 GR390 AEPVC 145157");
            Item_ADD(itemRepo, "03292460", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WSPORNIK FILTRA MOTYLEK 413 145229");
            Item_ADD(itemRepo, "03292489", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA PANEL.STER.98-413 W2 AE 145375");
            Item_ADD(itemRepo, "03292495", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA KONDENSATORA.425  AE  163051");
            Item_ADD(itemRepo, "03292508", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCZEK FR.TONDA 425 W2 AE  145203");
            Item_ADD(itemRepo, "03292540", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCZEK FR.INC.414 BL1DX AE 145220");
            Item_ADD(itemRepo, "03292541", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCZEK FR.INC.414 BL1SX AE 145221");
            Item_ADD(itemRepo, "03292544", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA PANEL.STER.98-413 GR3 AE  145375");
            Item_ADD(itemRepo, "03292545", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA PANEL.STER.98-413 BL1 AE 145375");
            Item_ADD(itemRepo, "03292561", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCZEK.FR.TONDA 425 CR AE  145203");
            Item_ADD(itemRepo, "03292629", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "TULEJA SIST.AMM.K2000  163110");
            Item_ADD(itemRepo, "03292631", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "REDUKCJA 150/125 K2000");
            Item_ADD(itemRepo, "03292673", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCZEK FR.INC.414 GR3 DX AE 145220");
            Item_ADD(itemRepo, "03292674", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCZEK INC.414 GR3 SX AE 145221");
            Item_ADD(itemRepo, "03292700", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "UCHWYT FRONTU 413 GR3 145332");
            Item_ADD(itemRepo, "03292849", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA.413 W2 50 AE PVC 2005");
            Item_ADD(itemRepo, "03292854", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA.413 W2 60 AE PVC 2005");
            Item_ADD(itemRepo, "03292856", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA.413 GR3 60 AE PVC 2005");
            Item_ADD(itemRepo, "03292861", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA.413 GR3 90 AE PVC 2005");
            Item_ADD(itemRepo, "03293031", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CURSORE MOLLA VIS.MOB.SP2000 AE   163262");
            Item_ADD(itemRepo, "03293079", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KLIPS MOCUJACY DO KRATKI SP2000 R7031 AE");
            Item_ADD(itemRepo, "03293222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOR.ZENSKA NILAMID");
            Item_ADD(itemRepo, "03293227", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOR.MESKA. ANTINTR.V0");
            Item_ADD(itemRepo, "03293307", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SZKIELKO PANELU. EL.SLIM TRASPARENT.");
            Item_ADD(itemRepo, "03293328", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "GRIGLIA DEFL.FILTR.VERSA");
            Item_ADD(itemRepo, "03293332", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "DISTANZ.413 GR3 50 AE 145150");
            Item_ADD(itemRepo, "03293339", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CEILING LAMP 110X62 NO PIN");
            Item_ADD(itemRepo, "03293341", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CLIP BLOCCACAVO COMANDI VERSA");
            Item_ADD(itemRepo, "03293347", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "HANDLE FRONTAL SKLOK");
            Item_ADD(itemRepo, "03293349", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BUTTON GUIDE NEW PUSH BOTTON EVEREL");
            Item_ADD(itemRepo, "03293350", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PLASTIC-BLOCK FOR GLASS SKLOK");
            Item_ADD(itemRepo, "03293351", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LATERAL SPACER SKLOK");
            Item_ADD(itemRepo, "03293355", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "RIGHT FRONTAL LATERAL PART SKLOK SS");
            Item_ADD(itemRepo, "03293356", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LEFT FRONTAL LATERAL PART SKLOK SS");
            Item_ADD(itemRepo, "03293358", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "HOUSING ROTATIVE CONTROL PUSH PULL");
            Item_ADD(itemRepo, "03293408", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "MASKOWNICA 413 BL1 30\" AEPVC");
            Item_ADD(itemRepo, "03293411", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "DEFL.FILTR.LANDSCAPE PP COP.V2 NERO");
            Item_ADD(itemRepo, "03293433", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OZNACZENIE LAMP 40W PP COPOLIMERO V2 N");
            Item_ADD(itemRepo, "03293446", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZASLEPKA BOKU LEWA ES5");
            Item_ADD(itemRepo, "03293447", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "MOCOWANIE FRONTU ES5");
            Item_ADD(itemRepo, "03293449", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "RAMKA PANELU STEROWANIA ES5");
            Item_ADD(itemRepo, "03293452", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZASLEPKA BOKU PRAWA ES5 R7004");
            Item_ADD(itemRepo, "03294186", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "DYSTANS PANELU  413     PP UL 94-V0");
            Item_ADD(itemRepo, "03294740", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "TAPPO POST. CARR.NILAMID A H2 FR HF2 VO");
            Item_ADD(itemRepo, "03294748", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "TAPPO  SUP.ES NILAMID A H2 FR HF2 VO");
            Item_ADD(itemRepo, "03294757", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA SP NEW 07");
            Item_ADD(itemRepo, "03294784", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PRZELOTKA SZUFLADY ES24  AE");
            Item_ADD(itemRepo, "03294785", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PRZELOT.SZUFLADY DOMEK ES24 CARR. ES AE");
            Item_ADD(itemRepo, "03295008", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA ZABEZP.KONDENSAT. 425 AU 163051");
            Item_ADD(itemRepo, "03295627", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "REDUKCJA USCITA ARIA CONV. SUPERSILENT");
            Item_ADD(itemRepo, "03295628", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA ELEKTRYKI. CONV. SUPERSILENT");
            Item_ADD(itemRepo, "03295629", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA ZASILANIA. SUPERSILENT");
            Item_ADD(itemRepo, "03300453", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR WEGLOWY 412 60     145326");
            Item_ADD(itemRepo, "03300454", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR WEGLOWY 412 90     145327");
            Item_ADD(itemRepo, "03300484", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR WEG.AKT. SRED.190 K2000 ALTE AE");
            Item_ADD(itemRepo, "03300513", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR WEGLOWY ES414 RAMKA ALL+CAR+RETE");
            Item_ADD(itemRepo, "03300551", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR WEGOWY 2SZT. DO MINIBLOWERA");
            Item_ADD(itemRepo, "04302047", "9103", null, null, ItemTypeEnum.BuyedItem, null, "WZORNIK OTWOR.K413 1/2M 90    145147");
            Item_ADD(itemRepo, "04302058", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "WZORNIK OTWOR. K413 1/2M 45/60 145147/1");
            Item_ADD(itemRepo, "04302076", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "WZORNIK OTWOROW 425    30093");
            Item_ADD(itemRepo, "04302172", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SZABLON MONTAZOWY VERSA");
            Item_ADD(itemRepo, "04302180", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "DRILLING JIG SKLOK");
            Item_ADD(itemRepo, "04302188", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "DRILLING JIG COMPASS");
            Item_ADD(itemRepo, "04306281", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA 98-414");
            Item_ADD(itemRepo, "04306306", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA 98-412 CL2 NEU.7L");
            Item_ADD(itemRepo, "04306345", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA AGG.MONT.FIL.CA 413/415 /1");
            Item_ADD(itemRepo, "04306466", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ULOTKA INSTR.Z NORMAMI KLI/KLII");
            Item_ADD(itemRepo, "04307136", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA ERRATA COR.SMONT.PLAFONIERA");
            Item_ADD(itemRepo, "04307404", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA ISTR.EXTRA TERMINAL BLOCK");
            Item_ADD(itemRepo, "04307442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA LIKWID.ODPADOW WEEE DIRECTIVE CEE");
            Item_ADD(itemRepo, "04308011", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA SAVO 414  FINL.");
            Item_ADD(itemRepo, "04308023", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA BROAN ELITE .US-ES24");
            Item_ADD(itemRepo, "04308244", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA BROAN ELITE US-ES24 EB40");
            Item_ADD(itemRepo, "04308245", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA LY-VENT");
            Item_ADD(itemRepo, "04308246", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA LY-VENT");
            Item_ADD(itemRepo, "04308296", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA SMEG AUSTRALIA");
            Item_ADD(itemRepo, "04308297", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO NORME SMEG AUSTRALIA");
            Item_ADD(itemRepo, "04308400", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA BEST RUSSIA");
            Item_ADD(itemRepo, "04308423", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA ES_PULLOUT PIATTAFORMA");
            Item_ADD(itemRepo, "04308458", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA M-SYSTEM");
            Item_ADD(itemRepo, "04308488", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA ES_PULLOUT PIATT.M_SYSTEM");
            Item_ADD(itemRepo, "04308504", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA WITCH");
            Item_ADD(itemRepo, "04308508", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA BEST GRECIA");
            Item_ADD(itemRepo, "04308519", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA ES_PULLOUT PIATT.CELLO KRONOS");
            Item_ADD(itemRepo, "04308535", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA ES_PULLOUT THERMEX PIATT.");
            Item_ADD(itemRepo, "04308579", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJASUPERSILENT PLUS THERMEX");
            Item_ADD(itemRepo, "04308626", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA KANT-LY 414 EBRAICO");
            Item_ADD(itemRepo, "04308658", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA SAVO BLISS FINL.");
            Item_ADD(itemRepo, "04308686", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA BERTAZZONI VISOR");
            Item_ADD(itemRepo, "04308689", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJE BEZPIECZEÑSTWA PELGRIM/ATAG");
            Item_ADD(itemRepo, "04308690", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GWARANCJA PELGRIM");
            Item_ADD(itemRepo, "04308704", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA SAVO 425 FINLANDESE");
            Item_ADD(itemRepo, "04308705", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA SAVO 417 FINLANDESE");
            Item_ADD(itemRepo, "04308719", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO 414 PHV-26 FINLANDESE");
            Item_ADD(itemRepo, "04308724", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA SAVO ES23 LED FINLANDESE");
            Item_ADD(itemRepo, "04308738", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO 414 PV-26 FINLANDESE");
            Item_ADD(itemRepo, "04308739", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO 414 ePHV-26 FINLANDESE");
            Item_ADD(itemRepo, "04308740", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO 414 PVK-26 FINLANDESE");
            Item_ADD(itemRepo, "04308753", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.IST.WITCH PELGRIM");
            Item_ADD(itemRepo, "04308754", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.USO WITCH PELGRIM");
            Item_ADD(itemRepo, "04308757", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.ISTR. 412 APELL");
            Item_ADD(itemRepo, "04308759", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA UZYTKOWNIKA VERSA AEG");
            Item_ADD(itemRepo, "04308761", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA JUNO");
            Item_ADD(itemRepo, "04308763", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA ELECTROLUX");
            Item_ADD(itemRepo, "04308765", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA ZANUSSI");
            Item_ADD(itemRepo, "04308767", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA FAURE");
            Item_ADD(itemRepo, "04308768", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA PROGRESS");
            Item_ADD(itemRepo, "04308769", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA ZANKER");
            Item_ADD(itemRepo, "04308775", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS AEG 801320720");
            Item_ADD(itemRepo, "04308776", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ELECTROLUX 801320634");
            Item_ADD(itemRepo, "04308778", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ZANKER 801320602");
            Item_ADD(itemRepo, "04308779", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA REJESTRACYJNA AEG 801320721");
            Item_ADD(itemRepo, "04308780", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ELECTROLUX 801320635");
            Item_ADD(itemRepo, "04308782", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ROZSZERZONA GWARANCJA AEG 801320722");
            Item_ADD(itemRepo, "04308783", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ELECTROLUX 801320636");
            Item_ADD(itemRepo, "04308785", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ELECTROLUX 801320578  S");
            Item_ADD(itemRepo, "04308786", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KSIAZKA SERWISOWA AEG 801320684  P");
            Item_ADD(itemRepo, "04308787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ZANUSSI 801320243  Y");
            Item_ADD(itemRepo, "04308788", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON 801320204  T");
            Item_ADD(itemRepo, "04308830", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.VERSA ELECTROLUX NEW RISER");
            Item_ADD(itemRepo, "04308831", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA ZANUSSI NEW RISER");
            Item_ADD(itemRepo, "04308832", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA FAURE NEW RISER");
            Item_ADD(itemRepo, "04308833", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA OBSLUGI VERSA AEG NEW RISER");
            Item_ADD(itemRepo, "04308834", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA JUNO NEW RISER");
            Item_ADD(itemRepo, "04308841", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "WARRANTY RUSSO AEG ELEC.ZANU.801320556 D");
            Item_ADD(itemRepo, "04308844", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERTIFICATO RUSSO ELECTROLUX 867351004");
            Item_ADD(itemRepo, "04308848", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON OEM-GB 801320605 Q");
            Item_ADD(itemRepo, "04308851", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUCTION MANUAL SKLOK ELECTROLUX");
            Item_ADD(itemRepo, "04308852", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL SKLOK ELECTROLUX");
            Item_ADD(itemRepo, "04308855", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI VERSA BEST");
            Item_ADD(itemRepo, "04308856", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO VERSA BEST");
            Item_ADD(itemRepo, "04308868", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ELECTR. 2Y 801320639");
            Item_ADD(itemRepo, "04308869", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ELECTR. UK 801320640");
            Item_ADD(itemRepo, "04308870", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ELECTROLUX 2Y  801320638");
            Item_ADD(itemRepo, "04308871", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUCTION MANUAL SKLOK AEG");
            Item_ADD(itemRepo, "04308872", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL SKLOK AEG");
            Item_ADD(itemRepo, "04308875", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL COMPASS NEUTRAL");
            Item_ADD(itemRepo, "04308876", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL COMPASS NEUTRAL");
            Item_ADD(itemRepo, "04308877", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL COMPASS ELECTROLUX");
            Item_ADD(itemRepo, "04308878", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "USER MANUAL COMPASS ELECTROLUX");
            Item_ADD(itemRepo, "04308916", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTALLATION MANUAL COMPASS SAVO");
            Item_ADD(itemRepo, "04308921", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERT.RUSSO ELECTR.RUC-SE.HA21.B.00612/19");
            Item_ADD(itemRepo, "04308932", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.ISTR.BROAN ELITE US-ES24 NEW HALOGEN");
            Item_ADD(itemRepo, "04310226", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA PRZYLEPNA Z SYMBOLEM ZIEMI");
            Item_ADD(itemRepo, "04316956", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA HVI - 990716881");
            Item_ADD(itemRepo, "04317803", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "NAKLEJKA OSTRZEGAWCZA SHI LEI");
            Item_ADD(itemRepo, "04318074", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY BIALY  N.8074 US");
            Item_ADD(itemRepo, "04318136", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY N.8136");
            Item_ADD(itemRepo, "04318152", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY BIALY N.8152 US");
            Item_ADD(itemRepo, "04333895", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TABL.\"WARNING 2004\" KANT-LY CZERWONA");
            Item_ADD(itemRepo, "04341606", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.GWAR.\"WARRANTY STICKER\" LY-VENT");
            Item_ADD(itemRepo, "04341737", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.KART.AGG.CELLO KRONOS 500260W 60CM");
            Item_ADD(itemRepo, "04341739", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.KART.AGG.CELLO KRONOS 500260I 60CM");
            Item_ADD(itemRepo, "04400331", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA SAMOPRZYLEPNA cULus 5000109");
            Item_ADD(itemRepo, "04404102", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA DATI TEC.425 ecc. SHIH LEI EP45");
            Item_ADD(itemRepo, "04404512", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA ECOLABEL TURCJA");
            Item_ADD(itemRepo, "04405212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA ECOLABEL UKRAINA");
            Item_ADD(itemRepo, "04405395", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA ECOLABEL A+++ NEUTRALNA");
            Item_ADD(itemRepo, "04500000", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PRZEZROCZYSTA 60X50");
            Item_ADD(itemRepo, "04500012", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PAPIEROWA 19mm x 50m");
            Item_ADD(itemRepo, "04500013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA TESA ART.4298 STR.50X66 NIEBIESKA");
            Item_ADD(itemRepo, "04500025", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PRZYLEPNA.BIALA 66X25 PVC");
            Item_ADD(itemRepo, "04500037", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PRZYLEPNA OSTRZEGAWCZA 54m");
            Item_ADD(itemRepo, "04808159", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY 8159");
            Item_ADD(itemRepo, "04808160", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8160");
            Item_ADD(itemRepo, "04808162", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING SHEET 8162");
            Item_ADD(itemRepo, "04808166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8166");
            Item_ADD(itemRepo, "04808175", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING SHEET 8175");
            Item_ADD(itemRepo, "04808176", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING SHEET 8176");
            Item_ADD(itemRepo, "06000445U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "conv.343 - mot.02310766a + 004202046 ok");
            Item_ADD(itemRepo, "06000520U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "ASS.CONV.ES23 S2 LOTT11_mot_02310760A,1A");
            Item_ADD(itemRepo, "06002013U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.932,933 - mot. EB40 02310219a");
            Item_ADD(itemRepo, "06002157U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.932,933 - mot. EP40 02310260a");
            Item_ADD(itemRepo, "06002158U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.932,933 - mot. EB40 02310219a");
            Item_ADD(itemRepo, "06002202U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.614 - mot. EB40 02310746A");
            Item_ADD(itemRepo, "06002213U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.615 - mot. EA27 02310266a");
            Item_ADD(itemRepo, "06002243U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.614 - mot. EB20 02310770a");
            Item_ADD(itemRepo, "06002244U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.615 - mot. EA37 02310120a");
            Item_ADD(itemRepo, "06002248U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOWER conv.616 - mot. EB20 02310776a");
            Item_ADD(itemRepo, "06002281U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.256 - mot. EB20 02310788a");
            Item_ADD(itemRepo, "06002288U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.256 - mot. EA37 02310766a");
            Item_ADD(itemRepo, "06002290", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "EMC BLOWER HEI-SK300");
            Item_ADD(itemRepo, "06002297U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOWER conv.932,933 - mot.EP45 02310803A");
            Item_ADD(itemRepo, "06002302", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "EMC ASS.MINI BLOWER HEI-SL400");
            Item_ADD(itemRepo, "06002310U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.614 - mot. EB40 02310746A");
            Item_ADD(itemRepo, "06021082U", "", null, null, ItemTypeEnum.BuyedItem, null, "ASS. CONV.SUPERSILENT EB30");
            Item_ADD(itemRepo, "06021086", "", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CONV.06020034+ASS.IE 06144732");
            Item_ADD(itemRepo, "06021087", "", null, null, ItemTypeEnum.BuyedItem, null, "ASS.CONV.06020035+ASS.IE 06144135");
            Item_ADD(itemRepo, "06101333", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.PANELU ELS ORIZZ.SUPERSILENT L70 664");
            Item_ADD(itemRepo, "06102589", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.TRASF.K24 EB");
            Item_ADD(itemRepo, "06102744", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES WIAZKI OSWIETLENIA ES 55/60+CONN. 4W");
            Item_ADD(itemRepo, "06102745", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.WIAZ.OSWIET.GI198 2L 60 V2015 4W");
            Item_ADD(itemRepo, "06102749", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.PANE.DOTYK. LED AMB. BL LUC.3V+TIME");
            Item_ADD(itemRepo, "06102755", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COM.SOFT TOUCH AMBRA NERO LUC.4V PEL");
            Item_ADD(itemRepo, "06102769", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COM.COMPLETO ES5 BASCULANTE ELX");
            Item_ADD(itemRepo, "06102770", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LED ES5 60 CM");
            Item_ADD(itemRepo, "06102787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LED K BETA");
            Item_ADD(itemRepo, "06102921", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.MICRO ES24 EL Piatt. B");
            Item_ADD(itemRepo, "06102922", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.PANELU ES24 EL/SLIM GRIGIO Piatt. B");
            Item_ADD(itemRepo, "06102950", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA. I.E. PLAT.LIGHT C. NYLAMID");
            Item_ADD(itemRepo, "06102956", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.PIATT. LIGHT EB BP-775");
            Item_ADD(itemRepo, "06102977", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP. PANELU STROWANIA ES5 PSQ TM4A006");
            Item_ADD(itemRepo, "06102979", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.PANELU STEROWANIA VISOR");
            Item_ADD(itemRepo, "06102986", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE ELETTRO VALVOLA SAVO");
            Item_ADD(itemRepo, "06103004", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LAMPADE 4W ES5 60 CM");
            Item_ADD(itemRepo, "06103011", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LAMPADE 4W ES5 90 CM");
            Item_ADD(itemRepo, "06103012", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LAMPADE 4W ES5 60 CM X RICAMBI");
            Item_ADD(itemRepo, "06103013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.LAMPADE 4W ES5 90 CM X RICAMBI");
            Item_ADD(itemRepo, "06103020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING COMPASS ASSEMBLY");
            Item_ADD(itemRepo, "06103750", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.MICRO US-ES24");
            Item_ADD(itemRepo, "06103973", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.PANELU.QUAD. US-ES24 A2 SL  BL1");
            Item_ADD(itemRepo, "06103974", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI TRASFORMATORA ES24 S.USA");
            Item_ADD(itemRepo, "06103983", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.CONDENS.+PRZEWOD US ES24");
            Item_ADD(itemRepo, "06142859", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING ASS.SKLOK EA MOTOR");
            Item_ADD(itemRepo, "06142955", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ELECTRICAL WIRING ASS.SKLOK EB MOTOR");
            Item_ADD(itemRepo, "06142979", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI 414 E2 BA 2 LAMPADE");
            Item_ADD(itemRepo, "06142980", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI.414 E2 BA 1 LAMPADA");
            Item_ADD(itemRepo, "06142983", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.ES414 V2 1  ATTIVA 2x4W");
            Item_ADD(itemRepo, "06142987", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.ES414 V2 1  ATTIVA 1x4W");
            Item_ADD(itemRepo, "06142988", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.414 EC A BA 2x4W");
            Item_ADD(itemRepo, "06142989", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.414 EC A BA 1x4W");
            Item_ADD(itemRepo, "06143517", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "AS.WIA.425 L1A2SL 90 MET. 120/60 PIATT_C");
            Item_ADD(itemRepo, "06143523", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI.425 P-C L1D2SL 90");
            Item_ADD(itemRepo, "06144314", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI. ES23 L2 SE W2 RoHS");
            Item_ADD(itemRepo, "06144732", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "AS.IE.98-413 1M A2 SL 60 EXT.TER.BL.RoHS");
            Item_ADD(itemRepo, "06144909", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI. US-ES24 L1 L2 SL. S.USA");
            Item_ADD(itemRepo, "06144998", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.413 V2D2BA 60 SS");
            Item_ADD(itemRepo, "06145196", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.WIA.98-412 S1 A2 SL 90.BL.RoHS");
            Item_ADD(itemRepo, "06145220", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.WIAZKI. PIATT.B SCHEDA STD MOT,EB/EP");
            Item_ADD(itemRepo, "06145279", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI.ES413 V2015");
            Item_ADD(itemRepo, "06145280", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI.ES415 V2015");
            Item_ADD(itemRepo, "06145296", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI. PB US-VISOR");
            Item_ADD(itemRepo, "06145665", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI.417 1M A BA  BL Piatt. C");
            Item_ADD(itemRepo, "08080885", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PLYTKA PCB IE EL 6F * LD1 ** ECO E 14");
            Item_ADD(itemRepo, "08087052", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.AL.LAMINATO 413 60");
            Item_ADD(itemRepo, "08087056", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.AL.LAMINATO 413 90");
            Item_ADD(itemRepo, "08087109", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU. METAL 414 50");
            Item_ADD(itemRepo, "08087144", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.INOX ES24  60");
            Item_ADD(itemRepo, "08087175", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU. INOX.ES 90");
            Item_ADD(itemRepo, "08087194", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGR.ALL. K1288 UL");
            Item_ADD(itemRepo, "08087508", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.ALL.ES 60 UL 30\"");
            Item_ADD(itemRepo, "08087527", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU. METAL.V.2011 414 60");
            Item_ADD(itemRepo, "08087567", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU. ALL.ES 50");
            Item_ADD(itemRepo, "08087569", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU. ALL.ES 60");
            Item_ADD(itemRepo, "08087584", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.ALL.ES 60 UL");
            Item_ADD(itemRepo, "08087589", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU. METAL.414 90 v.2011");
            Item_ADD(itemRepo, "08087593", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.AL.LAMINATO 412 60 2011");
            Item_ADD(itemRepo, "08087594", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.AL.LAMINATO 412 90 2011");
            Item_ADD(itemRepo, "08087718", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.377X203,5 ES5 50");
            Item_ADD(itemRepo, "08087722", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ALL.FILTER 184,5X286 COMPASS 90");
            Item_ADD(itemRepo, "08087723", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ALL.FILTER 184,5X286 COMPASS 60");
            Item_ADD(itemRepo, "08087724", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ALL.FILTER 184,5X286 COMPASS 120");
            Item_ADD(itemRepo, "08087757", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.477X203,5 ES5 60");
            Item_ADD(itemRepo, "08087770", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR0 ANTIGR.388X193 ES5 90");
            Item_ADD(itemRepo, "08087771", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.FILTRO INOX 234X296,5 PLAS-INOX CURS");
            Item_ADD(itemRepo, "08087959", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.PROF.AL.LAMINATO 413 60");
            Item_ADD(itemRepo, "08087962", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU.GR. WITCH CM 90");
            Item_ADD(itemRepo, "08087999", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU. ANTINTR. 398x81,5");
            Item_ADD(itemRepo, "08088362", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZES.ZAWOR.VR.D150 K2000 163610");
            Item_ADD(itemRepo, "08088370", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.ZAWOR.VR.D150 (AU) K2000 163610");
            Item_ADD(itemRepo, "08088378", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "Z.KOL.P.ZAW.Z.D150(AU)K2000S/G ZFLV50051");
            Item_ADD(itemRepo, "08089224", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR BUFFLE SUPERSILENT 234x296,5");
            Item_ADD(itemRepo, "08091564", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.WSPOR. MONTAZ. REG. ES23  PRAWY");
            Item_ADD(itemRepo, "08091565", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.WSPOR. MONTAZ. REG. ES23  LEWY");
            Item_ADD(itemRepo, "08091677", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.WSPOR. MONTAZ. REG. ES PRAWY");
            Item_ADD(itemRepo, "08091678", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.WSPOR. MONTAZ. REG. ES LEWY");
            Item_ADD(itemRepo, "08091948", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.PROWAD.415 LEWA NYL CARIC.VETR.25%");
            Item_ADD(itemRepo, "08091949", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.PROWAD.415 PRAWA NYL.CAR.VETRO 25%");
            Item_ADD(itemRepo, "08094246", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS COMPASS XS 90+RIVET ASSEMBLY");
            Item_ADD(itemRepo, "08094247", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS COMPASS XS 60+RIVET ASSEMBLY");
            Item_ADD(itemRepo, "08094248", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS COMPASS XS 120+RIVET ASSEMBLY");
            Item_ADD(itemRepo, "08094295", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MOTOR HOUSING XS ASSEMBLY");
            Item_ADD(itemRepo, "08094297", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ES5 CARC. 90CM - MINI BLOWER ELZ");
            Item_ADD(itemRepo, "08094298", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ES5 CARC. 50CM - MINI BLOWER ELZ");
            Item_ADD(itemRepo, "08094365", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.FAST INSTALL.HOOD TORX");
            Item_ADD(itemRepo, "08094368", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.STAFFA FIX MOB.ES5 CUSTOM SX ELZ");
            Item_ADD(itemRepo, "08094369", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.STAFFA FIX MOB.ES5 CUSTOM DX ELZ");
            Item_ADD(itemRepo, "08094378", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.FRONT.VERSA+GLASS WHITE 60+SER. ELUX");
            Item_ADD(itemRepo, "08094379", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.FRONT.VERSA+GLASS BLACK 60+SER. ELUX");
            Item_ADD(itemRepo, "08094390", "", null, null, ItemTypeEnum.BuyedItem, null, "ASSEMBLY SPACER COMPASS");
            Item_ADD(itemRepo, "08095900", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.FRONT.MET.415 60 REX+BOCZKI");
            Item_ADD(itemRepo, "08095901", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.FRONT.MET.415 90 REX+BOCZKI");
            Item_ADD(itemRepo, "08095903", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.FRONT.413 60 W2+BOCZKI");
            Item_ADD(itemRepo, "08095908", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.FRONT.413 50 W2+BOCZKI");
            Item_ADD(itemRepo, "08095915", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.FRONT.CARR.415 GALV.INOX 60+TESTATE");
            Item_ADD(itemRepo, "08095920", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CHASSIS SKLOK XS 60");
            Item_ADD(itemRepo, "08095921", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CHASSIS SKLOK XS 90");
            Item_ADD(itemRepo, "E3253492", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "L.901/xxx WSPORNIK PAN.STER.SLIM VISOR");
            Item_ADD(itemRepo, "E3302097", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI. COVER CHAS. CONV. SUPERSILENT 476004");
            Item_ADD(itemRepo, "E3302098", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "105 WSP. POSTERIORE S.SILENT 476007");
            Item_ADD(itemRepo, "E3334220", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.STAFFA FISS.89-325 ZINC.30027");
            Item_ADD(itemRepo, "E3341152", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "107 STAFFA - PULLOUT FIS.LAT.413 ELZ");
            Item_ADD(itemRepo, "E3341528", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "108 PI.PA.CAM.VE.413 60CM ELZ - 2M - 1/1");
            Item_ADD(itemRepo, "E3341800", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.STAFFA FISS.413  ELZ 145174");
            Item_ADD(itemRepo, "E3342440", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.PA.CA.VE.412 1M60ELZ 145257");
            Item_ADD(itemRepo, "E3342441", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.PA.CA.VE.412 2M60ELZ 145258");
            Item_ADD(itemRepo, "E3342442", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.PA.CA.VE.412 1M90ELZ 145259");
            Item_ADD(itemRepo, "E3343336", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.WSP.DMUCHAWY.K2000 163090");
            Item_ADD(itemRepo, "E3345906", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "LIST.SUPP.GUIDE DX/SX CARR.ES 60 ELZN");
            Item_ADD(itemRepo, "E3345920", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.2979 ES 60CM - COPERCHIO COM.CARR. XS");
            Item_ADD(itemRepo, "E3345958", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.KLAPA MOBILE ES23  50  ELZ");
            Item_ADD(itemRepo, "E3346900", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.KLAPA MOBILE ES23  60  ELZ");
            Item_ADD(itemRepo, "E3346958", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.2979 ES 90CM - COPERCHIO COM.CARR.XS");
            Item_ADD(itemRepo, "E3347647", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.STAFFA SUPP.MOBILE ES 23");
            Item_ADD(itemRepo, "E3350145", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PA.ALOG.XS EIC 60CM PB - p.2/2");
            Item_ADD(itemRepo, "E3350147", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.3246 PA.ALOG.XS EIC 90CM PB - p.2/2");
            Item_ADD(itemRepo, "E3350829", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "STAFFA TRASF.EL ES");
            Item_ADD(itemRepo, "E3351155", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO COM.CARR.ES 30\" XS");
            Item_ADD(itemRepo, "E3351157", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PANN.ALOG.ES XS 30\" - passo 2 / 2");
            Item_ADD(itemRepo, "E3351830", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "STAFFA FISS.CONV. GIATT.A");
            Item_ADD(itemRepo, "E3352865", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.COPERTURA SPESSORE PENSILE WHITCH");
            Item_ADD(itemRepo, "E3352943", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "CABLE COVER PXXX IE P-B");
            Item_ADD(itemRepo, "E3352962", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "105 ES5 FRONT. 60CM XS");
            Item_ADD(itemRepo, "E3353376", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.PLAF.LAMPADE ALO/LED XS 60");
            Item_ADD(itemRepo, "E3353377", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.STAFFA MICRO VERS.PULS.XS");
            Item_ADD(itemRepo, "E3353382", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.STAFFA COP.GIATTINA DX XS 60");
            Item_ADD(itemRepo, "E3353383", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.STAFFA COP.GIATTINA SX XS 60");
            Item_ADD(itemRepo, "E3353385", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.PLAF.LAMPADE ALO/LED XS 30\"");
            Item_ADD(itemRepo, "E3353388", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.DISTANZIALE FILTRO SX XS 30\"");
            Item_ADD(itemRepo, "E3353391", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.STAFFA COP.GIATTINA DX XS 30\"");
            Item_ADD(itemRepo, "E3353392", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.STAFFA COP.GIATTINA SX XS 30\"");
            Item_ADD(itemRepo, "E3353408", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GI.DISTANZIALE FILTRO DX XS 30\"");
            Item_ADD(itemRepo, "E3353451", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ES5. FRONT. 90CM XS");
            Item_ADD(itemRepo, "E3353478", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ES5. FRONT. 50CM XS");
            Item_ADD(itemRepo, "E3353479", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.065/1120 BATTUTA FILTRO XS");
            Item_ADD(itemRepo, "E3353536", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "WSP.MONT.LEWY ES5 CUSTOM ELZ");
            Item_ADD(itemRepo, "E3353537", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "WSP.MONT.PRAWY ES5 CUSTOM ELZ");
            Item_ADD(itemRepo, "E3353542", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 90 BEND.LAMP PANEL XS");
            Item_ADD(itemRepo, "E3353543", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 90 BEND.LOWER SPACER XS");
            Item_ADD(itemRepo, "E3353544", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 90 BEND.UPPER SPACER RIGHT XS");
            Item_ADD(itemRepo, "E3353545", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 90 BEND.UPPER SPACER LEFT XS");
            Item_ADD(itemRepo, "E3353552", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 60 BEND.LAMP PANEL XS");
            Item_ADD(itemRepo, "E3353553", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 60 BEND.LOWER SPACER XS");
            Item_ADD(itemRepo, "E3353554", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 60 BEND.UPPER SPACER RIGHT XS");
            Item_ADD(itemRepo, "E3353555", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 60 BEND.UPPER SPACER LEFT XS");
            Item_ADD(itemRepo, "E3353557", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 120 BEND.LAMP PANEL XS");
            Item_ADD(itemRepo, "E3353558", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 120 BEND.LOWER SPACER XS");
            Item_ADD(itemRepo, "E3353559", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 120 BEND.UPPER SPACER RIGHT XS");
            Item_ADD(itemRepo, "E3353560", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPASS 120 BEND.UPPER SPACER LEFT XS");
            Item_ADD(itemRepo, "E3353603", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.SUPP.LAMPADE PULL-OUT");
            Item_ADD(itemRepo, "E3353693", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 60 BEND.CONTROL PANEL XS");
            Item_ADD(itemRepo, "E3353694", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 60 BEND.REAR PANNEL XS");
            Item_ADD(itemRepo, "E3353695", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK BEND.MOTOR BOX ELZ");
            Item_ADD(itemRepo, "E3353700", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK BEND.COVER MOTOR BOX XS");
            Item_ADD(itemRepo, "E3353701", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 60 BEND.FILTER SPACER L-R XS");
            Item_ADD(itemRepo, "E3353702", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK BEND.LATERAL LEFT XS");
            Item_ADD(itemRepo, "E3353703", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK BEND.LATERAL RIGHT XS");
            Item_ADD(itemRepo, "E3353704", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 60 BEND.REAR SPACER XS");
            Item_ADD(itemRepo, "E3353705", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 60 BEND.REAR CABINET SUPPORT XS");
            Item_ADD(itemRepo, "E3353706", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK BEND.INSTALLION HOOK LEFT XS");
            Item_ADD(itemRepo, "E3353707", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK BEND.INSTALLION HOOK RIGHT XS");
            Item_ADD(itemRepo, "E3353709", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 90 BEND.CONTROL PANEL XS");
            Item_ADD(itemRepo, "E3353710", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 90 BEND.REAR PANNEL XS");
            Item_ADD(itemRepo, "E3353711", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 90 BEND.REAR SPACER XS");
            Item_ADD(itemRepo, "E3353712", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SKLOK 90 BEND.REAR CABINET SUPPORT XS");
            Item_ADD(itemRepo, "E3353713", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "COMPAS BEND.BRACKET HOUSING XS");
            Item_ADD(itemRepo, "E3353935", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.PANNELLO ALOGENE ES 2019 XS 30\"");
            Item_ADD(itemRepo, "E3353936", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.PANNELLO ALO.2019 XS EIC CM 60 P-B");
            Item_ADD(itemRepo, "E3400534", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRONT.413 XS 30\"");
            Item_ADD(itemRepo, "E3400536", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.MOBILE ALTO ES24 30\" ELZ");
            Item_ADD(itemRepo, "E3400544", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.CARCASSA ES 30\" XS");
            Item_ADD(itemRepo, "E3400549", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.CARRELLO ES 2F SL 30\" XS");
            Item_ADD(itemRepo, "E3401025", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRON.414 INC.XS 90 (FR5)  145629");
            Item_ADD(itemRepo, "E3401458", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRONT.413 60CM XS");
            Item_ADD(itemRepo, "E3401479", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRONT.413 50CM XS");
            Item_ADD(itemRepo, "E3401739", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRONT.412 60CM XS");
            Item_ADD(itemRepo, "E3401814", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRONT.412 90CM XS");
            Item_ADD(itemRepo, "E3401850", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRONT.414 60CM XS");
            Item_ADD(itemRepo, "E3401867", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRONT.425 60CM XS - ROT.");
            Item_ADD(itemRepo, "E3403028", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES 60CM - CARCASSA XS");
            Item_ADD(itemRepo, "E3403029", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES 60CM - CARRELLO 2F SL XS");
            Item_ADD(itemRepo, "E3403031", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES24 60CM - MOBILE ALTO ELZ");
            Item_ADD(itemRepo, "E3403033", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES 60CM - CARRELLO EL 2F XS");
            Item_ADD(itemRepo, "E3403085", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES23 50CM - MOBILE ELZ");
            Item_ADD(itemRepo, "E3403256", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES22 90CM - TELAIO FILTRI XS");
            Item_ADD(itemRepo, "E3403258", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES 90CM - CARCASSA XS");
            Item_ADD(itemRepo, "E3403549", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES 90CM - CARRELLO EL 2F XS");
            Item_ADD(itemRepo, "E3403551", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES24 90CM - MOBILE ALTO ELZ");
            Item_ADD(itemRepo, "E3403789", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES23 60CM - MOBILE MOT/CE ELZ");
            Item_ADD(itemRepo, "E3404791", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES24 90CM - FRONT. FR8 ELSLIM XS");
            Item_ADD(itemRepo, "E3405277", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. CORNICE LED SUPERSILENT XS 60");
            Item_ADD(itemRepo, "E3405278", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD. CHASSIS CONV. SUPERSILENT");
            Item_ADD(itemRepo, "E3405279", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.PLASZCZ INF. SUPERSILENT XS 476001");
            Item_ADD(itemRepo, "E3405280", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.FRONT. SUPERSILENT ELS XS 60 476600");
            Item_ADD(itemRepo, "E3405569", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.BOK VISOR PRAWY+ZASLEPKA XS");
            Item_ADD(itemRepo, "E3405570", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.BOK VISOR LEWY+ZASLEPKA XS");
            Item_ADD(itemRepo, "E3405571", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.CARR.VISOR+STAFFA CHI.CARR.XS 60");
            Item_ADD(itemRepo, "E3405572", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.PUNT.FRONTALE PULL-OUT ESD XS 60");
            Item_ADD(itemRepo, "E3405573", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.CARR.VISOR+STAFFA CHI.CARR.XS 30\"");
            Item_ADD(itemRepo, "E3405574", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ.PUNT.FRONTALE PULL-OUT ESD XS 30\"");
            Item_ADD(itemRepo, "E3405623", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "201 ES5 CARC.60 L.LIGHT DX51D sald.tox");
            Item_ADD(itemRepo, "E3408923", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZGRZ. ES24 60CM - FRONT. FR8 ELSLIM XS");
            Item_ADD(itemRepo, "P3200214", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "REDUKCJA D.123-100   AE");
            Item_ADD(itemRepo, "R2300132", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KOSTKA ZACISKOWA 6X3 ELEM");
            Item_ADD(itemRepo, "004201327", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PROLUNGA LAMPADE PASC780 FPX");
            Item_ADD(itemRepo, "004201874", "9103", null, null, ItemTypeEnum.BuyedItem, null, "FILERIA IMP.ELETTR.P194 1M");
            Item_ADD(itemRepo, "004202557", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILERIA EC L=630MM DIS. N 10");
            Item_ADD(itemRepo, "004203453", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD LAMP HALOGEN. N 2 DIS.N 15");
            Item_ADD(itemRepo, "004203524", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD SILNIKA PIAT.LIGHT DIS.N 6");
            Item_ADD(itemRepo, "004203631", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "WIRES CONNECTION EXTERN.MOTOR N 16");
            Item_ADD(itemRepo, "004203807", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD LED WITCH DIS. 2");
            Item_ADD(itemRepo, "004203929", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "WIRES LED BI-GROUP DIS.N.25");
            Item_ADD(itemRepo, "02000000", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 2,9X6,5 FRTCN");
            Item_ADD(itemRepo, "02000014", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 3,5X6 TC TC OCYNK.SP");
            Item_ADD(itemRepo, "02000039", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 3,4X15TSPTC OCYNK.PHILLIPS");
            Item_ADD(itemRepo, "02000097", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 3,5X13TLTCZIN-SP.SPEC");
            Item_ADD(itemRepo, "02000100", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X13TSPTC2PRZINC");
            Item_ADD(itemRepo, "02000104", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X4,5TCTCZINC-SP.SP");
            Item_ADD(itemRepo, "02000112", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X9,5TSPTCZIN-SP.S.PF");
            Item_ADD(itemRepo, "02000118", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA 3,9x9,5 TSP TC N SP");
            Item_ADD(itemRepo, "02000124", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,2X13TC2PRZINC");
            Item_ADD(itemRepo, "02000129", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.3,2X9,5TCTCZINC-SP.SP");
            Item_ADD(itemRepo, "02000135", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT. 2,9X9,5FRTCZINSP-PA.FE");
            Item_ADD(itemRepo, "02000137", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X15 TC TC ZINC.");
            Item_ADD(itemRepo, "02000140", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,2X9,5 TC TC N");
            Item_ADD(itemRepo, "02000142", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.3,2X13-TPZ  PA.TRU.19050");
            Item_ADD(itemRepo, "02000147", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 4,8X9,5TLTCZIN-SP.SPE");
            Item_ADD(itemRepo, "02000166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT. 3,9X6 TL TC Z PUNT.SP");
            Item_ADD(itemRepo, "02000167", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4x8 TSTC OCYNK");
            Item_ADD(itemRepo, "02000196", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC 30x35 FILTR.RED. GRUB 0,02");
            Item_ADD(itemRepo, "02000202", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK 16x19 SP 0,07 12056");
            Item_ADD(itemRepo, "02000209", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK FOLIOWY \"K\" 07070 SP0,04");
            Item_ADD(itemRepo, "02000212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WORECZEK FOLIOWY 19X27 GRUB. 0,07");
            Item_ADD(itemRepo, "02000222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NIT 3,2X9,5 TS ALUMINIUM");
            Item_ADD(itemRepo, "02000287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC 70x100 GRUB.0,07");
            Item_ADD(itemRepo, "02000605", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "UCHWYT KABLA PRZYLEPNY WCK-460-01A4-RT");
            Item_ADD(itemRepo, "02000669", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRÊTKA M6X10 OCYNKOWANA");
            Item_ADD(itemRepo, "02000690", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "UCHWYT FE DO WKLADKI Z WEGL.AKT.");
            Item_ADD(itemRepo, "020006940", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZSZYWKA DO KARTONU AR1 3/4");
            Item_ADD(itemRepo, "02000712", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OPASKA ZACISKOWA NATURALNA 98mm RG-203");
            Item_ADD(itemRepo, "02000721", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA M4X7 ZELAZO Z.");
            Item_ADD(itemRepo, "02000726", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA  4MX7 MOSIADZ");
            Item_ADD(itemRepo, "02000731", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA ZEBATA Sred.4,3");
            Item_ADD(itemRepo, "02000736", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA PLASKA  SRED.D.5,2X15");
            Item_ADD(itemRepo, "02000749", "", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZAWÓR ZWROTNY ALUM.  123");
            Item_ADD(itemRepo, "02000753", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA ZEBATA  Sred.3,2X6X0,4 CZARNA");
            Item_ADD(itemRepo, "02000756", "", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZAWOR ZWROTNY ALUMINIUM SRED.150");
            Item_ADD(itemRepo, "02001106", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "WYPELNIACZ STYROPIANOWY      90X60X32");
            Item_ADD(itemRepo, "02001306", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BOK STYROPIANOWY P194    99036");
            Item_ADD(itemRepo, "02001369", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BOK STYROPIANOWY P195     175035");
            Item_ADD(itemRepo, "02001411", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "NAROZNIK STYROPIANOWY K26");
            Item_ADD(itemRepo, "02001627", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "LATERAL SIDE PACKAGING TILIA");
            Item_ADD(itemRepo, "02009102", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NASADKA REFLEKTORA HALOGEN. CR 84008");
            Item_ADD(itemRepo, "02010004", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POLIETIL.BOLLATO  450");
            Item_ADD(itemRepo, "02011013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SPREZYNA MOCUJACA P194 ZN   99022");
            Item_ADD(itemRepo, "02011019", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA Z PODKLADKA ZEBATA M4X9,8X4,4");
            Item_ADD(itemRepo, "02011020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.UNIW.SPAX 4,2X15 TCTC ZN");
            Item_ADD(itemRepo, "02011110", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X12 TCTC RADELK.MOSIADZ");
            Item_ADD(itemRepo, "02011117", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA PODWOJNA ZEBATA 5,5x16 n");
            Item_ADD(itemRepo, "02011127", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA D6,5 GROWER UNI 1751 DIN127");
            Item_ADD(itemRepo, "02011155", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WYSTEP MOCUJACY FILTR KRATKA P");
            Item_ADD(itemRepo, "02011212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA KLATKOWA M4 H8,3 C70");
            Item_ADD(itemRepo, "02011262", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZATRZASK ROLKOWY (KOMPLET)    244.01.903");
            Item_ADD(itemRepo, "02011303", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WTYK M4 SZESCIOKATNY  09688-00413 RoHS");
            Item_ADD(itemRepo, "02011312", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA AU.UNIV.SPAX 4,2X20 TCTC ZN");
            Item_ADD(itemRepo, "02011340", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZES.ZAWORU ZWROT.SR.150 PIERSCIEN MET.");
            Item_ADD(itemRepo, "02011341", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZAWOR ZWROTNY ALUMINIOWYSRED.200 NEW");
            Item_ADD(itemRepo, "02011358", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC SP0,015 1000x600 FS340");
            Item_ADD(itemRepo, "02011359", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC SP0,015 1200x700 FS340");
            Item_ADD(itemRepo, "02011376", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZAWIAS 3372 INOX OTWARCIE TWARDE");
            Item_ADD(itemRepo, "02011399", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFILO MAGNETICO+BIAD.650X15X2");
            Item_ADD(itemRepo, "02011422", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "MAGNES ŒR.16X4,5 CSN-16");
            Item_ADD(itemRepo, "02011560", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4 X23 TESTA D14  TORX");
            Item_ADD(itemRepo, "02011672", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OPASKA ZACISKOWA PLT 1,5 CV-150");
            Item_ADD(itemRepo, "02011959", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA KOLNIERZOWA M4X37  TC CRI");
            Item_ADD(itemRepo, "02011986T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE 3,9X13 TORX T20");
            Item_ADD(itemRepo, "02300126", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA             3x0,75 150cm");
            Item_ADD(itemRepo, "02300127", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD Z. 3X0,75 MT 1,5 S.SV");
            Item_ADD(itemRepo, "02300128", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA AUSTRALIA   3x0,75 160cm");
            Item_ADD(itemRepo, "02300130", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL ZASILANIA 3X0,75MT1,50 SS H05");
            Item_ADD(itemRepo, "02300155", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL BEZ WTYCZKI KLII      2x0,75 150cm");
            Item_ADD(itemRepo, "02300156", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL BEZ WTYCZKI CII       2x0,75 100cm");
            Item_ADD(itemRepo, "02300160", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA 2PCII       2x0,75 100cm");
            Item_ADD(itemRepo, "02300187", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEW.ZASI.2X0,75 M 2 WTYCZKA-2BIEG.KLII");
            Item_ADD(itemRepo, "02300249", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL SJT 3X18AWG+WTY.UL C.N 730 L1200");
            Item_ADD(itemRepo, "02300255", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL ZASIL.2X0,75 MT.1,5 Z WTYCZKA CLII");
            Item_ADD(itemRepo, "02300256", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL ZASIL. BEZ WTYCZKI 2X0,75 MT2 CLII");
            Item_ADD(itemRepo, "02300260", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA EU KLII     2x0,75 150cm");
            Item_ADD(itemRepo, "02300525", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZAROWKA RUROWA E14\" 230 - 240V   40W");
            Item_ADD(itemRepo, "02300787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CZUJNIK HEAT SENSOR P/N VENMAR 2455R-909");
            Item_ADD(itemRepo, "02300798", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "HALOGEN.F&F ZIK7+ZLACZ.6,3L160 SAT.OSRAM");
            Item_ADD(itemRepo, "02300804", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "HEAT SENSOR 36 TR12 2254 F170-15F AO414");
            Item_ADD(itemRepo, "02300806", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LAMP.HALOGEN.CR S1000 20WG.E. ZV800");
            Item_ADD(itemRepo, "02300831", "9103", null, null, ItemTypeEnum.BuyedItem, null, "FAR..F&F QUAD+CO.6,3 L140 OSR 15300A041");
            Item_ADD(itemRepo, "02300861", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "TRASF.WA 60 117 V 60W ELETTRONICO");
            Item_ADD(itemRepo, "02300891", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZARÓW.HALOG.DWUSTYK.12V 20W TRAS OSRAM");
            Item_ADD(itemRepo, "02300981", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOROW  POLIPROP V2 AE");
            Item_ADD(itemRepo, "02300982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COPRICONETTORE Nylon Nilamid AH2 FRHF2");
            Item_ADD(itemRepo, "02301031", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL TASMOWY PANELU STER.  DIS N 12");
            Item_ADD(itemRepo, "02301035", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PLASKI PRZEWOD PAN. STER.  L500 DIS:9");
            Item_ADD(itemRepo, "02301058", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PLASKI PRZEWOD PAN. STER. WITCH  DIS.4");
            Item_ADD(itemRepo, "02310286", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LAMP.ALOGENA TWISTLINE120V 50W GE");
            Item_ADD(itemRepo, "02310761A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA27 0006 CL.F 230V~50Hz 130W CCW");
            Item_ADD(itemRepo, "02310800A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EC20 1.2.0.01 CLF220-240V50HZCCW2,5¾F70W");
            Item_ADD(itemRepo, "02310816A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA40 0004 CL.F 230V 50Hz RA 135W");
            Item_ADD(itemRepo, "02310817A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA40 0004.1 CL.F 230V 50Hz RO 135W");
            Item_ADD(itemRepo, "02320346", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZAROWKA SWIECZKOWA E14 230-240V 28W");
            Item_ADD(itemRepo, "02320347", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "KONDENSATOR 2,5 ¾F CL.B P2");
            Item_ADD(itemRepo, "02320351", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "NEON EN SAV CFQ13W/827 G24Q ESPEN 03623");
            Item_ADD(itemRepo, "02320371", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "TERMINAL BUSHING DC-68-2-2");
            Item_ADD(itemRepo, "02320386", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "COND.5MF 425/475V 10000H C878BE24500AA5J");
            Item_ADD(itemRepo, "02320387", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "REFLEKTOR LED BEST 3V 700MA 2,1W");
            Item_ADD(itemRepo, "02320389", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZAROWKA SWIECZKOWA LED E14 4W 230-240V");
            Item_ADD(itemRepo, "02320408", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FERRITE RRC14070728MK5B");
            Item_ADD(itemRepo, "02320409", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FAR.LED SQUARE 2,5W");
            Item_ADD(itemRepo, "02501313", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.P194 CM52       99025");
            Item_ADD(itemRepo, "02501700", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KARTON.96-P194 CM.70   99038");
            Item_ADD(itemRepo, "02501701", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KARTON.96-P194 CM.52   99037");
            Item_ADD(itemRepo, "02502727", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "CARTON BOX US P195 50CM 2017 691X386X408");
            Item_ADD(itemRepo, "02502728", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SPACER CARDBOARD PAKAG.US P195 50CM 2017");
            Item_ADD(itemRepo, "02502732", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "CARTON BOX US P195 70CM 2017 871X386X408");
            Item_ADD(itemRepo, "02502884", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "AVVOLGENTE P PIATTAFORMA MOBILE BASSO");
            Item_ADD(itemRepo, "02502916", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.PM390 GEA JVC33");
            Item_ADD(itemRepo, "02502917", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMBALLO P991");
            Item_ADD(itemRepo, "02502935", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAK.KARTONOWE P195 CM.52 ELUX");
            Item_ADD(itemRepo, "02502939", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.P195 CM.70 ELUX");
            Item_ADD(itemRepo, "02502940", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.TILIA CM.52 ELUX");
            Item_ADD(itemRepo, "02502949", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.TILIA CM.70 ELUX");
            Item_ADD(itemRepo, "03114618", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. M.1 P520 - GRIGLIA RO");
            Item_ADD(itemRepo, "03114887", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. M.5 P500 - SUPPORTO FILTRO ALOG.");
            Item_ADD(itemRepo, "03115020", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PIASTRA PERIM. - P.580");
            Item_ADD(itemRepo, "03115022", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PIASTRA PERIM. - P.780");
            Item_ADD(itemRepo, "03115084", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PIASTRA PERIM. - P.580");
            Item_ADD(itemRepo, "03115085", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PIASTRA PERIM. - P.780");
            Item_ADD(itemRepo, "03115315", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PANN. FILTRO - P195 FPX US 52");
            Item_ADD(itemRepo, "03115316", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PANN. FILTRO - P195 FPX US 70");
            Item_ADD(itemRepo, "03118005", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L7. P740 - SUPPORTO FILTRO");
            Item_ADD(itemRepo, "03118025", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L7. P500 - SUPPORTO FILTRO");
            Item_ADD(itemRepo, "03118052", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. M.5 P500 - SUPPORTO FILTRO ALOG.");
            Item_ADD(itemRepo, "03119077", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PIASTRA PERIM. - P.780 FPX EM");
            Item_ADD(itemRepo, "03119078", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PIASTRA PERIM. - P.580 FPX EM");
            Item_ADD(itemRepo, "03119090", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. PIASTRA PERIM. - P.580 FPX EM E");
            Item_ADD(itemRepo, "03119091", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. M.5 PIASTRA PERIM. - P.780 FPX EMEC");
            Item_ADD(itemRepo, "03119141", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W2. L7. SUPP.FILTRO P500 ALOG. 52");
            Item_ADD(itemRepo, "03119142", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BL1. L7. SUPP.FILTRO P500 ALOG. 52 (F)");
            Item_ADD(itemRepo, "03127560", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. M.1 P520 - GRIGLIA RO");
            Item_ADD(itemRepo, "03127628", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L7. P740 - SUPPORT FILTRO KWADRAT");
            Item_ADD(itemRepo, "03127682", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L7. PM390 - GRIGLIA RE");
            Item_ADD(itemRepo, "03145045", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L5. TILIA - FRAME 70");
            Item_ADD(itemRepo, "03145049", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L7. SUPP.FILTRO P500 ALOG");
            Item_ADD(itemRepo, "03145051", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L5. TILIA - FRAME 52");
            Item_ADD(itemRepo, "03145273", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L7. P500 - SUPPORT FILTRO KWADRAT");
            Item_ADD(itemRepo, "03145794", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L7. PME300 - CORNICE");
            Item_ADD(itemRepo, "03200376", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZYCISK CHROMOWANY SRED.11 SCHOLT. AE");
            Item_ADD(itemRepo, "03201014", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LAMPKA KONTROLNA K.CZERWONY 186PK");
            Item_ADD(itemRepo, "03202108", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFIL PVC W KSZTACIE \"U\" CZERWONY");
            Item_ADD(itemRepo, "03202286", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZELOTKA KABLOWA  OCB-500 SRED.8");
            Item_ADD(itemRepo, "03202287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZELOTKA KABLOWA SRED.13 PGSB-1519A");
            Item_ADD(itemRepo, "03202442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZACISK KABLA CZARNY SR/SP1 SRB-R-3");
            Item_ADD(itemRepo, "03290499", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA KABLA  12061         AE");
            Item_ADD(itemRepo, "032904990", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA KABLA  12061         AU");
            Item_ADD(itemRepo, "03291015", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA NAKRETKI SILNIKA CLII  AE");
            Item_ADD(itemRepo, "03291043", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KABLA K186 86163 AE");
            Item_ADD(itemRepo, "03292003", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA PRZEWODÓW SILNIKAP194 2M AE 99032");
            Item_ADD(itemRepo, "03292017", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA KABLA ZASILANIA AE  US032");
            Item_ADD(itemRepo, "03292018", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI KABLA ZASILANIA  AE US033");
            Item_ADD(itemRepo, "032920180", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI KAB. ZASIL. AU US033");
            Item_ADD(itemRepo, "03292020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KABLA PUSZKI INST.ELE.AE US022");
            Item_ADD(itemRepo, "032920200", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KABLA PUSZKI INST. AU US022");
            Item_ADD(itemRepo, "03292099", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "REDUKCJA SRED.150-125 AE 09048");
            Item_ADD(itemRepo, "03292114", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KOLNIERZ 88-313CE - (AE) US411");
            Item_ADD(itemRepo, "03292137", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KOSTKI ZACISKOWEJ K  AE  07542");
            Item_ADD(itemRepo, "03292199", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA ZASILANIA 413 AE  145025");
            Item_ADD(itemRepo, "03292200", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KAB.PUSZ.STER.ELE.413 AE145027");
            Item_ADD(itemRepo, "03292201", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI ZASILANIA 413 145026");
            Item_ADD(itemRepo, "03292265", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KANAL WENTYL. P194 2-SILNIKOWY AE 99002");
            Item_ADD(itemRepo, "03292266", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA PUSZKI INST.ELEK.P194  AE  99007");
            Item_ADD(itemRepo, "03292270", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA NAKRETKI SILNIKA AE");
            Item_ADD(itemRepo, "03292273", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI ZASILANIA P194 AE   99008");
            Item_ADD(itemRepo, "03292274", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KANAL WENTYLACYJNY P194 1SILNIK AE 99001");
            Item_ADD(itemRepo, "03292290", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO PRZEWODU REFLEKTORA K AU");
            Item_ADD(itemRepo, "03292410", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LACZNIK RUROWY P194 1M AE99035");
            Item_ADD(itemRepo, "03292495", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA KONDENSATORA.425  AE  163051");
            Item_ADD(itemRepo, "03292499", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA KABLA ZATRZASK AE   132171");
            Item_ADD(itemRepo, "03292596", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZELOTKA KABLOWA SRED.8 NYLON 66 V2");
            Item_ADD(itemRepo, "03292631", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "REDUKCJA 150/125 K2000");
            Item_ADD(itemRepo, "03292847", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "NASADKA PIERSCIENIOWA DO CZUJNIKA");
            Item_ADD(itemRepo, "03293045", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D147X68 54 LOPATEK RO AE  163280");
            Item_ADD(itemRepo, "03293046", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D147X68 54 LOPATEK RA AE  163281");
            Item_ADD(itemRepo, "03293051", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA P195        163268");
            Item_ADD(itemRepo, "03293065", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SUWAK ZAMKAJACY ABS 115 C BL1");
            Item_ADD(itemRepo, "03293066", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "HAK ZAMYKAJACY ABS 115 C BL1");
            Item_ADD(itemRepo, "03293067", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WSPORNIK SILNIKA RE P520 AE");
            Item_ADD(itemRepo, "03293095", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA CONDENSATORE KHB PP V2");
            Item_ADD(itemRepo, "03293142", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "DLAWNICA IZOLACYJNA MAXIBLOCK JASN_SZARY");
            Item_ADD(itemRepo, "03293222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOR.ZENSKA NILAMID");
            Item_ADD(itemRepo, "03293227", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOR.MESKA. ANTINTR.V0");
            Item_ADD(itemRepo, "03293295", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO CAVO ALIMENTAZIONE SMART PP GR");
            Item_ADD(itemRepo, "03293330", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SUPP.MOTORE EC P520540 AE");
            Item_ADD(itemRepo, "03293336", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CONVOGLIATORE P194 1M AE BLACK");
            Item_ADD(itemRepo, "03293345", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "COVER BRACKET PLASTIC TILIA");
            Item_ADD(itemRepo, "03293354", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "REDUCTION D.120-100 PA");
            Item_ADD(itemRepo, "03293433", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OZNACZENIE LAMP 40W PP COPOLIMERO V2 N");
            Item_ADD(itemRepo, "03294548", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OPRAWA ZAWORU+DIVIDER SRED.200 AU");
            Item_ADD(itemRepo, "03294549", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SUPPORTO VALVOLA +DIVIDER D150 AU");
            Item_ADD(itemRepo, "03295008", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA ZABEZP.KONDENSAT. 425 AU 163051");
            Item_ADD(itemRepo, "03295038", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SUWAK ZAMYKAJACY BL1 NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "03295039", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "HAK ZAMYKAJACY BL1 NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "03295083", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SUWAK ZAMYKAJACY GR3 NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "03295084", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "HAK ZAMYKAJACY GR3 NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "03295264", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "FLANGIA D150 K2000");
            Item_ADD(itemRepo, "03295623", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OBEJMA KABLOWA ZATRZASKOWA HCS1-01");
            Item_ADD(itemRepo, "03300437", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "N.1 FILTR WEGL. D.210 W PUDELKU (AE)");
            Item_ADD(itemRepo, "03300480", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR WEGL. D.211 SERIA SP2000      1632");
            Item_ADD(itemRepo, "03300484", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR WEG.AKT. SRED.190 K2000 ALTE AE");
            Item_ADD(itemRepo, "03300489", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "NR.2 F.WEG. D.190 K2000 ALTE+TERMORETRAI");
            Item_ADD(itemRepo, "03300551", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR WEGOWY 2SZT. DO MINIBLOWERA");
            Item_ADD(itemRepo, "03301014", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PAPIEROWY P520/P540 52   251063");
            Item_ADD(itemRepo, "04302173", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "MASCHERA FORATURA BI-GROUP");
            Item_ADD(itemRepo, "04302501", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTR. OBSL. P195 FPX US");
            Item_ADD(itemRepo, "04302502", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTR. OBSL. P195 ES 52 SB");
            Item_ADD(itemRepo, "04302503", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTR. OBSL. P195 ES 70 SB");
            Item_ADD(itemRepo, "04306466", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ULOTKA INSTR.Z NORMAMI KLI/KLII");
            Item_ADD(itemRepo, "04306753", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "IN.OBSL.P520/540/550/560/580 NEU.S.NORME");
            Item_ADD(itemRepo, "04306808", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTR.OBSLUGI P520/540/550/560/580 J.ROS");
            Item_ADD(itemRepo, "04307442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA LIKWID.ODPADOW WEEE DIRECTIVE CEE");
            Item_ADD(itemRepo, "04307532", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA OBSLUGI US-PM390 /3");
            Item_ADD(itemRepo, "04307564", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA OBSLUGI US-PM500");
            Item_ADD(itemRepo, "04307565", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA OBSLUGI US-PM390 HS");
            Item_ADD(itemRepo, "04307658", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTR.OBSL.US-PM390 HS NUTONE-PREMIER");
            Item_ADD(itemRepo, "04307766", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INST.OBSL.BROAN EL.US-P780HS F.T.R.");
            Item_ADD(itemRepo, "04307787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA OBSLUGI P560 KRONA");
            Item_ADD(itemRepo, "04308215", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA GLEM/AIRLUX  ITALIA");
            Item_ADD(itemRepo, "04308217", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTR.OBSLUGI P520/540/550/560/580 NEU.");
            Item_ADD(itemRepo, "04308245", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA LY-VENT");
            Item_ADD(itemRepo, "04308246", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA LY-VENT");
            Item_ADD(itemRepo, "04308296", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA SMEG AUSTRALIA");
            Item_ADD(itemRepo, "04308297", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO NORME SMEG AUSTRALIA");
            Item_ADD(itemRepo, "04308318", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.US-PME300 HS");
            Item_ADD(itemRepo, "04308349", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA PORTOGALLO");
            Item_ADD(itemRepo, "04308400", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA BEST RUSSIA");
            Item_ADD(itemRepo, "04308422", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.INSTR.SERIE P PIATTAFORMA");
            Item_ADD(itemRepo, "04308434", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GARANZIA CONVENZIONALE GLEM");
            Item_ADD(itemRepo, "04308441", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.P580 SAUTER SHG511 VERS.2014");
            Item_ADD(itemRepo, "04308443", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.IS.P542..2014.BRANDT EL-IT-NL-SV-ES");
            Item_ADD(itemRepo, "04308444", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.IS.P542..2014.BRANDT FR-EN-DA-DE");
            Item_ADD(itemRepo, "04308451", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.SERIE P PIATTAFORMA THERMEX");
            Item_ADD(itemRepo, "04308471", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.IST.GLEM SERIE P PIATTAFORMA");
            Item_ADD(itemRepo, "04308484", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ULOTKA INF.NT.4.PREDK 680-655");
            Item_ADD(itemRepo, "04308508", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA BEST GRECIA");
            Item_ADD(itemRepo, "04308525", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.INSTR.SERIE P PIATTAFORMA");
            Item_ADD(itemRepo, "04308538", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTAL./USO GE JVC3300");
            Item_ADD(itemRepo, "04308544", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CARD GE USA 245D1499P001 6/15");
            Item_ADD(itemRepo, "04308580", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.SERIE P PIATTAFORMA HAFELE");
            Item_ADD(itemRepo, "04308581", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.P580-780 FPX KRONA");
            Item_ADD(itemRepo, "04308608", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO ISTR.SAVO PASC580-780 EC FINL");
            Item_ADD(itemRepo, "04308619", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO ISTR.SAVO PASC580-780 EC SVED");
            Item_ADD(itemRepo, "04308660", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO PASC580 FPX PB FINL.");
            Item_ADD(itemRepo, "04308663", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO P580 FPX PD EM FINL.");
            Item_ADD(itemRepo, "04308689", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJE BEZPIECZEÑSTWA PELGRIM/ATAG");
            Item_ADD(itemRepo, "04308690", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GWARANCJA PELGRIM");
            Item_ADD(itemRepo, "04308692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO MATTONCINO POLISTIR.MOTORE HF800");
            Item_ADD(itemRepo, "04308714", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO PASC580 FINLANDESE");
            Item_ADD(itemRepo, "04308715", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBRETTO SAVO P580 EM FINLANDESE");
            Item_ADD(itemRepo, "04308772", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTKA A4");
            Item_ADD(itemRepo, "04308775", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS AEG 801320720");
            Item_ADD(itemRepo, "04308777", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ZANUSSI 801320510");
            Item_ADD(itemRepo, "04308779", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA REJESTRACYJNA AEG 801320721");
            Item_ADD(itemRepo, "04308781", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ZANUSSI 801320644");
            Item_ADD(itemRepo, "04308782", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ROZSZERZONA GWARANCJA AEG 801320722");
            Item_ADD(itemRepo, "04308784", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ZANUSSI 801320650");
            Item_ADD(itemRepo, "04308785", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ELECTROLUX 801320578  S");
            Item_ADD(itemRepo, "04308786", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KSIAZKA SERWISOWA AEG 801320684  P");
            Item_ADD(itemRepo, "04308787", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK ZANUSSI 801320243  Y");
            Item_ADD(itemRepo, "04308788", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON 801320204  T");
            Item_ADD(itemRepo, "04308802", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI P550/P750 AEG");
            Item_ADD(itemRepo, "04308803", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO P550/P750 AEG");
            Item_ADD(itemRepo, "04308804", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI P550/P750 ELECTROLUX");
            Item_ADD(itemRepo, "04308805", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO P550/P750 ELECTROLUX");
            Item_ADD(itemRepo, "04308837", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "GWARANCJA ATAG 307225");
            Item_ADD(itemRepo, "04308838", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA INSTAL. ATAG/PELGRIM 713909");
            Item_ADD(itemRepo, "04308839", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA OBS£UGI ATAG 713910");
            Item_ADD(itemRepo, "04308840", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUKCJA OBSLUGI PELGRIM 74191");
            Item_ADD(itemRepo, "04308841", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "WARRANTY RUSSO AEG ELEC.ZANU.801320556 D");
            Item_ADD(itemRepo, "04308842", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.P550/P750 ELECTROLUX RUSSIA");
            Item_ADD(itemRepo, "04308843", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO P550/P750 ELECTROLUX RUSSIA");
            Item_ADD(itemRepo, "04308844", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERTIFICATO RUSSO ELECTROLUX 867351004");
            Item_ADD(itemRepo, "04308848", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON OEM-GB 801320605 Q");
            Item_ADD(itemRepo, "04308849", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI TILIA ELECTROLUX");
            Item_ADD(itemRepo, "04308850", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO TILIA ELECTROLUX");
            Item_ADD(itemRepo, "04308857", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI TILIA ZANUSSI");
            Item_ADD(itemRepo, "04308858", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO TILIA ZANUSSI");
            Item_ADD(itemRepo, "04308860", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI TILIA AEG");
            Item_ADD(itemRepo, "04308861", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO TILIA AEG");
            Item_ADD(itemRepo, "04308866", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTRUZIONI TILIA FAURE");
            Item_ADD(itemRepo, "04308867", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.USO TILIA FAURE");
            Item_ADD(itemRepo, "04308868", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "REGISTRATION CARD ELECTR. 2Y 801320639");
            Item_ADD(itemRepo, "04308869", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "EXTENDED WARRANTY ELECTR. UK 801320640");
            Item_ADD(itemRepo, "04308870", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "STICKERS ELECTROLUX 2Y  801320638");
            Item_ADD(itemRepo, "04308921", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERT.RUSSO ELECTR.RUC-SE.HA21.B.00612/19");
            Item_ADD(itemRepo, "04308930", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.IST.P195 FPX US NEW HALOGEN");
            Item_ADD(itemRepo, "04308933", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIBR.ISTR.US-PM500 NEW HALOGEN");
            Item_ADD(itemRepo, "04308934", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "INSTRUC. BOOKLET P580-780 FPX KRONA LED");
            Item_ADD(itemRepo, "04309965", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AD.MET.74X74 (1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04309973", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ZN.SAMOPRZYL.. ZÓLTY R.1018 100X300(1)");
            Item_ADD(itemRepo, "04309976", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA POLIESTER  AD.PER 5x2,5");
            Item_ADD(itemRepo, "04309980", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.MET.110X150(1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04309981", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.MET.28X15 (3) 3M UL (SP-330)");
            Item_ADD(itemRepo, "04309982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA K.BIALY 100X300(1)");
            Item_ADD(itemRepo, "04309983", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA ADESIVA BIANC.110X150(2)");
            Item_ADD(itemRepo, "04309984", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA K.BIALY  72X23 (1)");
            Item_ADD(itemRepo, "04309987", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ZNACZ.SAMOPRZYL. BIALY 34X10 4(3)");
            Item_ADD(itemRepo, "04309988", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA SAMOPRZYL.BIALA 110X150(1)");
            Item_ADD(itemRepo, "04309989", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA K.BIALY 76X40(1)");
            Item_ADD(itemRepo, "04309990", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.MET.40X76(1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04309991", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.MET.34X10 (2) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04316956", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA HVI - 990716881");
            Item_ADD(itemRepo, "04318106", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY  NR 8106");
            Item_ADD(itemRepo, "04318107", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY N.8107");
            Item_ADD(itemRepo, "04318111", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY N.8111");
            Item_ADD(itemRepo, "04318114", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY NR 8114");
            Item_ADD(itemRepo, "04318136", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY N.8136");
            Item_ADD(itemRepo, "04318161", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TARGA SCHEMA ELETT. N.8161 BIANCA");
            Item_ADD(itemRepo, "04318692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY  N.499");
            Item_ADD(itemRepo, "04318973", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY BIALY  NR.8973 US");
            Item_ADD(itemRepo, "04318975", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY BIALY  N.8975 US");
            Item_ADD(itemRepo, "04318976", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY BIALY  N.8976 US");
            Item_ADD(itemRepo, "04318995", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY BIALY  N.8995 US");
            Item_ADD(itemRepo, "04333895", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TABL.\"WARNING 2004\" KANT-LY CZERWONA");
            Item_ADD(itemRepo, "04334030", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TABLICZKA OPAKOWANIA/OSTRZEGAWCZA  KRONA");
            Item_ADD(itemRepo, "04341606", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.GWAR.\"WARRANTY STICKER\" LY-VENT");
            Item_ADD(itemRepo, "04343684", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.IMB.-G.E.JVC 3300 JSA");
            Item_ADD(itemRepo, "04344768", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AVVERTENZA Prop65 USA TRASPARENTE");
            Item_ADD(itemRepo, "04400331", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA SAMOPRZYLEPNA cULus 5000109");
            Item_ADD(itemRepo, "04404512", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA ECOLABEL TURCJA");
            Item_ADD(itemRepo, "04405212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA ECOLABEL UKRAINA");
            Item_ADD(itemRepo, "04405395", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA ECOLABEL A+++ NEUTRALNA");
            Item_ADD(itemRepo, "04500000", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PRZEZROCZYSTA 60X50");
            Item_ADD(itemRepo, "04500012", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PAPIEROWA 19mm x 50m");
            Item_ADD(itemRepo, "04500013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA TESA ART.4298 STR.50X66 NIEBIESKA");
            Item_ADD(itemRepo, "04500019", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PAPIEROWA M50X75");
            Item_ADD(itemRepo, "04500023", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA 75mm/50m EKO-TECH 23182");
            Item_ADD(itemRepo, "04500037", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PRZYLEPNA OSTRZEGAWCZA 54m");
            Item_ADD(itemRepo, "04808136", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8136");
            Item_ADD(itemRepo, "04808169", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8169");
            Item_ADD(itemRepo, "04808692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8692");
            Item_ADD(itemRepo, "04808999", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "FOGLIO SCHEMA ELETTRICO 8999");
            Item_ADD(itemRepo, "06002013U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.932,933 - mot. EB40 02310219a");
            Item_ADD(itemRepo, "06002114U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.932,933 - mot.EP40 02310260A");
            Item_ADD(itemRepo, "06002195U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.932,933 - mot. EP45 02310713a");
            Item_ADD(itemRepo, "06002201U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.925,926 - mot.EP45 02310296a");
            Item_ADD(itemRepo, "06002202U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.614 - mot. EB40 02310746A");
            Item_ADD(itemRepo, "06002203U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW.conv.295,296 - mot. EP45 02310713a");
            Item_ADD(itemRepo, "06002213U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.615 - mot. EA27 02310266a");
            Item_ADD(itemRepo, "06002240U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOWER conv.616 - mot. EB40 02310769a");
            Item_ADD(itemRepo, "06002242U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.615 - mot. EA37 02310159a");
            Item_ADD(itemRepo, "06002244U", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOW. conv.615 - mot. EA37 02310120a");
            Item_ADD(itemRepo, "06002283", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP LINE OF BLOWERS").Id, ItemTypeEnum.BuyedItem, null, "BLOWER conv.431 - mot. R50 02310785K");
            Item_ADD(itemRepo, "06002290", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "EMC BLOWER HEI-SK300");
            Item_ADD(itemRepo, "06002302", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "EMC ASS.MINI BLOWER HEI-SL400");
            Item_ADD(itemRepo, "06002303", "9103", null, null, ItemTypeEnum.BuyedItem, null, "BLOW. conv.616 - mot. EB25 02310812");
            Item_ADD(itemRepo, "06002304", "9103", null, null, ItemTypeEnum.BuyedItem, null, "BLOW. conv.614 - mot. EB25 02310813 K");
            Item_ADD(itemRepo, "06002309U", "9103", null, null, ItemTypeEnum.BuyedItem, null, "BLOW. conv.925,926 - mot.EP45 02310694A");
            Item_ADD(itemRepo, "06002314", "9103", null, null, ItemTypeEnum.BuyedItem, null, "HF 800.1 Conv.Plast.S2011cond.5mF+Cutoff");
            Item_ADD(itemRepo, "06102631", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE SENSORE PIATT-B DIS No13");
            Item_ADD(itemRepo, "06102676", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.WIRES CONNEC.2 LEDS P.A JOINT N 22");
            Item_ADD(itemRepo, "06102758", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COM.P580 GR3 4V 08080A259 ELX");
            Item_ADD(itemRepo, "06102759", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COM.P580 BL1 4V 08080A259 ELX");
            Item_ADD(itemRepo, "06102760", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COM.P580 W2 4V 08080A259 ELX");
            Item_ADD(itemRepo, "06102761", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COM.P580 GR3 3V 08080A258 AEG");
            Item_ADD(itemRepo, "06102762", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COM.P580 GR3 3V 08080A258 ELX");
            Item_ADD(itemRepo, "06102763", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550 P-C GR3 EB25D2SL AEG");
            Item_ADD(itemRepo, "06102781", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.PIATT. LIGHT NO-COND.");
            Item_ADD(itemRepo, "06102904", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDI P580 W2 RoHS P.B. 08080A211");
            Item_ADD(itemRepo, "06102917", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PANEL STER. P580 GR3 RoHS P.B. 08080A211");
            Item_ADD(itemRepo, "06102918", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.COM. PASC580 FPX GR3 RoHS PIATT.B");
            Item_ADD(itemRepo, "06102919", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.COM.PASC580 FPX GR3 RoHS P-B BLU");
            Item_ADD(itemRepo, "06102928", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PANEL STEROWANIA P580 BL1 RoHS PIATT.B");
            Item_ADD(itemRepo, "06102938", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMAN P580 GR3RoHS P.B.08080A216 720");
            Item_ADD(itemRepo, "06102940", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDI PASC580 P-B GR3 08080A211");
            Item_ADD(itemRepo, "06102952", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.COM. P580 FPX GR3 RoHS W.S.P-D");
            Item_ADD(itemRepo, "06102956", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.PIATT. LIGHT EB BP-775");
            Item_ADD(itemRepo, "06102960", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA. I.E. PLAT. LIGHT HF8");
            Item_ADD(itemRepo, "06102961", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.COM.PASC580 FPX ELSD GR3 P-LIGHT");
            Item_ADD(itemRepo, "06102962", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.COMANDI L=700MM P-LIGHT");
            Item_ADD(itemRepo, "06102973", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.SMART PU LED");
            Item_ADD(itemRepo, "06102992", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE NEW SENSORE P-B");
            Item_ADD(itemRepo, "06103021", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.LED DRIVER BI-GROUP (02300458)");
            Item_ADD(itemRepo, "06103069", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.INST.ELEKTR.STEROW. P580  GR3 RoHS");
            Item_ADD(itemRepo, "06103070", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.INST.ELEKTR.STEROWANIA P580  W2 RoHS");
            Item_ADD(itemRepo, "06108882", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.ZAR.HAL.US.NAP.SIEC.WYJSCIE BOCZNE");
            Item_ADD(itemRepo, "06141605", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P560 M1 L2 EL RoHS V.2015");
            Item_ADD(itemRepo, "06141606", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.COMANDI P580 CE  P-D SAVO");
            Item_ADD(itemRepo, "06142990", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550 SE GR3 P-C 4W");
            Item_ADD(itemRepo, "06142991", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550 SL EB25 D2 GR3 ELUX");
            Item_ADD(itemRepo, "06142992", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550 SL EB25 D2 W2 ELUX");
            Item_ADD(itemRepo, "06142993", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550 SL EB25 D2 BL1 ELUX");
            Item_ADD(itemRepo, "06142999", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 52 GR3 ELECTROLUX");
            Item_ADD(itemRepo, "06143000", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 70 GR3 ZANUSSI");
            Item_ADD(itemRepo, "06143193", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 70 GR3 ELECTROLUX");
            Item_ADD(itemRepo, "06143477", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESPOL INST.ELEKTR. US-PM500");
            Item_ADD(itemRepo, "06143478", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.INST.ELEKTR. PM390 HS S.USA");
            Item_ADD(itemRepo, "06143513", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "WIAZKA OKABLOW. P195 FPX US 52");
            Item_ADD(itemRepo, "06143514", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "WIAZKA OKABLOW. P195 FPX 2M US 70");
            Item_ADD(itemRepo, "06143515", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "WIAZKA OKABLOW. P195 ES HS 120/60 S.USA");
            Item_ADD(itemRepo, "06143524", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 52 GR3 AEG");
            Item_ADD(itemRepo, "06143525", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 52 GR3 FAURE");
            Item_ADD(itemRepo, "06143526", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.TILIA 52 GR3 ZANUSSI");
            Item_ADD(itemRepo, "06144768", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "Z.IE.US-P780 HSREF.NAP.SIE.WT.AMERYK.GR3");
            Item_ADD(itemRepo, "06144982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P740 2M E2 SS GR3 RoHS");
            Item_ADD(itemRepo, "06144992", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550 P-C SE R7031 4W");
            Item_ADD(itemRepo, "06144994", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550 SE W2 P-C 4W");
            Item_ADD(itemRepo, "06144995", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P560 M1E2EL 4W");
            Item_ADD(itemRepo, "06144996", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P560 M1 A2 EL  ECO");
            Item_ADD(itemRepo, "06144997", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P520 1M GR3 4W");
            Item_ADD(itemRepo, "06145063", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E. PM390 BL1");
            Item_ADD(itemRepo, "06145220", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.WIAZKI. PIATT.B SCHEDA STD MOT,EB/EP");
            Item_ADD(itemRepo, "06145232", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E.PIATT.B SCHEDA STD LED MOT.EB/EP");
            Item_ADD(itemRepo, "06145240", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.I.E. PIATT.B SCH.BASE MOT.EXT LED");
            Item_ADD(itemRepo, "06145299", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P195 US 1 MOTORE");
            Item_ADD(itemRepo, "06145300", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P195 US 2 MOTORI");
            Item_ADD(itemRepo, "06145637", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.PME HS  120/60 S.USA");
            Item_ADD(itemRepo, "06145640", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550  SE GR3 P-C 28W");
            Item_ADD(itemRepo, "06145668", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P550 SE W2 PIATT_C");
            Item_ADD(itemRepo, "06145692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P520 ECODESIGN3 GR3 4W");
            Item_ADD(itemRepo, "08080892", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "AUX DRIVER 0-10V EXT-MOTOR");
            Item_ADD(itemRepo, "08087039", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR ALUMINIOWY US-PM500 UL METAL.L");
            Item_ADD(itemRepo, "08087153", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLUSZCZ.Z/UCHWYT P700 2F UL");
            Item_ADD(itemRepo, "08087198", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR CZUJNIKA      163467");
            Item_ADD(itemRepo, "08087284", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLUSZCZ.Z/UCHWYTEM P500 1F");
            Item_ADD(itemRepo, "08087285", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLUSZCZ.Z/UCHWYTEM P500 2F");
            Item_ADD(itemRepo, "08087287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTL.Z/UCHW-STD4+1 XS P500 2F");
            Item_ADD(itemRepo, "08087288", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTL.Z/UCHW-STD 4+1XS P700 2F");
            Item_ADD(itemRepo, "08087527", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLU. METAL.V.2011 414 60");
            Item_ADD(itemRepo, "08087692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.FILTRO ANTIGR.P780  SMEG");
            Item_ADD(itemRepo, "08087693", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.FILTRO ANTIGR.P580  SMEG");
            Item_ADD(itemRepo, "08087694", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGR.PME300 199X399");
            Item_ADD(itemRepo, "08087758", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO HONEI P991 285,5X173");
            Item_ADD(itemRepo, "08087768", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO INOX 469X170,5");
            Item_ADD(itemRepo, "08087769", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO ANTIGRASSO INOX 324X170,5");
            Item_ADD(itemRepo, "08087773", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO C/MANIGLIA TILIA 1F 488,5x183");
            Item_ADD(itemRepo, "08087775", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTRO C/MANIGLIA TILIA 222,5x183");
            Item_ADD(itemRepo, "08087922", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR ALUMINIOWY  2+1  PM250");
            Item_ADD(itemRepo, "08088362", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZES.ZAWOR.VR.D150 K2000 163610");
            Item_ADD(itemRepo, "08088378", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "Z.KOL.P.ZAW.Z.D150(AU)K2000S/G ZFLV50051");
            Item_ADD(itemRepo, "08092640", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.RACC. MET. D150+VALV+DIVIDER.AU");
            Item_ADD(itemRepo, "08092641", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.RACC. MET. D150 +DIVIDER AU");
            Item_ADD(itemRepo, "08093332", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZESPOL WSP.PRZEN.POW.P195FPX2M70 INSERTI");
            Item_ADD(itemRepo, "08094310", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "FLAT PACKAGING SET US P195 2017");
            Item_ADD(itemRepo, "08094365", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.FAST INSTALL.HOOD TORX");
            Item_ADD(itemRepo, "08094367", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-00 COVER BOX BLOWER DX51D 52 TILIA");
            Item_ADD(itemRepo, "08094371", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.COVER BOX BLOWER DX51D 70 TILIA");
            Item_ADD(itemRepo, "08094500", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARCASSA P991+GRD");
            Item_ADD(itemRepo, "08094506", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC..UNIF..PM500 S/AGG.FM ELZ+GRD");
            Item_ADD(itemRepo, "08094507", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.UNIF.P500 S/AGG.FM ELZ+GRD");
            Item_ADD(itemRepo, "08094509", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.UNIF.P700 S/AGG. ELZ FM+GRD");
            Item_ADD(itemRepo, "08094512", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARCASSA UNIF.C/AGG..P197 ELZ+GRD");
            Item_ADD(itemRepo, "08094516", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC. P195 FPX US 52+GRD");
            Item_ADD(itemRepo, "08094517", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC. P195 FPX US 1M 70+GRD");
            Item_ADD(itemRepo, "08094518", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC. P195 FPX US 2M 70+GRD");
            Item_ADD(itemRepo, "08094519", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.P195ES70 HS BOCCA Q.+GRD");
            Item_ADD(itemRepo, "08094521", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.UNIF.P500 FPX FM ELZ+GRD");
            Item_ADD(itemRepo, "08094522", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.UNIF.P700 FPX FM ELZ+GRD");
            Item_ADD(itemRepo, "08094523", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC..UNIF..P780 US  ELZ FM+GRD");
            Item_ADD(itemRepo, "08094524", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC..UNIF..PM300  S/AGG.HS E.S.+GRD");
            Item_ADD(itemRepo, "08094526", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ASS.CARC.UNIF.P791 FPX FM ELZ+GRD");
            Item_ADD(itemRepo, "E3246400", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "107s MANT. UNIFICATO.ELZ 52");
            Item_ADD(itemRepo, "E3246402", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "107z MANT. UNIFICATO.ELZ 70");
            Item_ADD(itemRepo, "E3253338", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "L.938/xxx- TR.STAFFA PSMART P591-791 ELZ");
            Item_ADD(itemRepo, "E3254707", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "L.xxx/xxx STAFFA FISS. CONNETTORE");
            Item_ADD(itemRepo, "E3254708", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "L.901/390 STAFFA FISS. CONNECTTORE");
            Item_ADD(itemRepo, "E3334250", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "GIECIE PUSZKI UKL.ZASILANIA 89US 21076");
            Item_ADD(itemRepo, "E3334252", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "GIECIE POKRYWY PUSZ.UKL.ZASIL.89US 21077");
            Item_ADD(itemRepo, "E3344830", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.SUPP.CONV.P740 2M 70 ELZ");
            Item_ADD(itemRepo, "E3344831", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.907/1276 STAFFA AGGANCIO P ELZ");
            Item_ADD(itemRepo, "E3344842", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "101z CORNICE P500 XS - KWADRAT - 4/4");
            Item_ADD(itemRepo, "E3344843", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.938/425 STAFFA COM.SLIDER P500/700");
            Item_ADD(itemRepo, "E3344845", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.907/221 STAFFA COM.ELETTR.P500/700 ELZ");
            Item_ADD(itemRepo, "E3344849", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "105z PI.SUPP.CONV.P520 1M 52 ELZ");
            Item_ADD(itemRepo, "E3344850", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "107 PI.SUPP.CONV.P540 2M 52 ELZ");
            Item_ADD(itemRepo, "E3344909", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.907/1102 STAFFA AGGANCIO SUPP.P ELZ");
            Item_ADD(itemRepo, "E3345063", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "103 CORNICE P700 XS KWADRAT - 5/5F");
            Item_ADD(itemRepo, "E3345066", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "105z CORNICE P700 XS HALOGEN - 6/6F");
            Item_ADD(itemRepo, "E3345077", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-15 PI.SUPP.CONV.P720 1M 70 ELZ");
            Item_ADD(itemRepo, "E3346400", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GIECIE POKR.STA.SERIE P500/197 B/ZAM.ELZ");
            Item_ADD(itemRepo, "E3346401", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "206s TESTATA UNIF.SERIE P500/197 ELZ");
            Item_ADD(itemRepo, "E3346402", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "GIECIE POKRYWY STAN.SERIE P700 B/ZAM.ELZ");
            Item_ADD(itemRepo, "E3346403", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "206s TESTATA UNIF.SERIE P700 ELZ");
            Item_ADD(itemRepo, "E3346942", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "73- STAFF AGG. FISSA P197-1 ELZ E3353686");
            Item_ADD(itemRepo, "E3347130", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "101z CORNICE P500 XS - HALOGEN ASC - 4/4");
            Item_ADD(itemRepo, "E3347131", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.STAFFA SENSORE+BRACKET P ASC ELZ");
            Item_ADD(itemRepo, "E3347570", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-00 STAFFA SUPP.BRACKET.US-P780 ELZ");
            Item_ADD(itemRepo, "E3347852", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-00 PI.STAFFA COM. P195 FPX US 52");
            Item_ADD(itemRepo, "E3347859", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.CORNICE P195ES52 XS");
            Item_ADD(itemRepo, "E3347860", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-29 PI.CORNICE P195ES70 XS");
            Item_ADD(itemRepo, "E3348670", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*PI.2997 STAFFA COM.PU  PM500");
            Item_ADD(itemRepo, "E3348671", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "104z CORNICE PM500 ALOG. XS - 2/2");
            Item_ADD(itemRepo, "E3348672", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*1-750 RIFLETTORE  PM390 XS");
            Item_ADD(itemRepo, "E3348849", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.938/300 STAFFA FAR. ALOGENE PM500 ELZ");
            Item_ADD(itemRepo, "E3348860", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#STAFFA COM.ELETTR. P580 XS");
            Item_ADD(itemRepo, "E3349126", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.907/928 NEW STAFFA AGG. CORN.P ELZ");
            Item_ADD(itemRepo, "E3349127", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*PI.STAFFA FAR.ALOG.T.RETE  PM500 ELZ");
            Item_ADD(itemRepo, "E3349275", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#PI.STAFFA FAR.ALOG 2007  P500/700 ELZ");
            Item_ADD(itemRepo, "E3349350", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "104z RACCORDO METALLICO D200");
            Item_ADD(itemRepo, "E3349376", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "104z RACCORDO METALLICO D150");
            Item_ADD(itemRepo, "E3350233", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-00 PI.SCAT.CONN.ALIM.USA");
            Item_ADD(itemRepo, "E3350829", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-00 STAFFA TRASF.EL ES");
            Item_ADD(itemRepo, "E3351266", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*PI.STAFFA FISS.TRASF.TCI PM");
            Item_ADD(itemRepo, "E3351462", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-375 PI.STAFFA COM.SLIDER PME300");
            Item_ADD(itemRepo, "E3351463", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-240 RIFLETTORE SX LAMP.NEON PME300");
            Item_ADD(itemRepo, "E3351464", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#PI.RIFLETTORE DX LAMP.NEON PME300");
            Item_ADD(itemRepo, "E3351830", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#1-00 STAFFA FISS.CONV. GIATT.A");
            Item_ADD(itemRepo, "E3352943", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#CABLE COVER PXXX IE P-B");
            Item_ADD(itemRepo, "E3352954", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.2997 STAFFA FISS.SCAT.ALIM. PM390 GE");
            Item_ADD(itemRepo, "E3352955", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-240 PI.CABLE COVER PXXX IE EM P-D");
            Item_ADD(itemRepo, "E3352998", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.PANNELLO LAMPADE P991 XS");
            Item_ADD(itemRepo, "E3352999", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-00 STAFFA COMANDI P991 ELZ");
            Item_ADD(itemRepo, "E3353169", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "101z CORNICE P500 XS - HALOGENNOWY - 4/4");
            Item_ADD(itemRepo, "E3353177", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "105z CORNICE P700 XS HALOGEN NOWY - 5/5F");
            Item_ADD(itemRepo, "E3353222", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#  STAFFA SUPP.CONV.PM");
            Item_ADD(itemRepo, "E3353319", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-279 RINFORZO CARCASSA P591 ELZ");
            Item_ADD(itemRepo, "E3353320", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*PI.STAFFA FISS.IE P-LIGHT P591 ELZ");
            Item_ADD(itemRepo, "E3353323", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*PI.PANNELLO LAMPADE P591 XS");
            Item_ADD(itemRepo, "E3353325", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-58 PANNELLO LAMPADE P791 XS");
            Item_ADD(itemRepo, "E3353334", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*PI.PANNELLO LAMPADE P591 PU A2 XS");
            Item_ADD(itemRepo, "E3353335", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*PI.PANNELLO LAMPADE P791 PU A2 XS");
            Item_ADD(itemRepo, "E3353336", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*PI.SUPP.STAFFA FISS.PSMART P591-791 ELZ");
            Item_ADD(itemRepo, "E3353470", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.STAFFA COMANDI P591 ELD ELZ");
            Item_ADD(itemRepo, "E3353471", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.PANNELLO LAMPADE P591 ELD XS");
            Item_ADD(itemRepo, "E3353472", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.PANNELLO LAMPADE P791 ELD XS");
            Item_ADD(itemRepo, "E3353473", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.PANNELLO LAMPADE P991 ELD XS");
            Item_ADD(itemRepo, "E3353546", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "PI.SWITCH SUPPORT ON/OFF P560");
            Item_ADD(itemRepo, "E3353718", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.450/400 PI.BRACKET CONTROL TILIA");
            Item_ADD(itemRepo, "E3353745", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BEND.BRACKET CAPACITOR P520 ECOD.");
            Item_ADD(itemRepo, "E3403870", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD.CARC.UNIF.P500 S/AGG.FM ELZ");
            Item_ADD(itemRepo, "E3403872", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD.CARC.UNIF.P700 S/AGG. ELZ FM");
            Item_ADD(itemRepo, "E3403877", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD.CARC.UNIF.C/AGG..P197 ELZ");
            Item_ADD(itemRepo, "E3403940", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-13 SAT. PANN. XS P195 FPX US 52");
            Item_ADD(itemRepo, "E3403941", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-12 SAT. PANN. XS P195 FPX US 70");
            Item_ADD(itemRepo, "E3404284", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#SAT. CORNICE PASC580 CM53 ESD.XS");
            Item_ADD(itemRepo, "E3404294", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-12 SAT. CORNICE PASC780 CM70 ESD.XS");
            Item_ADD(itemRepo, "E3405290", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "# SAT. FILTRO PERIM. PASC580 2014");
            Item_ADD(itemRepo, "E3405291", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SAT. FILTRO PERIM. PASC780 2014");
            Item_ADD(itemRepo, "E3405410", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-85 MANTELLO P580 CE 52 ELZ H100");
            Item_ADD(itemRepo, "E3405411", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD.MANTELLO P780 CE 70 ELZ H100");
            Item_ADD(itemRepo, "E3405523", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD.COP. SCAT. ALIM. PM390 GE");
            Item_ADD(itemRepo, "E3405526", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD.CORNICE P991 XS");
            Item_ADD(itemRepo, "E3405551", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "*SALD.CORNICE P591 XS");
            Item_ADD(itemRepo, "E3405552", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-16 SALD.CORNICE P791 XS");
            Item_ADD(itemRepo, "E3405558", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SALD.CARC.UNIF.P791 FPX FM ELZ");
            Item_ADD(itemRepo, "E3405682", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SAT.PANN.US P195 FPX 2019 XS 52");
            Item_ADD(itemRepo, "E3405683", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SAT.PANN.US P195 FPX 2019 XS 70");
            Item_ADD(itemRepo, "E3409938", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-13 SAT. CORNICE P195 FPX US 52");
            Item_ADD(itemRepo, "E3409939", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-12 SAT. CORNICE P195 FPX US 70");
            Item_ADD(itemRepo, "E3448863", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SAT. FILTRO PERIM. PASC580 FORI QUADR.");
            Item_ADD(itemRepo, "E3448884", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SAT. FILTRO PERIM. P580 FORI ROTONDI");
            Item_ADD(itemRepo, "E3448885", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "SAT. FILTRO PERIM. P780 FORI ROTONDI");
            Item_ADD(itemRepo, "E3448899", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-00 SAT. FILTRO PASC780 FORI QUADR.");
            Item_ADD(itemRepo, "R2300131", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KOSTKA ZACISKOWA 3,5X2 ELEM.LEG.N");
            Item_ADD(itemRepo, "R2300132", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KOSTKA ZACISKOWA 6X3 ELEM");
            Item_ADD(itemRepo, "E3344850", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "z105 PI.SUPP.CONV.P540 2M 52 ELZ");
            Item_ADD(itemRepo, "E3344909", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "W.907/1102 STAFFA AGGANCIO SUPP.P ELZ");
            Item_ADD(itemRepo, "02000222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NIT 3,2X9,5 TS ALUMINIUM");
            Item_ADD(itemRepo, "02011013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SPREZYNA MOCUJACA P194 ZN   99022");
            Item_ADD(itemRepo, "02000104", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X4,5TCTCZINC-SP.SP");
            Item_ADD(itemRepo, "02000100", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X13TSPTC2PRZINC");
            Item_ADD(itemRepo, "03293067", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WSPORNIK SILNIKA RE P520 AE");
            Item_ADD(itemRepo, "02011019", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA Z PODKLADKA ZEBATA M4X9,8X4,4");
            Item_ADD(itemRepo, "03292270", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA NAKRETKI SILNIKA AE");
            Item_ADD(itemRepo, "02000097", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 3,5X13TLTCZIN-SP.SPEC");
            Item_ADD(itemRepo, "03291043", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KABLA K186 86163 AE");
            Item_ADD(itemRepo, "02000124", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,2X13TC2PRZINC");
            Item_ADD(itemRepo, "04318692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SCHEMAT ELEKTRYCZNY  N.499");
            Item_ADD(itemRepo, "02320389", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZAROWKA SWIECZKOWA LED E14 4W 230-240V");
            Item_ADD(itemRepo, "03295083", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SUWAK ZAMYKAJACY GR3 NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "03295084", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "HAK ZAMYKAJACY GR3 NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "02000112", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X9,5TSPTCZIN-SP.S.PF");
            Item_ADD(itemRepo, "03293051", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PLAFONIERA P195        163268");
            Item_ADD(itemRepo, "02000142", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.3,2X13-TPZ  PA.TRU.19050");
            Item_ADD(itemRepo, "04306753", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "IN.OBSL.P520/540/550/560/580 NEU.S.NORME");
            Item_ADD(itemRepo, "04306466", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ULOTKA INSTR.Z NORMAMI KLI/KLII");
            Item_ADD(itemRepo, "04308848", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "SERVICE BOOK COMMON OEM-GB 801320605 Q");
            Item_ADD(itemRepo, "04307442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA LIKWID.ODPADOW WEEE DIRECTIVE CEE");
            Item_ADD(itemRepo, "04308484", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ULOTKA INF.NT.4.PREDK 680-655");
            Item_ADD(itemRepo, "04308921", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "CERT.RUSSO ELECTR.RUC-SE.HA21.B.00612/19");
            Item_ADD(itemRepo, "04308400", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "KARTA GWARANCYJNA BEST RUSSIA");
            Item_ADD(itemRepo, "02501701", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KARTON.96-P194 CM.52   99037");
            Item_ADD(itemRepo, "04309982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA K.BIALY 100X300(1)");
            Item_ADD(itemRepo, "02001306", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "BOK STYROPIANOWY P194    99036");
            Item_ADD(itemRepo, "04309987", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ZNACZ.SAMOPRZYL. BIALY 34X10 4(3)");
            Item_ADD(itemRepo, "04331001", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA  - WYNIK KONCOWEJ PROBY -");
            Item_ADD(itemRepo, "04309976", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA POLIESTER  AD.PER 5x2,5");
            Item_ADD(itemRepo, "04405395", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA ECOLABEL A+++ NEUTRALNA");
            Item_ADD(itemRepo, "E3344830", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-15 PI.SUPP.CONV.P740 2M 70 ELZ");
            Item_ADD(itemRepo, "03292265", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KANAL WENTYL. P194 2-SILNIKOWY AE 99002");
            Item_ADD(itemRepo, "02310816A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA40 0004 CL.F 230V 50Hz RA 135W");
            Item_ADD(itemRepo, "02310817A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA40 0004.1 CL.F 230V 50Hz RO 135W");
            Item_ADD(itemRepo, "03293045", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D147X68 54 LOPATEK RO AE  163280");
            Item_ADD(itemRepo, "03293046", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D147X68 54 LOPATEK RA AE  163281");
            Item_ADD(itemRepo, "06144982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P740 2M E2 SS GR3 RoHS");
            Item_ADD(itemRepo, "R2300131", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KOSTKA ZACISKOWA 3,5X2 ELEM.LEG.N");
            Item_ADD(itemRepo, "02300156", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL BEZ WTYCZKI CII       2x0,75 100cm");
            Item_ADD(itemRepo, "03292266", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA PUSZKI INST.ELEK.P194  AE  99007");
            Item_ADD(itemRepo, "03292273", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRYWA PUSZKI ZASILANIA P194 AE   99008");
            Item_ADD(itemRepo, "04309965", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "TA.AD.MET.74X74 (1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04309990", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.MET.40X76(1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "03292003", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA PRZEWODÓW SILNIKAP194 2M AE 99032");
            Item_ADD(itemRepo, "03127628", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L7. P740 - SUPPORT FILTRO KWADRAT");
            Item_ADD(itemRepo, "08087153", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLUSZCZ.Z/UCHWYT P700 2F UL");
            Item_ADD(itemRepo, "02000287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC 70x100 GRUB.0,07");
            Item_ADD(itemRepo, "02501700", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OPAKOWANIE KARTON.96-P194 CM.70   99038");
            Item_ADD(itemRepo, "E3344849", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "105z PI.SUPP.CONV.P520 1M 52 ELZ");
            Item_ADD(itemRepo, "03293336", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CONVOGLIATORE P194 1M AE BLACK");
            Item_ADD(itemRepo, "02000749", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "ZAWÓR ZWROTNY ALUM.  123");
            Item_ADD(itemRepo, "02310800A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EC20 1.2.0.01 CLF220-240V50HZCCW2,5ľF70W");
            Item_ADD(itemRepo, "03293330", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SUPP.MOTORE EC P520540 AE");
            Item_ADD(itemRepo, "03291015", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA NAKRETKI SILNIKA CLII  AE");
            Item_ADD(itemRepo, "06145692", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ASS.IE.P520 ECODESIGN3 GR3 4W");
            Item_ADD(itemRepo, "03293095", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA CONDENSATORE KHB PP V2");
            Item_ADD(itemRepo, "02320347", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "KONDENSATOR 2,5 ľF CL.B P2");
            Item_ADD(itemRepo, "E3353745", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "BEND.BRACKET CAPACITOR P520 ECOD.");
            Item_ADD(itemRepo, "03292410", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LACZNIK RUROWY P194 1M AE99035");
            Item_ADD(itemRepo, "03292137", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KOSTKI ZACISKOWEJ K  AE  07542");
            Item_ADD(itemRepo, "03145273", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "MET. L7. P500 - SUPPORT FILTRO KWADRAT");
            Item_ADD(itemRepo, "08087284", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "FILTR PRZECIWTLUSZCZ.Z/UCHWYTEM P500 1F");
            Item_ADD(itemRepo, "E3345077", "9140", null, groupList.FirstOrDefault(x => x.Name == "GROUP BUFOR").Id, ItemTypeEnum.BuyedItem, null, "#2-15 PI.SUPP.CONV.P720 1M 70 ELZ");
            Item_ADD(itemRepo, "04309981", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.MET.28X15 (3) 3M UL (SP-330)");
            Item_ADD(itemRepo, "5000B019", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PALLET 80X110 C/TRATTAMENTO ISPM-15");
            Item_ADD(itemRepo, "03291015", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA NAKRETKI SILNIKA CLII  AE");
            Item_ADD(itemRepo, "03293045", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D147X68 54 LOPATEK RO AE  163280");
            Item_ADD(itemRepo, "03292194", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSL.NAKRET.WIRNIKA AE    145080");
            Item_ADD(itemRepo, "02000124", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,2X13TC2PRZINC");
            Item_ADD(itemRepo, "ZSGN00006", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "DRUT CYNA D.1MM RoHS CONFORM manual");
            Item_ADD(itemRepo, "03290343", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CONVOGLIAT.1M GI-86 AE 11080");
            Item_ADD(itemRepo, "02310766A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA37 0022.1 CL.F 230V~50Hz 135W 3V RO");
            Item_ADD(itemRepo, "004202046", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CAVI  MOTORE ES23 C/CONN.");
            Item_ADD(itemRepo, "03293043", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LOZYSKO SILNIKA SP2000 2M AE  163279");
            Item_ADD(itemRepo, "02011019", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA Z PODKLADKA ZEBATA M4X9,8X4,4");
            Item_ADD(itemRepo, "03293046", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D147X68 54 LOPATEK RA AE  163281");
            Item_ADD(itemRepo, "02502850", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SEPARATOR cm.94X113 KARTON TFC/242A");
            Item_ADD(itemRepo, "03292620", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CZ.PRAWA K.WENT.PLASTYK.SILNIK EA(AE)");
            Item_ADD(itemRepo, "03292621", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CZ.LEWA K.WENT. PLASTYK.SILNIK EA(AE)");
            Item_ADD(itemRepo, "03292270", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA NAKRETKI SILNIKA AE");
            Item_ADD(itemRepo, "02310266A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA27 07.340.50 C CL.F 220-240V~50/60HzCW");
            Item_ADD(itemRepo, "03292622", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK 18-LOPATKOWY K2000 AE");
            Item_ADD(itemRepo, "03292678", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA PRZEW.SILNIKA EA AE ZPRP50116");
            Item_ADD(itemRepo, "03290325", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CONVOGL.2M GI.900006  AE");
            Item_ADD(itemRepo, "004202042", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CAVI  MOTORE ES23 C/CONN.");
            Item_ADD(itemRepo, "03293067", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WSPORNIK SILNIKA RE P520 AE");
            Item_ADD(itemRepo, "02000712", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OPASKA ZACISKOWA NATURALNA 98mm RG-203");
            Item_ADD(itemRepo, "02310760A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA27 0006.1 CL.F 230V~50Hz 130W CW");
            Item_ADD(itemRepo, "02310761A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA27 0006 CL.F 230V~50Hz 130W CCW");
            Item_ADD(itemRepo, "004300566", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OKABLOWANIE SILNIKA K24");
            Item_ADD(itemRepo, "03292031", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SUPP.MOTORE 313-S15 AE 03232");
            Item_ADD(itemRepo, "02011204", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA ZEBATA M4X7 Z LBEM TRÓJKAT. 040033");
            Item_ADD(itemRepo, "02000653", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "BOCCOLA OTTONE D.4,4X6X8 US059");
            Item_ADD(itemRepo, "02000035", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X6 TC TC ZIG N");
            Item_ADD(itemRepo, "03295000", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK PODWOJNY D150 59P V2000 AU");
            Item_ADD(itemRepo, "02011128", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "DISTANZIALE 4,4X6X14,5XD15");
            Item_ADD(itemRepo, "02011672", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OPASKA ZACISKOWA PLT 1,5 CV-150");
            Item_ADD(itemRepo, "03295076", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK 59-LOP.MZ6 AU RO NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "02000731", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA ZEBATA Sred.4,3");
            Item_ADD(itemRepo, "03292017", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA KABLA ZASILANIA AE  US032");
            Item_ADD(itemRepo, "R2300132", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "KOSTKA ZACISKOWA 6X3 ELEM");
            Item_ADD(itemRepo, "04309991", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.MET.34X10 (2) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "03292015", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SUPP.CART.CA S35 SX 02128 AE");
            Item_ADD(itemRepo, "02011806", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET AF 3,9x16 TBL TC N ZINC.");
            Item_ADD(itemRepo, "03294080", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D150 59-LOP.PODW. V.95AE163013");
            Item_ADD(itemRepo, "02011808", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE AF 3,2x22 TCNB ZINCATA");
            Item_ADD(itemRepo, "02000135", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT. 2,9X9,5FRTCZINSP-PA.FE");
            Item_ADD(itemRepo, "02000295", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.PRAWA B.QUADRA WER.2011");
            Item_ADD(itemRepo, "02000296", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.LEWA B.QUADRA WER.2011");
            Item_ADD(itemRepo, "02011801", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET AF 3,9X13 TCTC ZN KARBOWANY");
            Item_ADD(itemRepo, "03290375", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "GRIGLIA VENT.CENTRI.(AE)163010");
            Item_ADD(itemRepo, "03290376", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WSPORNIK WKLADKI FILTRU Z WEG.AKTYW AE");
            Item_ADD(itemRepo, "03202288", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZEPUST KABLOWY SRED.20 PGSB-9A");
            Item_ADD(itemRepo, "02011620", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.LEWA FRES.WER.2011");
            Item_ADD(itemRepo, "02011619", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.PRAWA FRES.WER.2011");
            Item_ADD(itemRepo, "03290377", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WSPORNIK DO WKLADU Z WEGL.AKTYW. AU");
            Item_ADD(itemRepo, "03204177", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GUMA ANTYWIBR. 6x8x12   65 SHORE");
            Item_ADD(itemRepo, "02320219", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "FAS.F.2.8 x 0.8sez.0.5-1ot.30003.52.081");
            Item_ADD(itemRepo, "02320211", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "COPRIFASTON a scatto 2.8x0.8");
            Item_ADD(itemRepo, "03295001", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "KRATKA WENTYLACYJNA AU");
            Item_ADD(itemRepo, "02000118", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA 3,9x9,5 TSP TC N SP");
            Item_ADD(itemRepo, "02011932", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.PRAWA WER.2013");
            Item_ADD(itemRepo, "02011933", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.LEWA WER.2013");
            Item_ADD(itemRepo, "03295617", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROWADNICA X CONV.ALL  S2013");
            Item_ADD(itemRepo, "02310161A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB40 1559.13 CL.F 230-240V 50HZ 250W CW");
            Item_ADD(itemRepo, "02011212", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "NAKRETKA KLATKOWA M4 H8,3 C70");
            Item_ADD(itemRepo, "02320218", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "FASTON FEM.6,3x0,8");
            Item_ADD(itemRepo, "02320210", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "COPRIFASTON 6,3 SCAT.CT2002F");
            Item_ADD(itemRepo, "004201811", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO CONDENSATORE CE EMA 1000");
            Item_ADD(itemRepo, "03295615", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.DMUCHAWY PLAST.S2011 DX+SX EA BEST");
            Item_ADD(itemRepo, "03295606", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA NAKRETKI SILNIKA EA S2011");
            Item_ADD(itemRepo, "03295609", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PRZELOTKA KABLOWA X CONV. EB S2011");
            Item_ADD(itemRepo, "02300920", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TERMINALE FEMM. AMP 2807021");
            Item_ADD(itemRepo, "02310168A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB40 1559.33 CL.F230-240V50HZ 250W CW");
            Item_ADD(itemRepo, "02000469", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "STAFFA FISS.RACC.OLIO ZN + VITE 1/4 BSW");
            Item_ADD(itemRepo, "02000166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT. 3,9X6 TL TC Z PUNT.SP");
            Item_ADD(itemRepo, "02300412A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB40 0003.19 120V60HZ CL.F 250W 2.2A CW");
            Item_ADD(itemRepo, "03294151", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK 59-LOPAT.PODW.MZ8 AE RO ZGIR4902");
            Item_ADD(itemRepo, "02011203", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWIN.M4X15,6 SPIRALFORM 050015");
            Item_ADD(itemRepo, "02310231S", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6211.20CLB120V60HZ320W2.7A 25¾F250V");
            Item_ADD(itemRepo, "03202456", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "GUMA ANTYWIBRACYJNA 6X8X12  GR R.7001");
            Item_ADD(itemRepo, "02000137", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X15 TC TC ZINC.");
            Item_ADD(itemRepo, "02310219A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB40MTA0003.18 120V60HZ+CON.JST02310219A");
            Item_ADD(itemRepo, "03292645", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "CZESC PRAWA K.WENTYL.EB/EP PP ZCVP50088");
            Item_ADD(itemRepo, "03292646", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "CZESC LEWA K.WENTYL.EB/EP PP  ZCVP50089");
            Item_ADD(itemRepo, "03295614", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.DMUCHAWY PLAST.S2011 DX+SX EB BEST");
            Item_ADD(itemRepo, "02300233", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KONDENSATOR 25MF F.6,3X0,8(UL) 40X72");
            Item_ADD(itemRepo, "02320182", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CONN.FEMMINA AMP 280593 8 VIE");
            Item_ADD(itemRepo, "02310230A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6210.12 120V60Hz 360W2.9A 25¾F 250V");
            Item_ADD(itemRepo, "02502847", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SEPARATOR KART. cm118X77 TFC/222/B");
            Item_ADD(itemRepo, "02320166", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "KO.6,3uF 470V 10000h Cl.B  F.6,3X0,8 ARC");
            Item_ADD(itemRepo, "02310120A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA37 00.602C CL.F 230-240V~ 50Hz 155W CW");
            Item_ADD(itemRepo, "03293025", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK 20-LOP. ROVES. SP2000 AE  163259");
            Item_ADD(itemRepo, "02000129", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.3,2X9,5TCTCZINC-SP.SP");
            Item_ADD(itemRepo, "04309990", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYK.MET.40X76(1) 3M 7818 UL (SP-330)");
            Item_ADD(itemRepo, "04309982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA K.BIALY 100X300(1)");
            Item_ADD(itemRepo, "02000196", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC 30x35 FILTR.RED. GRUB 0,02");
            Item_ADD(itemRepo, "04640002", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "LIB.IST. WOLF EPGR/EBGR 2M 1M 99526932B");
            Item_ADD(itemRepo, "02502939", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCAT.IMB.P195 CM.70 ELUX");
            Item_ADD(itemRepo, "02001620", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SFRIDO IN POLISTIROLO");
            Item_ADD(itemRepo, "02502256", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "TAMPONE IMB.P650");
            Item_ADD(itemRepo, "5000B017", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PALLET ULTRA LIGHT");
            Item_ADD(itemRepo, "02011793", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE ZVIT00047 + ZINCATURA");
            Item_ADD(itemRepo, "02011156", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PROFILO AERSTOP CN24/A 10X6");
            Item_ADD(itemRepo, "02000209", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK FOLIOWY \"K\" 07070 SP0,04");
            Item_ADD(itemRepo, "02310797S", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6216..  CLF 220-240V50HZ 370W 6,3¾F");
            Item_ADD(itemRepo, "02320175", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CONN.FEMMINA 8 POLI MX508");
            Item_ADD(itemRepo, "02320302", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "TERMINALE EX5085-T  x CONN.FEM.");
            Item_ADD(itemRepo, "02320407", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "TERMINALE A PUNTALE SENZA AGGRAFF.GUAINA");
            Item_ADD(itemRepo, "02011321", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.PRAWA 2M FREZ+2 OTW.W.2011");
            Item_ADD(itemRepo, "02011322", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.LEWA 2M FREZ+2 OTW.W.2011");
            Item_ADD(itemRepo, "02011652", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "DADO FLANG.W3/16\" - 24 Wolf 99260488");
            Item_ADD(itemRepo, "02000287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WOREK PVC 70x100 GRUB.0,07");
            Item_ADD(itemRepo, "02011830", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X15 DENTELLATA ZINC.BIANCA TORX");
            Item_ADD(itemRepo, "03295436", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SCAT.CONDENSATORE PORTINOX+FERM.C/FISS.");
            Item_ADD(itemRepo, "06145595", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "IMP.EL.CONDENSATORE + ALIM. SS EB-EP");
            Item_ADD(itemRepo, "03295213", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PA.DX CONV.MOT.EB/EP PORTINOX");
            Item_ADD(itemRepo, "03295214", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PA.SX CONV.MOT.EB/EP PORTINOX");
            Item_ADD(itemRepo, "03295071", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK 59-LOP.MZ8 AU RO NILAMID AH2FRHF2");
            Item_ADD(itemRepo, "02000147", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET 4,8X9,5TLTCZIN-SP.SPE");
            Item_ADD(itemRepo, "03295352", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK 59P DOP.MZ8 AU VERS.2000 RO");
            Item_ADD(itemRepo, "02011517", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.LEWA 2M D7 NOWA");
            Item_ADD(itemRepo, "02011518", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.PRAWA 2M D7 NOWA");
            Item_ADD(itemRepo, "02310292A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6202.21 Cl.A 120V 60Hz 180W 1,8A 12");
            Item_ADD(itemRepo, "02310291A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6210.14 120V60Hz 360W2.9A 25¾F 250V");
            Item_ADD(itemRepo, "02000117", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,9X9,5 TSP TC N S");
            Item_ADD(itemRepo, "02011925", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.WLOT");
            Item_ADD(itemRepo, "02011926", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.AL.DMUCH.LOZYSKO SILNIKA");
            Item_ADD(itemRepo, "02011930", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4X16 TORX");
            Item_ADD(itemRepo, "02011955", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PLYTKA OSADCZA DIN 6799 - PN 85112 10MM");
            Item_ADD(itemRepo, "02300985", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "KONDENSATOR 25(mikro)F CBB61-A - UL");
            Item_ADD(itemRepo, "02310746A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB40 1559.87 CL.F 230-240V50HZ 250W CW");
            Item_ADD(itemRepo, "02012003T", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X6 DENTELL.TRILOBATA T20");
            Item_ADD(itemRepo, "03295405", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PROTEZIONE X CONNETTORE AMP 6 VIE");
            Item_ADD(itemRepo, "02310713A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP45 0007 CLB 120V60HZ 290W 2.4A CW");
            Item_ADD(itemRepo, "004203442", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CAVI MOT. S2011 BEST EA PIATT.\"C\" DIS.16");
            Item_ADD(itemRepo, "004203443", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "PRZEW.SILN.S2011BESTEA PIATT.\"A - B\" DI.14");
            Item_ADD(itemRepo, "02310753A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6220 CL.F 230V50HZ 260W 6.3¾F CW");
            Item_ADD(itemRepo, "02000736", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PODKLADKA PLASKA  SRED.D.5,2X15");
            Item_ADD(itemRepo, "02310131A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA27 0002 CL.B 120V~60H 155W CW");
            Item_ADD(itemRepo, "03293239", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA KOND. 25mF AU  NILAMID PLAT.");
            Item_ADD(itemRepo, "03293240", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "POKRY.PUSZK.KOND. 25mF AU  NILAMID PLAT.");
            Item_ADD(itemRepo, "02310769A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB40 1559.88 CL.F230-240V50HZ P-C 250WCW");
            Item_ADD(itemRepo, "03293227", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KONEKTOR.MESKA. ANTINTR.V0");
            Item_ADD(itemRepo, "03295616", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.DMUCHAW.S2011 DX+SX EB+BOX NA KOND.");
            Item_ADD(itemRepo, "02310159A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA37 01.603C CL.F 230-240V~50Hz 155W CW");
            Item_ADD(itemRepo, "02310770A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB20 1140.8.CL.F220-240V50HZ PI-A-B 180W");
            Item_ADD(itemRepo, "02310792S", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6219.6 CL.F 230V50HZ 260W 6.3¾F");
            Item_ADD(itemRepo, "02320396", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "UNIV.SOCKET CONTA.OPEN INARCA 0011060101");
            Item_ADD(itemRepo, "02320401", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PLUG HOUSING UNI.6 WAY INARCA 0863060700");
            Item_ADD(itemRepo, "02320240", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "GUAINA TERMORETRAIBILE UL-CSA  L=8cm Ø19");
            Item_ADD(itemRepo, "02310776A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB20 1140.9 CL.F220-240V50HZ P-C 180W CW");
            Item_ADD(itemRepo, "02310777", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "BLDC Motor J. Elec. Europa");
            Item_ADD(itemRepo, "02011009", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "VITE M4X14,6 T30 ZN.BIANCA");
            Item_ADD(itemRepo, "02310771A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6211.36 CL.B 120V~60Hz 320W 2,7A 25");
            Item_ADD(itemRepo, "02310773", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "BLDC Motor J. Elec. 199-1150012");
            Item_ADD(itemRepo, "02011488", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "GROUND CLIPS");
            Item_ADD(itemRepo, "04401177", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "ETYK. IQ BLOWER SYSTEM QR 990726537");
            Item_ADD(itemRepo, "02310780A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6219.5 CL.F 230V50HZ 260W 6.3¾F");
            Item_ADD(itemRepo, "02310260A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6211.21CLB120V60HZ320W2.7A 25¾F250V");
            Item_ADD(itemRepo, "02310783A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6211.38 CLB 120V60HZ 320W 2.7A CW");
            Item_ADD(itemRepo, "03293040", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZAWOR  A/F PRZEN.2M SP2000 AE     1");
            Item_ADD(itemRepo, "03293050", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LEVA A/F PRZENOSN.2M SP2000 AE (FM)   16");
            Item_ADD(itemRepo, "004200184", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "OKABLOWANIE SILNIKA SP2000 2M RO");
            Item_ADD(itemRepo, "004200185", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "OKABLOWANIE SILNIKA SP2000 2M RA");
            Item_ADD(itemRepo, "03293048", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PIASTRINO CAVO ALIM.SP2000 1M     163297");
            Item_ADD(itemRepo, "03293256", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CONVOGLIATORE 1M ES ECOLABLE V2015");
            Item_ADD(itemRepo, "004203760", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "MOTOR CABLES 1 GI198 V2015 DIS.1");
            Item_ADD(itemRepo, "03293257", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SUPP.MOTORE EA ES4 ECOLABEL");
            Item_ADD(itemRepo, "02320347", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "KONDENSATOR 2,5 ¾F CL.B P2");
            Item_ADD(itemRepo, "03293258", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D147x68 54P.ROmz6 AE");
            Item_ADD(itemRepo, "03293252", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "TAPPO COPRIVITI AE MOT.EB GAMMA ES");
            Item_ADD(itemRepo, "02310788A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB20 1144.1 CL.F220-240V50HZ 100W CW");
            Item_ADD(itemRepo, "03293252", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "TAPPO COPRIVITI AE MOT.EB GAMMA ES");
            Item_ADD(itemRepo, "03293431", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.DMU.S2011 DX+SX EB BEST (ZPLA50234)");
            Item_ADD(itemRepo, "02310785", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "MOTORE R50DT 027 230-240 50HZ 265W");
            Item_ADD(itemRepo, "03290502", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "CUT OFF PER CONVOGLIATORE S2011");
            Item_ADD(itemRepo, "03294350", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "GIR.DOPP.D150 H130 MZ d6 59P PPV2BL(EMC)");
            Item_ADD(itemRepo, "02310790", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "MOTORE R40DT035 230-240 50HZ 210W");
            Item_ADD(itemRepo, "02320385", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "COND.4MF 425/475V 10000H C878BE24400AA6J");
            Item_ADD(itemRepo, "03293430", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CONV.PL.S2011 DX+SX EB+COND.(ZPLA50234)");
            Item_ADD(itemRepo, "02310791", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SILNIK R50DT043 230-240 50HZ 265W");
            Item_ADD(itemRepo, "02320386", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "COND.5MF 425/475V 10000H C878BE24500AA5J");
            Item_ADD(itemRepo, "03293300", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CONVOGLIATORE 1M GI198 V2016");
            Item_ADD(itemRepo, "004203839", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "MOTOR CABLES1 GI198 V2016 DIS.1");
            Item_ADD(itemRepo, "02310621A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA27NT0023 CL.F 220V 60HZ RO 3V");
            Item_ADD(itemRepo, "02310803A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP45 0003.2CLB4V120V60HZ420W3.5A25¾F250V");
            Item_ADD(itemRepo, "02310804A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP45 0003.3CLB4V120V60HZ420W3.5A25¾F250V");
            Item_ADD(itemRepo, "02310805A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP45 0003.4CLB4V120V60HZ420W3.5A25¾F250V");
            Item_ADD(itemRepo, "02310812", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB25 0001 CLF 220-240V 50HZ 210W 5¾F");
            Item_ADD(itemRepo, "02310813", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EB25 0001.1 CLF 220-240V 50HZ 210W");
            Item_ADD(itemRepo, "02502820", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "KARTON cm.118x79x88    E731");
            Item_ADD(itemRepo, "02502801", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ALVEARE 6T CM.78,5X21");
            Item_ADD(itemRepo, "02502802", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "ALVEARI 8T.CM.119,1X21");
            Item_ADD(itemRepo, "5000B050", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EUROPALETA 120X80");
            Item_ADD(itemRepo, "02310694A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP45 0006 CLF 120V60HZ 360W 3A(25¾F250W)");
            Item_ADD(itemRepo, "03293359", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "GIR.DOPP.D150 H130 MZ d6 59P PPV2WH(EMC)");
            Item_ADD(itemRepo, "02310818", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "MOTORE R50DT043.1 230-240 50HZ 265W");
            Item_ADD(itemRepo, "02310103A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA27 53.33C CL.F 230-240V~50Hz 100W CCW");
            Item_ADD(itemRepo, "004260287", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PRZEWOD SILNIKA 413 1M 55/60CM");
            Item_ADD(itemRepo, "03292192", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "LOZYSKO SILNIKA  K196  AE   132014");
            Item_ADD(itemRepo, "03291013", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KABLA SILNIKA K86 86019 AE");
            Item_ADD(itemRepo, "02000140", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINTUJACY 3,2X9,5 TC TC N");
            Item_ADD(itemRepo, "03292193", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D190 16 PALE AE 163027");
            Item_ADD(itemRepo, "03293020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PRZEN. POW. 1SIL SP2000 AE 163255");
            Item_ADD(itemRepo, "03293021", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "BEZP. A/FPRZEN.P.1SIL SP2000 AE   163256");
            Item_ADD(itemRepo, "03293022", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LEVA A/F CONV.1M SP2000 AE        163257");
            Item_ADD(itemRepo, "02310229A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA25 0001 CL.F 230-240V~50Hz 120W CCW");
            Item_ADD(itemRepo, "004200139", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OKABLOWANIE SILNIKA SP2000 1M");
            Item_ADD(itemRepo, "03293023", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LOZYSKO SILNIKA SP2000 AE         163253");
            Item_ADD(itemRepo, "04500062", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TRANSPARENT STICKING TAPE M60XMM19");
            Item_ADD(itemRepo, "03295624", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "PRZENOSNIK POWIETRZA SUPERSILENT");
            Item_ADD(itemRepo, "02010532", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "IZOLACJA PIANKOWA+FILM 90x1000x15");
            Item_ADD(itemRepo, "03295625", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "LOZYSKO SILNIKA SUPERSILENT");
            Item_ADD(itemRepo, "02011827", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOFORM. Z GL. ZEBATA M4X9");
            Item_ADD(itemRepo, "03295626", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK D200 52 LOPATKI+NAKRETKA");
            Item_ADD(itemRepo, "02000167", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4x8 TSTC OCYNK");
            Item_ADD(itemRepo, "02310796A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EA25 0007 CL.F 230-240V~50Hz 100W CCW");
            Item_ADD(itemRepo, "06144135", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZESP.WIAZKI.98-413 2M L 60   CUSF27 RoHS");
            Item_ADD(itemRepo, "06145196", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "ZES.WIA.98-412 S1 A2 SL 90.BL.RoHS");
            Item_ADD(itemRepo, "02011800", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ŒRUBA AF.3.9x9.5 WEWN. TORX T20");
            Item_ADD(itemRepo, "04309988", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP INSTRUKCJE").Id, ItemTypeEnum.BuyedItem, null, "ETYKIETA SAMOPRZYL.BIALA 110X150(1)");
            Item_ADD(itemRepo, "02310576A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6211.28 CL.B 120V~60Hz 320W 2.7A CW");
            Item_ADD(itemRepo, "02502839", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA cm118x79x84,3 ap.");
            Item_ADD(itemRepo, "02310604A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP45 0001.3 CL.B 120V~ 60Hz 530W 4.4A CW");
            Item_ADD(itemRepo, "02502849", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "SEPARATOR KARTONOWY  cm.76x74");
            Item_ADD(itemRepo, "02310607A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP45 0002.5 CLB120V60HZ530W4.4A 25¾F250V");
            Item_ADD(itemRepo, "02310738A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP40 6211.35 CLB 120V60HZ 320W 2.7A CW");
            Item_ADD(itemRepo, "5000B001", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PALLET  120X80");
            Item_ADD(itemRepo, "02310296A", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "EP45 0005 CLB 120V60HZ 360W 3A(25¾F250W)");
            Item_ADD(itemRepo, "03295221", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBS! PA.DX +SX CONV.PLASTICA EA S2004 AE");
            Item_ADD(itemRepo, "03295510", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZATYCZKA ZAMYKAJACA OTWOR CONV. S2004 AE");
            Item_ADD(itemRepo, "02011786", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WORECZEK PIANKOWY 19X30");
            Item_ADD(itemRepo, "02502858", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP KARTONY").Id, ItemTypeEnum.BuyedItem, null, "OBS! SEPARATOR 118X19.25 TFC/222/B");
            Item_ADD(itemRepo, "04500012", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "TASMA PAPIEROWA 19mm x 50m");
            Item_ADD(itemRepo, "03295404", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA KABLA SILNIKA S2004 AE");
            Item_ADD(itemRepo, "03295378", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PASSACAVO APERTO NEFF VERS.2002 V2");
            Item_ADD(itemRepo, "02011784", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WORECZEK PIANKOWY 1-15X30");
            Item_ADD(itemRepo, "02011797", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET AF 3,2x16 TCTCZ2P.");
            Item_ADD(itemRepo, "03295351", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK 40 PALE DOP.MZ5 AE VERS.2000 RO");
            Item_ADD(itemRepo, "03292677", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SUPP.CART.CONV.LAM.DX (rif.Elek ZSUP562)");
            Item_ADD(itemRepo, "03292676", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SUPP.CART.CONV.LAM.SX (rif.Elek ZSUP563)");
            Item_ADD(itemRepo, "03295356", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WIRNIK 40-LOP. DOPPIA MZ5  S2004 AE RO");
            Item_ADD(itemRepo, "02011788", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WORECZEK PIANKOWY WZMOCNIONY 20X30");
            Item_ADD(itemRepo, "03295222", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBS! PA.DX +SX CONV.PLASTICA EB S2004 AE");
            Item_ADD(itemRepo, "03295503", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WSPORNIK FILTRA Z KRATK¥ S2004 AE");
            Item_ADD(itemRepo, "02300126", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ELEKTRYKA").Id, ItemTypeEnum.BuyedItem, null, "KABEL Z WTYCZKA             3x0,75 150cm");
            Item_ADD(itemRepo, "03291043", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "ZACISK DO KABLA K186 86163 AE");
            Item_ADD(itemRepo, "03295223", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUDOWA PLAST.PRAWA/LEWA EB S2004 AE CAR");
            Item_ADD(itemRepo, "ZIMB00031", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "KOSZ SIATKOWY SK£AD. ELZN 120x80 IMPERIA");
            Item_ADD(itemRepo, "004202088", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO 4 FILI L=620MM C/CONN.STOCKO");
            Item_ADD(itemRepo, "03295379", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PASSACAVO x CONV. S2007 AE");
            Item_ADD(itemRepo, "03295264", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "FLANGIA D150 K2000");
            Item_ADD(itemRepo, "02000100", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.2,9X13TSPTC2PRZINC");
            Item_ADD(itemRepo, "004201994", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO MOTORE 4FILI L=390MM C/MORS.");
            Item_ADD(itemRepo, "03295377", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PASSACAVO NEFF VERS.2003 X PIASTRA PICC.");
            Item_ADD(itemRepo, "03295605", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.DMUCHAWY PLAST.S2011 DX+SX EA");
            Item_ADD(itemRepo, "004202525", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABL. 4FILI L=860MM C/STAGN. AWG22");
            Item_ADD(itemRepo, "004201907", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO MOTORE X EAGR0155.3");
            Item_ADD(itemRepo, "004201995", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO MOTORE 3FILI L=390MM C/MORS.");
            Item_ADD(itemRepo, "03295605", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "OBUD.DMUCHAWY PLAST.S2011 DX+SX EA");
            Item_ADD(itemRepo, "03295185", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO ISOLATORE SUPP. EA");
            Item_ADD(itemRepo, "03292171", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SUPP.MOT.CA P190M DX (AE)62026");
            Item_ADD(itemRepo, "004201959", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO MOTORE L=425MM");
            Item_ADD(itemRepo, "03295185", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO ISOLATORE SUPP. EA");
            Item_ADD(itemRepo, "004201960", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO MOTORE L=565MM");
            Item_ADD(itemRepo, "03295185", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "COPERCHIO ISOLATORE SUPP. EA");
            Item_ADD(itemRepo, "004201961", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO 4 FILI L=450MM+CONNETTORE");
            Item_ADD(itemRepo, "02502833", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "SCATOLA CM118X79X106 APERTA");
            Item_ADD(itemRepo, "5000B016", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "PALLET 80x120 RINFORZATO");
            Item_ADD(itemRepo, "08088362", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "ZES.ZAWOR.VR.D150 K2000 163610");
            Item_ADD(itemRepo, "004201982", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABLAGGIO 4 FILI L=640MM+CONNETTORE");
            Item_ADD(itemRepo, "004202485", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CABL.4 FILI C/CONN.STOCKO L=570MM AWG22");
            Item_ADD(itemRepo, "03295608", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OBUD.PLAST.DMUCH.PRAWA+LEWA EB WER.2011");
            Item_ADD(itemRepo, "03295610", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "TAPPO COPRIVITE EB x CONV. S2011 AE");
            Item_ADD(itemRepo, "03295225", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OBS! CONV.PLAST.S2007 DX+SX EB C/GRIGLIA");
            Item_ADD(itemRepo, "03295511", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "TAPPO COPRIVITI CONV.S2007 AE");
            Item_ADD(itemRepo, "03295465", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "PUSZKA KONDENSATORA  X CONV. S2007 AE");
            Item_ADD(itemRepo, "03295227", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP PLASTIKI").Id, ItemTypeEnum.BuyedItem, null, "CONV.PLAST.S2007 DX+SX EB S/GRIGLIA");
            Item_ADD(itemRepo, "03295396", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA INT.CON PASS.VERS.99 V2");
            Item_ADD(itemRepo, "03295397", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "OSLONA INT.SEN PASS.VERS.99 V2");
            Item_ADD(itemRepo, "03295228", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "CONV.PLAST.S2007 DX+SX EB S/GRIGLIA CAR.");
            Item_ADD(itemRepo, "02011020", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "WKRET SAMOGWINT.UNIW.SPAX 4,2X15 TCTC ZN");
            Item_ADD(itemRepo, "02011829", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "SRUBA M4 X16,8 AUTOF. TRILOBATA ZN NERA");
            Item_ADD(itemRepo, "03292631", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP ZASYP").Id, ItemTypeEnum.BuyedItem, null, "REDUKCJA 150/125 K2000");
            Item_ADD(itemRepo, "02011663", "9103", null, groupList.FirstOrDefault(x => x.Name == "GROUP SILNIKI").Id, ItemTypeEnum.BuyedItem, null, "DISTANZIALE IN FERRO D.12");

            db.Database.ExecuteSqlCommand("INSERT INTO [iLOGIS].[CONFIG_Item]([ItemId],[Weight],[H],[PickerNo],[TrainNo],[ABC],[XYZ]) SELECT Id, 0,0,0,0,0,0 FROM[_MPPL].[MASTERDATA_Item] WHERE Id > 18");
        }
        public static void Seed_PLB_MasterData_ItemsPNC(IDbContextMasterData db)
        {
            ItemRepo itemRepo = new ItemRepo(db);

            Item_ADD(itemRepo, "942022000", "9102", null, 2, ItemTypeEnum.Product, null, "AEG WIN S1E2PU XS A/F 90 942022000");
            Item_ADD(itemRepo, "942022001", "9102", null, 2, ItemTypeEnum.Product, null, "AEG WIN S1E2PU XS A/F 60 942022001");
            Item_ADD(itemRepo, "942022002", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU XS A/F 60 942022002 UK");
            Item_ADD(itemRepo, "942022003", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU XS A/F 90 942022003 UK");
            Item_ADD(itemRepo, "942022004", "9102", null, 2, ItemTypeEnum.Product, null, "ELECT.WIN S1E2PU XS A/F 60 942022004");
            Item_ADD(itemRepo, "942022005", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU BL1 A/F 60 942022005 UK");
            Item_ADD(itemRepo, "942022006", "9102", null, 2, ItemTypeEnum.Product, null, "FAURE WIN S1E2PU W2 A/F 60 942022006");
            Item_ADD(itemRepo, "942022007", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU BL1 A/F 90 942022007 UK");
            Item_ADD(itemRepo, "942022008", "9102", null, 2, ItemTypeEnum.Product, null, "ELECT.WIN S1E2PU XS A/F 90 942022008");
            Item_ADD(itemRepo, "942022009", "9102", null, 2, ItemTypeEnum.Product, null, "FAURE WIN S1E2PU BL1 A/F 60 942022009");
            Item_ADD(itemRepo, "942022010", "9102", null, 2, ItemTypeEnum.Product, null, "FAURE WIN S1E2PU XS A/F 60 942022010");
            Item_ADD(itemRepo, "942022011", "9102", null, 2, ItemTypeEnum.Product, null, "FAURE WIN S1E2PU W2 A/F 90 942022011");
            Item_ADD(itemRepo, "942022012", "9102", null, 2, ItemTypeEnum.Product, null, "FAURE WIN S1E2PU BL1 A/F 90 942022012");
            Item_ADD(itemRepo, "942022013", "9102", null, 2, ItemTypeEnum.Product, null, "FAURE WIN S1E2PU XS A/F 90 942022013");
            Item_ADD(itemRepo, "942022014", "9102", null, 2, ItemTypeEnum.Product, null, "ZANUSSI WIN S1E2PU XS A/F 60 942022014");
            Item_ADD(itemRepo, "942022015", "9102", null, 2, ItemTypeEnum.Product, null, "ZANUSSI WIN S1E2PU XS A/F 90 942022015");
            Item_ADD(itemRepo, "942022016", "9102", null, 2, ItemTypeEnum.Product, null, "ZANUSSI WIN S1E2PU XS A/F60 942022016 UK");
            Item_ADD(itemRepo, "942022017", "9102", null, 2, ItemTypeEnum.Product, null, "NEUE WIN S1E2PU XS A/F 90 942022017 UK");
            Item_ADD(itemRepo, "942022018", "9102", null, 2, ItemTypeEnum.Product, null, "NEUE WIN S1E2PU XS A/F 60 942022018 UK");
            Item_ADD(itemRepo, "942022019", "9102", null, 2, ItemTypeEnum.Product, null, "PROGRESS WIN S1E2PU XS A/F 60 942022019");
            Item_ADD(itemRepo, "942022020", "9102", null, 2, ItemTypeEnum.Product, null, "PROGRESS WIN S1E2PU XS A/F 90 942022020");
            Item_ADD(itemRepo, "942022021", "9102", null, 2, ItemTypeEnum.Product, null, "ZANKER WIN S1E2PU XS A/F 90 942022021");
            Item_ADD(itemRepo, "942022022", "9102", null, 2, ItemTypeEnum.Product, null, "ZANKER WIN S1E2PU XS A/F 60 942022022");
            Item_ADD(itemRepo, "942022023", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA EB25D2PU XS FM AF90 942022023");
            Item_ADD(itemRepo, "942022024", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA EB25D2PU XS FM AF60 942022024");
            Item_ADD(itemRepo, "942022025", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA EB25D2PU W2 FM AF90 942022025");
            Item_ADD(itemRepo, "942022026", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA EB25D2PU BL1FM AF90 942022026");
            Item_ADD(itemRepo, "942022027", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA S1E2BAXSFM AF60 942022027");
            Item_ADD(itemRepo, "942022028", "9102", null, 3, ItemTypeEnum.Product, null, "FAURE BETA EB25D2PU XS FM AF90 942022028");
            Item_ADD(itemRepo, "942022029", "9102", null, 3, ItemTypeEnum.Product, null, "FAURE BETA EB25D2PU XS FM AF60 942022029");
            Item_ADD(itemRepo, "942022030", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA S1E2BAXSFM AF60 942022030");
            Item_ADD(itemRepo, "942022031", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA S1E2BAXSFM AF90 942022031");
            Item_ADD(itemRepo, "942022032", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA EB25D2PUXSFM AF90 942022032");
            Item_ADD(itemRepo, "942022033", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA EB25D2PUXSFM AF60 942022033");
            Item_ADD(itemRepo, "942022034", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUS.BETA EB25D2PUXSFMAF90 942022034 UK");
            Item_ADD(itemRepo, "942022035", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUS.BETA EB25D2PUXSFMAF60 942022035 UK");
            Item_ADD(itemRepo, "942022036", "9102", null, 3, ItemTypeEnum.Product, null, "JUNO BETA S1D2PU XS FM A/F 60 942022036");
            Item_ADD(itemRepo, "942022037", "9102", null, 3, ItemTypeEnum.Product, null, "JUNO BETA S1D2PU XS FM A/F 90 942022037");
            Item_ADD(itemRepo, "942022040", "9102", null, 3, ItemTypeEnum.Product, null, "PROGRESS BETA S1E2BAXSFM AF60 942022040");
            Item_ADD(itemRepo, "942022041", "9102", null, 3, ItemTypeEnum.Product, null, "PROGRESS BETA S1E2BAXSFM AF90 942022041");
            Item_ADD(itemRepo, "942022042", "9102", null, 3, ItemTypeEnum.Product, null, "PROGRESS BETA EB25D2PUXSFMAF60 942022042");
            Item_ADD(itemRepo, "942022043", "9102", null, 3, ItemTypeEnum.Product, null, "PROGRESS BETA EB25D2PUXSFMAF90 942022043");
            Item_ADD(itemRepo, "942022044", "9102", null, 3, ItemTypeEnum.Product, null, "ZANKER BETA S1E2BAXSFM AF60 942022044");
            Item_ADD(itemRepo, "942022045", "9102", null, 3, ItemTypeEnum.Product, null, "ZANKER BETA S1E2BAXSFM AF90 942022045");
            Item_ADD(itemRepo, "942022046", "9102", null, 3, ItemTypeEnum.Product, null, "ZANKER BETA EB25D2PUXSFM AF60 942022046");
            Item_ADD(itemRepo, "942022047", "9102", null, 3, ItemTypeEnum.Product, null, "ZANKER BETA EB25D2PUXSFM AF90 942022047");
            Item_ADD(itemRepo, "942022048", "9102", null, 4, ItemTypeEnum.Product, null, "AEG VERSA MBSE2PSQ MET A60 942022048");
            Item_ADD(itemRepo, "942022049", "9102", null, 4, ItemTypeEnum.Product, null, "AEG VERSA MBME2PSQ MET A60 942022049");
            Item_ADD(itemRepo, "942022050", "9102", null, 4, ItemTypeEnum.Product, null, "AEG VERSA MBME2PSQ MET F60 942022050 UK");
            Item_ADD(itemRepo, "942022051", "9102", null, 4, ItemTypeEnum.Product, null, "AEG VERSA MBME2PSQ MET F90 942022051 UK");
            Item_ADD(itemRepo, "942022052", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA 1GFA MBSE2BA MET A60 RUSSIA");
            Item_ADD(itemRepo, "942022053", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA 1GFA MBSE2BA W2A60 942022053");
            Item_ADD(itemRepo, "942022054", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA MBME2BA MET A50 942022054");
            Item_ADD(itemRepo, "942022055", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA MBME2BA MET A60 942022055 UK");
            Item_ADD(itemRepo, "942022056", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA MBME2BA MET A60 942022056");
            Item_ADD(itemRepo, "942022057", "9102", null, 4, ItemTypeEnum.Product, null, "ZANUSSI VERSA GL MBSE2BA W2A60 942022057");
            Item_ADD(itemRepo, "942022058", "9102", null, 4, ItemTypeEnum.Product, null, "ZANUSSI VERSA MBME2BA MET A60 942022058");
            Item_ADD(itemRepo, "942022059", "9102", null, 4, ItemTypeEnum.Product, null, "ZANUSSI VERSA GL MBSE2BAMETA60 942022059");
            Item_ADD(itemRepo, "942022060", "9102", null, 4, ItemTypeEnum.Product, null, "FAURE VERSA MBME2BA MET F60 942022060");
            Item_ADD(itemRepo, "942022061", "9102", null, 4, ItemTypeEnum.Product, null, "ZANKER VERSA GL MBSE2BA META60 942022061");
            Item_ADD(itemRepo, "942022062", "9102", null, 4, ItemTypeEnum.Product, null, "JUNO VERSA GL MBSE2BA MET A60 942022062");
            Item_ADD(itemRepo, "942022063", "9102", null, 4, ItemTypeEnum.Product, null, "JUNO VERSA 1GFA MBSD2BA META60 942022063");
            Item_ADD(itemRepo, "942022064", "9102", null, 4, ItemTypeEnum.Product, null, "PROGRESS VERSA GLMBSE2BAMETA60 942022064");
            Item_ADD(itemRepo, "942022065", "9102", null, 5, ItemTypeEnum.Product, null, "AEG P780 EB25D2EL FM XS A70 942022065");
            Item_ADD(itemRepo, "942022066", "9102", null, 5, ItemTypeEnum.Product, null, "AEG P550 M1D2SL FM XS A52 942022066");
            Item_ADD(itemRepo, "942022067", "9102", null, 5, ItemTypeEnum.Product, null, "AEG P780 EB25D2EL FM XS A70 942022067 UK");
            Item_ADD(itemRepo, "942022068", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P780 EB25D2EL FM XS A70 942022068");
            Item_ADD(itemRepo, "942022069", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P580 L1D2EL FM XS A52 942022069");
            Item_ADD(itemRepo, "942022070", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P580 L1D2EL FM W2 A52 942022070");
            Item_ADD(itemRepo, "942022071", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P580 L1D2EL FM BL1 A52 942022071");
            Item_ADD(itemRepo, "942022072", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P550 EB25D2SL FM MET A52 942022072");
            Item_ADD(itemRepo, "942022073", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P550 EB25D2SL FM W2 A52 942022073");
            Item_ADD(itemRepo, "942022074", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P550 EB25D2SL FM BL1 A52 942022074");
            Item_ADD(itemRepo, "942022075", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P550 EB25D2SLFM XS A52 SV");
            Item_ADD(itemRepo, "942022076", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P750 EB25D2SLFM XS A70 SV");
            Item_ADD(itemRepo, "942022077", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU XS A/F 70 942022077 UK");
            Item_ADD(itemRepo, "942022078", "9102", null, 2, ItemTypeEnum.Product, null, "ZANUSSI WIN S1E2PU XS A/F 70 942022078");
            Item_ADD(itemRepo, "942022079", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU XS A/F 60 942022079 UK");
            Item_ADD(itemRepo, "942022081", "9102", null, 6, ItemTypeEnum.Product, null, "AEG RHO EB25D2PU XS F 60-70-80 UK");
            Item_ADD(itemRepo, "942022082", "9102", null, 6, ItemTypeEnum.Product, null, "RHO S1D2PU XS F 60 UK");
            Item_ADD(itemRepo, "942022083", "9102", null, 6, ItemTypeEnum.Product, null, "DELTA S1D2PU XS F60 UK");
            Item_ADD(itemRepo, "942022084", "9102", null, 6, ItemTypeEnum.Product, null, "ELECTROLUX RHO EB25D2PU XS A/F 90");
            Item_ADD(itemRepo, "942022085", "9102", null, 6, ItemTypeEnum.Product, null, "DELTA EB25D2PU XS A/F 90 UK");
            Item_ADD(itemRepo, "942022087", "9102", null, 6, ItemTypeEnum.Product, null, "ELECTROLUX RHO EC20D2PU XS A/F 60-70 UK");
            Item_ADD(itemRepo, "942022088", "9102", null, 6, ItemTypeEnum.Product, null, "RHO S1D2PU XS A/F 60 UK");
            Item_ADD(itemRepo, "942022089", "9102", null, 6, ItemTypeEnum.Product, null, "DELTA S1D2PU XS A/F 60 UK");
            Item_ADD(itemRepo, "942022090", "9102", null, 6, ItemTypeEnum.Product, null, "FAURE DELTA EC20D2PU XS F 90");
            Item_ADD(itemRepo, "942022097", "9102", null, 6, ItemTypeEnum.Product, null, "ZANUSSI DELTA EC20D2PU XS A/F 60 UK");
            Item_ADD(itemRepo, "942022098", "9102", null, 6, ItemTypeEnum.Product, null, "RHO EB25D2PU XS A/F 60 UK");
            Item_ADD(itemRepo, "942022099", "9102", null, 6, ItemTypeEnum.Product, null, "ZANUSSI DELTA EC20D2PU XS A/F 90 UK");
            Item_ADD(itemRepo, "942022100", "9102", null, 7, ItemTypeEnum.Product, null, "AEG TILIA MBSE2SL MET A52 RU");
            Item_ADD(itemRepo, "942022101", "9102", null, 7, ItemTypeEnum.Product, null, "AEG.TILIA MBSE2SL MET A52");
            Item_ADD(itemRepo, "942022102", "9102", null, 7, ItemTypeEnum.Product, null, "ELECT.TILIA MBSE2SL MET A52 UK");
            Item_ADD(itemRepo, "942022103", "9102", null, 7, ItemTypeEnum.Product, null, "ELECT.TILIA MBSE2SL MET A52");
            Item_ADD(itemRepo, "942022104", "9102", null, 7, ItemTypeEnum.Product, null, "ELECT.TILIA MBME2SL MET A52");
            Item_ADD(itemRepo, "942022105", "9102", null, 7, ItemTypeEnum.Product, null, "ELECT.TILIA MBSE2SL MET A70");
            Item_ADD(itemRepo, "942022106", "9102", null, 7, ItemTypeEnum.Product, null, "ELECT.TILIA MBME2SL MET A70");
            Item_ADD(itemRepo, "942022107", "9102", null, 7, ItemTypeEnum.Product, null, "FAURE TILIA MBSE2SL MET F52");
            Item_ADD(itemRepo, "942022108", "9102", null, 7, ItemTypeEnum.Product, null, "ZANUSSI TILIA MBME2SL MET A52");
            Item_ADD(itemRepo, "942022109", "9102", null, 7, ItemTypeEnum.Product, null, "ZANUSSI TILIA MBSE2SL MET A52");
            Item_ADD(itemRepo, "942022110", "9102", null, 7, ItemTypeEnum.Product, null, "ZANUSSI TILIA MBSE2SL MET A52 UK");
            Item_ADD(itemRepo, "942022111", "9102", null, 7, ItemTypeEnum.Product, null, "ZANUSSI TILIA MBME2SL MET A70");
            Item_ADD(itemRepo, "942022112", "9102", null, 4, ItemTypeEnum.Product, null, "AEG SKLOK EB40D2PU XS A60");
            Item_ADD(itemRepo, "942022113", "9102", null, 4, ItemTypeEnum.Product, null, "AEG SKLOK EB40D2PU XS A90");
            Item_ADD(itemRepo, "942022114", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.SKLOK EA37D2PU XS A60");
            Item_ADD(itemRepo, "942022115", "9102", null, 4, ItemTypeEnum.Product, null, "ELECTROLUX SKLOK EA37D2PU XS A60 UK");
            Item_ADD(itemRepo, "942022116", "9102", null, 4, ItemTypeEnum.Product, null, "SKLOK EB40D2PU XS A60 ELECT. RU");
            Item_ADD(itemRepo, "942022117", "9102", null, 4, ItemTypeEnum.Product, null, "SKLOK EA37 D2 PU XS A90 ELECT.");
            Item_ADD(itemRepo, "942022118", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.SKLOK EB40D2PU XS A90");
            Item_ADD(itemRepo, "942022119", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P550 EB25D2SL FMMETA52 942022119 RU");
            Item_ADD(itemRepo, "942022120", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P550 EB25D2SL FMW2A52 942022120 RU");
            Item_ADD(itemRepo, "942022121", "9102", null, 5, ItemTypeEnum.Product, null, "ELEC.P550 EB25D2SL FMBL1A52 942022121 RU");
            Item_ADD(itemRepo, "942022122", "9102", null, 2, ItemTypeEnum.Product, null, "ELECT.WIN S1E2PU XS A/F 60 942022122 RU");
            Item_ADD(itemRepo, "942022127", "9102", null, 2, ItemTypeEnum.Product, null, "AEG WIN S1E2PU XS A/F 60 942022127");
            Item_ADD(itemRepo, "942022128", "9102", null, 2, ItemTypeEnum.Product, null, "AEG WIN S1E2PU XS A/F 90 942022128");
            Item_ADD(itemRepo, "942022129", "9102", null, 2, ItemTypeEnum.Product, null, "ZANUSSI WIN S1E2PU XS A/F 90 942022129");
            Item_ADD(itemRepo, "942022130", "9102", null, 4, ItemTypeEnum.Product, null, "ZANUSSI VERSA GL MBSE2BAMETA60 942022130");
            Item_ADD(itemRepo, "942022131", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA S1E2BAXSFM AF90 942022131");
            Item_ADD(itemRepo, "942022132", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA S1E2BAXSFM AF60 942022132");
            Item_ADD(itemRepo, "942022133", "9102", null, 3, ItemTypeEnum.Product, null, "BEST BETA S1D2PU H.6 2FM XS A/F 60");
            Item_ADD(itemRepo, "942022134", "9102", null, 3, ItemTypeEnum.Product, null, "BEST BETA S1D2PU H.6 3FM XS A/F 80");
            Item_ADD(itemRepo, "942022135", "9102", null, 3, ItemTypeEnum.Product, null, "BEST BETA S1D2PU H.6 3FM XS A/F 90");
            Item_ADD(itemRepo, "942022136", "9102", null, 3, ItemTypeEnum.Product, null, "BEST BETA S1D2PU H.10 3FM XS A/F 90");
            Item_ADD(itemRepo, "942022137", "9102", null, 3, ItemTypeEnum.Product, null, "BEST THETA HF8 D2 ST A/F60 XS");
            Item_ADD(itemRepo, "942022138", "9102", null, 3, ItemTypeEnum.Product, null, "BEST THETA HF8 D2 ST A/F80 XS");
            Item_ADD(itemRepo, "942022139", "9102", null, 3, ItemTypeEnum.Product, null, "BEST THETA HF8 D2 ST A/F90 XS");
            Item_ADD(itemRepo, "942022140", "9102", null, 2, ItemTypeEnum.Product, null, "BEST WIN S1E2PU XS A/F 60");
            Item_ADD(itemRepo, "942022141", "9102", null, 2, ItemTypeEnum.Product, null, "BEST WIN S1E2PU XS A/F 70");
            Item_ADD(itemRepo, "942022142", "9102", null, 2, ItemTypeEnum.Product, null, "BEST WIN S1E2PU XS A/F 80");
            Item_ADD(itemRepo, "942022143", "9102", null, 2, ItemTypeEnum.Product, null, "BEST WIN S1E2PU XS A/F 90");
            Item_ADD(itemRepo, "942022144", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P580 P-LIGHT L1D2EL FM XS A52 SS");
            Item_ADD(itemRepo, "942022145", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P780 P-LIGHT L1D2EL FM XS A70 SS");
            Item_ADD(itemRepo, "942022146", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P550 P-C S1E2SL FM XS A52");
            Item_ADD(itemRepo, "942022147", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P750 P-C S1E2SL FM XS A70");
            Item_ADD(itemRepo, "942022148", "9102", null, 4, ItemTypeEnum.Product, null, "BEST 04/417 P-C S1D2BA GR3/AL.A60");
            Item_ADD(itemRepo, "942022149", "9102", null, 4, ItemTypeEnum.Product, null, "BEST 04/417 P-C S1D2BA GR3/AL. A90");
            Item_ADD(itemRepo, "942022150", "9102", null, 4, ItemTypeEnum.Product, null, "BEST ES415 BELLAGIO M1E2BA G.MET A60");
            Item_ADD(itemRepo, "942022151", "9102", null, 4, ItemTypeEnum.Product, null, "BEST ES415 BELLAGIO M1E2BA G.MET A90");
            Item_ADD(itemRepo, "942022152", "9102", null, 7, ItemTypeEnum.Product, null, "BEST P540 2EA40E2SL MET.A52 CAT.ITALIA");
            Item_ADD(itemRepo, "942022153", "9102", null, 7, ItemTypeEnum.Product, null, "BEST P740 2EA40E2SL MET.A70 CAT.ITALIA");
            Item_ADD(itemRepo, "942022154", "9102", null, 7, ItemTypeEnum.Product, null, "BEST P520 EC20E2SL MET FM A52 CAT.ITALIA");
            Item_ADD(itemRepo, "942022155", "9102", null, 7, ItemTypeEnum.Product, null, "BEST P720 EC20E2SL MET FM A72 CAT.ITALIA");
            Item_ADD(itemRepo, "942022156", "9102", null, 6, ItemTypeEnum.Product, null, "BEST DELTA S1 D2 PU XS A/F 60");
            Item_ADD(itemRepo, "942022157", "9102", null, 6, ItemTypeEnum.Product, null, "BEST DELTA S1 D2 PU XS A/F 90");
            Item_ADD(itemRepo, "942022158", "9102", null, 2, ItemTypeEnum.Product, null, "BEST K24R P-C S1E2SLS GRA/OTL AF 60 C.IT");
            Item_ADD(itemRepo, "942022159", "9102", null, 2, ItemTypeEnum.Product, null, "BEST K24R P-C S1E2SLS GRA/OTL AF 90 C.IT");
            Item_ADD(itemRepo, "942022160", "9102", null, 8, ItemTypeEnum.Product, null, "BEST SP2196-3 2EA40E2PU XSA60 CAT.ITALIA");
            Item_ADD(itemRepo, "942022161", "9102", null, 8, ItemTypeEnum.Product, null, "BEST SP2196-3 2EA40E2PU XSA90 CAT.ITALIA");
            Item_ADD(itemRepo, "942022162", "9102", null, 6, ItemTypeEnum.Product, null, "BEST RHO S1 D2 PU A/F60 XS");
            Item_ADD(itemRepo, "942022163", "9102", null, 6, ItemTypeEnum.Product, null, "BEST RHO S1 D2 PU A/F90 XS");
            Item_ADD(itemRepo, "942022164", "9102", null, 5, ItemTypeEnum.Product, null, "BEST PASC580 P-B FPX L1D2ESD XS A52");
            Item_ADD(itemRepo, "942022165", "9102", null, 5, ItemTypeEnum.Product, null, "BEST PASC780 P-B FPX L1D2ESD XS A70");
            Item_ADD(itemRepo, "942022166", "9102", null, 4, ItemTypeEnum.Product, null, "BEST WITCH MBSD2EL MET. A60");
            Item_ADD(itemRepo, "942022167", "9102", null, 4, ItemTypeEnum.Product, null, "BEST WITCH MBSD2EL MET. A90");
            Item_ADD(itemRepo, "942022168", "9102", null, 3, ItemTypeEnum.Product, null, "BEST ZETA HF8 D2 TC A/F60 BL/XS");
            Item_ADD(itemRepo, "942022169", "9102", null, 3, ItemTypeEnum.Product, null, "BEST ZETA HF8 D2 TC A/F90 BL/XS");
            Item_ADD(itemRepo, "942022170", "9102", null, 7, ItemTypeEnum.Product, null, "AEG TILIA MBSE2SL MET A52 MEA");
            Item_ADD(itemRepo, "942022176", "9102", null, 3, ItemTypeEnum.Product, null, "BEST BETA S1E2PU ANTRANERO H6 2FM 90");
            Item_ADD(itemRepo, "942022177", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P550 S1E2SL FM W2 A52 RUSSIA");
            Item_ADD(itemRepo, "942022178", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P560 M1E2EL MET.1FM A52 RUSSIA");
            Item_ADD(itemRepo, "942022179", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P560 M1E2EL FM W2 A52");
            Item_ADD(itemRepo, "942022180", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P760 M1E2EL FM W2 A70 RUSSIA");
            Item_ADD(itemRepo, "942022181", "9102", null, 8, ItemTypeEnum.Product, null, "BEST SP2002 EC1E1SLSV W2 A60");
            Item_ADD(itemRepo, "942022182", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P591 L1D2ELSD XS A52 P-LIGHT RUSSIA");
            Item_ADD(itemRepo, "942022183", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P791 L1D2ELSD XS A70 P-LIGHT RUSSIA");
            Item_ADD(itemRepo, "942022184", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P991 L1D2ELSD XS A90 P-LIGHT RUSSIA");
            Item_ADD(itemRepo, "942022185", "9102", null, 3, ItemTypeEnum.Product, null, "BEST K19L PCL1L2PUXS304AF90 K181 SHI LEI");
            Item_ADD(itemRepo, "942022186", "9102", null, 3, ItemTypeEnum.Product, null, "BEST K19LPCL1L2PUXS304AF120 K181 SHI LEI");
            Item_ADD(itemRepo, "942022187", "9102", null, 5, ItemTypeEnum.Product, null, "BEST PASC580 P-B FPX HF8D2ESD FM XS A52");
            Item_ADD(itemRepo, "942022188", "9102", null, 5, ItemTypeEnum.Product, null, "BEST PASC780 P-B FPX HF8D2ESD FM XS A70");
            Item_ADD(itemRepo, "942022189", "9102", null, 2, ItemTypeEnum.Product, null, "BEST K24R P-C S1E2SLS AVORIO R1015 AF 60");
            Item_ADD(itemRepo, "942022190", "9102", null, 2, ItemTypeEnum.Product, null, "BEST K24R P-C S1E2SLS AVORIO R1015  AF90");
            Item_ADD(itemRepo, "942022197", "9102", null, 5, ItemTypeEnum.Product, null, "GLEM P560 M1D2EL 2FM XS F52 GHF651IX");
            Item_ADD(itemRepo, "942022199", "9102", null, 7, ItemTypeEnum.Product, null, "COBAL P520 EC20E2SL MET F52");
            Item_ADD(itemRepo, "942022202", "9102", null, 7, ItemTypeEnum.Product, null, "GLEM P520 S1E2SL MET FM A52 GHF621SI2");
            Item_ADD(itemRepo, "942022204", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P195P1M52SB6");
            Item_ADD(itemRepo, "942022205", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P195P1M70SB6");
            Item_ADD(itemRepo, "942022206", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P195P2M70SB");
            Item_ADD(itemRepo, "942022218", "9102", null, 2, ItemTypeEnum.Product, null, "GORENJE K24R M1D2PU R9005 A/F60 DK63CLB");
            Item_ADD(itemRepo, "942022219", "9102", null, 2, ItemTypeEnum.Product, null, "GORENJE K24R M1D2PU AVORIO A/F60 DK63CLI");
            Item_ADD(itemRepo, "942022222", "9102", null, 2, ItemTypeEnum.Product, null, "M-SYS.WIN W.S1E2PU XS A/F 60 MSK651IX");
            Item_ADD(itemRepo, "942022223", "9102", null, 2, ItemTypeEnum.Product, null, "M-SYS.WIN W.S1E2PU XS A/F 90 MSK951IX");
            Item_ADD(itemRepo, "942022224", "9102", null, 3, ItemTypeEnum.Product, null, "BELDEKO K19 P-C S1E2PU2FM BL1 F90 EREBIA");
            Item_ADD(itemRepo, "942022227", "9102", null, 3, ItemTypeEnum.Product, null, "GLEM BETA L1A2ELER XS 2FM F90 GHB98IX008");
            Item_ADD(itemRepo, "942022229", "9102", null, 3, ItemTypeEnum.Product, null, "MORA BETA S1D2PU XS H6 2FM A/F 60 OK637X");
            Item_ADD(itemRepo, "942022232", "9102", null, 3, ItemTypeEnum.Product, null, "M-SYSTEM BETA HF6E2PU XS A/F60 MCHS60IX");
            Item_ADD(itemRepo, "942022237", "9102", null, 3, ItemTypeEnum.Product, null, "M-SYSTEM BETA S1E2PUXS  2FM 60 MSPK651IX");
            Item_ADD(itemRepo, "942022238", "9102", null, 3, ItemTypeEnum.Product, null, "M-SYSTEM BETA S1E2PUXS  2FM 90 MSPK951IX");
            Item_ADD(itemRepo, "942022241", "9102", null, 4, ItemTypeEnum.Product, null, "THERM.415/2015 M1E2BA MET A90 SLIM HIDE9");
            Item_ADD(itemRepo, "942022242", "9102", null, 4, ItemTypeEnum.Product, null, "THERM.415/2015 M1E2BA MET A60 SLIM HIDE6");
            Item_ADD(itemRepo, "942022243", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414-2 V2D2BA W2/XS FR1 60 90617");
            Item_ADD(itemRepo, "942022244", "9102", null, 8, ItemTypeEnum.Product, null, "AIRLUX SP2195 EC1E1PUBL1 F60 AHC631BK");
            Item_ADD(itemRepo, "942022245", "9102", null, 8, ItemTypeEnum.Product, null, "AIRLUX SP2195 EC1E1PU W2 F60 AHC631WH");
            Item_ADD(itemRepo, "942022246", "9102", null, 8, ItemTypeEnum.Product, null, "AIRLUX SP2195 EC1E1PU XS F60 AHC631IX");
            Item_ADD(itemRepo, "942022247", "9102", null, 8, ItemTypeEnum.Product, null, "BELDEKO SP2195 EC1E2PU-CR W2F60 GRETA WH");
            Item_ADD(itemRepo, "942022248", "9102", null, 8, ItemTypeEnum.Product, null, "BELDEKO SP2195 EC1E2PU-CR BLF60 GRETA BL");
            Item_ADD(itemRepo, "942022249", "9102", null, 8, ItemTypeEnum.Product, null, "BELDEKO SP2195 EC1E2PU-CR XSF60 GRETA XS");
            Item_ADD(itemRepo, "942022250", "9102", null, 8, ItemTypeEnum.Product, null, "COBAL  SP2001-1 EC1E1SLSV W2 3+1 F60");
            Item_ADD(itemRepo, "942022251", "9102", null, 8, ItemTypeEnum.Product, null, "COBAL SP2001 EC1E1SL SV XS F60");
            Item_ADD(itemRepo, "942022252", "9102", null, 8, ItemTypeEnum.Product, null, "GLEM SP2195 EC1E1PU W2 F60 GHC631WH");
            Item_ADD(itemRepo, "942022259", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT. BETA EB25E2BAXSFM AF90 942022259");
            Item_ADD(itemRepo, "942022260", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT. BETA EB25E2BABL1FM AF90 942022260");
            Item_ADD(itemRepo, "942022261", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA EB25D2PU XS FM AF90 942022261");
            Item_ADD(itemRepo, "942022262", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA EB25D2PU W2 FM AF90 942022262");
            Item_ADD(itemRepo, "942022263", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA EB25D2PU BL1FM AF90 942022263");
            Item_ADD(itemRepo, "942022264", "9102", null, 3, ItemTypeEnum.Product, null, "BEST BETA T1A2PU XS H.6 A/F 90 SHI LEI");
            Item_ADD(itemRepo, "942022265", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.COMPASS MBMD2RO XS A/F 90");
            Item_ADD(itemRepo, "942022266", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.COMPASS MBMD2RO XS A/F 60");
            Item_ADD(itemRepo, "942022267", "9102", null, 4, ItemTypeEnum.Product, null, "BEST COMPASS MBMD2RO XS A/F 60");
            Item_ADD(itemRepo, "942022268", "9102", null, 4, ItemTypeEnum.Product, null, "BEST COMPASS MBMD2RO XS A/F 90");
            Item_ADD(itemRepo, "942022269", "9102", null, 4, ItemTypeEnum.Product, null, "BEST COMPASS MBMD2RO XS A/F120");
            Item_ADD(itemRepo, "942022282", "9102", null, 4, ItemTypeEnum.Product, null, "APELL 412S1ASLMEXSF60ETB CSA520TX60");
            Item_ADD(itemRepo, "942022283", "9102", null, 4, ItemTypeEnum.Product, null, "APELL 412S1ASLMEXSF90ETB CSA520TX90");
            Item_ADD(itemRepo, "942022285", "9102", null, 8, ItemTypeEnum.Product, null, "SP2196-3 S1L1SL VM XS F90 3+1 BERTAZZONI");
            Item_ADD(itemRepo, "942022291", "9102", null, 5, ItemTypeEnum.Product, null, "BROAN-ELI USP780HS FMXSA REV.C RMP17004");
            Item_ADD(itemRepo, "942022292", "9102", null, 5, ItemTypeEnum.Product, null, "US-PM500 SS");
            Item_ADD(itemRepo, "942022293", "9102", null, 5, ItemTypeEnum.Product, null, "US-PM390  SILVER     PM390S");
            Item_ADD(itemRepo, "942022294", "9102", null, 5, ItemTypeEnum.Product, null, "US-PM390 HS SILVER");
            Item_ADD(itemRepo, "942022295", "9102", null, 5, ItemTypeEnum.Product, null, "NUTONE US-PM390 HS SILVER NSPM390");
            Item_ADD(itemRepo, "942022296", "9102", null, 5, ItemTypeEnum.Product, null, "BROAN US-PME300 HS SILVER Rev.B");
            Item_ADD(itemRepo, "942022297", "9102", null, 4, ItemTypeEnum.Product, null, "BROAN ELITE US-ES24 XS A30 E1230SS");
            Item_ADD(itemRepo, "942022298", "9102", null, 4, ItemTypeEnum.Product, null, "BROAN/EL US-ES24L1A2SL XS A60 E1224SSLS");
            Item_ADD(itemRepo, "942022299", "9102", null, 4, ItemTypeEnum.Product, null, "BROAN/ELITE US-ES24 XS A 30 E1230SS LS");
            Item_ADD(itemRepo, "942022302", "9102", null, 4, ItemTypeEnum.Product, null, "CELLO 413/2015M1E2SLW2A/F60KRONOS500260W");
            Item_ADD(itemRepo, "942022303", "9102", null, 4, ItemTypeEnum.Product, null, "CELLO 413/2015M1E2SLWXA/F60KRONOS500260I");
            Item_ADD(itemRepo, "942022304", "9102", null, 5, ItemTypeEnum.Product, null, "US/GEN.ELEC.PM390 HS SILVER");
            Item_ADD(itemRepo, "942022318", "9102", null, 2, ItemTypeEnum.Product, null, "GORENJE K24R L1A2PU R9005 A/F60 DK63CLB");
            Item_ADD(itemRepo, "942022319", "9102", null, 2, ItemTypeEnum.Product, null, "GORENJE K24R L1A2PU AVORIO A/F60 DK63CLI");
            Item_ADD(itemRepo, "942022320", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA P550 P-C S1C2SLFMW2AF MINI600WHITE");
            Item_ADD(itemRepo, "942022321", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA P750 P-C S1C2SL FMW2 A/F-MINI900WH");
            Item_ADD(itemRepo, "942022322", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA P560 M1L2EL FM W2 A52-MINI600WHITE");
            Item_ADD(itemRepo, "942022323", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA P760 M1L2EL FM W2 A70-MINI900WHITE");
            Item_ADD(itemRepo, "942022324", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA P560 M1L2EL FM XS A52-MINI600INOX");
            Item_ADD(itemRepo, "942022325", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA P760 M1L2EL FM XS A70-MINI900INOX");
            Item_ADD(itemRepo, "942022326", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA PASC580 P-B FPX L1A2ESD XS A52");
            Item_ADD(itemRepo, "942022327", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA PASC780 P-B FPX L1A2ESD XS A70");
            Item_ADD(itemRepo, "942022328", "9102", null, 4, ItemTypeEnum.Product, null, "LY-VENT 04/414 S2LSL XS F60");
            Item_ADD(itemRepo, "942022329", "9102", null, 8, ItemTypeEnum.Product, null, "LY VENTSP2003S2C2PUXSFMF60-LV9760XS76603");
            Item_ADD(itemRepo, "942022330", "9102", null, 5, ItemTypeEnum.Product, null, "LY-VENT P550 P-C S1C2SL FM XS F");
            Item_ADD(itemRepo, "942022331", "9102", null, 5, ItemTypeEnum.Product, null, "LY-VENT P750 P-C  S1C2SL FM XS F");
            Item_ADD(itemRepo, "942022332", "9102", null, 4, ItemTypeEnum.Product, null, "MALLOCA 04/417 P-C S1A2BA GR3/AL.F60");
            Item_ADD(itemRepo, "942022333", "9102", null, 5, ItemTypeEnum.Product, null, "MALLOCA PASC780 P-BFPXL1A2ESDXSF70 K7205");
            Item_ADD(itemRepo, "942022334", "9102", null, 6, ItemTypeEnum.Product, null, "MALLOCA SIGMA L1D2ST XS F90 K820T");
            Item_ADD(itemRepo, "942022335", "9102", null, 6, ItemTypeEnum.Product, null, "MALLOCA DELTA L1A2PU XS F90 K890V");
            Item_ADD(itemRepo, "942022336", "9102", null, 4, ItemTypeEnum.Product, null, "MEX 04-412 S2 L SL W2/XS F60 4122MW/X60");
            Item_ADD(itemRepo, "942022337", "9102", null, 8, ItemTypeEnum.Product, null, "MEX SP2195-3 S2L2 PUCR EB MET 2FM F60");
            Item_ADD(itemRepo, "942022339", "9102", null, 8, ItemTypeEnum.Product, null, "MEX SP2195-3 S2L2PUCR  EB XS 2FM F60");
            Item_ADD(itemRepo, "942022340", "9102", null, 8, ItemTypeEnum.Product, null, "MEX SP2195-3 S2L2 PUCR EB XS 2FM F90");
            Item_ADD(itemRepo, "942022341", "9102", null, 3, ItemTypeEnum.Product, null, "MORA ZETA L1D2TC BL/XS A/F 60 OK697GX");
            Item_ADD(itemRepo, "942022342", "9102", null, 3, ItemTypeEnum.Product, null, "MORA ZETA L1D2TC BL/XS A/F 90 OK997GX");
            Item_ADD(itemRepo, "942022343", "9102", null, 5, ItemTypeEnum.Product, null, "M-SYS.P580FPXL1D2ESDXSA52 MSUN52 P.LIGHT");
            Item_ADD(itemRepo, "942022344", "9102", null, 5, ItemTypeEnum.Product, null, "M-SYS.P780FPXL1D2ESDXSA52 MSUN72 P.LIGHT");
            Item_ADD(itemRepo, "942022345", "9102", null, 3, ItemTypeEnum.Product, null, "M-SYSTEM BETA EVA2PU XS A60 MCHS600IX");
            Item_ADD(itemRepo, "942022346", "9102", null, 3, ItemTypeEnum.Product, null, "M-SYSTEM BETA EVA2PU XS A90 MCHS900IX");
            Item_ADD(itemRepo, "942022347", "9102", null, 2, ItemTypeEnum.Product, null, "WIN W.HF6D2PU XS F60 BEIT60INL");
            Item_ADD(itemRepo, "942022348", "9102", null, 2, ItemTypeEnum.Product, null, "WIN W.HF6D2PU XS F90 BEIT90INL");
            Item_ADD(itemRepo, "942022349", "9102", null, 5, ItemTypeEnum.Product, null, "HAFELE PASC780 P-B FPX L1D2ESD XS F70");
            Item_ADD(itemRepo, "942022350", "9102", null, 3, ItemTypeEnum.Product, null, "ZETA HF8 D2 TC BL/XS F90 BESMMVNXA");
            Item_ADD(itemRepo, "942022351", "9102", null, 3, ItemTypeEnum.Product, null, "ZETA HF8 D2 TC WH/XS F90 BESMMVBXA");
            Item_ADD(itemRepo, "942022352", "9102", null, 3, ItemTypeEnum.Product, null, "THETA ASC HF8 D2 ST XS A/F 90 BEBOMINA");
            Item_ADD(itemRepo, "942022353", "9102", null, 3, ItemTypeEnum.Product, null, "BETA HF6D2PU H.6 2FM XS F60 BEIB60INL");
            Item_ADD(itemRepo, "942022354", "9102", null, 3, ItemTypeEnum.Product, null, "BETA HF6D2PU H.6 2FM XS F90 BEIB90INL");
            Item_ADD(itemRepo, "942022355", "9102", null, 6, ItemTypeEnum.Product, null, "DELTA HF6 D2 PU F60 XS BECU60VIA");
            Item_ADD(itemRepo, "942022356", "9102", null, 6, ItemTypeEnum.Product, null, "DELTA HF6 D2 PU F90 XS BECU90VIA");
            Item_ADD(itemRepo, "942022357", "9102", null, 8, ItemTypeEnum.Product, null, "PELGRIM SP2196 EC1E2PU FMW2A60 WA205WIT");
            Item_ADD(itemRepo, "942022359", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO WIN W.S1E2PU T XS A/F 50 90326");
            Item_ADD(itemRepo, "942022360", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO WIN W.S1E2PU T XS A/F 60 90327");
            Item_ADD(itemRepo, "942022361", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO WIN W.HF8D2ELER XS A/F 50 90322");
            Item_ADD(itemRepo, "942022362", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO WIN W.HF8D2ELER XS A/F 60 90323");
            Item_ADD(itemRepo, "942022363", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO WIN HF8D2ELER W2 A/F 50 92322");
            Item_ADD(itemRepo, "942022364", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO WIN HF8D2ELER W2 A/F 60 92323");
            Item_ADD(itemRepo, "942022365", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO K24 P-D EM E2 PU XS A50 90328");
            Item_ADD(itemRepo, "942022366", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO K24 P-D EM E2 PU XS A60 90329");
            Item_ADD(itemRepo, "942022368", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO ES23 S2E2SL 2F W2 FR1 A60 90609");
            Item_ADD(itemRepo, "942022369", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO ES23 M1E2SL 2F W2 A50 90608");
            Item_ADD(itemRepo, "942022370", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 417 S1D2BA GR3/AL. A60 90656");
            Item_ADD(itemRepo, "942022371", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414 VMot E2BAEXT W2/XS FR1 60 95622");
            Item_ADD(itemRepo, "942022372", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414 VMot E1BAEXT W2/XS FR1 50 95621");
            Item_ADD(itemRepo, "942022373", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414 VMot E2 BA W2/XS FR1 60 95642");
            Item_ADD(itemRepo, "942022374", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414 VMot E1 BA W2/XS FR1 50 95641");
            Item_ADD(itemRepo, "942022375", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414 VMot E2BAEC W2/XS FR1 60 97804");
            Item_ADD(itemRepo, "942022376", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414 VMot E1BAEC W2/XS FR1 50 97803");
            Item_ADD(itemRepo, "942022377", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414 VMan E2 BA W2/XS FR1 60 95632");
            Item_ADD(itemRepo, "942022378", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414 VMan E1 BA W2/XS FR1 50 95631");
            Item_ADD(itemRepo, "942022379", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 414-2 V2D2BA W2/XS FR1 50 90616");
            Item_ADD(itemRepo, "942022381", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO 98/425 M1D2SL MET/XS A60 90606");
            Item_ADD(itemRepo, "942022388", "9102", null, 8, ItemTypeEnum.Product, null, "SAVO SP2195-3 EC1E2SL W2 1FM A50 90715");
            Item_ADD(itemRepo, "942022389", "9102", null, 8, ItemTypeEnum.Product, null, "SAVO SP2195-3 EC1E2SL XS 1FM A50 90713");
            Item_ADD(itemRepo, "942022391", "9102", null, 5, ItemTypeEnum.Product, null, "SAVO P580 V3D2EL FM XS A52 90623");
            Item_ADD(itemRepo, "942022392", "9102", null, 5, ItemTypeEnum.Product, null, "SAVO P580 P-D FPX D2 EM ESD XS 90638");
            Item_ADD(itemRepo, "942022393", "9102", null, 5, ItemTypeEnum.Product, null, "SAVO P780 P-D FPX D2 EM ESD XS 90639");
            Item_ADD(itemRepo, "942022394", "9102", null, 5, ItemTypeEnum.Product, null, "SAVO PASC580 EC D2 ESD FPX XS 97331");
            Item_ADD(itemRepo, "942022395", "9102", null, 5, ItemTypeEnum.Product, null, "SAVO PASC780 EC D2 ESD FPX XS 97332");
            Item_ADD(itemRepo, "942022397", "9102", null, 5, ItemTypeEnum.Product, null, "SAVO PASC580 L1D2ELD FM XS A52 90624");
            Item_ADD(itemRepo, "942022398", "9102", null, 5, ItemTypeEnum.Product, null, "SAVO PASC580 P-B FPX L1D2ESD XSA52 90634");
            Item_ADD(itemRepo, "942022399", "9102", null, 5, ItemTypeEnum.Product, null, "SAVO PASC780 P-B FPX L1D2ESD XSA70 90636");
            Item_ADD(itemRepo, "942022400", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO BLISS W2 A60 90742");
            Item_ADD(itemRepo, "942022401", "9102", null, 3, ItemTypeEnum.Product, null, "SAVO ZETA HF8D2TC BL/XS A/F 60 86626");
            Item_ADD(itemRepo, "942022402", "9102", null, 3, ItemTypeEnum.Product, null, "SAVO ZETA HF8D2TC BL/XS A/F 90 86629");
            Item_ADD(itemRepo, "942022403", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO TAU SALD.L1 D2 EL ASC XS AF90 90423");
            Item_ADD(itemRepo, "942022407", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO SIGMA ASC ECD2EL XS A/F 90 97025");
            Item_ADD(itemRepo, "942022418", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO RHO S1D2PU XS A/F 90 90416");
            Item_ADD(itemRepo, "942022419", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO RHO S1D2PU XS A/F 60 90415");
            Item_ADD(itemRepo, "942022420", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO RHO EMD2PU XS A60 90418");
            Item_ADD(itemRepo, "942022421", "9102", null, 5, ItemTypeEnum.Product, null, "THE.P580FPXHF8D2ESD FMXSA52 TFP580 P-LIG");
            Item_ADD(itemRepo, "942022422", "9102", null, 5, ItemTypeEnum.Product, null, "THE.P780FPXHF8D2ESD FMXSA52 TFP780 P-LIG");
            Item_ADD(itemRepo, "942022423", "9102", null, 4, ItemTypeEnum.Product, null, "THERMEX SUPERSILENT XS A60");
            Item_ADD(itemRepo, "942022424", "9102", null, 4, ItemTypeEnum.Product, null, "THERMEX SUPERSILENT W2 A60");
            Item_ADD(itemRepo, "942022426", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA GLASS HF8D2VU XS F90");
            Item_ADD(itemRepo, "942022428", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA GLASS HF8D2VU XS F60");
            Item_ADD(itemRepo, "942022430", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA PLUS EB25D2VU XS A/F 90");
            Item_ADD(itemRepo, "942022431", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA PLUS EB25D2VU XS A/F 60");
            Item_ADD(itemRepo, "942022432", "9102", null, 5, ItemTypeEnum.Product, null, "BEST US-195ES XS A70 - P195ES70SB");
            Item_ADD(itemRepo, "942022433", "9102", null, 2, ItemTypeEnum.Product, null, "ELECTROLUX ROCOCO S1E2PU BL262/BRASS 60");
            Item_ADD(itemRepo, "942022434", "9102", null, 2, ItemTypeEnum.Product, null, "ELECTR.ROCOCO S1E2PU ANCIENT WH/BRASS 60");
            Item_ADD(itemRepo, "942022435", "9102", null, 2, ItemTypeEnum.Product, null, "ELECTROLUX ROCOCO S1E2PU BL262/CHROME 60");
            Item_ADD(itemRepo, "942022436", "9102", null, 2, ItemTypeEnum.Product, null, "ELECTR.ROCOCO S1E2PU ANCIENT WH/CHROME60");
            Item_ADD(itemRepo, "942022437", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA MBME2BA MET/BL A60 942022437");
            Item_ADD(itemRepo, "942022438", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA MBME2BA MET/W A60 942022438");
            Item_ADD(itemRepo, "942022439", "9102", null, 6, ItemTypeEnum.Product, null, "MORA DELTA L1D2PU XS A/F 60 OK637G");
            Item_ADD(itemRepo, "942022440", "9102", null, 6, ItemTypeEnum.Product, null, "MORA RHO L1D2PU XS A/F 60 OK647G");
            Item_ADD(itemRepo, "942022441", "9102", null, 5, ItemTypeEnum.Product, null, "BRANDT P580 P-LIG L1D2EL FMXSF52 AG9501X");
            Item_ADD(itemRepo, "942022442", "9102", null, 5, ItemTypeEnum.Product, null, "SAUTER P580 P-LIG L1D2EL FMXSF52 SHG501X");
            Item_ADD(itemRepo, "942022443", "9102", null, 5, ItemTypeEnum.Product, null, "PELGRIM P591PSMARTS1D2PUXSA52 ISW855RVS");
            Item_ADD(itemRepo, "942022444", "9102", null, 5, ItemTypeEnum.Product, null, "PELGRIM P791PSMARTS1D2PUXSA70 ISW875RVS");
            Item_ADD(itemRepo, "942022445", "9102", null, 4, ItemTypeEnum.Product, null, "PELGRIM WITCH M1D2ELST MET.A60 SLK645RVS");
            Item_ADD(itemRepo, "942022446", "9102", null, 4, ItemTypeEnum.Product, null, "PELGRIM WITCH M1D2ELST MET.A90 SLK945RVS");
            Item_ADD(itemRepo, "942022447", "9102", null, 5, ItemTypeEnum.Product, null, "ATAG P591 L1D2EL XSA52 P-LIGHT WU50211BM");
            Item_ADD(itemRepo, "942022448", "9102", null, 5, ItemTypeEnum.Product, null, "ATAG P791 L1D2EL XSA70 P-LIGHT WU70211BM");
            Item_ADD(itemRepo, "942022449", "9102", null, 6, ItemTypeEnum.Product, null, "AEG RHO S1D2PU XS F60 MEA");
            Item_ADD(itemRepo, "942022450", "9102", null, 4, ItemTypeEnum.Product, null, "SAVO COMPASS MBMD2RO XS A/F 60 90640");
            Item_ADD(itemRepo, "942022451", "9102", null, 2, ItemTypeEnum.Product, null, "SAVO WIN HF8D2ELER ANTRANER0 AF 60 93323");
            Item_ADD(itemRepo, "942022453", "9102", null, 3, ItemTypeEnum.Product, null, "SAVO THETA HF8 D2 ST A/F 60 XS 90352");
            Item_ADD(itemRepo, "942022454", "9102", null, 3, ItemTypeEnum.Product, null, "SAVO THETA HF8 D2 ST A/F 90 XS 90353");
            Item_ADD(itemRepo, "942022455", "9102", null, 3, ItemTypeEnum.Product, null, "SAVO THETA EM D2 ST A60 XS 90357");
            Item_ADD(itemRepo, "942022456", "9102", null, 3, ItemTypeEnum.Product, null, "SAVO THETA EM D2 ST A90 XS 90358");
            Item_ADD(itemRepo, "942022457", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO SIGMA HF8D2ST XS A/F 60 90451");
            Item_ADD(itemRepo, "942022458", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO SIGMA HF8D2ST XS A/F 90 90452");
            Item_ADD(itemRepo, "942022459", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO SIGMA EM D2 ST XS A60 90453");
            Item_ADD(itemRepo, "942022460", "9102", null, 6, ItemTypeEnum.Product, null, "SAVO SIGMA EM D2 ST XS A90 90454");
            Item_ADD(itemRepo, "942022461", "9102", null, 3, ItemTypeEnum.Product, null, "SAVO THETA HF8D2ST A/F6 ANTRANERO 93352");
            Item_ADD(itemRepo, "942022462", "9102", null, 3, ItemTypeEnum.Product, null, "SAVO THETA HF8D2ST A/F90 ANTRANERO 93353");
            Item_ADD(itemRepo, "942022463", "9102", null, 8, ItemTypeEnum.Product, null, "PELGRIM SP2196 EC1E2PU FMXSA60 OWA205RVS");
            Item_ADD(itemRepo, "942022464", "9102", null, 8, ItemTypeEnum.Product, null, "PELGRIM SP2196 EC1E2PU FMW2A60 OWA205WIT");
            Item_ADD(itemRepo, "942022465", "9102", null, 3, ItemTypeEnum.Product, null, "BETA GLASS HF8D2VUI BL/XS A/F 60 LAM2875");
            Item_ADD(itemRepo, "942022466", "9102", null, 3, ItemTypeEnum.Product, null, "BETA GLASS HF8D2VUI BL/XS A/F 90 LAM2876");
            Item_ADD(itemRepo, "942022467", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA PLUS EB25D2VU XS A/F 60");
            Item_ADD(itemRepo, "942022468", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA PLUS EB25D2VU W2 A/F 60");
            Item_ADD(itemRepo, "942022469", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA PLUS EB25D2VU BL1 A/F 60");
            Item_ADD(itemRepo, "942022470", "9102", null, 3, ItemTypeEnum.Product, null, "A.MARTIN BETA GLASS HF8D2VU XS A/F 90");
            Item_ADD(itemRepo, "942022471", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA PLUS EB25D2VU BL1 A/F 90");
            Item_ADD(itemRepo, "942022472", "9102", null, 3, ItemTypeEnum.Product, null, "WESTINGHOUSE BETA GLASS HF8D2VU XS A/F90");
            Item_ADD(itemRepo, "942022473", "9102", null, 8, ItemTypeEnum.Product, null, "SAVO SP2002 EC1E1SLVM W2 F60 90734");
            Item_ADD(itemRepo, "942022474", "9102", null, 8, ItemTypeEnum.Product, null, "SAVO SP2002 EC1E1SLVM W2 F50 90733");
            Item_ADD(itemRepo, "942022475", "9102", null, 8, ItemTypeEnum.Product, null, "SAVO SP2195-3 EC1E2SL W2 1FM A60 90716");
            Item_ADD(itemRepo, "942022476", "9102", null, 8, ItemTypeEnum.Product, null, "SAVO SP2195-3 EC1E2SL W2 1FM A50 90715");
            Item_ADD(itemRepo, "942022477", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA S1E2BAXSFM AF90 942022477");
            Item_ADD(itemRepo, "942022478", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA 2GFA MBME2PSQ CHAMPAGNE A60");
            Item_ADD(itemRepo, "942022479", "9102", null, 4, ItemTypeEnum.Product, null, "SMEG ES24 L1A2ELS FR8 2F XSA60 070A1616A");
            Item_ADD(itemRepo, "942022480", "9102", null, 4, ItemTypeEnum.Product, null, "SMEG ES24L1A2ELS FR8 F2XSA90-070A1617A");
            Item_ADD(itemRepo, "942022481", "9102", null, 5, ItemTypeEnum.Product, null, "SMEG P791 L1D2EL XS A70 P-LIGHT AUSTR");
            Item_ADD(itemRepo, "942022482", "9102", null, 5, ItemTypeEnum.Product, null, "SMEG P591 L1D2EL XS A52 P-LIGHT AUSTR");
            Item_ADD(itemRepo, "942022483", "9102", null, 5, ItemTypeEnum.Product, null, "SMEG P991 L1D2EL XS A90 P-LIGHT AUSTR");
            Item_ADD(itemRepo, "942022484", "9102", null, 4, ItemTypeEnum.Product, null, "SMEG 98/412S1ASL METXS F60 ETB SA520TX60");
            Item_ADD(itemRepo, "942022485", "9102", null, 3, ItemTypeEnum.Product, null, "D.KNOLL BETA GLASS HF8D2VUI BL/XS A/F 90");
            Item_ADD(itemRepo, "942022486", "9102", null, 3, ItemTypeEnum.Product, null, "D.KNOLL BETA GLASS HF8D2VUI BL/XS A/F 60");
            Item_ADD(itemRepo, "942022487", "9102", null, 3, ItemTypeEnum.Product, null, "SMEG THETA L1D2NE1RXSAF60P.LIG SHW181X60");
            Item_ADD(itemRepo, "942022488", "9102", null, 3, ItemTypeEnum.Product, null, "SMEG THETA L1D2NE1RXSAF70P.LIG SHW181X70");
            Item_ADD(itemRepo, "942022489", "9102", null, 3, ItemTypeEnum.Product, null, "SMEG THETA L1D2NE1RXSAF90P.LIG SHW181X90");
            Item_ADD(itemRepo, "942022490", "9102", null, 3, ItemTypeEnum.Product, null, "AEG BETA PLUS EB25D2VU XS A/F 90");
            Item_ADD(itemRepo, "942022491", "9102", null, 3, ItemTypeEnum.Product, null, "AEG BETA PLUS EB25D2VU XS A/F 60");
            Item_ADD(itemRepo, "942022492", "9102", null, 3, ItemTypeEnum.Product, null, "AEG BETA PLUS EB25D2VU XS A/F 90");
            Item_ADD(itemRepo, "942022493", "9102", null, 3, ItemTypeEnum.Product, null, "AEG BETA PLUS EB25D2VU XS A/F 60");
            Item_ADD(itemRepo, "942022494", "9102", null, 3, ItemTypeEnum.Product, null, "ELECT.BETA S1E2BAXSFM A/F 60 MEA");
            Item_ADD(itemRepo, "942022495", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA X-PLUS EB25D2VU XS A/F 90");
            Item_ADD(itemRepo, "942022496", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA X-PLUS EB25D2VU XS A/F 90UK");
            Item_ADD(itemRepo, "942022497", "9102", null, 3, ItemTypeEnum.Product, null, "FAURE BETA X-PLUS EB25D2VU XS A/F 90");
            Item_ADD(itemRepo, "942022498", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA PLUS EB25D2VU XS A/F 90");
            Item_ADD(itemRepo, "942022499", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA PLUS EB25D2VU XS A/F 90 UK");
            Item_ADD(itemRepo, "942022500", "9102", null, 3, ItemTypeEnum.Product, null, "FAURE BETA PLUS EB25D2VU XS A/F 90");
            Item_ADD(itemRepo, "942022501", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA PLUS EB25D2VU XS A/F 60 UK");
            Item_ADD(itemRepo, "942022502", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P195P1M52SB6");
            Item_ADD(itemRepo, "942022503", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P195P1M70SB6");
            Item_ADD(itemRepo, "942022504", "9102", null, 5, ItemTypeEnum.Product, null, "BEST P195P2M70SB");
            Item_ADD(itemRepo, "942022505", "9102", null, 5, ItemTypeEnum.Product, null, "US-PM500  SS");
            Item_ADD(itemRepo, "942022506", "9102", null, 4, ItemTypeEnum.Product, null, "BROAN ELITE US-ES24 XS A30 E1230SS");
            Item_ADD(itemRepo, "942022507", "9102", null, 4, ItemTypeEnum.Product, null, "BROAN/ELITE US-ES24 XS A 30 E1230SS LS");
            Item_ADD(itemRepo, "942022508", "9102", null, 2, ItemTypeEnum.Product, null, "GORENJE K24R L1D2PU R9005 A/F60 DK63CLB");
            Item_ADD(itemRepo, "942022509", "9102", null, 2, ItemTypeEnum.Product, null, "GORENJE K24R L1D2PU AVORIO A/F60 DK63CLI");
            Item_ADD(itemRepo, "942022510", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA PASC580 P-B FPX L1D2ESD XS A52");
            Item_ADD(itemRepo, "942022511", "9102", null, 5, ItemTypeEnum.Product, null, "KRONA PASC780 P-B FPX L1D2ESD XS A70");
            Item_ADD(itemRepo, "942022512", "9102", null, 4, ItemTypeEnum.Product, null, "AEG SKLOK EB40D2PU XS F60 UK");
            Item_ADD(itemRepo, "942022513", "9102", null, 4, ItemTypeEnum.Product, null, "AEG SKLOK EB40D2PU XS F90 UK");
            Item_ADD(itemRepo, "942022514", "9102", null, 4, ItemTypeEnum.Product, null, "BROAN/EL US-ES24L1A2SL XS A60 E1224SSLS");
            Item_ADD(itemRepo, "942022515", "9102", null, 6, ItemTypeEnum.Product, null, "ZANUSSI RHO EB25D2PU XS A/F 90 UK");
            Item_ADD(itemRepo, "942022516", "9102", null, 4, ItemTypeEnum.Product, null, "ELEC.VERSA 2GFA MBME2PSQ BL/BL R9005 A60");
            Item_ADD(itemRepo, "942022517", "9102", null, 4, ItemTypeEnum.Product, null, "ELECT.VERSA 2GFA MBME2PSQ WH/WH A60");
            Item_ADD(itemRepo, "942022518", "9102", null, 3, ItemTypeEnum.Product, null, "ZANUSSI BETA EB25D2PU BL1 FM A/F 90");
            Item_ADD(itemRepo, "942022519", "9102", null, 3, ItemTypeEnum.Product, null, "AEG THETA HF8D2ST A/F90 ANTRANERO");
            Item_ADD(itemRepo, "942022520", "9102", null, 6, ItemTypeEnum.Product, null, "AEG DELTA EC20D2PU XS A/F 60 UK");
            Item_ADD(itemRepo, "942022521", "9102", null, 6, ItemTypeEnum.Product, null, "AEG DELTA EC20D2PU XS A/F 90 UK");
            Item_ADD(itemRepo, "942022522", "9102", null, 6, ItemTypeEnum.Product, null, "AEG RHO EB25D2PU XS A/F 60 UK");
            Item_ADD(itemRepo, "942022523", "9102", null, 6, ItemTypeEnum.Product, null, "AEG RHO EB25D2PU XS A/F 90 UK");
            Item_ADD(itemRepo, "942022524", "9102", null, 3, ItemTypeEnum.Product, null, "BETA PLUS EB25D2VU XS A/F 60 UK-WR");
            Item_ADD(itemRepo, "942022525", "9102", null, 3, ItemTypeEnum.Product, null, "BETA PLUS EB25D2VU XS A/F 90 UK-WR");
            Item_ADD(itemRepo, "942022526", "9102", null, 6, ItemTypeEnum.Product, null, "RHO S1D2PU XS A/F 60 UK-WR");
            Item_ADD(itemRepo, "942022527", "9102", null, 6, ItemTypeEnum.Product, null, "RHO EB25D2PU XS A/F 90 UK-WR");
            Item_ADD(itemRepo, "942022528", "9102", null, 6, ItemTypeEnum.Product, null, "DELTA EC20D2PU XS A/F 60 UK-WR");
            Item_ADD(itemRepo, "942022529", "9102", null, 6, ItemTypeEnum.Product, null, "DELTA EC20D2PU XS A/F 90 UK-WR");
            Item_ADD(itemRepo, "942022530", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU XS A/F 60 UK-WR");
            Item_ADD(itemRepo, "942022531", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU XS A/F 70 UK-WR");
            Item_ADD(itemRepo, "942022532", "9102", null, 2, ItemTypeEnum.Product, null, "WIN S1E2PU XS A/F 90 UK-WR");
            Item_ADD(itemRepo, "942022533", "9102", null, 2, ItemTypeEnum.Product, null, "ROCOCO S1E2PU BL262/CHROME A/F 60 UK-WR");
            Item_ADD(itemRepo, "942022540", "9102", null, 3, ItemTypeEnum.Product, null, "AEG THETA HF8D2ST A/F 90 XS");
            Item_ADD(itemRepo, "942022541", "9102", null, 3, ItemTypeEnum.Product, null, "AEG THETA HF8D2ST A/F 90 XS");
            Item_ADD(itemRepo, "942022559", "9102", null, 4, ItemTypeEnum.Product, null, "BEST VERSA MBSE2PSQ MET A60 942022559");
            Item_ADD(itemRepo, "942022562", "9102", null, 8, ItemTypeEnum.Product, null, "SP2003 EC1E2SLSVME.1FMA60 NU2003 BEST D");
            Item_ADD(itemRepo, "942022563", "9102", null, 4, ItemTypeEnum.Product, null, "BEST 425 P-C L1ASL MET/XS A90 SHI LEI");
            Item_ADD(itemRepo, "942022564", "9102", null, 3, ItemTypeEnum.Product, null, "BEST BETA T1A2PUXS304 H.6 A/F 90 SHI LEI");
            Item_ADD(itemRepo, "942022565", "9102", null, 3, ItemTypeEnum.Product, null, "BEST BETA S1E2PU ANTRANERO H6 2FM 60");

        }
        public static void Seed_PLB_SampleProdOrders(IDbContextCore db)
        {
            ProductionOrderRepo repo = new ProductionOrderRepo(db);

            repo.Add(new ProductionOrder()
            {
                LineId = 3,
                StartDate = DateTime.Now.AddDays(4),
                EndDate = DateTime.Now.AddDays(4).AddMinutes(20),
                Notice = "Testowe",
                OrderNumber = "1608009010",
                PncId = db.Items.FirstOrDefault(x => x.Code == "942022016").Id,
                QtyPlanned = 30,
                QtyRemain = 30,
                SerialNoFrom = "00001",
                SerialNoTo = "00030",
                LastUpdate = DateTime.Now
            });

            repo.Add(new ProductionOrder()
            {
                LineId = 3,
                StartDate = DateTime.Now.AddDays(4),
                EndDate = DateTime.Now.AddDays(4).AddMinutes(20),
                Notice = "Testowe",
                OrderNumber = "1608009020",
                PncId = db.Items.FirstOrDefault(x => x.Code == "942022218").Id,
                QtyPlanned = 40,
                QtyRemain = 40,
                SerialNoFrom = "00031",
                SerialNoTo = "00070",
                LastUpdate = DateTime.Now
            });

            repo.Add(new ProductionOrder()
            {
                LineId = 3,
                StartDate = DateTime.Now.AddDays(4),
                EndDate = DateTime.Now.AddDays(4).AddMinutes(20),
                Notice = "Testowe",
                OrderNumber = "1608009030",
                PncId = db.Items.FirstOrDefault(x => x.Code == "942022222").Id,
                QtyPlanned = 20,
                QtyRemain = 20,
                SerialNoFrom = "00071",
                SerialNoTo = "00090",
                LastUpdate = DateTime.Now
            });
        }

        public static void Seed_PLB_MasterData(IDbContextMasterData db)
        {
            //BRIGADES ---------------------------------------------------------------------------------------------------------------------------------------
            RepoLabourBrigade lbRepo = new RepoLabourBrigade(db);
            lbRepo.Add(new LabourBrigade() { Name = "Brygada A" });
            lbRepo.Add(new LabourBrigade() { Name = "Brygada B" });
            lbRepo.Add(new LabourBrigade() { Name = "Brygada C" });

            //SUPPLIERS ---------------------------------------------------------------------------------------------------------------------------------------
            RepoContractor contractorRepo = new RepoContractor(db);
            contractorRepo.Add(new Contractor() { Code = "100200", Country = "PL", Name = "Supplier1", NIP = "6004589901", ContactEmail = "email1@test987.text" });

            //********************************************************OBSZARY, ZASOBY, STANOWISKA, PROCESY********************************************************
            //****************************************************************************************************************************************************
            //****************************************************************************************************************************************************
            ProcessRepo processRepo = new ProcessRepo(db, null);
            RepoArea repoArea = new RepoArea(db);
            ItemRepo itemRepo = new ItemRepo(db);
            //ItemRepo itemRepo = new ItemRepo(db);
            //ResourceRepo resourceRepo = new ResourceRepo(db);
            ResourceRepo resourceRepo = new ResourceRepo(db);
            RepoWorkstation repoWorkstation = new RepoWorkstation(db);

            repoArea.Add(new Area() { Name = "Factory" });
            int virtualId = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.VirtualResource, 0, null, "Virual Line");

            int? resrcGrp = 0;
            int? itmGrp = 0;
            //int lineId = 0;

            //ASSEMBLY-AREA---------------------------------------------------------------------------------------------------------------------------------------------------
            resrcGrp = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Group, 0, null, "Assembly Lines");
            int lineId1 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE 1");
            int lineId2 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE 2");
            int lineId3 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE 3");
            int lineId4 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE 4");
            int lineId5 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE 5");
            int lineId6 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE 6");
            int lineId7 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE 7");
            int lineId8 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE 8");
            int lineId9 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "MON PAN");
            int lineId10 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "LINE OF Blowers 1");
            int lineId11 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "Line of blowers 2");
            int lineId12 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "Line of blowers 3");
            int lineId13 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "central sito tuby");
            int lineId14 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "central sito slight of components");
            int lineId15 = Resource_ADD(resourceRepo, 1, ResourceTypeEnum.Resource, 60, resrcGrp, "central sito of carcas");

            Workstation_ADD(repoWorkstation, lineId1, 1, "W.LINE 1");
            Workstation_ADD(repoWorkstation, lineId2, 1, "W.LINE 2");
            Workstation_ADD(repoWorkstation, lineId3, 1, "W.LINE 3");
            Workstation_ADD(repoWorkstation, lineId4, 1, "W.LINE 4");
            Workstation_ADD(repoWorkstation, lineId5, 1, "W.LINE 5");
            Workstation_ADD(repoWorkstation, lineId6, 1, "W.LINE 6");
            Workstation_ADD(repoWorkstation, lineId7, 1, "W.LINE 7");
            Workstation_ADD(repoWorkstation, lineId8, 1, "W.LINE 8");
            Workstation_ADD(repoWorkstation, lineId9, 1, "W.MON PAN");
            Workstation_ADD(repoWorkstation, lineId10, 1, "W.LINE OF Blowers 1");
            Workstation_ADD(repoWorkstation, lineId11, 1, "W.Line of blowers 2");
            Workstation_ADD(repoWorkstation, lineId12, 1, "W.Line of blowers 3");
            Workstation_ADD(repoWorkstation, lineId13, 1, "W.central sito tuby");
            Workstation_ADD(repoWorkstation, lineId14, 1, "W.central sito slight of components");
            Workstation_ADD(repoWorkstation, lineId15, 1, "W.central sito of carcas");

            int hoodAssemblyProcessId = Process_ADD(processRepo, "Hood Assembly");
            itmGrp = Item_ADD(itemRepo, "GROUP", "000000", resrcGrp, null, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD");

            Item_ADD(itemRepo, "GROUP", "9010", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD Standard Chimney");
            Item_ADD(itemRepo, "GROUP", "9020", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD T-Shape");
            Item_ADD(itemRepo, "GROUP", "9030", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD Pull Out");
            Item_ADD(itemRepo, "GROUP", "9040", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD Bi-Group High");
            Item_ADD(itemRepo, "GROUP", "9050", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD Glass decorative");
            Item_ADD(itemRepo, "GROUP", "9060", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD Bi-Group Low");
            Item_ADD(itemRepo, "GROUP", "9070", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP HOOD Traditional");

            Item_ADD(itemRepo, "GROUP", "0010", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP ZASYP");
            Item_ADD(itemRepo, "GROUP", "0020", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP KARTONY");
            Item_ADD(itemRepo, "GROUP", "0030", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP ELEKTRYKA");
            Item_ADD(itemRepo, "GROUP", "0040", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP BUFOR");
            Item_ADD(itemRepo, "GROUP", "0050", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP PLASTIKI");
            Item_ADD(itemRepo, "GROUP", "0060", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP INSTRUKCJE");
            Item_ADD(itemRepo, "GROUP", "0070", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP LINE OF BLOWERS");
            Item_ADD(itemRepo, "GROUP", "0080", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP CENTRAL SITO TUBY");
            Item_ADD(itemRepo, "GROUP", "0090", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP SILNIKI");
            Item_ADD(itemRepo, "GROUP", "0100", resrcGrp, itmGrp, ItemTypeEnum.ItemGroup, hoodAssemblyProcessId, "GROUP ?");
            
            //ItemOP_ADD(itemRepo, "900222101", "900000", resrcGrp, itmGrp, ItemTypeEnum.Product, "Ford Focus Hatchback");

        }
        public static void Seed_PLB_Core(IDbContextCore db)
        {
            //ResourceRepo resourceRepo = new ResourceRepo(db);
            //ProductionOrderRepo poRepo = new ProductionOrderRepo(db);
            //BomRepo bomRepo = new BomRepo(db);
            //ItemRepo itemRepo = new ItemRepo(db);
            PrinterRepo printerRepo = new PrinterRepo(db);
            Printer printer = new Printer() { Name = "Brother DCP-1610W series", PrinterType = PrinterType.Laser, Model = "DCP-1610W", IpAdress = "0.0.0.0" };
            printerRepo.AddOrUpdate(printer);

            //Resource2 assLine = resourceRepo.GetList().Where(x => x.Name == "Ass.Line-01").FirstOrDefault();

            //WO_ADD(poRepo, "2020000011", 5, assLine.Id, sedan.Id);
            //WO_ADD(poRepo, "2020000012", 20, assLine.Id, sedan.Id);

            SystemVariableRepo systemVariableRepo = new SystemVariableRepo(db);

            systemVariableRepo.Add(new SystemVariable() { Type = EnumVariableType.String, Value = "CCCCCCCCCCCCQQQQQQDDDDDLLLLLLLLLSSSSSSSSSSSS", Name = "BarcodeTemplate_WH" });
            systemVariableRepo.Add(new SystemVariable() { Type = EnumVariableType.Int, Value = "0", Name = "SerialNumber_StockUnit" });

            //List<Resource2> resources = resourceRepo.GetList().Where(x => x.Type == ResourceTypeEnum.Resource).ToList();
            //foreach (Resource2 resource in resources)
            //{
            //    systemVariableRepo.Add(new SystemVariable() { Type = EnumVariableType.Int, Value = "0", Name = "SerialNumber_resourceId_" + resource.Id });
            //}

            
        }
        public static void Seed_PLB_iLOGIS(IDbContextiLOGIS db)
        {
            //Add ItemOP basing on Items from MasterData and PREFIX parameter
            ItemRepo itemRepo = new ItemRepo(db);
            ItemWMSRepo itemWMSRepo = new ItemWMSRepo(db);
            PackageRepo packageRepo = new PackageRepo(db);
            PackageItemRepo packageItemRepo = new PackageItemRepo(db);
            StockUnitRepo stockUnitRepo = new StockUnitRepo(db);
            WarehouseRepo whRepo = new WarehouseRepo(db);
            WarehouseLocationRepo whlRepo = new WarehouseLocationRepo(db);
            WarehouseLocationTypeRepo warehouseLocationTypeRepo = new WarehouseLocationTypeRepo(db);
            RepoWorkstation repoWorkstation = new RepoWorkstation(db);
            WorkstationItemRepo wiRepo = new WorkstationItemRepo(db);

            //INSERT INTO[EPSS_2_fixtures].[iLOGIS].[CONFIG_Item] ([Id],[Weight],[PickerNo],[TrainNo],[H],[ABC],[XYZ]) SELECT ID, 0, 100, 100, 0, 0 ,0 FROM[_MPPL].[MASTERDATA_Item]
            //itemWMSRepo.ExecuteSqlCommand("INSERT INTO[EPSS_2_fixtures].[iLOGIS].[CONFIG_Item] ([Id],[Weight],[PickerNo],[TrainNo],[H],[ABC],[XYZ]) SELECT ID, 0, 100, 100, 0, 0 ,0 FROM[_MPPL].[MASTERDATA_Item]");

            List<Item> items = itemRepo.GetList().ToList();
            foreach (Item itm in items)
            {
                itemWMSRepo.AddOrUpdate(new ItemWMS() { ItemId = itm.Id, PickerNo = 100, TrainNo = 100, ABC = ClassificationABCEnum.Undefined, XYZ = ClassificationXYZEnum.Undefined, Weight = 1, H = 0 });
            }

            int whlocTypeSTD = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Shelf, "STD");
            int whlocType40cm = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Shelf, "STD_40cm");
            int whlocType180cm = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Shelf, "STD_180cm");
            int whlocTypeTro = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Trolley, "Picking Platform");
            int whlocTypeFrk = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.FlowRack, "FlowRack");
            int whlocTypeFee = WarehouseLocationType_ADD(warehouseLocationTypeRepo, WarehouseLocationTypeEnum.Feeder, "Feeder");

            Package_ADD(packageRepo, "0001", "Cartoon Package Unknown", 25, 40, 60);
            Package_ADD(packageRepo, "0001", "No Package", 0, 0, 0);

            int extWarehouseId = Warehouse_ADD(whRepo, "0000", null, null, WarehouseTypeEnum.ExternalWarehouse, "External");
            WarehouseLocation_ADD(whlRepo, "External", "0", 0, whlocTypeSTD, extWarehouseId);

            int warehouseId;
            warehouseId = Warehouse_ADD(whRepo, "9103", null, null, WarehouseTypeEnum.AccountingWarehouse, "magazyn komponetów zakupowych");
            Warehouse_ADD(whRepo, "9106", null, null, WarehouseTypeEnum.AccountingWarehouse, "podukcja");
            Warehouse_ADD(whRepo, "9156", null, null, WarehouseTypeEnum.AccountingWarehouse, "magazyn blowerów");
            Warehouse_ADD(whRepo, "9102", null, null, WarehouseTypeEnum.AccountingWarehouse, "MWG");
            Warehouse_ADD(whRepo, "9173", null, null, WarehouseTypeEnum.AccountingWarehouse, "magazyn technologii -WZ od dostawców");
            Warehouse_ADD(whRepo, "9176", null, null, WarehouseTypeEnum.AccountingWarehouse, "Mech.production(technologia)");
            Warehouse_ADD(whRepo, "9140", null, null, WarehouseTypeEnum.AccountingWarehouse, "bufor komponentów z technologii");
            Warehouse_ADD(whRepo, "9112", null, null, WarehouseTypeEnum.AccountingWarehouse, "blokada części niezgodnych(Quality issue)");
            Warehouse_ADD(whRepo, "9108", null, null, WarehouseTypeEnum.AccountingWarehouse, "reklamacja od klienta(supplier returns)");
            Warehouse_ADD(whRepo, "9141", null, null, WarehouseTypeEnum.AccountingWarehouse, "magazyn blokad części zaginionych");
            Warehouse_ADD(whRepo, "9131", null, null, WarehouseTypeEnum.AccountingWarehouse, "technologia - polerka(Painted repair)");
            Warehouse_ADD(whRepo, "9104", null, null, WarehouseTypeEnum.AccountingWarehouse, "złom(scrap)");
            Warehouse_ADD(whRepo, "9107", null, null, WarehouseTypeEnum.AccountingWarehouse, "customer returns");
            Warehouse_ADD(whRepo, "9115", null, null, WarehouseTypeEnum.AccountingWarehouse, "PLWB Qual.issue");
            Warehouse_ADD(whRepo, "9120", null, null, WarehouseTypeEnum.AccountingWarehouse, "repair");
            Warehouse_ADD(whRepo, "9122", null, null, WarehouseTypeEnum.AccountingWarehouse, "FGW Bufor");
            Warehouse_ADD(whRepo, "9123", null, null, WarehouseTypeEnum.AccountingWarehouse, "PLWB Bufor");
            Warehouse_ADD(whRepo, "9130", null, null, WarehouseTypeEnum.AccountingWarehouse, "Deposit");
            Warehouse_ADD(whRepo, "9152", null, null, WarehouseTypeEnum.AccountingWarehouse, "Mot / Blow f.production");
            Warehouse_ADD(whRepo, "9153", null, null, WarehouseTypeEnum.AccountingWarehouse, "Metal Warehouse");
            Warehouse_ADD(whRepo, "9188", null, null, WarehouseTypeEnum.AccountingWarehouse, "DVT");
            Warehouse_ADD(whRepo, "9190", null, null, WarehouseTypeEnum.AccountingWarehouse, "Obsolete");
            Warehouse_ADD(whRepo, "9191", null, null, WarehouseTypeEnum.AccountingWarehouse, "Showroom");
            Warehouse_ADD(whRepo, "9192", null, null, WarehouseTypeEnum.AccountingWarehouse, "Employee sales");
            Warehouse_ADD(whRepo, "9199", null, null, WarehouseTypeEnum.AccountingWarehouse, "Ext.Wh.Lila");

            WarehouseLocation_ADD(whlRepo, "Strafe Przyjęć", "0", 0, whlocTypeSTD, warehouseId);

            int warehouseId_R00_ST = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:00,01 (Styropian)");
            int warehouseId_R00_CS = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:00,01 (Części Silnik)");
            int warehouseId_R00__K = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:00,01 (Kartony)");
            int warehouseId_R02___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:02,03 (Instrukcje, Bazetki)");
            int warehouseId_R04___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:04,05 (Silniki, Plastik)");
            int warehouseId_R06___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:06,07 (Filtry)");
            int warehouseId_R08_SP = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:08,09 (Zasypy:śruby, plastiki)");
            int warehouseId_R08__Z = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:08,09 (Zasypy:żarówki, plastiki, nadwyż)");
            int warehouseId_R09__N = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:09 (Nagrzewnica)");
            int warehouseId_R10___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:10 (Plastiki)");
            int warehouseId_R11___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:11 (Plafoniery, Plastiki)");
            int warehouseId_R12___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:12 (Redukcje, Panele, Plastiki)");
            int warehouseId_R13___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:13 (Worki, kołki, fronty, mix)");
            int warehouseId_R15___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:14,15 (szyby, kable)");
            int warehouseId_R16___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:16,17,18,19 (Elektryka)");
            int warehouseId_R20___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:20 (Elektryka, silniki - szyby, filtry, mix)");
            int warehouseId_R99___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Reg:99 (Silniki)");
            int warehouseId_K01___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Kanały:80 (Reca i Improdex)");
            int warehouseId_K02___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Kanały:60-65 (Miniblowery)");
            int warehouseId_K03___ = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Kanały:82 (Silniki)");
            int warehouseId_K30__K = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Kanały:30,31 (Kartony)");
            int warehouseId_K30__S = Warehouse_ADD(whRepo, "9103-S", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Kanały:30,31 (Styropian)");

            int warehouseId_K01_IN = Warehouse_ADD(whRepo, "9100", warehouseId, warehouseId, WarehouseTypeEnum.SubWarehouse, "Strefa Przyjęć");
            WarehouseLocation_ADD(whlRepo, "BRAK", "0", 0, whlocTypeSTD, warehouseId);
            WarehouseLocation_ADD(whlRepo, "BRAK", "0", 0, whlocTypeSTD, warehouseId_K01_IN);

            WarehouseLocation_ADD(whlRepo, 0, whlocTypeSTD, warehouseId_R00_CS, 1, 11, new int[] { 1, 2 });
            WarehouseLocation_ADD(whlRepo, 0, whlocTypeSTD, warehouseId_R00__K, 1, 11, new int[] { 3 });
            WarehouseLocation_ADD(whlRepo, 0, whlocTypeSTD, warehouseId_R00_ST, 12, 26, new int[] { 1, 2 });
            WarehouseLocation_ADD(whlRepo, 1, whlocTypeSTD, warehouseId_R00_CS, 1, 11, new int[] { 1, 2 });
            WarehouseLocation_ADD(whlRepo, 1, whlocTypeSTD, warehouseId_R00__K, 1, 11, new int[] { 3 });
            WarehouseLocation_ADD(whlRepo, 1, whlocTypeSTD, warehouseId_R00_ST, 12, 26, new int[] { 1, 2 });
            WarehouseLocation_ADD(whlRepo, 2, whlocTypeSTD, warehouseId_R02___, 1, 20, new int[] { 1, 3, 4, 5, 6 });
            WarehouseLocation_ADD(whlRepo, 2, whlocType40cm, warehouseId_R02___, 1, 20, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 3, whlocTypeSTD, warehouseId_R02___, 1, 20, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 3, whlocType40cm, warehouseId_R02___, 1, 20, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 4, whlocTypeSTD, warehouseId_R04___, 1, 23, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 4, whlocType40cm, warehouseId_R04___, 1, 23, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 5, whlocTypeSTD, warehouseId_R04___, 1, 24, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 5, whlocType40cm, warehouseId_R04___, 1, 24, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 6, whlocTypeSTD, warehouseId_R06___, 1, 24, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 6, whlocType40cm, warehouseId_R06___, 1, 24, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 7, whlocTypeSTD, warehouseId_R06___, 1, 24, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 7, whlocType40cm, warehouseId_R06___, 1, 24, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 8, whlocTypeSTD, warehouseId_R08_SP, 1, 24, new int[] { 1, 3 });
            WarehouseLocation_ADD(whlRepo, 8, whlocType40cm, warehouseId_R08_SP, 1, 24, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 8, whlocTypeSTD, warehouseId_R08__Z, 1, 24, new int[] { 4, 5 });
            WarehouseLocation_ADD(whlRepo, 9, whlocTypeSTD, warehouseId_R08_SP, 1, 54, new int[] { 1, 3, });
            WarehouseLocation_ADD(whlRepo, 9, whlocType40cm, warehouseId_R08_SP, 1, 54, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 9, whlocTypeSTD, warehouseId_R08__Z, 1, 54, new int[] { 4, 5 });
            WarehouseLocation_ADD(whlRepo, 10, whlocTypeSTD, warehouseId_R10___, 1, 16, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 10, whlocType40cm, warehouseId_R10___, 1, 16, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 11, whlocTypeSTD, warehouseId_R11___, 1, 16, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 11, whlocType40cm, warehouseId_R11___, 1, 16, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 12, whlocTypeSTD, warehouseId_R12___, 1, 16, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 12, whlocType40cm, warehouseId_R12___, 1, 16, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 13, whlocTypeSTD, warehouseId_R13___, 1, 16, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 13, whlocType40cm, warehouseId_R13___, 1, 16, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 14, whlocTypeSTD, warehouseId_R15___, 1, 12, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 14, whlocType40cm, warehouseId_R15___, 1, 12, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 14, whlocType180cm, warehouseId_R15___, 13, 21, new int[] { 1, 2 });
            WarehouseLocation_ADD(whlRepo, 15, whlocTypeSTD, warehouseId_R15___, 1, 12, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 15, whlocType40cm, warehouseId_R15___, 1, 12, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 15, whlocType180cm, warehouseId_R15___, 13, 21, new int[] { 1, 2 });
            WarehouseLocation_ADD(whlRepo, 16, whlocTypeSTD, warehouseId_R16___, 1, 12, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 16, whlocType40cm, warehouseId_R16___, 1, 12, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 16, whlocType180cm, warehouseId_R16___, 13, 21, new int[] { 1, 2 });
            WarehouseLocation_ADD(whlRepo, 17, whlocTypeSTD, warehouseId_R16___, 1, 12, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 17, whlocType40cm, warehouseId_R16___, 1, 12, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 17, whlocType180cm, warehouseId_R16___, 13, 15, new int[] { 1, 2 });
            WarehouseLocation_ADD(whlRepo, 18, whlocTypeSTD, warehouseId_R16___, 1, 16, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 18, whlocType40cm, warehouseId_R16___, 1, 16, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 19, whlocTypeSTD, warehouseId_R16___, 1, 16, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 19, whlocType40cm, warehouseId_R16___, 1, 16, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 20, whlocTypeSTD, warehouseId_R20___, 1, 16, new int[] { 1, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 20, whlocType40cm, warehouseId_R20___, 1, 16, new int[] { 2 });
            WarehouseLocation_ADD(whlRepo, 99, whlocType40cm, warehouseId_R99___, 1, 8, new int[] { 1, 2, 3, 4, 5 });

            WarehouseLocation_ADD(whlRepo, 80, whlocTypeSTD, warehouseId_K01___, 1, 16, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 60, whlocTypeSTD, warehouseId_K02___, 1, 1, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 61, whlocTypeSTD, warehouseId_K02___, 1, 1, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 62, whlocTypeSTD, warehouseId_K02___, 1, 1, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 63, whlocTypeSTD, warehouseId_K02___, 1, 1, new int[] { 1, 2, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 64, whlocTypeSTD, warehouseId_K02___, 1, 1, new int[] { 1, 2, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 65, whlocTypeSTD, warehouseId_K02___, 1, 1, new int[] { 1, 2, 3, 4, 5 });
            WarehouseLocation_ADD(whlRepo, 82, whlocTypeSTD, warehouseId_K03___, 1, 9, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 83, whlocTypeSTD, warehouseId_K03___, 1, 12, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 30, whlocTypeSTD, warehouseId_K30__K, 1, 12, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 30, whlocTypeSTD, warehouseId_K30__S, 13, 19, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 31, whlocTypeSTD, warehouseId_K30__K, 1, 15, new int[] { 1 });
            WarehouseLocation_ADD(whlRepo, 31, whlocTypeSTD, warehouseId_K30__S, 16, 21, new int[] { 1 });

            TransporterRepo transporterRepo = new TransporterRepo(db);
            transporterRepo.Add(new Transporter() { Name = "Picker 1", Code = "100", DedicatedResources = "Assembly Line", Type = EnumTransporterType.Picker, LoopQty = 5 });
            transporterRepo.Add(new Transporter() { Name = "Train 1", Code = "100", DedicatedResources = "Assembly Line", Type = EnumTransporterType.Train, LoopQty = 5 });

            List<Param1> itemGroups = new List<Param1>();
            itemGroups.Add(new Param1() { Key = "0010", Name = "ZASYP------------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R08_SP, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0010", Name = "ZASYP------------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R08__Z, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0020", Name = "KARTONY----------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R00__K, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 100, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0020", Name = "KARTONY----------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_K30__K, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 100, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0030", Name = "ELEKTRYKA--------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R16___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0030", Name = "ELEKTRYKA--------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R20___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R04___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R08_SP, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R08__Z, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R10___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R11___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R12___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0060", Name = "INSTRUKCJE-------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R02___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0070", Name = "LINE OF BLOWERS--", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_K02___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 40, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "0090", Name = "SILNIKI----------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_R99___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 50, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "0090", Name = "SILNIKI----------", Type = ItemTypeEnum.ItemGroup, WhId = warehouseId_K03___, WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 50, PickStrat = PickingStrategyEnum.UpToOrderQty });
            //itemGroups.Add(new Param1() { Key = "0080", Name = "CENTRAL SITO TUBY", Type = ItemTypeEnum.BuyedItem, WhId = 1  , WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 12, PickStrat = PickingStrategyEnum.UpToOrderQty });
            //itemGroups.Add(new Param1() { Key = "0040", Name = "BUFOR------------", Type = ItemTypeEnum.BuyedItem, WhId = 1  , WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            //itemGroups.Add(new Param1() { Key = "0100", Name = "?----------------", Type = ItemTypeEnum.BuyedItem, WhId = 1  , WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });

            foreach (var itmgroup in itemGroups)
            {
                List<int> itemIds = itemRepo.GetList().Where(x => x.PREFIX == itmgroup.Key && x.Type == itmgroup.Type).Select(x => x.Id).ToList();
                Package package = packageRepo.GetList().Where(x => x.Name == itmgroup.PckgName).FirstOrDefault();
                WarehouseLocationType whLocType = warehouseLocationTypeRepo.GetList().Where(x => x.Name == itmgroup.WhLocType).FirstOrDefault();
                //Warehouse wh = whRepo.GetList().Where(x => x.Name == itmgroup.WhName).FirstOrDefault();

                if (package != null && whLocType != null) //&& wh != null)
                {
                    foreach (int itemId in itemIds)
                    {
                        PackageItem_ADD(packageItemRepo, itemId, package.Id, itmgroup.QtyPerPckg, whLocType.Id, itmgroup.PckgPerPallet, itmgroup.PickStrat, itmgroup.WhId);
                    }
                }
            }



            //List<Param1> itemW = new List<Param1>();
            //itemW.Add(new Param1() { Key = "600100", Name = "GROUP BODY------", Type = ItemTypeEnum.ItemGroup, WorkstationName = "ASS1.01 IN" });
            //itemW.Add(new Param1() { Key = "600200", Name = "GROUP REARDOOR--", Type = ItemTypeEnum.ItemGroup, WorkstationName = "BWL1.01 IN" });
            //itemW.Add(new Param1() { Key = "300100", Name = "Engines-", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.20 Engine" });
            //itemW.Add(new Param1() { Key = "400100", Name = "Steer.Wh", Type = ItemTypeEnum.BuyedItem, WorkstationName = "ASS1.10 Interior" });


            //foreach (var itmgroup in itemW)
            //{
            //    List<int> itemIds = itemRepo.GetList().Where(x => x.PREFIX == itmgroup.Key && x.Type == itmgroup.Type).Select(x => x.Id).ToList();
            //    Workstation workstation = repoWorkstation.GetList().Where(x => x.Name == itmgroup.WorkstationName).FirstOrDefault();

            //    if (workstation != null)
            //    {
            //        foreach (int itemId in itemIds)
            //        {
            //            WorkstationItem_ADD(wiRepo, itemId, workstation.Id);
            //        }
            //    }
            //}

            //List<string> itemCodes = itemRepo.GetList().Where(x => x.Type == ItemTypeEnum.BuyedItem).Select(x => x.Code).ToList();
            //foreach (string itemcode in itemCodes)
            //{
            //    Stock_ADD(stockUnitRepo, itemRepo, itemcode, 1000);
            //}
        }

        public static void Seed_PLB_iLOGIS_PackageItem(IDbContextiLOGIS db)
        {
            //Add ItemOP basing on Items from MasterData and PREFIX parameter
            ItemRepo itemRepo = new ItemRepo(db);
            ItemWMSRepo itemWMSRepo = new ItemWMSRepo(db);
            PackageRepo packageRepo = new PackageRepo(db);
            PackageItemRepo packageItemRepo = new PackageItemRepo(db);
            StockUnitRepo stockUnitRepo = new StockUnitRepo(db);
            WarehouseRepo whRepo = new WarehouseRepo(db);
            WarehouseLocationRepo whlRepo = new WarehouseLocationRepo(db);
            WarehouseLocationTypeRepo warehouseLocationTypeRepo = new WarehouseLocationTypeRepo(db);
            RepoWorkstation repoWorkstation = new RepoWorkstation(db);
            WorkstationItemRepo wiRepo = new WorkstationItemRepo(db);
         
            List<Param1> itemGroups = new List<Param1>();
            itemGroups.Add(new Param1() { Key = "0010", Name = "ZASYP------------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg:08,09 (Zasypy:śruby, plastiki)"                , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0010", Name = "ZASYP------------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg:08,09 (Zasypy:żarówki, plastiki, nadwyż)"      , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0020", Name = "KARTONY----------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg:00,01 (Kartony)"                               , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 2, QtyPerPckg = 100, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0020", Name = "KARTONY----------", Type = ItemTypeEnum.ItemGroup, WhName = "Kanały:30,31 (Kartony)"                            , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 2, QtyPerPckg = 100, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0030", Name = "ELEKTRYKA--------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg:16,17,18,19 (Elektryka)"                       , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 1, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0030", Name = "ELEKTRYKA--------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg:20 (Elektryka, silniki - szyby, filtry, mix)"  , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 1, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg:04,05 (Silniki, Plastik)"                      , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 1, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg: 08,09(Zasypy: śruby, plastiki)"               , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg: 08,09(Zasypy: żarówki, plastiki, nadwyż)"     , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg: 10(Plastiki)"                                 , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg: 11(Plafoniery, Plastiki)"                     , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0050", Name = "PLASTIKI---------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg: 12(Redukcje, Panele, Plastiki)"               , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 80, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0060", Name = "INSTRUKCJE-------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg:02,03 (Instrukcje, Bazetki)"                   , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            itemGroups.Add(new Param1() { Key = "0070", Name = "LINE OF BLOWERS--", Type = ItemTypeEnum.ItemGroup, WhName = "Kanały:60-65 (Miniblowery)"                        , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 8, QtyPerPckg = 40, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "0090", Name = "SILNIKI----------", Type = ItemTypeEnum.ItemGroup, WhName = "Reg:99 (Silniki)"                                  , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 1, QtyPerPckg = 50, PickStrat = PickingStrategyEnum.UpToOrderQty });
            itemGroups.Add(new Param1() { Key = "0090", Name = "SILNIKI----------", Type = ItemTypeEnum.ItemGroup, WhName = "Kanały:82 (Silniki)"                               , WhLocType = "STD", PckgName = "Cartoon Package Unknown", PckgPerPallet = 1, QtyPerPckg = 50, PickStrat = PickingStrategyEnum.UpToOrderQty });
            //itemGroups.Add(new Param1() { Key = "0080", Name = "CENTRAL SITO TUBY", Type = ItemTypeEnum.BuyedItem, WhId = 1  , WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 12, PickStrat = PickingStrategyEnum.UpToOrderQty });
            //itemGroups.Add(new Param1() { Key = "0040", Name = "BUFOR------------", Type = ItemTypeEnum.BuyedItem, WhId = 1  , WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });
            //itemGroups.Add(new Param1() { Key = "0100", Name = "?----------------", Type = ItemTypeEnum.BuyedItem, WhId = 1  , WhLocType = "STD", PckgName = "Cartoon Package", PckgPerPallet = 8, QtyPerPckg = 20, PickStrat = PickingStrategyEnum.FullPackage });

            foreach (var itmgroup in itemGroups)
            {
                List<int> itemWMSIds = itemWMSRepo.GetList().Where(x => x.Item.PREFIX == itmgroup.Key && x.Item.Type == itmgroup.Type).Select(x => x.Id).ToList();
                Package package = packageRepo.GetList().Where(x => x.Name == itmgroup.PckgName).FirstOrDefault();
                WarehouseLocationType whLocType = warehouseLocationTypeRepo.GetList().Where(x => x.Name == itmgroup.WhLocType).FirstOrDefault();
                Warehouse wh = whRepo.GetList().Where(x => x.Name == itmgroup.WhName).FirstOrDefault();

                if (package != null && whLocType != null && wh != null)
                {
                    foreach (int itemWMSId in itemWMSIds)
                    {
                        PackageItem_ADD(packageItemRepo, itemWMSId, package.Id, itmgroup.QtyPerPckg, whLocType.Id, itmgroup.PckgPerPallet, itmgroup.PickStrat, wh.Id);
                    }
                }
            }
        }

        //---------------------------------------------------------------------------------------------
        public static int WO_ADD(ProductionOrderRepo repo, string number, int qty, int lineId, int productItemId)
        {
            DateTime now = DateTime.Now;

            return repo.Add(new ProductionOrder() { OrderNumber = number, QtyPlanned = qty, QtyRemain = qty, LineId = lineId, StartDate = now, EndDate = now.AddMinutes(5 * qty), LastUpdate = now, PncId = productItemId });
        }
        public static int Item_ADD(ItemRepo repo, string code, string prefix, int? resourceGroup, int? itemGroup, ItemTypeEnum itemType, int? processId = null, string name = "")
        {
            return repo.Add(new Item() { Code = code, PREFIX = prefix, ResourceGroupId = resourceGroup, ItemGroupId = itemGroup, Type = itemType, Name = name, ProcessId = processId });
        }
        public static int ItemOP_ADD(ItemOPRepo repo, string code, string prefix, int? resourceGroup, int? itemGroup, ItemTypeEnum itemType, string name, int? processId = null)
        {
            return repo.Add(new ItemOP() { Code = code, PREFIX = prefix, ResourceGroupId = resourceGroup, ItemGroupId = itemGroup, Type = itemType, Name = name, ProcessId = processId });
        }
        public static int Process_ADD(ProcessRepo repo, string name, int parentId = 0)
        {
            return repo.Add(new Process() { Name = name, ParentId = parentId });
        }
        public static int Workstation_ADD(RepoWorkstation repo, int? lineId, int sortOrder, string name)
        {
            return repo.Add(new Workstation() { LineId = lineId, SortOrder = sortOrder, SortOrderTrain = sortOrder, ProductsFromIn = 0, ProductsFromOut = 0, Name = name });
        }
        public static int Resource_ADD(ResourceRepo repo, int areaId, ResourceTypeEnum type, int flowTime, int? resourceGroupId, string name)
        {
            return repo.Add(new Resource2() { AreaId = areaId, FlowTime = flowTime, Type = type, ResourceGroupId = resourceGroupId, Name = name });
        }
        public static int ResourceOP_ADD(ResourceOPRepo repo, int areaId, ResourceTypeEnum type, int flowTime, int? resourceGroupId, string name)
        {
            return repo.Add(new ResourceOP() { AreaId = areaId, FlowTime = flowTime, Type = type, ResourceGroupId = resourceGroupId, Name = name });
        }
        public static int Warehouse_ADD(WarehouseRepo repo, string code, int? accountingWarehouseId, int? parentWarehouseId, WarehouseTypeEnum type, string name)
        {
            return repo.Add(new Warehouse() { Code = code,  Name = name, ParentWarehouseId = parentWarehouseId, AccountingWarehouseId = accountingWarehouseId, WarehouseType = type });
        }
        public static int WarehouseLocation_ADD(WarehouseLocationRepo repo, string name, string regalNumber, int columnNumber, int typeId, int warehouseId)
        {
            return repo.Add(new WarehouseLocation() { Name = name, RegalNumber = regalNumber, ColumnNumber = columnNumber, TypeId = typeId, WarehouseId = warehouseId, UpdateDate = DateTime.Now });
        }
        public static void WarehouseLocation_ADD(WarehouseLocationRepo repo, int regalNumber, int typeId, int warehouseId, int startColumn, int endColumn, int levels)
        {
            string name = string.Empty;

            for(int col= startColumn; col <= endColumn; col++)
            {
                for (int l = 10; l <= levels*10; l+=10)
                {
                    name = regalNumber.ToString("D2") + col.ToString("D2") + l.ToString("D2");
                    repo.Add(new WarehouseLocation() { 
                        Name = name, 
                        RegalNumber = regalNumber.ToString(), 
                        ColumnNumber = col, 
                        TypeId = typeId, 
                        WarehouseId = warehouseId, 
                        UpdateDate = DateTime.Now,
                        ShelfNumber = l
                    });
                }
            }
        }
        public static void WarehouseLocation_ADD(WarehouseLocationRepo repo, int regalNumber, int typeId, int warehouseId, int startColumn, int endColumn, int startLevel, int endLevel)
        {
            string name = string.Empty;

            for (int col = startColumn; col <= endColumn; col++)
            {
                for (int l = startLevel*10; l <= endLevel*10; l += 10)
                {
                    name = regalNumber.ToString("D2") + col.ToString("D2") + l.ToString("D2");
                    repo.Add(new WarehouseLocation() { 
                        Name = name, 
                        RegalNumber = regalNumber.ToString(), 
                        ColumnNumber = col, 
                        TypeId = typeId, 
                        WarehouseId = warehouseId, 
                        UpdateDate = DateTime.Now,
                        ShelfNumber = l
                    });
                }
            }
        }
        public static void WarehouseLocation_ADD(WarehouseLocationRepo repo, int regalNumber, int typeId, int warehouseId, int startColumn, int endColumn, int[] levels)
        {
            string name = string.Empty;

            for (int col = startColumn; col <= endColumn; col++)
            {
                foreach(int l in levels)
                {
                    name = regalNumber.ToString("D2") + col.ToString("D2") + (l*10).ToString("D2");
                    repo.Add(new WarehouseLocation()
                    {
                        Name = name,
                        RegalNumber = regalNumber.ToString(),
                        ColumnNumber = col,
                        TypeId = typeId,
                        WarehouseId = warehouseId,
                        UpdateDate = DateTime.Now,
                        ShelfNumber = l * 10
                    });
                }
            }
        }
        public static int WarehouseLocationType_ADD(WarehouseLocationTypeRepo repo, WarehouseLocationTypeEnum type, string name)
        {
            return repo.Add(new WarehouseLocationType() { Name = name, Width = 80, Depth = 120, Height = 100, TypeEnum = type });
        }
        public static int Package_ADD(PackageRepo repo, string code, string name, int height, int width, int depth)
        {
            return repo.Add(new Package() { Name = name, Code = "", PackagesPerPallet = 0, Type = EnumPackageType.Undefined, UnitOfMeasure = UnitOfMeasure.szt, Height = height, Width = width, Depth = depth });
        }
        public static int PackageItem_ADD(PackageItemRepo repo, int itemWMSId, int packageId, int QtyPerPackage, int whLocTypeId, int packagesPerPallet, PickingStrategyEnum pickingStrategy, int warehouseId)
        {
            return repo.Add(new PackageItem()
            {
                ItemWMSId = itemWMSId,
                PackageId = packageId,
                QtyPerPackage = QtyPerPackage,
                WarehouseLocationTypeId = whLocTypeId,
                PackagesPerPallet = packagesPerPallet,
                PickingStrategy = pickingStrategy,
                WarehouseId = warehouseId,
            });
        }
        public static int WorkstationItem_ADD(WorkstationItemRepo repo, int itemId, int workstationId)
        {
            return repo.Add(new WorkstationItem()
            {
                ItemWMSId = itemId,
                WorkstationId = workstationId,
                MaxPackages = 8,
                SafetyStock = 0,
                CheckOnly = false
            });
        }
        public static int Stock_ADD(StockUnitRepo repo, ItemRepo itemRepo, string itemCode, decimal qty)
        {
            Item item = itemRepo.GetByCode(itemCode);

            if (item == null)
            {
                return 0;
            }
            else
            {
                return repo.Add(new StockUnit()
                {
                    ItemWMSId = item.Id,
                    CreatedDate = DateTime.Now,
                    BestBeforeDate = DateTime.Now.AddDays(60),
                    CurrentQtyinPackage = qty,
                    IsLocated = false,
                    MaxQtyPerPackage = (int)qty,
                    SerialNumber = null,
                    Status = StatusEnum.Available,
                    WMSLastCheck = DateTime.Now,
                });
            }
        }
        public static int Bom_ADD(BomRepo repo, ItemRepo itemRepo, string parentCode, string childCode, decimal qtyUsed, int lv)
        {
            Item parent = itemRepo.GetByCode(parentCode);
            Item child = itemRepo.GetByCode(childCode);

            if (parent != null && child != null)
            {
                Bom bomEntry = new Bom();
                bomEntry.AncId = child.Id;
                bomEntry.PncId = parent.Id;
                bomEntry.Prefix = child.PREFIX;
                bomEntry.LV = lv;
                bomEntry.PCS = qtyUsed;
                bomEntry.StartDate = DateTime.Now;
                bomEntry.EndDate = DateTime.Now.AddDays(180);
                return repo.Add(bomEntry);
            }
            else
            {
                return 0;
            }
        }
        public static int OEEReasonType_ADD(ReasonTypeRepo repo, string name, string nameEng, int entryType, int sortOrder, string Color, bool deleted)
        {
            ReasonType rt = new ReasonType();
            rt.Name = name;
            rt.NameEnglish = nameEng;
            rt.EntryType = (MDL_ONEPROD.Model.Scheduling.Interface.EnumEntryType)entryType;
            rt.SortOrder = sortOrder;
            rt.Color = Color;
            rt.Deleted = deleted;
            return repo.AddOrUpdate(rt);
        }
    }
}

public class Param1
{
    public string Key { get; set; }
    public string Name { get; set; }
    public decimal CycleTime { get; set; }
    public int MinBatch { get; set; }
    public string Param4 { get; set; }
    public string Param5 { get; set; }
    public string WhLocType { get; set; }
    public ItemTypeEnum Type { get; set; }
    public PickingStrategyEnum PickStrat { get; set; }
    public int QtyPerPckg { get; set; }
    public int PckgPerPallet { get; set; }
    public int WhId { get; set; }
    public string WhName { get; set; }
    public string PckgName { get; set; }
    public string WorkstationName { get; set; }
}



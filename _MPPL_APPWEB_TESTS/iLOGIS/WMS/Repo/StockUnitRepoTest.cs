using _MPPL_WEB_START.Areas.iLOGIS.Controllers;
using _MPPL_WEB_START.Migrations;
using AutoFixture;
using Effort;
using EntityFrameworkMock;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_CORE.ComponentCore.Entities;
using MDL_CORE.ComponentCore.Enums;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_iLOGIS.ComponentWMS.Models;
using MDL_iLOGIS.ComponentWMS.Repos;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Model;

namespace _MPPL_APPWEB_TESTS.iLOGIS.WMS.Repo
{
    [TestClass]
    public class StockUnitRepoTest
    {
        //To bardziej jako testy integracyjne ponieważ są FAIL'e gdy nie jest się w sieci Electroluxa

        //[TestMethod]
        //public void TestGetLocations_API()
        //{
        //    string itemCode = "A00309111";
        //    var rawJson = StockUnitRepo_FSDS_PLV.GetLocationsOfItemFSDS_API(itemCode);
        //    Assert.IsNotNull(rawJson);
        //}
        //[TestMethod]
        //public void TestParsing_API()
        //{
        //    string itemCode = "A00309111";
        //    var rawJson = StockUnitRepo_FSDS_PLV.GetLocationsOfItemFSDS_API(itemCode);
        //    List<ApiLocation> locations = StockUnitRepo_FSDS_PLV.ParseFSDSItemLocationsData(rawJson);

        //    Assert.IsFalse(locations.Count <= 0);
        //}

        public Mock<DbContextAPP_Dev> PrepareContextData()
        {
            DbContextDataCreator dbCDC = new DbContextDataCreator();

            new GenericMocker<Warehouse>().BindObjects(dbCDC.Warehouses, dbCDC);
            new GenericMocker<WarehouseLocation>().BindObjects(dbCDC.WarehouseLocations, dbCDC);
            new GenericMocker<Item>().BindObjects(dbCDC.Items, dbCDC);
            new GenericMocker<ItemUoM>().BindObjects(dbCDC.ItemUoMs, dbCDC);
            new GenericMocker<ItemWMS>().BindObjects(dbCDC.ItemsWMS, dbCDC);
            new GenericMocker<Package>().BindObjects(dbCDC.Packages, dbCDC);
            new GenericMocker<PackageItem>().BindObjects(dbCDC.PackageItems, dbCDC);
            new GenericMocker<StockUnit>().BindObjects(dbCDC.StockUnits, dbCDC);
            //new GenericMocker<User>().BindObjects(dbContextDataCreator.Users, dbContextDataCreator);
            //new GenericMocker<IdentityRole>().BindObjects(dbContextDataCreator.Roles, dbContextDataCreator);
            //new GenericMocker<UserRole>().BindObjects(dbContextDataCreator.UserRoles, dbContextDataCreator);

            var itemMock = new GenericMocker<Item>().CreateDbSetMock(dbCDC.Items.AsQueryable());
            var itemUoMMock = new GenericMocker<ItemUoM>().CreateDbSetMock(dbCDC.ItemUoMs.AsQueryable());
            var itemWMSMock = new GenericMocker<ItemWMS>().CreateDbSetMock(dbCDC.ItemsWMS.AsQueryable());
            var warehouseMock = new GenericMocker<Warehouse>().CreateDbSetMock(dbCDC.Warehouses.AsQueryable());
            var warehouseLocationTypeMock = new GenericMocker<WarehouseLocationType>().CreateDbSetMock(dbCDC.WarehouseLocationTypes.AsQueryable());
            var warehouseLocationMock = new GenericMocker<WarehouseLocation>().CreateDbSetMock(dbCDC.WarehouseLocations.AsQueryable());
            var packageMock = new GenericMocker<Package>().CreateDbSetMock(dbCDC.Packages.AsQueryable());
            var packageItemMock = new GenericMocker<PackageItem>().CreateDbSetMock(dbCDC.PackageItems.AsQueryable());
            var stockUnitMock = new GenericMocker<StockUnit>().CreateDbSetMock(dbCDC.StockUnits.AsQueryable());
            var userMock = new GenericMocker<User>().CreateDbSetMock(dbCDC.Users.AsQueryable());
            var roleMock = new GenericMocker<ApplicationRole>().CreateDbSetMock(dbCDC.Roles.AsQueryable());
            var userRoleMock = new GenericMocker<UserRole>().CreateDbSetMock(dbCDC.UserRoles.AsQueryable());
            var userClaimMock = new GenericMocker<UserClaim>().CreateDbSetMock(dbCDC.UserClaims.AsQueryable());
            var userLoginMock = new GenericMocker<UserLogin>().CreateDbSetMock(dbCDC.UserLogins.AsQueryable());
            var systemVariablesMock = new GenericMocker<SystemVariable>().CreateDbSetMock(dbCDC.SystemVariables.AsQueryable());
            var printersMock = new GenericMocker<Printer>().CreateDbSetMock(dbCDC.Printers.AsQueryable());
            DatabaseMPPL databaseMPPL = new DatabaseMPPL();

            //warehouseMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(_obj.Provider);

            DbConnection connection = DbConnectionFactory.CreateTransient();
            //var ctx = new DbContextAPP_Dev(connection);

            DbContextMock<DbContextAPP_Dev> mock = new DbContextMock<DbContextAPP_Dev>(connection); //{ CallBase = true };
            //mock.Object.Database.Initialize(false);
            //mock.Object.TestModelCreation(new DbModelBuilder());


            //mock.CreateDbSetMock(x => x.Items, dbCDC.Items.AsQueryable());
            //mock.CreateDbSetMock(x => x.ItemWMS, dbCDC.ItemsWMS.AsQueryable());
            //mock.CreateDbSetMock(x => x.Warehouses, dbCDC.Warehouses.AsQueryable());
            //mock.CreateDbSetMock(x => x.WarehouseLocationTypes, dbCDC.WarehouseLocationTypes.AsQueryable());
            //mock.CreateDbSetMock(x => x.WarehouseLocations, dbCDC.WarehouseLocations.AsQueryable());
            //mock.CreateDbSetMock(x => x.Packages, dbCDC.Packages.AsQueryable());
            //mock.CreateDbSetMock(x => x.PackageItems, dbCDC.PackageItems.AsQueryable());
            //mock.CreateDbSetMock(x => x.StockUnits, dbCDC.StockUnits.AsQueryable());
            //mock.CreateDbSetMock(x => x.Users, dbCDC.Users.AsQueryable());
            //mock.CreateDbSetMock(x => x.Roles, dbCDC.Roles.AsQueryable());
            //mock.CreateDbSetMock(x => x.UserRoles, dbCDC.UserRoles.AsQueryable());
            //mock.CreateDbSetMock(x => x.UserClaims, dbCDC.UserClaims.AsQueryable());
            //mock.CreateDbSetMock(x => x.UserLogins, dbCDC.UserLogins.AsQueryable());
            //mock.CreateDbSetMock(x => x.SystemVariables, dbCDC.SystemVariables.AsQueryable());
            //mock.CreateDbSetMock(x => x.Printers, dbCDC.Printers.AsQueryable());

            try
            {
                mock.Setup(x => x.Items).Returns(itemMock.Object);
                mock.Setup(x => x.ItemUoMs).Returns(itemUoMMock.Object);
                mock.Setup(x => x.ItemWMS).Returns(itemWMSMock.Object);
                mock.Setup(x => x.Warehouses).Returns(warehouseMock.Object);
                mock.Setup(x => x.WarehouseLocationTypes).Returns(warehouseLocationTypeMock.Object);
                mock.Setup(x => x.WarehouseLocations).Returns(warehouseLocationMock.Object);
                mock.Setup(x => x.Packages).Returns(packageMock.Object);
                mock.Setup(x => x.PackageItems).Returns(packageItemMock.Object);
                mock.Setup(x => x.StockUnits).Returns(stockUnitMock.Object);
                mock.Setup(x => x.Users).Returns(userMock.Object);
                mock.Setup(x => x.UserRoles).Returns(userRoleMock.Object);
                mock.Setup(x => x.Roles).Returns(roleMock.Object);
                mock.Setup(x => x.UserLogins).Returns(userLoginMock.Object);
                mock.Setup(x => x.UserClaims).Returns(userClaimMock.Object);
                mock.Setup(x => x.SystemVariables).Returns(systemVariablesMock.Object);
                mock.Setup(x => x.Printers).Returns(printersMock.Object);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            //mockIDbContextiLOGIS.Setup(x => x.Database).Returns()

            return mock;
        }

        [TestMethod]
        public void TestMoq()
        {
            var id = 12;

            var sn = "DDASDSASD";

            StockUnit stockUnit = new StockUnit()
            {
                Id = id,
                CreatedDate = DateTime.Now,
                CurrentQtyinPackage = 10,
                IsLocated = true,
                SerialNumber = "DDASDSASD"
            };
            // Create the mock
            var mock = new Mock<IDbContextiLOGIS>();

            var controller = new StockUnitController(mock.Object);
            var actual = controller.Get(id);

            // Demonstrate that the configuration works
            Assert.AreSame(stockUnit, actual);
        }

        [TestMethod]
        public void TestMoq2()
        {
            // Arrange
            Fixture fixture = new Fixture();
            Mock<DbContextAPP_Dev> mockContext = PrepareContextData();

            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(x => x.Identity.Name).Returns("Admin");
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal.Object);
            var reqContext = new RequestContext(httpContext.Object, new RouteData());

            // Act
            var stockUnitController = new StockUnitController(mockContext.Object);
            stockUnitController.ControllerContext = new System.Web.Mvc.ControllerContext(reqContext, stockUnitController);
            JsonResult jsonResult = stockUnitController.CreateNew(itemCode: "SLM002", qty: 20, maxQtyPerPackage: 20, numberOfPackages: 1);
            StockUnitViewModel suvm = (StockUnitViewModel)jsonResult.Data.GetPropValue("data");

            // Assert
            Assert.AreEqual("100401", suvm.WarehouseLocationName);
        }

        [TestMethod]
        public void CreateWhTest()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            var ctx = new DbContextAPP_Dev(connection);
            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(ctx);
            uow.WarehouseRepo.Add(new Warehouse()
            {
                Code = "TEST",
                Name = "Magazyn",
                WarehouseType = WarehouseTypeEnum.AccountingWarehouse
            });

            var wh = uow.WarehouseRepo.GetById(1);

            Assert.AreEqual("TEST", wh.Code);
        }

        [TestMethod]
        public void CreateGroupTest()
        {
            // Arrange
            Fixture fixture = new Fixture();
            Mock<DbContextAPP_Dev> mockContext = PrepareContextData();

            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(x => x.Identity.Name).Returns("Admin");
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal.Object);
            var reqContext = new RequestContext(httpContext.Object, new RouteData());

            // Act
            var stockUnitController = new StockUnitController(mockContext.Object);
            stockUnitController.ControllerContext = new System.Web.Mvc.ControllerContext(reqContext, stockUnitController);
            JsonResult jsonResult = stockUnitController.GreateGroup(new int[] { 3, 6, 7, 8 });
            //StockUnitViewModel suvm = (StockUnitViewModel)jsonResult.Data.GetPropValue("data");

            StockUnit su = (StockUnit)stockUnitController.Get(6).Data;
            StockUnit stockUnitGroup = (StockUnit)stockUnitController.Get(su.GroupId??0).Data;
            List<StockUnit> stockUnitsOfGroup = new UnitOfWork_iLogis(mockContext.Object).StockUnitRepo.GetByGroupId(stockUnitGroup?.Id).ToList();

            // Assert
            Assert.IsTrue(su != null && su.GroupId != null && su.GroupId > 0);
            Assert.IsTrue(stockUnitGroup != null && stockUnitGroup.IsGroup);
            Assert.IsTrue(stockUnitsOfGroup != null && stockUnitsOfGroup.Count == 4);
        }
    }

    public class DbContextDataCreator
    {
        DateTime date = new DateTime(2020, 1, 1, 12, 0, 0);
        public List<Warehouse> Warehouses { get; set; }
        public List<WarehouseLocation> WarehouseLocations { get; set; }
        public List<WarehouseLocationType> WarehouseLocationTypes { get; set; }
        public List<Item> Items { get; set; }
        public List<ItemUoM> ItemUoMs { get; set; }
        public List<ItemWMS> ItemsWMS { get; set; }
        public List<Package> Packages { get; set; }
        public List<PackageItem> PackageItems { get; set; }
        public List<StockUnit> StockUnits { get; set; }
        public List<User> Users{ get; set; }
        public List<ApplicationRole> Roles { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<UserClaim> UserClaims { get; set; }
        public List<UserLogin> UserLogins { get; set; }
        public List<SystemVariable> SystemVariables { get; set; }
        public List<Printer> Printers { get; set; }


        public DbContextDataCreator()
        {
            _Warehouses();
            _WarehouseLocations();
            _WarehouseLocationTypes();
            _Items();
            _ItemUoMs();
            _ItemWMS();
            _Packages();
            _PackageItems();
            _StockUnits();
            _Identity();
            _SystemVariables();
            _Printers();
        }

        private void _Warehouses()
        {
            Warehouses = new List<Warehouse>
            {
                new Warehouse() { Id = 1, Name = "MAG", AccountingWarehouse = null  },
                new Warehouse() { Id = 2, Name = "MAG-OWOCE", AccountingWarehouse = null  },
                new Warehouse() { Id = 3, Name = "MAG-CHEMIA", AccountingWarehouse = null  },
                new Warehouse() { Id = 4, Name = "MAG-SŁODYCZE", AccountingWarehouse = null  },
                new Warehouse() { Id = 5, Name = "EXTERNAL", AccountingWarehouse = null  },
            };
        }
        private void _WarehouseLocations()
        {
            WarehouseLocations = new List<WarehouseLocation>
            {
                new WarehouseLocation() { Id = 1, Name = "100201", WarehouseId = 2, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 2, Name = "100202", WarehouseId = 2, Utilization =  0.25m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 3, Name = "100203", WarehouseId = 2, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 4, Name = "100204", WarehouseId = 2, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 5, Name = "100301", WarehouseId = 3, Utilization =  0.1m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 6, Name = "100302", WarehouseId = 3, Utilization =  0.8m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 7, Name = "100303", WarehouseId = 3, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 8, Name = "100304", WarehouseId = 3, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 9, Name = "100401", WarehouseId = 4, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 10, Name = "100402", WarehouseId = 4, Utilization = 0.15m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 11, Name = "100403", WarehouseId = 4, Utilization = 0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 12, Name = "100404", WarehouseId = 4, Utilization = 0.25m, TypeId = 1, Deleted = false, UpdateDate = date },
            };
        }
        private void _WarehouseLocationTypes()
        {
            WarehouseLocationTypes = new List<WarehouseLocationType>
            {
                new WarehouseLocationType(){ Id = 1, Deleted = false, TypeEnum = WarehouseLocationTypeEnum.Shelf, Description = "STD", Name = "STD", DisplayFormat = "[2]-[2]-[2]" },
                new WarehouseLocationType(){ Id = 2, Deleted = false, TypeEnum = WarehouseLocationTypeEnum.Trolley, Description = "Trolley", Name = "Trolley", DisplayFormat = "" }
            };
        }
        private void _Items()
        {
            Items = new List<Item>
            {
                new Item() { Id = 1, Name = "Banan", Code= "OWB001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.kg, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 2, Name = "Kiwi", Code= "OWK001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.szt, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 3, Name = "Arbuz", Code= "OWA001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.kg, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 4, Name = "SILAN", Code= "CHS001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.szt, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 5, Name = "PERSIL", Code= "CHP001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.szt, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 6, Name = "AJAX_", Code= "CHA001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.szt, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 7, Name = "ELMEX", Code= "CHE001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.szt, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 8, Name = "DELICJE", Code= "SLD001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.szt, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 9, Name = "MICHAŁKI", Code= "SLM001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.kg, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 10, Name = "MARS", Code= "SLM002", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.szt, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
                new Item() { Id = 11, Name = "CIASTKA", Code= "SLC001", Type = ItemTypeEnum.BuyedItem, UnitOfMeasure = UnitOfMeasure.szt, StartDate = date, CreatedDate = date, Deleted = false, UnitOfMeasures = new List<ItemUoM>() },
            };
        }
        private void _ItemUoMs()
        {
            ItemUoMs = new List<ItemUoM>
            {
            };
        }
        //ItemUoM
        private void _ItemWMS()
        {
            ItemsWMS = new List<ItemWMS>
            {
                new ItemWMS(){ Id = 1, ItemId = 1,  Item = Items.ToList()[0] },
                new ItemWMS(){ Id = 2, ItemId = 2,   Item = Items.ToList()[1] },
                new ItemWMS(){ Id = 3, ItemId = 3,   Item = Items.ToList()[2] },
                new ItemWMS(){ Id = 4, ItemId = 4,   Item = Items.ToList()[3] },
                new ItemWMS(){ Id = 5, ItemId = 5,   Item = Items.ToList()[4] },
                new ItemWMS(){ Id = 6, ItemId = 6,   Item = Items.ToList()[5] },
                new ItemWMS(){ Id = 7, ItemId = 7,   Item = Items.ToList()[6] },
                new ItemWMS(){ Id = 8, ItemId = 8,   Item = Items.ToList()[7] },
                new ItemWMS(){ Id = 9, ItemId = 9,   Item = Items.ToList()[8] },
                new ItemWMS(){ Id = 10, ItemId = 10, Item = Items.ToList()[9] },
                new ItemWMS(){ Id = 11, ItemId = 11,Item = Items.ToList()[10] },
            };
        }
        private void _Packages()
        {
            Packages = new List<Package>
            {
                new Package(){ Id = 1, Code = "00000000", Deleted = false, Name = "Luzem", Type = EnumPackageType.CartonBox }
            };
        }
        private void _PackageItems()
        {
            PackageItems = new List<PackageItem>
            {
                new PackageItem(){ Id = 1,  ItemWMSId = 1,  PackageId = 1, PackagesPerPallet = 4, QtyPerPackage = 50, WarehouseId = 2, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 2,  ItemWMSId = 2,  PackageId = 1, PackagesPerPallet = 200, QtyPerPackage = 1, WarehouseId = 2, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 3,  ItemWMSId = 3,  PackageId = 1, PackagesPerPallet = 20,  QtyPerPackage = 1, WarehouseId = 2, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 4,  ItemWMSId = 4,  PackageId = 1, PackagesPerPallet = 10,  QtyPerPackage = 5, WarehouseId = 3, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 5,  ItemWMSId = 5,  PackageId = 1, PackagesPerPallet = 50,  QtyPerPackage = 1, WarehouseId = 3, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 6,  ItemWMSId = 6,  PackageId = 1, PackagesPerPallet = 5,  QtyPerPackage = 10, WarehouseId = 3, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 7,  ItemWMSId = 7,  PackageId = 1, PackagesPerPallet = 500, QtyPerPackage = 1, WarehouseId = 3, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 8,  ItemWMSId = 8,  PackageId = 1, PackagesPerPallet = 100, QtyPerPackage = 10, WarehouseId = 4, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 9,  ItemWMSId = 9,  PackageId = 1, PackagesPerPallet = 10,  QtyPerPackage = 1, WarehouseId = 4, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.UpToOrderQty },
                new PackageItem(){ Id = 10, ItemWMSId = 10, PackageId = 1, PackagesPerPallet = 12, QtyPerPackage = 20, WarehouseId = 4, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.FullPackage },
                new PackageItem(){ Id = 11, ItemWMSId = 11, PackageId = 1, PackagesPerPallet = 20,  QtyPerPackage = 40, WarehouseId = 4, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.FullPackage },
            };
        }
        private void _StockUnits()
        {
            StockUnits = new List<StockUnit>
            {
                new StockUnit(){ Id = 1, ItemWMSId = 1 ,SerialNumber = "2001000001", PackageItemId = 1, WarehouseLocationId = 2,    MaxQtyPerPackage = 50, CurrentQtyinPackage = 50, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 2, ItemWMSId = 4 ,SerialNumber = "2001000002", PackageItemId = 4, WarehouseLocationId = 5,    MaxQtyPerPackage = 5, CurrentQtyinPackage = 5, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 3, ItemWMSId = 6 ,SerialNumber = "2001000003", PackageItemId = 6, WarehouseLocationId = 6,    MaxQtyPerPackage = 10, CurrentQtyinPackage = 10, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 6, ItemWMSId = 6 ,SerialNumber = "2001000006", PackageItemId = 6, WarehouseLocationId = 6,    MaxQtyPerPackage = 10, CurrentQtyinPackage = 10, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 7, ItemWMSId = 6 ,SerialNumber = "2001000007", PackageItemId = 6, WarehouseLocationId = 6,    MaxQtyPerPackage = 10, CurrentQtyinPackage = 10, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 8, ItemWMSId = 6 ,SerialNumber = "2001000008", PackageItemId = 6, WarehouseLocationId = 6,    MaxQtyPerPackage = 10, CurrentQtyinPackage = 10, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 9, ItemWMSId = 10 ,SerialNumber = "2001000009", PackageItemId = 10, WarehouseLocationId = 12, MaxQtyPerPackage = 20, CurrentQtyinPackage = 20, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 10, ItemWMSId = 10 ,SerialNumber = "2001000010", PackageItemId = 10, WarehouseLocationId = 12, MaxQtyPerPackage = 20, CurrentQtyinPackage = 20, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 4, ItemWMSId = 10 ,SerialNumber = "2001000004", PackageItemId = 10, WarehouseLocationId = 12, MaxQtyPerPackage = 20, CurrentQtyinPackage = 20, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 5, ItemWMSId = 11 ,SerialNumber = "2001000005", PackageItemId = 11, WarehouseLocationId = 10, MaxQtyPerPackage = 40, CurrentQtyinPackage = 40, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 11, ItemWMSId = 11 ,SerialNumber = "2001000011", PackageItemId = 11, WarehouseLocationId = 10, MaxQtyPerPackage = 40, CurrentQtyinPackage = 40, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 12, ItemWMSId = 11 ,SerialNumber = "2001000012", PackageItemId = 11, WarehouseLocationId = 10, MaxQtyPerPackage = 40, CurrentQtyinPackage = 40, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
            };
        }
        private void _Identity()
        {
            
            Roles = new List<ApplicationRole>() { 
                new ApplicationRole() { Id="6e78068c-089d-4324-936e-9772ee779ad8", Name = "ADMIN" },
                //new IdentityRole() { Id="96ec6381-d5b5-4850-a9ff-4c2c119b4551", Name = "ILOGIS_ADMIN" },
                //new IdentityRole() { Id="a70f2fa7-710b-41e9-89b7-42c6b4cf48ae", Name = "ILOGIS_VIEWER" },
                //new IdentityRole() { Id="aa586495-cbbd-4671-a470-06bcb31c669b", Name = "ILOGIS_CONFIG_EDITOR_WH" },
                //new IdentityRole() { Id="dac3c072-1ce0-44e4-879e-9e748a7c3436", Name = "ILOGIS_WHDOC_EDITOR" },
                //new IdentityRole() { Id="0bf3d2a2-75ec-45cf-8501-3079eb2ed42b", Name = "ILOGIS_WHDOC_VIEWER" },
                //new IdentityRole() { Id="1badf267-a515-46a8-b216-52c7fcc431f3", Name = "ILOGIS_CONFIG_EDITOR_PRD" },
                //new IdentityRole() { Id="2a1dfaad-92fd-4150-87ef-2af9c5219b0f", Name = "ILOGIS_OPERATOR" },
                //new IdentityRole() { Id="815a03b5-52c8-45ae-9164-e4238d013364", Name = "ILOGIS_WHDOC_APPROVER" }
                //new IdentityRole() { Id="", Name = "" }
            };

            Users = new List<User>() {
                new User(){ Id = "55de5ee8-221e-448a-bf5e-130065dd5e84", Name = "Admin", UserName = "Admin", 
                    PasswordHash = "AITM3pr9jhutlneCD8MZCOVt+YvvJH+5TKPCmwPIrmZg7zvFA17JuFtGRNpxOfBM9A==", SecurityStamp = "126a9907-ead8-4920-a41f-99b42e9de0c8" }
            };

            UserRoles = new List<UserRole>()
            {
                //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "0bf3d2a2-75ec-45cf-8501-3079eb2ed42b" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "11a15572-76bd-4e70-9f81-4b6c015f88c6" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "1ad2912b-ed78-4b22-bfc8-98136246f398" },
                //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "1badf267-a515-46a8-b216-52c7fcc431f3" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "1f2d058a-cebf-41b7-879c-88145676e0d9" },
                //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "2a1dfaad-92fd-4150-87ef-2af9c5219b0f" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "2a4009dc-cb53-4239-a927-0d29554edcd7" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "47703453-396c-4bc6-80f4-d4bbadd53758" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "6bfdf9c1-0c40-4e40-a48a-d075b0559509" },
                new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "6e78068c-089d-4324-936e-9772ee779ad8" },
                //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "815a03b5-52c8-45ae-9164-e4238d013364" },
                //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "96ec6381-d5b5-4850-a9ff-4c2c119b4551" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "9949530f-84b2-4f48-bf4f-12c254c55c17" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "9b9c13df-8b34-41ff-8350-ca4ac9a005e8" },
                //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "a70f2fa7-710b-41e9-89b7-42c6b4cf48ae" },
                //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "aa586495-cbbd-4671-a470-06bcb31c669b" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "b838f053-522c-4467-b3bc-797c7a6c700f" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "cf615f4f-92f8-455c-b31a-cb02cfba7432" },
                ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "d4aa9fe9-ffcb-4c2b-94d2-cc84f173ad06" },
                //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "dac3c072-1ce0-44e4-879e-9e748a7c3436" }
            };

            UserClaims = new List<UserClaim>()
            {
                new UserClaim() { Id = 1, UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", ClaimType = "", ClaimValue = ""}
            };

            UserLogins = new List<UserLogin>()
            {
                new UserLogin(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", LoginProvider = "", ProviderKey = "" }
            };
        }
        private void _SystemVariables()
        {
            SystemVariables = new List<SystemVariable>()
            {
                new SystemVariable(){ Id = 1, Name = "IncomingWarehouseLocationId", Type = EnumVariableType.Int, Value = "1" },
                new SystemVariable(){ Id = 2, Name = "ExternalWarehouseLocationId", Type = EnumVariableType.Int, Value = "5" },
                new SystemVariable(){ Id = 3, Name = "SerialNumber_StockUnit", Type = EnumVariableType.Int, Value = "99" },
                //new SystemVariable(){ Id = 1, Name = "", Type = EnumVariableType.String, Value = "" },
            };
        }
        private void _Printers()
        {
            Printers = new List<Printer>
            {
                new Printer() { Id = 1, Name = "Brother DCP-1610W series", IpAdress="0.0.0.1", PrinterType = PrinterType.Laser },
            };
        }
    }

    public class GenericMocker<T> where T : class
    {
        public Mock<DbSet<T>> CreateDbSetMock(IQueryable<T> _obj)
        {
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(_obj.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(_obj.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(_obj.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(_obj.GetEnumerator());

            return dbSetMock;
        }

        public void BindObjects(List<T> _objList, DbContextDataCreator dbContextDataCreator)
        {
            foreach (var obj in _objList)
            {
                Type type = obj.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == "WarehouseId")
                    {
                        int warehouseId = (int)property.GetValue(obj, null);
                        PropertyInfo propTemp = type.GetProperty("Warehouse");
                        propTemp.SetValue(obj, dbContextDataCreator.Warehouses.FirstOrDefault(x => x.Id == warehouseId));
                    }

                    if (property.Name == "WarehouseLocationId")
                    {
                        int warehouseLocationId = (int)property.GetValue(obj, null);
                        PropertyInfo propTemp = type.GetProperty("WarehouseLocation");
                        propTemp.SetValue(obj, dbContextDataCreator.WarehouseLocations.FirstOrDefault(x => x.Id == warehouseLocationId));
                    }

                    if (property.Name == "ItemId")
                    {
                        int itemId = (int)property.GetValue(obj, null);
                        PropertyInfo propTemp = type.GetProperty("Item");
                        propTemp.SetValue(obj, dbContextDataCreator.Items.FirstOrDefault(x => x.Id == itemId));
                    }


                    if (property.Name == "ItemWMSId")
                    {
                        int itemWmsId = (int)property.GetValue(obj, null);
                        PropertyInfo propTemp = type.GetProperty("ItemWMS");
                        propTemp.SetValue(obj, dbContextDataCreator.ItemsWMS.FirstOrDefault(x => x.Id == itemWmsId));
                    }

                    if (property.Name == "PackageId")
                    {
                        int packageId = (int)property.GetValue(obj, null);
                        PropertyInfo propTemp = type.GetProperty("Package");
                        propTemp.SetValue(obj, dbContextDataCreator.Packages.FirstOrDefault(x => x.Id == packageId));
                    }

                    if (property.Name == "PackageItemId")
                    {
                        int packageItemId = (int)property.GetValue(obj, null);
                        PropertyInfo propTemp = type.GetProperty("PackageItem");
                        propTemp.SetValue(obj, dbContextDataCreator.PackageItems.FirstOrDefault(x => x.Id == packageItemId));
                    }

                    if (property.Name == "WarehouseLocationTypeId")
                    {
                        int warehouseLocationTypeId = (int)property.GetValue(obj, null);
                        PropertyInfo propTemp = type.GetProperty("WarehouseLocationType");
                        propTemp.SetValue(obj, dbContextDataCreator.WarehouseLocationTypes.FirstOrDefault(x => x.Id == warehouseLocationTypeId));
                    }

                    if (property.Name == "TypeId")
                    {
                        int warehouseLocationTypeId = (int)property.GetValue(obj, null);
                        PropertyInfo propTemp = type.GetProperty("Type");
                        propTemp.SetValue(obj, dbContextDataCreator.WarehouseLocationTypes.FirstOrDefault(x => x.Id == warehouseLocationTypeId));
                    }
                }
            }
        }
    }

}

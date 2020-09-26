using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
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
using MDL_iLOGIS.ComponentCore.Enums;
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
    public class StockUnitRepoTest2
    {
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
            DbContextAPP_Dev db = new DbContextDataCreator2().Create();

            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(x => x.Identity.Name).Returns("Admin");
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal.Object);
            var reqContext = new RequestContext(httpContext.Object, new RouteData());

            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(db);
            var list = uow.StockUnitRepo.GetList().ToList();

            // Act
            var stockUnitController = new StockUnitController(db);
            stockUnitController.ControllerContext = new System.Web.Mvc.ControllerContext(reqContext, stockUnitController);
            JsonResult jsonResult = stockUnitController.GreateGroup(new int[] { 3, 6, 7, 8 });
            //StockUnitViewModel suvm = (StockUnitViewModel)jsonResult.Data.GetPropValue("data");

            StockUnit su = (StockUnit)stockUnitController.Get(6).Data;
            StockUnit stockUnitGroup = (StockUnit)stockUnitController.Get(su.GroupId??0).Data;
            List<StockUnit> stockUnitsOfGroup = new UnitOfWork_iLogis(db).StockUnitRepo.GetByGroupId(stockUnitGroup?.Id).ToList();

            // Assert
            Assert.IsTrue(su != null && su.GroupId != null && su.GroupId > 0);
            Assert.IsTrue(stockUnitGroup != null && stockUnitGroup.IsGroup);
            Assert.IsTrue(stockUnitsOfGroup != null && stockUnitsOfGroup.Count == 4);
        }

        [TestMethod]
        public void NewDeliveryTest()
        {
            // Arrange
            DbContextAPP_Dev db = new DbContextDataCreator2().Create();
            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(db);

            ItemWMS itemWMS1 = uow.ItemWMSRepo.GetById(1);
            ItemWMS itemWMS3 = uow.ItemWMSRepo.GetById(3);

            Delivery delivery = new Delivery();
            delivery.DocumentDate = new DateTime(2020, 1, 1);
            delivery.DocumentNumber = "DOC1";
            delivery.EnumDeliveryStatus = EnumDeliveryStatus.Init;
            delivery.ExternalId = "1";
            delivery.ExternalUserName = "IMPLEA";
            delivery.StampTime = new DateTime(2020, 1, 2, 13, 45, 15);
            delivery.SupplierId = 1;
            delivery.UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84";
            delivery.DeliveryItems = new List<DeliveryItem>()
            {
                new DeliveryItem(){ AdminEntry = true, ItemWMSId = 1, ItemWMS = itemWMS1, MovementType = EnumMovementType.CODE_102, QtyInPackage = 200, NumberOfPackages = 1, TotalQty = 200, UnitOfMeasure = UnitOfMeasure.kg, StockStatus = StatusEnum.Available },
                new DeliveryItem(){ AdminEntry = true, ItemWMSId = 3, ItemWMS = itemWMS3, MovementType = EnumMovementType.CODE_102, QtyInPackage = 40, NumberOfPackages = 1, TotalQty = 40, UnitOfMeasure = UnitOfMeasure.kg, StockStatus = StatusEnum.Available },
            };

            //Act 
            WarehouseLocation incomingLocation = uow.WarehouseLocationRepo.GetIncomingWarehouseLocation();
            Movement lastMovement = uow.MovementRepo.GetList().OrderByDescending(x => x.Id).FirstOrDefault();
            int lastMovementId = lastMovement != null ? lastMovement.Id : 0;

            DeliveryModel delM = new DeliveryModel(db);
            delM.DeliveryAdd(delivery);
            delM.Save();

            Delivery d = uow.DeliveryRepo.GetById(1);
            List<StockUnit> stockUnits = uow.StockUnitRepo.GetList().Where(x => x.WarehouseLocationId == incomingLocation.Id).ToList();
            List<Movement> movements = uow.MovementRepo.GetList().Where(x => x.Id > lastMovementId).ToList();

            // Assert
            Assert.IsTrue(d != null && d.DeliveryItems.Count == 2);     //czy wygenerował dostawę i itemy
            Assert.IsTrue(movements != null && movements.Count == 2);   //czy wygenerował movementy
            Assert.IsTrue(stockUnits != null && stockUnits.Count == 2); //czy wygenerował stockunity
            Assert.IsTrue(stockUnits[0].ItemWMSId == 1);                //czy stockUnit jest odpowiedni
            Assert.IsTrue(stockUnits[1].ItemWMSId == 3);                //czy stockUnit jest odpowiedni
            Assert.IsTrue(stockUnits.Sum(x=>x.CurrentQtyinPackage) == 240); //Czy ilosć w stockUnitach jest OK?
        }

        [TestMethod]
        public void MoveManualTest()
        {
            // Arrange
            DbContextAPP_Dev db = new DbContextDataCreator2().Create();
            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(db);

            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(x => x.Identity.Name).Returns("Admin");
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal.Object);
            var reqContext = new RequestContext(httpContext.Object, new RouteData());

            //Act 
            var stockUnitController = new StockUnitController(db);
            stockUnitController.ControllerContext = new System.Web.Mvc.ControllerContext(reqContext, stockUnitController);
            JsonResult jsonResult = stockUnitController.MoveManual("OWB001", 10, UnitOfMeasure.kg, EnumMovementType.CODE_311, "MAG", "MAG-OWOCE");

            ItemWMS itemWMS = uow.ItemWMSRepo.GetByCode("OWB001");
            WarehouseLocation incomingLocation = uow.WarehouseLocationRepo.GetByName("MAG"); ;
            WarehouseLocation owoceLocation = uow.WarehouseLocationRepo.GetByName("OWOCE");

            StockUnit suSource = uow.StockUnitRepo.GetFromWarehouseLoc_CreateIfNotExists(itemWMS, incomingLocation);
            StockUnit suDest = uow.StockUnitRepo.GetFromWarehouseLoc_CreateIfNotExists(itemWMS, owoceLocation);

            // Assert
            Assert.AreEqual(0, ((JsonModel)(jsonResult.Data)).Status);
            Assert.AreEqual(-10, suSource.CurrentQtyinPackage);
            Assert.AreEqual(10, suDest.CurrentQtyinPackage);
        }

        [TestMethod]
        public void MoveToStockUnitTest()
        {
            // Arrange
            DbContextAPP_Dev db = new DbContextDataCreator2().Create();
            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(db);

            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(x => x.Identity.Name).Returns("Admin");
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal.Object);
            var reqContext = new RequestContext(httpContext.Object, new RouteData());

            //Act 
            decimal qtyToMove = 6;
            StockUnit suSource = uow.StockUnitRepo.GetById(11);
            StockUnit suDest = uow.StockUnitRepo.GetById(12);
            decimal expectedQtyS = suSource.CurrentQtyinPackage - qtyToMove;
            decimal expectedQtyD = suDest.CurrentQtyinPackage + qtyToMove;

            var stockUnitController = new StockUnitController(db);
            stockUnitController.ControllerContext = new System.Web.Mvc.ControllerContext(reqContext, stockUnitController);
            JsonResult jsonResult = stockUnitController.MoveToStockUnit(11, 12, qtyToMove);

            StockUnit suSourceNew = uow.StockUnitRepo.GetById(11);
            StockUnit suDestNew = uow.StockUnitRepo.GetById(12);

            // Assert
            Assert.AreEqual(0, ((JsonModel)(jsonResult.Data)).Status);
            Assert.AreEqual(expectedQtyS, suSourceNew.CurrentQtyinPackage);
            Assert.AreEqual(expectedQtyD, suDestNew.CurrentQtyinPackage);
        }

        [TestMethod]
        public void MoveToStockUnitTest_DifferentItemCodes()
        {
            // Arrange
            DbContextAPP_Dev db = new DbContextDataCreator2().Create();
            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(db);

            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(x => x.Identity.Name).Returns("Admin");
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal.Object);
            var reqContext = new RequestContext(httpContext.Object, new RouteData());

            //Act 
            decimal qtyToMove = 6;
            StockUnit suSource = uow.StockUnitRepo.GetById(1);
            StockUnit suDest = uow.StockUnitRepo.GetById(12);
            decimal expectedQtyS = suSource.CurrentQtyinPackage;
            decimal expectedQtyD = suDest.CurrentQtyinPackage;

            var stockUnitController = new StockUnitController(db);
            stockUnitController.ControllerContext = new System.Web.Mvc.ControllerContext(reqContext, stockUnitController);
            JsonResult jsonResult = stockUnitController.MoveToStockUnit(1, 12, qtyToMove);

            StockUnit suSourceNew = uow.StockUnitRepo.GetById(1);
            StockUnit suDestNew = uow.StockUnitRepo.GetById(12);

            // Assert
            Assert.AreEqual(iLogisStatus.ItemWMSNotTheSame, (iLogisStatus)(((JsonModel)(jsonResult.Data)).Status));
            Assert.AreEqual(expectedQtyS, suSourceNew.CurrentQtyinPackage);
            Assert.AreEqual(expectedQtyD, suDestNew.CurrentQtyinPackage);
        }

        [TestMethod]
        public void MoveToStockUnitTest_MoreQtyThanAvailable()
        {
            // Arrange
            DbContextAPP_Dev db = new DbContextDataCreator2().Create();
            UnitOfWork_iLogis uow = new UnitOfWork_iLogis(db);

            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(x => x.Identity.Name).Returns("Admin");
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal.Object);
            var reqContext = new RequestContext(httpContext.Object, new RouteData());

            //Act 
            decimal qtyToMove = 1000;
            StockUnit suSource = uow.StockUnitRepo.GetById(11);
            StockUnit suDest = uow.StockUnitRepo.GetById(12);
            decimal expectedQtyS = suSource.CurrentQtyinPackage;
            decimal expectedQtyD = suDest.CurrentQtyinPackage;

            var stockUnitController = new StockUnitController(db);
            stockUnitController.ControllerContext = new System.Web.Mvc.ControllerContext(reqContext, stockUnitController);
            JsonResult jsonResult = stockUnitController.MoveToStockUnit(11, 12, qtyToMove);

            StockUnit suSourceNew = uow.StockUnitRepo.GetById(11);
            StockUnit suDestNew = uow.StockUnitRepo.GetById(12);

            // Assert
            Assert.AreEqual(iLogisStatus.StockUnitRequestedQtyNotAvailable, (iLogisStatus)(((JsonModel)(jsonResult.Data)).Status));
            Assert.AreEqual(expectedQtyS, suSourceNew.CurrentQtyinPackage);
            Assert.AreEqual(expectedQtyD, suDestNew.CurrentQtyinPackage);
        }
    }

    public class DbContextDataCreator2
    {
        private DateTime date = new DateTime(2020, 1, 1, 12, 0, 0);
        private DbConnection connection;
        private DbContextAPP_Dev db;

        public DbContextDataCreator2()
        {
            connection = DbConnectionFactory.CreateTransient();
            db = new DbContextAPP_Dev(connection);
        }

        public DbContextAPP_Dev Create()
        {
            _Identity();
            _Printers();
            _SystemVariables();
            _Warehouses();
            _WarehouseLocationTypes();
            _WarehouseLocations();
            _Items();
            _ItemUoMs();
            _ItemWMS();
            _Packages();
            _PackageItems();
            _StockUnits();
            _Suppliers();

            return db;
        }
        private void _Warehouses()
        {
            db.Warehouses.AddRange(new List<Warehouse>() {
                new Warehouse() { Id = 1, Name = "MAG", Code = "MAG", AccountingWarehouse = null },
                new Warehouse() { Id = 2, Name = "MAG-OWOCE", Code = "MAG-OWOCE", AccountingWarehouse = null },
                new Warehouse() { Id = 3, Name = "MAG-CHEMIA", Code = "MAG-CHEMIA", AccountingWarehouse = null },
                new Warehouse() { Id = 4, Name = "MAG-SŁODYCZE", Code = "MAG-SŁODYCZE", AccountingWarehouse = null },
                new Warehouse() { Id = 5, Name = "EXTERNAL", Code = "EXTERNAL", AccountingWarehouse = null }
            });
            db.SaveChanges();
        }
        private void _WarehouseLocations()
        {
            db.WarehouseLocations.AddRange(new List<WarehouseLocation>() {
                new WarehouseLocation() { Id = 1, Name = "200201", WarehouseId = 2, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 2, Name = "200202", WarehouseId = 2, Utilization =  0.25m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 3, Name = "200203", WarehouseId = 2, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 4, Name = "200204", WarehouseId = 2, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 5, Name = "300301", WarehouseId = 3, Utilization =  0.1m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 6, Name = "300302", WarehouseId = 3, Utilization =  0.8m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 7, Name = "300303", WarehouseId = 3, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 8, Name = "300304", WarehouseId = 3, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 9, Name =  "400401", WarehouseId = 4, Utilization =  0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 10, Name = "400402", WarehouseId = 4, Utilization = 0.5m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 11, Name = "400403", WarehouseId = 4, Utilization = 0.0m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 12, Name = "400404", WarehouseId = 4, Utilization = 0.25m, TypeId = 1, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 13, Name = "MAG", WarehouseId = 1, Utilization = 0m, TypeId = 3, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 14, Name = "OWOCE", WarehouseId = 2, Utilization = 0m, TypeId = 3, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 15, Name = "CHEMIA", WarehouseId = 3, Utilization = 0m, TypeId = 3, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 16, Name = "SLODYCZE", WarehouseId = 4, Utilization = 0m, TypeId = 3, Deleted = false, UpdateDate = date },
                new WarehouseLocation() { Id = 17, Name = "EXTERNAL", WarehouseId = 5, Utilization = 0m, TypeId = 3, Deleted = false, UpdateDate = date }
            });
            db.SaveChanges();
        }
        private void _WarehouseLocationTypes()
        {
            db.WarehouseLocationTypes.AddRange(new List<WarehouseLocationType>
            {
                new WarehouseLocationType(){ Id = 1, Deleted = false, TypeEnum = WarehouseLocationTypeEnum.Shelf, Description = "STD", Name = "STD", DisplayFormat = "[2]-[2]-[2]" },
                new WarehouseLocationType(){ Id = 2, Deleted = false, TypeEnum = WarehouseLocationTypeEnum.Trolley, Description = "Trolley", Name = "Trolley", DisplayFormat = "" },
                new WarehouseLocationType(){ Id = 3, Deleted = false, TypeEnum = WarehouseLocationTypeEnum.Virtual, Description = "Virtual", Name = "Virtual", DisplayFormat = "" }
            });
            db.SaveChanges();
        }
        private void _Items()
        {
            db.Items.AddRange(new List<Item>
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
            });
            db.SaveChanges();
        }
        private void _ItemUoMs()
        {
            db.ItemUoMs.AddRange(new List<ItemUoM>{});
            db.SaveChanges();
        }
        private void _ItemWMS()
        {
            db.ItemWMS.AddRange(new List<ItemWMS>
            {
                new ItemWMS(){ Id = 1, ItemId = 1   },
                new ItemWMS(){ Id = 2, ItemId = 2   },
                new ItemWMS(){ Id = 3, ItemId = 3   },
                new ItemWMS(){ Id = 4, ItemId = 4   },
                new ItemWMS(){ Id = 5, ItemId = 5   },
                new ItemWMS(){ Id = 6, ItemId = 6   },
                new ItemWMS(){ Id = 7, ItemId = 7   },
                new ItemWMS(){ Id = 8, ItemId = 8   },
                new ItemWMS(){ Id = 9, ItemId = 9   },
                new ItemWMS(){ Id = 10, ItemId = 10 },
                new ItemWMS(){ Id = 11, ItemId = 11 },
            });
            db.SaveChanges();
        }
        private void _Packages()
        {
            db.Packages.AddRange( new List<Package>
            {
                new Package(){ Id = 1, Code = "00000000", Deleted = false, Name = "Luzem", Type = EnumPackageType.CartonBox }
            });
            db.SaveChanges();
        }
        private void _PackageItems()
        {
            db.PackageItems.AddRange(new List<PackageItem>
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
                new PackageItem(){ Id = 11, ItemWMSId = 11, PackageId = 1, PackagesPerPallet = 6,  QtyPerPackage = 40, WarehouseId = 4, WarehouseLocationTypeId = 1, PickingStrategy = PickingStrategyEnum.FullPackage },
            });
            db.SaveChanges();
        }
        private void _StockUnits()
        {
            db.StockUnits.AddRange(new List<StockUnit>
            {
                new StockUnit(){ Id = 1, ItemWMSId = 1  ,SerialNumber = "2001000001", PackageItemId = 1, WarehouseLocationId = 2,    MaxQtyPerPackage = 50, CurrentQtyinPackage = 50, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 2, ItemWMSId = 4  ,SerialNumber = "2001000002", PackageItemId = 4, WarehouseLocationId = 5,    MaxQtyPerPackage = 5, CurrentQtyinPackage = 5, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 3, ItemWMSId = 6  ,SerialNumber = "2001000003", PackageItemId = 6, WarehouseLocationId = 6,    MaxQtyPerPackage = 10, CurrentQtyinPackage = 10, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 6, ItemWMSId = 6  ,SerialNumber = "2001000006", PackageItemId = 6, WarehouseLocationId = 6,    MaxQtyPerPackage = 10, CurrentQtyinPackage = 10, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 7, ItemWMSId = 6  ,SerialNumber = "2001000007", PackageItemId = 6, WarehouseLocationId = 6,    MaxQtyPerPackage = 10, CurrentQtyinPackage = 10, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 8, ItemWMSId = 6  ,SerialNumber = "2001000008", PackageItemId = 6, WarehouseLocationId = 6,    MaxQtyPerPackage = 10, CurrentQtyinPackage = 10, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 9, ItemWMSId = 10 ,SerialNumber = "2001000009", PackageItemId = 10, WarehouseLocationId = 12, MaxQtyPerPackage = 20, CurrentQtyinPackage = 20, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 10,ItemWMSId = 10 ,SerialNumber = "2001000010", PackageItemId = 10, WarehouseLocationId = 12, MaxQtyPerPackage = 20, CurrentQtyinPackage = 20, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 4, ItemWMSId = 10 ,SerialNumber = "2001000004", PackageItemId = 10, WarehouseLocationId = 12, MaxQtyPerPackage = 20, CurrentQtyinPackage = 20, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 5, ItemWMSId = 11 ,SerialNumber = "2001000005", PackageItemId = 11, WarehouseLocationId = 10, MaxQtyPerPackage = 40, CurrentQtyinPackage = 40, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 11,ItemWMSId = 11 ,SerialNumber = "2001000011", PackageItemId = 11, WarehouseLocationId = 10, MaxQtyPerPackage = 40, CurrentQtyinPackage = 40, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
                new StockUnit(){ Id = 12,ItemWMSId = 11 ,SerialNumber = "2001000012", PackageItemId = 11, WarehouseLocationId = 10, MaxQtyPerPackage = 40, CurrentQtyinPackage = 40, Status = StatusEnum.Available, Deleted = false, CreatedDate = date, BestBeforeDate = date.AddDays(180), IsLocated = true, WMSLastCheck = date, },
            });
            db.SaveChanges();
        }
        private void _Identity()
        {
            db.Roles.Add(new ApplicationRole() { Id = "6e78068c-089d-4324-936e-9772ee779ad8", Name = "ADMIN" });
            //new IdentityRole() { Id="96ec6381-d5b5-4850-a9ff-4c2c119b4551", Name = "ILOGIS_ADMIN" },
            //new IdentityRole() { Id="a70f2fa7-710b-41e9-89b7-42c6b4cf48ae", Name = "ILOGIS_VIEWER" },
            //new IdentityRole() { Id="aa586495-cbbd-4671-a470-06bcb31c669b", Name = "ILOGIS_CONFIG_EDITOR_WH" },
            //new IdentityRole() { Id="dac3c072-1ce0-44e4-879e-9e748a7c3436", Name = "ILOGIS_WHDOC_EDITOR" },
            //new IdentityRole() { Id="0bf3d2a2-75ec-45cf-8501-3079eb2ed42b", Name = "ILOGIS_WHDOC_VIEWER" },
            //new IdentityRole() { Id="1badf267-a515-46a8-b216-52c7fcc431f3", Name = "ILOGIS_CONFIG_EDITOR_PRD" },
            //new IdentityRole() { Id="2a1dfaad-92fd-4150-87ef-2af9c5219b0f", Name = "ILOGIS_OPERATOR" },
            //new IdentityRole() { Id="815a03b5-52c8-45ae-9164-e4238d013364", Name = "ILOGIS_WHDOC_APPROVER" }
            //new IdentityRole() { Id="", Name = "" }

            db.Users.Add(new User()
            {
                Id = "55de5ee8-221e-448a-bf5e-130065dd5e84",
                Name = "Admin",
                UserName = "Admin",
                PasswordHash = "AITM3pr9jhutlneCD8MZCOVt+YvvJH+5TKPCmwPIrmZg7zvFA17JuFtGRNpxOfBM9A==",
                SecurityStamp = "126a9907-ead8-4920-a41f-99b42e9de0c8"
            }
            );

            db.UserRoles.Add(new UserRole() { UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "6e78068c-089d-4324-936e-9772ee779ad8" });

            //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "0bf3d2a2-75ec-45cf-8501-3079eb2ed42b" },
            ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "11a15572-76bd-4e70-9f81-4b6c015f88c6" },
            ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "1ad2912b-ed78-4b22-bfc8-98136246f398" },
            //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "1badf267-a515-46a8-b216-52c7fcc431f3" },
            ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "1f2d058a-cebf-41b7-879c-88145676e0d9" },
            //new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "2a1dfaad-92fd-4150-87ef-2af9c5219b0f" },
            ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "2a4009dc-cb53-4239-a927-0d29554edcd7" },
            ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "47703453-396c-4bc6-80f4-d4bbadd53758" },
            ////new UserRole(){ UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", RoleId = "6bfdf9c1-0c40-4e40-a48a-d075b0559509" },

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

            //db.UserClaims.Add(new UserClaim() { Id = 1, UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", ClaimType = "", ClaimValue = "" });
            //db.UserLogins.Add(new UserLogin() { UserId = "55de5ee8-221e-448a-bf5e-130065dd5e84", LoginProvider = "", ProviderKey = "" });
            db.SaveChanges();
        }
        private void _SystemVariables()
        {
            db.SystemVariables.AddRange(new List<SystemVariable>()
            {
                new SystemVariable(){ Id = 1, Name = "IncomingWarehouseLocationId", Type = EnumVariableType.Int, Value = "1" },
                new SystemVariable(){ Id = 2, Name = "ExternalWarehouseLocationId", Type = EnumVariableType.Int, Value = "5" },
                new SystemVariable(){ Id = 3, Name = "SerialNumber_StockUnit", Type = EnumVariableType.Int, Value = "12" },
                //new SystemVariable(){ Id = 1, Name = "", Type = EnumVariableType.String, Value = "" },
            });
            db.SaveChanges();
        }
        private void _Printers()
        {
            db.Printers.AddRange(new List<Printer>
            {
                new Printer() { Id = 1, Name = "Brother DCP-1610W series", IpAdress="0.0.0.1", PrinterType = PrinterType.Laser },
            });
            db.SaveChanges();
        }
        private void _Suppliers()
        {
            db.Contractors.Add(new MDL_BASE.Models.MasterData.Contractor() { Code = "0", Name = "Dostawca1" });
            db.SaveChanges();
        }
    }
}

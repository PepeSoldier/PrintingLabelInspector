using XLIB_COMMON.Repo;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using XLIB_COMMON.Interface;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDLX_MASTERDATA.Entities;
using System.Net;
using Newtonsoft.Json.Linq;
using System;
using MDL_iLOGIS.ComponentWMS.Models;
using MDL_iLOGIS.ComponentWMS._Interfaces;
using MDL_iLOGIS.ComponentWMS.ViewModels;
using System.Data.SqlClient;
using XLIB_COMMON.Repo.Base;
using MDL_iLOGIS.ComponentConfig.Repos;
using MDL_iLOGIS.ComponentWMS.Enums;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class StockUnitRepo : RepoGenericAbstract<StockUnit>
    {
        protected new IDbContextiLOGIS db;
        protected ILocataionManager locataionManager;

        public StockUnitRepo(IDbContextiLOGIS db, ILocataionManager locataionManager = null) : base(db)
        {
            this.db = db;
            this.locataionManager = locataionManager;
        }

        public override StockUnit GetById(int id)
        {
            //return db.StockUnits.Include(y => y.ItemWMS.Item.UnitOfMeasures).FirstOrDefault(x => x.Id == id);
            return db.StockUnits.FirstOrDefault(x => x.Id == id);
        }
        public StockUnit Get(int id, string serialNumber)
        {
            StockUnit stockUnit = GetById(id);
            if (stockUnit == null)
            {
                stockUnit = GetBySerialNumber(serialNumber);
            }
            return stockUnit;
        }
        public StockUnit GetByCodeAndLocationAndQty(string itemCode, string warehouseLocationName, decimal qty = 0)
        {
            WarehouseLocation wl = db.WarehouseLocations.Where(x => x.Name == warehouseLocationName).FirstOrDefault();
            StockUnit su = null;

            if (wl != null && wl.Type.TypeEnum == WarehouseLocationTypeEnum.Virtual)
            {
                ItemWMS itemWMS = db.ItemWMS.FirstOrDefault(x => x.Item.Code == itemCode);
                su = GetFromWarehouseLoc_CreateIfNotExists(itemWMS, wl);
                su.CurrentQtyinPackage = 0;
            }
            else
            {
                su = db.StockUnits.FirstOrDefault(x =>
                    x.Deleted == false &&
                    x.ItemWMS.Item.Code == itemCode &&
                    x.WarehouseLocation.Name == warehouseLocationName &&
                    (qty == 0 || x.CurrentQtyinPackage == qty)
                    );

                if (su == null)
                {
                    su = db.StockUnits.FirstOrDefault(x =>
                    (x.Deleted == true &&
                        x.WarehouseLocation.Warehouse.AccountingWarehouse != null ?
                        x.WarehouseLocation.Warehouse.AccountingWarehouse.isOutOfScore == true :
                        x.WarehouseLocation.Warehouse.isOutOfScore == true) &&
                    x.ItemWMS.Item.Code == itemCode &&
                    x.WarehouseLocation.Name == warehouseLocationName
                    );

                    if (su != null)
                        su.CurrentQtyinPackage = 0;
                }
            }

            return su;
        }
        public StockUnit GetBySerialNumber(string serialNumber)
        {
            if (serialNumber != null)
            {
                return db.StockUnits.FirstOrDefault(x => x.SerialNumber == serialNumber);
            }
            else
            {
                return null;
            }
        }
        public StockUnit GetBySerialNumberAndItemCode(string serialNumber, string itemCode, StatusEnum status = StatusEnum.Undefined, int warehouseLocationId = 0)
        {
            if (serialNumber != null && serialNumber.Length > 0 && itemCode != null && itemCode.Length > 0)
            {
                return db.StockUnits.FirstOrDefault(
                    x => x.SerialNumber == serialNumber &&
                    x.ItemWMS.Item.Code == itemCode &&
                    (status == StatusEnum.Undefined || x.Status == status) &&
                    (warehouseLocationId == 0 || x.WarehouseLocation.Id == warehouseLocationId)
                );
            }
            else
            {
                return null;
            }
        }
        public StockUnit GetPLVCoilBySerialNumber(string serialNumber)
        {
            if (serialNumber != null)
            {
                var _id = new SqlParameter("@ID", serialNumber);
                CoilData data = db.Database.SqlQuery<CoilData>("EXEC [dbo].[iLOGIS_GetCoilByID] @ID", _id).FirstOrDefault();
                ItemWMS itemWMS = db.ItemWMS.FirstOrDefault(x => x.Id == data.ItemWMSId);

                if (itemWMS != null)
                {
                    StockUnit stockUnit = new StockUnit();
                    stockUnit.ItemWMS = itemWMS;
                    stockUnit.ItemWMSId = itemWMS.Id;
                    stockUnit.CurrentQtyinPackage = data.CurrentQtyInPackage;
                    stockUnit.WMSQtyinPackage = data.WMSQtyInPackage;
                    stockUnit.SerialNumber = data.SerialNumber;
                    return stockUnit;
                }
            }

            return null;
        }
        public StockUnit GetFromIncomingAreaByItemWMSId(int itemWMSId)
        {
            int incomingWarehouseLocationId = new SystemVariableRepo(db).GetValueInt("IncomingWarehouseLocationId");

            return db.StockUnits.Where(x =>
                        x.WarehouseLocationId == incomingWarehouseLocationId &&
                        x.ItemWMSId == itemWMSId)
                    .FirstOrDefault();
        }
        public StockUnit GetFromWarehouseLoc_CreateIfNotExists(ItemWMS itemWMS, WarehouseLocation whl)
        {
            if (itemWMS == null || whl == null) return null;

            StockUnit su = db.StockUnits.FirstOrDefault(x => x.ItemWMSId == itemWMS.Id && x.WarehouseLocationId == whl.Id && x.SerialNumber == "0");

            if (su == null) { su = CreateNewStockUnit(itemWMS, whl, 0, 0); AddOrUpdate(su); }

            return su;
        }
        public StockUnit GetFromWarehouse_CreateIfNotExists(ItemWMS itemWMS, Warehouse wh)
        {
            if (itemWMS == null || wh == null) return null;

            WarehouseLocation whl = new WarehouseLocationRepo(db).GetVirtualForWarehouse(wh.Id);
            StockUnit su = db.StockUnits.FirstOrDefault(x => x.ItemWMSId == itemWMS.Id && x.WarehouseLocationId == whl.Id && x.SerialNumber == "0");

            if (su == null) su = CreateNewStockUnit(itemWMS, whl, 0, 0);

            return su;
        }

        public override IQueryable<StockUnit> GetList()
        {
            return db.StockUnits.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }
        public IQueryable<StockUnit> GetList(string warehouseCode, string warehouseLocationName, string itemCode, string serialNumber, int? locationTypeId = 0)
        {
            itemCode = itemCode == "" ? null : itemCode;
            warehouseCode = warehouseCode == "" ? null : warehouseCode;
            warehouseLocationName = warehouseLocationName == "" ? null : warehouseLocationName;
            serialNumber = serialNumber == "" ? null : serialNumber;
            //locationTypeId = locationTypeId == 0 ? null : locationTypeId;

            return db.StockUnits.Where(x =>
                    (x.Deleted == false) &&
                    (warehouseCode == null || x.WarehouseLocation.Warehouse.Code == warehouseCode) &&
                    ((locationTypeId == null || locationTypeId == 0) || x.WarehouseLocation.TypeId == locationTypeId) &&
                    (warehouseLocationName == null || x.WarehouseLocation.Name.StartsWith(warehouseLocationName)) &&
                    (itemCode == null || x.ItemWMS.Item.Code.Contains(itemCode)) &&
                    (serialNumber == null || x.SerialNumber.Contains(serialNumber))
                    )
                .OrderBy(x => x.Id);
        }
        public IQueryable<StockUnit> GetDataList(string warehouseName, string warehouseLocationName, string itemCode, string serialNo)
        {
            itemCode = itemCode == "" ? null : itemCode;
            warehouseName = warehouseName == "" ? null : warehouseName;
            warehouseLocationName = warehouseLocationName == "" ? null : warehouseLocationName;
            serialNo = serialNo == "" ? null : serialNo;

            return db.StockUnits.Where(x =>
                    (x.Deleted == false) &&
                    (warehouseName == null || x.WarehouseLocation.Warehouse.Name == warehouseName) &&
                    (warehouseLocationName == null || x.WarehouseLocation.Name == warehouseLocationName) &&
                    (itemCode == null || x.ItemWMS.Item.Code.Contains(itemCode)) &&
                    (serialNo == null || x.SerialNumber.Contains(serialNo))
                    )
                .OrderBy(x => x.Id);
        }
        public IQueryable<StockUnit> GetByGroupId(int? groupId)
        {
            return db.StockUnits
                .Where(x =>
                    (x.Deleted == false) &&
                    (groupId != null && x.GroupId == groupId)
                )
                .OrderBy(x => x.WarehouseLocation.Type.TypeEnum)
                .ThenBy(x => x.Id);
        }
        public IEnumerable<StockUnit> GetGrouppedByWarehouses(List<string> WarehouseCodes)
        {
            var data = db.StockUnits //.Where(x => WarehouseCodes.Contains(x.WarehouseLocation.Warehouse.AccountingWarehouse != null? x.WarehouseLocation.Warehouse.AccountingWarehouse.Code : x.WarehouseLocation.Warehouse.Code))
                .GroupBy(x => new { AccountingWarehouse = x.WarehouseLocation.Warehouse.AccountingWarehouse ?? x.WarehouseLocation.Warehouse, x.ItemWMS, x.Status })
                .ToList()
                .Select(x => new StockUnit()
                {
                    ItemWMS = x.Key.ItemWMS,
                    ItemWMSId = x.Key.ItemWMS.Id,
                    AccountingWarehouse = x.Key.AccountingWarehouse,
                    CurrentQtyinPackage = x.Sum(s => s.CurrentQtyinPackage),
                    ReservedQty = x.Sum(s => s.ReservedQty),
                    CreatedDate = DateTime.Now,
                    IncomeDate = DateTime.Now,
                    Status = x.Key.Status
                });

            return data;
        }
        
        //Funkcje dla PICKING LISTY
        public void ReserveStockUnit(StockUnit stockUnit, decimal qty)
        {
            stockUnit.ReservedQty = Math.Min(stockUnit.ReservedQty + qty, stockUnit.CurrentQtyinPackage);
            AddOrUpdate(stockUnit);
        }
        public void UnreserveQtyFromStockUnit(StockUnit stockUnit, decimal qty)
        {
            if (stockUnit != null)
            {
                stockUnit.ReservedQty = Math.Max(stockUnit.ReservedQty - qty, 0);
                AddOrUpdate(stockUnit);
            }
        }

        [Obsolete]
        public void UnReserveStockUnit(int pickingListId, StockUnit stockUnit, decimal qty, int skip)
        {
            decimal remainingQtyToUnreserve = qty;

            //ordezeruj ze zeskanowanego
            if (stockUnit.ReservedQty > 0)
            {
                decimal toUnreserve = Math.Min(stockUnit.ReservedQty, qty); //(stockUnit.ReservedQty >= qtyRequested ? qtyRequested : stockUnit.ReservedQty);//Math.Min((sbyte)packageInst.ReservedQty, (sbyte)remainingQtyToUnreserve);
                remainingQtyToUnreserve -= toUnreserve;
                stockUnit.ReservedQty -= toUnreserve;
                AddOrUpdate(stockUnit);
            }
            //znajdzo cokolwiek w tej samej lokacji i odrezerwuj
            //if (remainingQtyToUnreserve > 0)
            //{
            //    StockUnit pi = GetReservedStockUnit(pickingListId, stockUnit.ItemWMSId, stockUnit.WarehouseLocationId, skip);

            //    if (pi != null)
            //    {
            //        UnReserveStockUnit(pickingListId, pi, remainingQtyToUnreserve, skip + 1);
            //    }
            //}
        }
        public StockUnit GetReservedStockUnit(int pickingListId, int itemId, int warehouseLocationId, int skip)
        {
            var data = db.PickingListItems.Where(x =>
                x.StockUnit.ReservedQty > 0 &&
                x.StockUnit.ItemWMSId == itemId &&
                x.StockUnit.WarehouseLocationId == warehouseLocationId &&
                x.PickingListId == pickingListId)
                .OrderBy(x => x.Id)
                .Skip(skip).Take(1).FirstOrDefault();

            return data == null ? null : data.StockUnit;
        }
        public StockUnit GetStockUnit(int pickingListId, int itemId, int warehouseLocationId, int skip)
        {
            var data = db.PickingListItems.Where(x =>
                x.StockUnit.ItemWMSId == itemId &&
                x.StockUnit.WarehouseLocationId == warehouseLocationId &&
                x.PickingListId == pickingListId)
                .OrderBy(x => x.Id)
                .Skip(skip).Take(1).FirstOrDefault();

            return data == null ? null : data.StockUnit;
        }

        public List<StockUnit> GetLocationsOfItemByQty(string itemCode, decimal qtyRequested)
        {
            List<StockUnit> stockUnitListToReturn = new List<StockUnit>();
            decimal qtyFound = 0;
            int pageNumber = 0;
            bool noLocationsFound = false;

            while (qtyFound < qtyRequested && !noLocationsFound)
            {
                List<StockUnit> stockUnitList = GetLocationsOfItem(itemCode, 10, pageNumber);
                noLocationsFound = stockUnitList.Count <= 0;

                foreach (StockUnit stockUnit in stockUnitList)
                {
                    if (qtyFound < qtyRequested)
                    {
                        if (stockUnit.Status != StatusEnum.PickerProblem) //StatusEnum.PickerProblem ustawiamy wtedy gdy użytkownik pobrał mniejszą ilość czyli w lokacji brakuje sztuk
                        {
                            decimal qtyAvailable = stockUnit.CurrentQtyinPackage - stockUnit.ReservedQty;
                            decimal currentQtyToPick = Math.Min(qtyAvailable, qtyRequested);

                            if (currentQtyToPick > 0)
                            {
                                AddOrUpdate(stockUnit); //dopieru tutaj zapisujemy do bazy, gdyż nie wszystkie zwrócone instancje będą nam potrzebne
                                stockUnitListToReturn.Add(stockUnit);
                                qtyFound += currentQtyToPick;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                pageNumber++;
            }

            return stockUnitListToReturn;
        }
        public List<StockUnit> GetLocationsOfItem(string itemCode, int pageSize = 10, int pageNumber = 0)
        {
            if (locataionManager != null && !locataionManager.IsFake)
            {
                List<StockUnit> stockUnitList = new List<StockUnit>();
                ItemWMS item = db.ItemWMS.FirstOrDefault(x => x.Item.Code == itemCode);

                if (item != null)
                {
                    List<ApiLocation> apiLocations = locataionManager.GetLocationsOfItem(itemCode, pageSize, pageNumber);
                    List<WarehouseLocation> locationList = GetOrCreateLocations(apiLocations, item);
                    stockUnitList = GetOrCreateStockUnitList(apiLocations, item);
                }

                return stockUnitList;
            }
            else
            {
                return db.StockUnits
                    .Where(x =>
                        (x.Deleted == false) &&
                        (x.ItemWMS.Item.Code == itemCode) &&
                        (x.CurrentQtyinPackage - x.ReservedQty > 0) &&
                        (x.Status <= StatusEnum.Available) &&
                        (x.WarehouseLocation.Type.TypeEnum != WarehouseLocationTypeEnum.Trolley) &&
                        (x.WarehouseLocation.Warehouse.isMRP == true) &&
                        (x.WarehouseLocation.Warehouse.isOutOfScore == false)
                    )
                    .OrderByDescending(x => x.WarehouseLocation.AvailableForPicker)
                    .ThenBy(x => x.IncomeDate)
                    .ThenBy(x => x.WarehouseLocation.Type.TypeEnum)
                    .ThenBy(x => x.Id)
                    .Skip(pageSize * pageNumber)
                    .Take(pageSize)
                    .ToList();
            }
        }
        private List<WarehouseLocation> GetOrCreateLocations(List<ApiLocation> apilocations, ItemWMS item)
        {
            List<string> locationNames = apilocations.Select(x => x.LocationCode).ToList();
            List<WarehouseLocation> foundWarehouseLocations = db.WarehouseLocations.Where(x => locationNames.Contains(x.Name)).ToList();
            List<string> foundLocationNames = foundWarehouseLocations.Select(x => x.Name).ToList();
            List<string> notFoundLocationNames = locationNames.Where(x => !foundLocationNames.Contains(x)).ToList();

            foreach (ApiLocation apiLoc in apilocations)
            {
                if (notFoundLocationNames.Contains(apiLoc.LocationCode))
                {
                    Warehouse wh = new WarehouseRepo(db).GetOrCreate("Wh", apiLoc.WarehouseCode);

                    WarehouseLocation wl = new WarehouseLocation()
                    {
                        Name = apiLoc.LocationCode,
                        WarehouseId = wh.Id,
                        RegalNumber = apiLoc.RegalNumber,
                        ColumnNumber = apiLoc.ColumnNumber,
                        TypeId = 1,
                        //Type = WarehouseLocationTypeEnum.Shelf,
                    };
                    AddOrUpdate(wl);
                    foundWarehouseLocations.Add(db.WarehouseLocations.FirstOrDefault(x => x.Name == apiLoc.LocationCode));
                }
            }

            return foundWarehouseLocations;
        }
        private List<StockUnit> GetOrCreateStockUnitList(List<ApiLocation> apilocations, ItemWMS item)
        {
            List<StockUnit> stockUnitList = new List<StockUnit>();
            List<string> locationNames = apilocations.Select(x => x.LocationCode).ToList();
            List<WarehouseLocation> foundWarehouseLocations = db.WarehouseLocations.Where(x => locationNames.Contains(x.Name)).ToList();

            foreach (ApiLocation apiLoc in apilocations)
            {
                WarehouseLocation wl = foundWarehouseLocations.FirstOrDefault(x => x.Name == apiLoc.LocationCode);
                StockUnit su = GetExisitngStockUnit(item, wl);
                if (su == null)
                {
                    su = CreateNewStockUnit(item, wl, apiLoc.QtyInBox, apiLoc.QtyReserved);
                }

                int qty = apiLoc.QtyInBox - apiLoc.QtyReserved;
                // Funkcja rozpatruje warunki ilości w FSDS w stosunku do bazy lokalnej 
                //(Przypisuje wartosci z FSDS -> Dodaje roznice z FSDS -> Odejmuje roznice z FSDS)
                if (su.WMSLastCheck.Year > 1900 && (DateTime.Now - su.WMSLastCheck).TotalHours > 24)
                {
                    su.SerialNumber = null;
                }

                if (su.WMSLastCheck.Year > 1900 && (DateTime.Now - su.WMSLastCheck).TotalHours > 4)
                {
                    su.CurrentQtyinPackage = qty;
                    su.WMSQtyinPackage = qty;
                }
                else
                {
                    if (qty > su.WMSQtyinPackage)
                    {
                        su.CurrentQtyinPackage += Math.Abs(qty - su.WMSQtyinPackage);
                        su.WMSQtyinPackage += Math.Abs(qty - su.WMSQtyinPackage);
                        su.SerialNumber = null;
                    }
                    else if (qty <= su.WMSQtyinPackage)
                    {
                        su.CurrentQtyinPackage -= Math.Abs(qty - su.WMSQtyinPackage);
                        su.WMSQtyinPackage -= Math.Abs(qty - su.WMSQtyinPackage);
                        //pi.SerialNumber = null;
                    }
                    else if (qty == su.CurrentQtyinPackage)
                    {
                        su.CurrentQtyinPackage = qty;
                        su.WMSQtyinPackage = qty;
                    }
                }
                su.WMSLastCheck = DateTime.Now;


                stockUnitList.Add(su);
            }

            return stockUnitList;
        }
        private StockUnit CreateNewStockUnit(ItemWMS itemWMS, WarehouseLocation whlocation, int qtyInBox, int qtyReserved)
        {
            var pi = new StockUnit()
            {
                Id = 0,
                ItemWMS = itemWMS,
                ItemWMSId = itemWMS.Id,
                CurrentQtyinPackage = qtyInBox - qtyReserved,
                WarehouseLocation = whlocation,
                WarehouseLocationId = whlocation.Id,
                MaxQtyPerPackage = qtyInBox,
                WMSQtyinPackage = qtyInBox - qtyReserved,
                WMSLastCheck = DateTime.Now,
                Deleted = false,
                SerialNumber = "0",
                Status = StatusEnum.Available,
                UnitOfMeasure = itemWMS.Item.UnitOfMeasure
            };
            return pi;

        }
        private StockUnit GetExisitngStockUnit(ItemWMS item, WarehouseLocation wl)
        {
            return db.StockUnits.FirstOrDefault(x => x.ItemWMSId == item.Id && x.WarehouseLocationId == wl.Id);
        }
    }

    class CoilData
    {
        public int Id { get; set; }
        public int ItemWMSId { get; set; }
        public decimal CurrentQtyInPackage { get; set; }
        public decimal WMSQtyInPackage { get; set; }
        public string SerialNumber { get; set; }
    }

}
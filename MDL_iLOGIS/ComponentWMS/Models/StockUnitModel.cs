using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentCore;
using MDL_iLOGIS.ComponentCore.Enums;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.ComponentWMS.Models
{
    public class StockUnitModel
    {
        IDbContextiLOGIS db;
        UnitOfWork_iLogis uow;
        List<StockUnit> stockUnits;

        WarehouseLocation incomingWhLoc;
        WarehouseLocation IncomingWhLoc { get { if (incomingWhLoc == null) incomingWhLoc = uow.WarehouseLocationRepo.GetIncomingWarehouseLocation(); return incomingWhLoc; } }

        public StockUnitModel(IDbContextiLOGIS db)
        {
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
            stockUnits = new List<StockUnit>();
            //externalWarehouseLocation = new WarehouseLocation();
        }

        public StockUnit CreateStockUnit_OnIncoming(ItemWMS itemWMS, Warehouse warehouse, PackageItem packageItem, 
            decimal currentQtyInPackage, decimal maxQtyPerPackage, string serialNumber,
            StatusEnum status = StatusEnum.Undefined, bool addQtyIfExists = false)
        {
            int whlId = IncomingWhLoc.Id;
            StockUnit stockUnit = uow.StockUnitRepo.GetBySerialNumberAndItemCode(serialNumber, itemWMS.Item.Code, status, whlId);

            if (stockUnit == null)
            {
                serialNumber = serialNumber == null || serialNumber.Length <= 0 ? iLogisSerialNumberManager.GetNext(db, warehouse) : serialNumber;

                stockUnit = new StockUnit();
                stockUnit.WMSLastCheck = DateTime.Now;
                stockUnit.ItemWMS = itemWMS;
                stockUnit.ItemWMSId = itemWMS.Id;
                stockUnit.CurrentQtyinPackage = currentQtyInPackage;
                stockUnit.InitialQty = currentQtyInPackage;
                stockUnit.UnitOfMeasure = itemWMS.Item.UnitOfMeasure;
                stockUnit.MaxQtyPerPackage = maxQtyPerPackage;
                stockUnit.WarehouseLocation = IncomingWhLoc;
                stockUnit.WarehouseLocationId = IncomingWhLoc.Id;
                stockUnit.SerialNumber = serialNumber;
                stockUnit.PackageItem = packageItem;
                stockUnit.PackageItemId = packageItem != null? (int?)packageItem.Id : null;
                stockUnit.CreatedDate = DateTime.Now;
                stockUnit.IncomeDate = DateTime.Now;
                stockUnit.BestBeforeDate = DateTime.Now.AddDays(365);
                stockUnit.Status = status == StatusEnum.Undefined? StatusEnum.Available : status;
                //uow.StockUnitRepo.AddOrUpdate(stockUnit);
                stockUnits.Add(stockUnit);
            }
            else if(stockUnit != null && addQtyIfExists)
            {
                stockUnit.WMSLastCheck = DateTime.Now;
                stockUnit.MaxQtyPerPackage += maxQtyPerPackage;
                stockUnit.CurrentQtyinPackage += currentQtyInPackage;

                if (stockUnit.CurrentQtyinPackage != 0)
                {
                    stockUnits.Add(stockUnit);
                }
                else
                {
                    uow.StockUnitRepo.Delete(stockUnit);
                }
            }
            else
            {
                stockUnit = null; //nie tworzy nowego bo już istnieje
            }

            return stockUnit;
        }
        public StockUnit CreateStockUnit_OnVirtual(ItemWMS itemWMS, Warehouse warehouse, PackageItem packageItem,
            decimal currentQtyInPackage, decimal maxQtyPerPackage, string serialNumber,
            StatusEnum status = StatusEnum.Undefined, bool addQtyIfExists = false)
        {
            StockUnit stockUnit = uow.StockUnitRepo.GetBySerialNumberAndItemCode(serialNumber, itemWMS.Item.Code, status);
            WarehouseLocation whl = uow.WarehouseLocationRepo.GetVirtualForWarehouse(warehouse.Id);

            if(whl == null) { return null; }

            if (stockUnit == null)
            {
                serialNumber = serialNumber == null || serialNumber.Length <= 0 ? iLogisSerialNumberManager.GetNext(db, warehouse) : serialNumber;

                stockUnit = new StockUnit();
                stockUnit.WMSLastCheck = DateTime.Now;
                stockUnit.ItemWMS = itemWMS;
                stockUnit.ItemWMSId = itemWMS.Id;
                stockUnit.CurrentQtyinPackage = currentQtyInPackage;
                stockUnit.MaxQtyPerPackage = maxQtyPerPackage;
                stockUnit.UnitOfMeasure = itemWMS.Item.UnitOfMeasure;
                stockUnit.WarehouseLocation = whl;
                stockUnit.WarehouseLocationId = whl.Id;
                stockUnit.SerialNumber = serialNumber;
                stockUnit.PackageItem = packageItem;
                stockUnit.PackageItemId = packageItem != null ? (int?)packageItem.Id : null;
                stockUnit.CreatedDate = DateTime.Now;
                stockUnit.IncomeDate = DateTime.Now;
                stockUnit.BestBeforeDate = DateTime.Now.AddDays(365);
                stockUnit.Status = status == StatusEnum.Undefined ? StatusEnum.Available : status;
                //uow.StockUnitRepo.AddOrUpdate(stockUnit);
                stockUnits.Add(stockUnit);
            }
            else if (stockUnit != null && addQtyIfExists)
            {
                stockUnit.WMSLastCheck = DateTime.Now;
                stockUnit.MaxQtyPerPackage += maxQtyPerPackage;
                stockUnit.CurrentQtyinPackage += currentQtyInPackage;

                if (stockUnit.CurrentQtyinPackage != 0)
                {
                    stockUnits.Add(stockUnit);
                }
                else
                {
                    uow.StockUnitRepo.Delete(stockUnit);
                }
            }
            else
            {
                stockUnit = null; //nie tworzy nowego bo już istnieje
            }

            return stockUnit;
        }
        public StockUnit CreateStockUnitFromExisting(StockUnit stockUnit, Warehouse warehouse, decimal qtyToMove)
        {
            StockUnit stockUnitNew = new StockUnit();
            stockUnitNew.BestBeforeDate = stockUnit.BestBeforeDate;
            stockUnitNew.CreatedDate = DateTime.Now;
            stockUnitNew.IncomeDate = stockUnit.IncomeDate;
            stockUnitNew.CurrentQtyinPackage = qtyToMove;
            stockUnitNew.IsLocated = stockUnit.IsLocated;
            stockUnitNew.ItemWMS = stockUnit.ItemWMS;
            stockUnitNew.ItemWMSId = stockUnit.ItemWMSId;
            stockUnitNew.MaxQtyPerPackage = stockUnit.MaxQtyPerPackage;
            stockUnitNew.PackageItemId = stockUnit.PackageItemId;
            stockUnitNew.SerialNumber = iLogisSerialNumberManager.GetNext(db, warehouse);
            stockUnitNew.Status = StatusEnum.Available;
            stockUnitNew.UnitOfMeasure = stockUnit.UnitOfMeasure;

            //uow.StockUnitRepo.AddOrUpdate(stockUnitNew);
            stockUnits.Add(stockUnitNew);

            return stockUnitNew;
        }

        public StockUnit CreateStockUnitGroup_OnVirtual(ItemWMS itemWMS, Warehouse warehouse, decimal qty)
        {
            WarehouseLocation whl = uow.WarehouseLocationRepo.GetVirtualForWarehouse(warehouse.Id);
            if (whl == null) { return null; }

            StockUnit stockUnitGroup = new StockUnit();
            string serialNumber = iLogisSerialNumberManager.GetNext(db, warehouse);

            stockUnitGroup = new StockUnit();
            stockUnitGroup.WMSLastCheck = DateTime.Now;
            stockUnitGroup.ItemWMS = itemWMS;
            stockUnitGroup.ItemWMSId = itemWMS.Id;
            stockUnitGroup.CurrentQtyinPackage = qty;
            stockUnitGroup.MaxQtyPerPackage = 0;
            stockUnitGroup.WarehouseLocation = whl;
            stockUnitGroup.WarehouseLocationId = whl.Id;
            stockUnitGroup.SerialNumber = serialNumber;
            stockUnitGroup.PackageItem = null;
            stockUnitGroup.PackageItemId = null;
            stockUnitGroup.CreatedDate = DateTime.Now;
            stockUnitGroup.IncomeDate = DateTime.Now;
            stockUnitGroup.BestBeforeDate = DateTime.Now.AddDays(365);
            stockUnitGroup.Status = StatusEnum.Undefined;
            stockUnitGroup.IsGroup = true;
            //uow.StockUnitRepo.AddOrUpdate(stockUnit);
            stockUnits.Add(stockUnitGroup);
            

            return stockUnitGroup;
        }

        public StockUnit TakeQtyFromPackage(StockUnit stockUnit, decimal qtyToTake)
        {
            return ChangeQtyInPackage(stockUnit, qtyToTake * -1);
        }
        public StockUnit AddQtyToPackage(StockUnit stockUnit, decimal qtyToAdd)
        {
            return ChangeQtyInPackage(stockUnit, qtyToAdd);
        }
        public StockUnit ChangeQtyInPackage(StockUnit stockUnit, decimal qtyToMove)
        {
            stockUnit.CurrentQtyinPackage += qtyToMove;

            if (stockUnit.CurrentQtyinPackage != 0)
            {
                uow.StockUnitRepo.AddOrUpdate(stockUnit);
                return stockUnit;
            }
            else
            {
                //gdy ilość wynosi dokładnie 0, wtedy można usunąć stockUnit
                //ale nie zawsze jest to mozliwe np. poprzez zwiazanie z picking listą dlatego jest try catch
                try
                {
                    StockUnit stockUnitTemp = new StockUnit();
                    ReflectionHelper.CopyProperties(stockUnit, stockUnitTemp);
                    stockUnitTemp.Deleted = true;
                    uow.StockUnitRepo.Delete(stockUnit);
                    return stockUnitTemp;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    uow.StockUnitRepo.AddOrUpdate(stockUnit);
                    return stockUnit;
                }
            }
        }

        public bool Save()
        {
            return uow.StockUnitRepo.AddOrUpdateRange(stockUnits);
        }
    }
}

using MDL_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentCore.Enums;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.ComponentWMS.Models
{
    public class WarehouseLocationModel
    {
        UnitOfWork_iLogis uow;

        public WarehouseLocationModel(UnitOfWork_iLogis uow)
        {
            this.uow = uow;
        }

        
        public iLogisStatus LocateStockUnit(StockUnit stockUnit, int warehouseId = 0, int locationTypeId = 0, int packageId = 0, int locationId = 0, bool force = false)
        {
            if (stockUnit == null)
            {
                return iLogisStatus.StockUnitNotFound;
            }

            WarehouseLocation wl = null;
            PackageItem packageItem = null;
            decimal requiredUtilization = 1;

            if (locationId > 0)
            {
                wl = uow.WarehouseLocationRepo.GetById(locationId);
                if (wl == null)
                {
                    return iLogisStatus.LocationNotFound;
                }

                if (wl.Type.TypeEnum == WarehouseLocationTypeEnum.Virtual || wl.Type.TypeEnum == WarehouseLocationTypeEnum.Trolley)
                {
                    requiredUtilization = 0;
                }
                else
                {
                    packageItem = uow.PackageItemRepo.Get(stockUnit.ItemWMS, stockUnit.MaxQtyPerPackage, wl.WarehouseId, wl.TypeId ?? 0, packageId, parentWarehouseId: wl.Warehouse.ParentWarehouseId);

                    if (packageItem == null && !force) 
                        return iLogisStatus.PackageItemNotFound;
                    
                    requiredUtilization = GetRequiredUtilization(packageItem, stockUnit.CurrentQtyinPackage, force);

                    if (wl.Utilization + requiredUtilization > 1) 
                        return iLogisStatus.LocationUtilizationInsufficient;
                }
            }
            else if (locationId == -1 && warehouseId > 0)
            {
                wl = uow.WarehouseLocationRepo.GetVirtualForWarehouse(warehouseId);
                
                packageItem = uow.PackageItemRepo.Get(stockUnit.ItemWMS, stockUnit.MaxQtyPerPackage, warehouseId, 0, 0);
                requiredUtilization = 0;
            }
            else
            {
                packageItem = uow.PackageItemRepo.Get(stockUnit.ItemWMS, stockUnit.MaxQtyPerPackage, warehouseId, locationTypeId, packageId);
                requiredUtilization = GetRequiredUtilization(packageItem, stockUnit.CurrentQtyinPackage);

                wl = uow.WarehouseLocationRepo.FindPlace(requiredUtilization, packageItem.WarehouseId??0, packageItem.WarehouseLocationTypeId);
            }


            if (wl != null)      
            {
                //if (wl.Warehouse.GetAccountingWarehouse.isOutOfScore)
                //{
                //    uow.StockUnitRepo.Delete(stockUnit);
                //    return iLogisStatus.NoError;
                //}
                //else
                //{
                    return SetLocationOfStockUnit(stockUnit, wl, packageItem, requiredUtilization, force);
                //}
            }
            else
            {
                return iLogisStatus.LocationNotFound;
            }
        }
        public iLogisStatus LocateStockUnit(int stockUnitId, int warehouseId = 0, int locationTypeId = 0, int packageId = 0, int locationId = 0)
        {
            StockUnit stockUnit = uow.StockUnitRepo.GetById(stockUnitId);
            return LocateStockUnit(stockUnit, warehouseId, locationTypeId, packageId, locationId);
        }
        public iLogisStatus DeleteIfWarehouseIsOutOfScope(StockUnit stockUnit)
        {
            try
            {
                Warehouse awh = stockUnit.WarehouseLocation.Warehouse.AccountingWarehouse ?? stockUnit.WarehouseLocation.Warehouse;
                if (awh.isOutOfScore)
                {
                    //uow.StockUnitRepo.Delete(stockUnit);
                    stockUnit.Deleted = true;
                    uow.StockUnitRepo.AddOrUpdate(stockUnit);
                }
                return iLogisStatus.NoError;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return iLogisStatus.StockUnitDeletionProblem;
            }
        }

        public iLogisStatus SetLocationOfStockUnit(StockUnit stockUnit, WarehouseLocation newLocation, PackageItem packageItem, decimal requiredUtilization = -1, bool force = false)
        {
            if(packageItem == null && !force && newLocation.Type.TypeEnum != WarehouseLocationTypeEnum.Virtual && newLocation.Type.TypeEnum != WarehouseLocationTypeEnum.Trolley)
            {
                return iLogisStatus.PackageItemNotFound;
            }
            
            if(packageItem == null && (force || newLocation.Type.TypeEnum == WarehouseLocationTypeEnum.Virtual || newLocation.Type.TypeEnum == WarehouseLocationTypeEnum.Trolley))
            {
                requiredUtilization = 0;
            }

            if (requiredUtilization <= -1)
            {
                requiredUtilization = GetRequiredUtilization(packageItem, stockUnit.CurrentQtyinPackage, force);
            }

            if (requiredUtilization >= 0)
            {
                DecreaseUtilization(stockUnit.WarehouseLocation, requiredUtilization);
                IncreaseUtilization(newLocation, requiredUtilization);

                stockUnit.WarehouseLocation = newLocation;
                stockUnit.WarehouseLocationId = newLocation.Id;
                stockUnit.PackageItem = packageItem;
                stockUnit.PackageItemId = packageItem != null? (int?)packageItem.Id : null;
                uow.StockUnitRepo.AddOrUpdate(stockUnit);
            }

            return iLogisStatus.NoError;
        }
        
        public iLogisStatus DeleteFromLocation(StockUnit stockUnit)
        {
            decimal requiredUtilization = 0;
            
            if(stockUnit.PackageItem != null)
                requiredUtilization = GetRequiredUtilization(stockUnit.PackageItem, stockUnit.CurrentQtyinPackage);

            DecreaseUtilization(stockUnit.WarehouseLocation, requiredUtilization);
            return iLogisStatus.NoError;
        }

        //private decimal GetRequiredUtilization(StockUnit stockUnit)
        //{
        //    //zwraca 1 w razie problemów z policzeniem, bo oznacza, że będzie wymagać całkiem pustej lokacji
        //    decimal requiredUtilization = 1;

        //    if (stockUnit.PackageItem != null)
        //    {
        //        if (stockUnit.PackageItem.PickingStrategy == PickingStrategyEnum.UpToOrderQty)
        //        {
        //            decimal qtyToBeLocalized = stockUnit.CurrentQtyinPackage;
        //            decimal qtyPerPackage = stockUnit.PackageItem.QtyPerPackage;
        //            decimal packagesPerPallet = stockUnit.PackageItem.PackagesPerPallet;

        //            if (packagesPerPallet != 0 && qtyPerPackage != 0)
        //            {
        //                requiredUtilization = (qtyToBeLocalized * 1m) / (packagesPerPallet * qtyPerPackage);
        //            }
        //        }
        //        else
        //        {
        //            decimal packagesPerPallet = stockUnit.PackageItem.PackagesPerPallet;
        //            requiredUtilization = packagesPerPallet != 0 ? 1m / packagesPerPallet : 1m;

        //            if(stockUnit.CurrentQtyinPackage > stockUnit.PackageItem.QtyPerPackage)
        //            {
        //                requiredUtilization *= stockUnit.CurrentQtyinPackage / stockUnit.PackageItem.QtyPerPackage;
        //            }
        //        }
        //    }

        //    return requiredUtilization;
        //}
        private decimal GetRequiredUtilization(PackageItem packageItem, decimal qtyToByLocalized, bool force = false)
        {
            //StockUnit stockUnit = new StockUnit();
            //stockUnit.PackageItem = packageItem;
            //stockUnit.CurrentQtyinPackage = qtyToByLocalized;
            //return GetRequiredUtilization(stockUnit);

            if (force) return 0;

            decimal requiredUtilization = 1;

            if (packageItem != null)
            {
                if (packageItem.PickingStrategy == PickingStrategyEnum.UpToOrderQty)
                {
                    decimal qtyToBeLocalized = qtyToByLocalized;
                    decimal qtyPerPackage = packageItem.QtyPerPackage;
                    decimal packagesPerPallet = packageItem.PackagesPerPallet;

                    if (packagesPerPallet != 0 && qtyPerPackage != 0)
                    {
                        requiredUtilization = (qtyToBeLocalized * 1m) / (packagesPerPallet * qtyPerPackage);
                    }
                }
                else
                {
                    decimal packagesPerPallet = Math.Max(1, packageItem.PackagesPerPallet);
                    decimal qtyPerPackage = Math.Max(1, packageItem.QtyPerPackage);
                    requiredUtilization = packagesPerPallet != 0 ? 1m / packagesPerPallet : 1m;

                    if (qtyToByLocalized > qtyPerPackage)
                    {
                        requiredUtilization *= qtyToByLocalized / qtyPerPackage;
                    }
                }
            }

            return requiredUtilization;
        }
        private void UnlocateStockUnit(StockUnit stockUnit)
        {
            decimal packageUtilization = 1m / (decimal)stockUnit.PackageItem.PackagesPerPallet;
            DecreaseUtilization(stockUnit.WarehouseLocation, packageUtilization);
        }
        private void DecreaseUtilization(WarehouseLocation wl, decimal utilization)
        {
            if (wl != null)
            {
                wl.Utilization -= utilization;
                wl.Utilization = Math.Max(0, wl.Utilization);
                wl.Utilization = Math.Min(1, wl.Utilization);
                uow.WarehouseLocationRepo.AddOrUpdate(wl);
            }
        }
        private void IncreaseUtilization(WarehouseLocation wl, decimal utilization)
        {
            if (wl != null)
            {
                wl.Utilization += utilization;
                wl.Utilization = Math.Max(0, wl.Utilization);
                wl.Utilization = Math.Min(1, wl.Utilization);
                uow.WarehouseLocationRepo.AddOrUpdate(wl);
            }
        }

        public WarehouseLocation FindPlaceForItem(ItemWMS itemWMS, int qtyPerPackage, int warehouseId = 0, int locationTypeId = 0, int packageId = 0)
        {
            WarehouseLocation wl = null;

            PackageItem packageItem = uow.PackageItemRepo.Get(itemWMS, qtyPerPackage, warehouseId, locationTypeId, packageId);

            if(packageItem == null && (warehouseId > 0))
            {
                //Warehouse wh = uow.WarehouseRepo.GetById(warehouseId);
                //bool isOutOfScope = wh.AccountingWarehouse != null ? wh.AccountingWarehouse.isOutOfScore : wh.isOutOfScore;
                //if (isOutOfScope)
                    wl = uow.WarehouseLocationRepo.GetVirtualForWarehouse(warehouseId);
            }

            if (packageItem != null)
            {
                decimal requiredUtilization = GetRequiredUtilization(packageItem, qtyPerPackage);
                wl = uow.WarehouseLocationRepo.FindPlace(requiredUtilization, warehouseId, locationTypeId);

                if (wl == null && packageItem.WarehouseId != null)
                {
                    wl = uow.WarehouseLocationRepo.FindPlace(requiredUtilization, (int)packageItem.WarehouseId, locationTypeId);
                }
            }

            return wl;
        }
        public bool SetParentLocation(WarehouseLocation whLoc, WarehouseLocation parentWhLoc)
        {
            if (whLoc != null && parentWhLoc != null)
            {
                whLoc.ParentWarehouseLocation = parentWhLoc;
                whLoc.ParentWarehouseLocationId = parentWhLoc.Id;
                whLoc.WarehouseId = parentWhLoc.WarehouseId;
                uow.WarehouseLocationRepo.AddOrUpdate(whLoc);
                return true;
            }
            else
            {
                return false;
            }
        }


        //public WarehouseLocation FindEmptyLocationForItemCode(string code)
        //{
        //    ItemWMS itemWMS = uow.ItemWMSRepo.GetByCode(code);
        //    return FindEmptyLocationForItem(itemWMS);
        //}
        //public WarehouseLocation FindEmptyLocationForItem(ItemWMS itemWMS)
        //{
        //    WarehouseLocation whLoc = uow.StockUnitRepo.FindSuitedLocation(itemWMS, 100).Take(1).FirstOrDefault();
        //    return whLoc;
        //}

        //public void AddOrRemoveItemToBoxLocation(WarehouseLocation lok, ItemWMS item, int qty)
        //{
        //    if (lok.InsertCounter % 25 == 0)
        //    {
        //        UpdateUtilizationWithFullCheck(lok, item, qty);
        //    }
        //    else
        //    {
        //        UpdateUtilization(lok, item, qty);
        //    }
        //}
        //public void UpdateUtilization(WarehouseLocation lok, ItemWMS item, int qtyAdded = 1)
        //{
        //    lok.InsertCounter += 1;
        //    decimal utilizationForQtyAdded = uow.WarehouseItemRepo.GetById((int)item.Item.ItemGroupId).UtylizationPerOnepiece * qtyAdded;
        //    lok.Utilization -= Math.Abs(utilizationForQtyAdded);

        //    uow.WarehouseLocationRepo.AddOrUpdate(lok);
        //    uow.StockUnitRepo.AddOrUpdate(new StockUnit() { WarehouseLocation = lok, Item = item, CurrentQtyinPackage = qtyAdded });
        //}
        //public void UpdateUtilizationWithFullCheck(WarehouseLocation lok, ItemWMS item, int qtyAdded)
        //{
        //    decimal? utilizationPerOnePiece;
        //    decimal? utilizationSum = 0.0M;
        //    decimal? numberOfItemInLocation;

        //    List<ItemWMS> itemsInWarehouseLocation = uow.StockUnitRepo.FindItemsInLocation(lok, item);
        //    foreach (ItemWMS itemLoop in itemsInWarehouseLocation)
        //    {
        //        utilizationPerOnePiece = uow.WarehouseItemRepo.GetById((int)item.Item.ItemGroupId).UtylizationPerOnepiece; // Pobiera utilization na 1 sztuke 
        //        numberOfItemInLocation = uow.StockUnitRepo.SumAllQtyInLocationForItem(itemLoop, lok); // Pobiera ilosc itemow w danym wearehouseLocation

        //        utilizationSum += utilizationPerOnePiece * numberOfItemInLocation; // Sumuje dla danego Itemu utylizację w WarehouseLocation i dodaje do kolejnych itemow
        //    }

        //    if (lok.Utilization == utilizationSum)
        //    {
        //        UpdateUtilization(lok, item, qtyAdded);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Nie jest dobrze");
        //    }
        //}



    }
}
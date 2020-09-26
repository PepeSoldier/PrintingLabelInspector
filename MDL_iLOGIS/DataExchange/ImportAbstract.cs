using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Enums;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.DataExchange
{
    public abstract class Import
    {
        public List<string> Errors { get; protected set; }
        protected DateTime ImportStartTime;
        protected UnitOfWork_iLogis uow;
        protected List<Item> items;
        protected List<ItemWMS> itemWMSs;
        protected List<Contractor> contractors;
        protected List<Warehouse> warehouses;
        protected List<WarehouseLocation> warehouseLocations;

        public Import(IDbContextiLOGIS db)
        {
            uow = new UnitOfWork_iLogis(db);
            items = new List<Item>();
            itemWMSs = new List<ItemWMS>();
            contractors = new List<Contractor>();
            warehouses = new List<Warehouse>();
            warehouseLocations = new List<WarehouseLocation>();
            Errors = new List<string>();
        }

        public virtual void ImportData()
        {
            ImportStartTime = DateTime.Now;
            ReadData();
            //AdaptData();
            //InsertToDatabase();
        }
        protected abstract void ReadData();
        protected abstract void AdaptData();
        protected abstract void InsertToDatabase();
        protected void MoveFileToArch(string filePath)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileName(filePath);
                string destFileName = dir + @"\arch\" + fileName;

                if (File.Exists(destFileName))
                {
                    File.Delete(destFileName);
                }

                File.Move(filePath, destFileName);
            }
            catch(Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog(ex.Message);
                Logger2FileSingleton.Instance.SaveLog("Can't move to arch, so file has been deleted: " + filePath);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        protected DateTime ConvertToDate(string dateYYYYMMDD)
        {
            string year, month, day;

            if (dateYYYYMMDD.Length >= 8)
            {
                year = dateYYYYMMDD.Substring(0, 4);
                month = dateYYYYMMDD.Substring(4, 2);
                day = dateYYYYMMDD.Substring(6, 2);

                return new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            }
            else
            {
                return new DateTime(2000, 1, 1);
            }
        }
        protected DateTime ConvertToTime(string timeHHmmss)
        {
            string hour, minute, sec;

            if (timeHHmmss.Length >= 6)
            {
                hour = timeHHmmss.Substring(0, 2);
                minute = timeHHmmss.Substring(2, 2);
                sec = timeHHmmss.Substring(4, 2);

                return new DateTime(2000, 1, 1, Convert.ToInt32(hour), Convert.ToInt32(minute), Convert.ToInt32(sec));
            }
            else
            {
                return new DateTime(2000, 1, 1);
            }
        }

        protected Item GetItemByCode(string code)
        {
            Item itemtemp;
            itemtemp = items.FirstOrDefault(x => x.Code == code);

            if (itemtemp == null)
            {
                itemtemp = uow.ItemRepo.GetByCode(code);

                if (itemtemp != null)
                {
                    items.Add(itemtemp);
                }
            }

            return itemtemp;
        }
        protected Item InsertMissingItem(string code, string name, UnitOfMeasure uom, ItemTypeEnum typeEnum)
        {
            Debug.WriteLine("Add missing item: " + code);

            Item item = new Item();
            item.Code = code;
            item.Name = name;
            item.Type = typeEnum;
            item.CreatedDate = ImportStartTime;
            item.StartDate = ImportStartTime.Date;
            item.UnitOfMeasure = uom;

            uow.ItemRepo.Add(item);
            items.Add(item);

            return item;
        }
        protected ItemWMS GetItemWMSByCode(string code)
        {
            ItemWMS itemWMS;
            itemWMS = itemWMSs.FirstOrDefault(x => x.Item.Code == code);

            if (itemWMS == null)
            {
                itemWMS = uow.ItemWMSRepo.GetByCodeWithUnitOfMeasures(code);

                if (itemWMS != null)
                {
                    itemWMSs.Add(itemWMS);
                }
            }

            return itemWMS;
        }
        protected ItemWMS InsertMissingItemWMS(string code, string name, UnitOfMeasure uom, ItemTypeEnum typeEnum)
        {
            Debug.WriteLine("Add missing itemWMS: " + code);

            Item item = GetItemByCode(code);
            if (item == null)
            {
                item = InsertMissingItem(code, name, uom, typeEnum);
            }

            ItemWMS itemWMS = new ItemWMS();
            itemWMS.Item = item;
            itemWMS.ItemId = item.Id;
            itemWMS.H = 0;
            itemWMS.ABC = ComponentWMS.Enums.ClassificationABCEnum.Undefined;
            itemWMS.XYZ = ComponentWMS.Enums.ClassificationXYZEnum.Undefined;
            itemWMS.Weight = 0;
            itemWMS.PickerNo = 0;
            itemWMS.TrainNo = 0;

            uow.ItemWMSRepo.Add(itemWMS);
            itemWMSs.Add(itemWMS);

            return itemWMS;
        }
        protected void UpdateItemWMSData(ItemWMS itemWMS, string name, UnitOfMeasure uom)
        {
            itemWMS.Item.Name = name;
            itemWMS.Item.UnitOfMeasure = uom;
        }
        protected void SaveItemWMSs()
        {
            uow.ItemWMSRepo.AddOrUpdateRange(itemWMSs);
        }
        protected Contractor GetContractorByCode(string code)
        {
            Contractor contractor;
            contractor = contractors.FirstOrDefault(x => x.Code == code);

            if (contractor == null)
            {
                contractor = uow.RepoContractor.Get(code, null);
                
                if (contractor != null)
                {
                    contractors.Add(contractor);
                }
            }

            return contractor;
        }
        protected Contractor InsertMissingContractor(string code, string name)
        {
            Debug.WriteLine("Add missing contractor: " + code);

            Contractor contractor = new Contractor();
            contractor.Code = code;
            contractor.Name = name;
            contractor.Deleted = false;

            uow.RepoContractor.Add(contractor);
            contractors.Add(contractor);

            return contractor;
        }
        protected Warehouse GetWarehouseByCode(string code)
        {
            Warehouse whTemp;
            whTemp = warehouses.FirstOrDefault(x => x.Code == code);

            if (whTemp == null)
            {
                whTemp = uow.WarehouseRepo.GetList().Where(x => x.Code == code).FirstOrDefault();

                if (whTemp != null)
                {
                    warehouses.Add(whTemp);
                }
            }

            return whTemp;
        }
        protected Warehouse InsertMissingWarehouse(string code, string name)
        {
            Debug.WriteLine("Add missing warehouse: " + code);

            Warehouse wh = new Warehouse();
            wh.Code = code;
            wh.Name = name;
            wh.WarehouseType = ComponentWMS.Enums.WarehouseTypeEnum.Unknow;
            wh.Deleted = false;

            uow.WarehouseRepo.Add(wh);
            warehouses.Add(wh);

            return wh;
        }
        protected WarehouseLocation GetWarehouseLocationByWarehouse(Warehouse wh)
        {
            WarehouseLocation whLocTemp;
            whLocTemp = warehouseLocations.FirstOrDefault(x => x.WarehouseId == wh.Id);

            if (whLocTemp == null)
            {
                whLocTemp = uow.WarehouseLocationRepo.GetVirtualForWarehouse(wh.Id);

                if (whLocTemp != null)
                {
                    warehouseLocations.Add(whLocTemp);
                }
            }

            return whLocTemp;
        }
        protected WarehouseLocation InsertMissingWarehouseLocation(Warehouse wh)
        {
            Debug.WriteLine("Add missing warehouseLocation: " + wh.Name);

            WarehouseLocationType whLocType = uow.WarehouseLocationTypeRepo.GetList().FirstOrDefault(x=>x.TypeEnum == WarehouseLocationTypeEnum.Virtual);
            WarehouseLocation whLoc = new WarehouseLocation();
            whLoc.Name = wh.Name;
            whLoc.ColumnNumber = 0;
            whLoc.InsertCounter = 0;
            whLoc.QtyOfSubLocations = 0;
            whLoc.RegalNumber = "0";
            whLoc.RemoveCounter = 0;
            whLoc.ShelfNumber = 0;
            whLoc.UpdateDate = DateTime.Now;
            whLoc.WarehouseId = wh.Id;
            whLoc.TypeId = whLocType.Id;
            whLoc.Deleted = false;

            uow.WarehouseLocationRepo.Add(whLoc);
            warehouseLocations.Add(whLoc);

            return whLoc;
        }
    }
}

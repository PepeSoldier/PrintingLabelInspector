using XLIB_COMMON.Repo;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_CORE.ComponentCore.Models;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class WarehouseRepo : RepoGenericAbstract<Warehouse>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public WarehouseRepo(IDbContextiLOGIS db, IAlertManager alertManager = null) : base(db)
        {
            this.db = db;
        }

        public override Warehouse GetById(int id)
        {
            if (id > 0)
                return db.Warehouses.FirstOrDefault(x => x.Id == id);
            else
                return null;
        }
        public override IQueryable<Warehouse> GetList()
        {
            return db.Warehouses.Where(x=>x.Deleted == false).OrderBy(x => x.Name);
        }
        public IQueryable<Warehouse> GetList(Warehouse filter)
        {
            return db.Warehouses.Where(x => 
                (x.Deleted == false) &&
                (filter.ParentWarehouseId == null || filter.ParentWarehouseId <= 0 || x.ParentWarehouseId == filter.ParentWarehouseId ) &&
                (filter.Name == null || x.Name.StartsWith(filter.Name)))
            .OrderBy(x => x.Code)
            .ThenBy(x => x.Name);
        }
        public IQueryable<Warehouse> GetListAsNoTracking()
        {
            return db.Warehouses.AsNoTracking().OrderBy(x => x.Name);
        }
        public Warehouse GetByCode(string code)
        {
            return db.Warehouses.FirstOrDefault(x => x.Code == code);
        }
        public Warehouse CreateNew()
        {
            Warehouse wh = new Warehouse();
            wh.Name = "Set Warehouse Name";
            //wh.ParentWarehouseLocationId = null;
            wh.ParentWarehouseId = null;
            //wh.UpdateDate = DateTime.Now;
            return wh;
        }
        public Warehouse GetOrCreate(string name, string code) {

            Warehouse wh = db.Warehouses.FirstOrDefault(x => x.Code == code);

            if (wh == null)
            {
                wh = new Warehouse();
                wh.Code = code;
                wh.IndependentSerialNumber = false;
                wh.Name = "";
                wh.WarehouseType = WarehouseTypeEnum.AccountingWarehouse;
                AddOrUpdate(wh);
            }

            return wh;
        }

        public IQueryable<Warehouse> GetChildWarehouses(int parentWarehouseId = 0)
        {
            IQueryable<Warehouse> zm;
            if (parentWarehouseId == 0)
            {
                zm = db.Warehouses.Where(x => x.Deleted == false && x.ParentWarehouseId == null && x.WarehouseType != WarehouseTypeEnum.ExternalWarehouse);
            }
            else
            {
                zm = db.Warehouses.Where(x => x.Deleted == false && x.ParentWarehouseId == parentWarehouseId && x.WarehouseType != WarehouseTypeEnum.ExternalWarehouse);
            }
            return zm;
        }
        public IQueryable<Warehouse> GetParentWarehouses(int warehouseId = 0)
        {
            IQueryable<Warehouse> zm = db.Warehouses;

            if (warehouseId > 0)
            {
                Warehouse warehouse = db.Warehouses.FirstOrDefault(x => x.Id == warehouseId);
                if(warehouse != null && warehouse.ParentWarehouseId != null)
                {
                    int parentWarehouseId = (int)warehouse.ParentWarehouseId;
                    zm = GetChildWarehouses(parentWarehouseId);
                }
            }
            return zm;
        }

        //Buffor
        public IQueryable<Warehouse> GetBufforBoxes()
        {
            return db.Warehouses.Where(b => b.Deleted == false).OrderBy(b => b.Name);
        }
        public WarehouseItem GetBufforBoxItemGroup(int id)
        {
            return db.WarehouseItems.FirstOrDefault(b => b.Id == id);
        }
        public List<WarehouseItem> GetBufforBoxItemGroups(int boxId = 0)
        {
            return db.WarehouseItems.Where(b => b.Warehouse.Id == boxId || boxId == 0).ToList();
        }
        public int GetBufforBoxItemGroupCount(int partCategoryId)
        {
            //using (DbContextPreprod db2 = new DbContextPreprod())
            //{
                return db.WarehouseItems.AsNoTracking().Where(b => b.ItemGroupId == partCategoryId).Count();
            //}
        }
        public Warehouse GetBox(int id)
        {
            return db.Warehouses.FirstOrDefault(b => b.Id == id);
        }
        public int DeleteBox(int id)
        {
            Warehouse bb = GetBox(id);
            Delete(bb);
            return -1;
        }

        public Warehouse GetPreparationAreaWarehouse()
        {
            string preparationZoneWarehouse =  db.SystemVariables.Where(x => x.Name == DefSystemVariables.PREPARATION_AREA_WAREHOUSE_ID).FirstOrDefault().Value;
            Warehouse warehouse = new Warehouse();
            try
            {
                int preparationZoneWarehouseId = Int32.Parse(preparationZoneWarehouse);
                warehouse = db.Warehouses.Where(x => x.Id == preparationZoneWarehouseId).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return warehouse;
        }

        

        public string GetMainWarehouseCode()
        {
            string mainWarehouseCode = "";
            try
            {
                mainWarehouseCode = db.SystemVariables.Where(x => x.Name == DefSystemVariables.MAIN_WAREHOUSE_CODE).FirstOrDefault().Value;
            }
            catch (Exception)
            { 
            }
            return mainWarehouseCode;
        }
    }
}
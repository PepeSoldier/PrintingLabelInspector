using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.Linq;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class WarehouseLocationRepo : RepoGenericAbstract<WarehouseLocation>
    {
        protected new IDbContextiLOGIS db;
        public WarehouseLocationRepo(IDbContextiLOGIS db, IAlertManager alertManager = null) : base(db)
        {
            this.db = db;
        }

        public override WarehouseLocation GetById(int id)
        {
            return db.WarehouseLocations.FirstOrDefault(x => x.Deleted == false && x.Id == id);
        }
        public override IQueryable<WarehouseLocation> GetList()
        {
            //UWAGA Zaptypanie robi LEFT JOIN w celu wyeliminowania z listy warehousów, 
            //gdyż warehouse dziedziczy po lokacji i normalnie zapytanie uwzględnia elementy dziedziczące na liscie.
            //już join nie jest potrzebny, bo warehouse przestał dziedziczyć po location

            return db.WarehouseLocations
                //.SelectMany(l =>
                //    db.Warehouses.Where(x => x.Id == l.Id).DefaultIfEmpty()
                //    .Select(c => new { l, wh = c == null? (int?)null : 1 })
                //    )
                //.Where(x => x.wh == null)
                .Where(x => x.Deleted == false)
                .OrderBy(x=>x.Id);
        }
        public WarehouseLocation GetByName(string locationName)
        {            
            var zm = db.WarehouseLocations.Where(x => x.Deleted == false && x.Name == locationName).FirstOrDefault();
            return zm;
        }
        public WarehouseLocation GetByNameAndType(string locationName, WarehouseLocationTypeEnum type)
        {
            var zm = db.WarehouseLocations
                .Where(x => 
                    x.Deleted == false &&
                    x.Name == locationName &&
                    (type == WarehouseLocationTypeEnum.Shelf || x.Type.TypeEnum == type))
                .FirstOrDefault();
            return zm;
        }
        public WarehouseLocation CreateNew()
        {
            WarehouseLocation wl = new WarehouseLocation();
            wl.Name = "Set Location Name";
            wl.ParentWarehouseLocationId = null;
            //wl.WarehouseId = null;
            wl.UpdateDate = DateTime.Now;
            return wl;
        }
        public WarehouseLocation GetVirtualForWarehouse(int warehouseId)
        {
            WarehouseLocation wl = db.WarehouseLocations.Where(x => 
                    x.WarehouseId == warehouseId && 
                    (x.Type.TypeEnum == WarehouseLocationTypeEnum.Virtual))
                .Take(1)
                .FirstOrDefault();

            if(wl == null)
            {
                Warehouse wh = db.Warehouses.FirstOrDefault(x => x.Id == warehouseId);
                WarehouseLocationType wlt = db.WarehouseLocationTypes.Where(x => x.TypeEnum == WarehouseLocationTypeEnum.Virtual).Take(1).FirstOrDefault();
                
                if(wlt == null)
                {
                    wlt = new WarehouseLocationType(){
                        Description = "Wirtualna",
                        Name = "Virtual",
                        TypeEnum = WarehouseLocationTypeEnum.Virtual,
                    };
                    Add(wlt);
                }

                wl = CreateNew();
                wl.Name = wh.Code;
                wl.Type = wlt;
                wl.TypeId = wlt.Id;
                wl.WarehouseId = warehouseId;
                Add(wl);
            }

            return wl;
        }
        public WarehouseLocation FindPlace(decimal neededUtylization, int warehouseId = 0, int warehouseLocationTypeId = 0)
        {
            var whl = db.WarehouseLocations
                .Where(x => 
                    (x.Deleted == false) &&
                    (x.Utilization + neededUtylization <= 1 ) &&
                    (warehouseId <= 0 || x.WarehouseId == warehouseId) &&
                    (warehouseLocationTypeId <= 0 || x.TypeId == warehouseLocationTypeId) &&
                    (x.Type.TypeEnum != WarehouseLocationTypeEnum.Virtual))
                .OrderBy(x => x.Type.TypeEnum).Take(1).FirstOrDefault();

            if(whl == null)
            {
                whl = db.WarehouseLocations
                .Where(x =>
                    (x.Deleted == false) &&
                    (x.Utilization + neededUtylization <= 1) &&
                    (warehouseId <= 0 || x.WarehouseId == warehouseId) &&
                    (warehouseLocationTypeId <= 0 || x.TypeId == warehouseLocationTypeId) &&
                    (x.Type.TypeEnum == WarehouseLocationTypeEnum.Virtual))
                .OrderBy(x => x.Type.TypeEnum).Take(1).FirstOrDefault();
            }

            return whl;
        }
        public WarehouseLocation GetExternalWarehouseLocation()
        {
            int warehouseLocaionId = Int32.Parse(db.SystemVariables.Where(x => x.Name == "ExternalWarehouseLocationId").FirstOrDefault().Value);
            return GetById(warehouseLocaionId);
        }
        public WarehouseLocation GetIncomingWarehouseLocation()
        {
            int warehouseLocaionId = Int32.Parse(db.SystemVariables.Where(x => x.Name == "IncomingWarehouseLocationId").FirstOrDefault().Value);
            return GetById(warehouseLocaionId);
        }
        public IQueryable<WarehouseLocation> GetList(WarehouseLocation filter)
        {
            decimal minVal = 0.0m;
            decimal maxVal = 0.0m;
            if(filter.Utilization != -1)
            {
                if(filter.Utilization == 1)
                {
                    maxVal = 0.25m;
                }
                if (filter.Utilization == 2)
                {
                    maxVal = 1.0m;
                    minVal = 0.25m;
                }
                if (filter.Utilization == 3)
                {
                    maxVal = 0.5m;
                }
                if (filter.Utilization == 4)
                {
                    maxVal = 1.0m;
                    minVal = 0.5m;
                }
                if (filter.Utilization == 5)
                {
                    maxVal = 0.75m;
                }
                if (filter.Utilization == 6)
                {
                    minVal = 0.75m;
                    maxVal = 1.0m;
                }
            }

            //wyklucza magazyny, ponieważ magazyn to też lokacja i byłby na liście
            //już join nie jest potrzebny, bo warehouse przestał dziedziczyć po location
            return db.WarehouseLocations
                //.SelectMany(l =>
                //    db.Warehouses.Where(x => x.Id == l.Id).DefaultIfEmpty()
                //    .Select(c => new { l, wh = c == null ? (int?)null : 1 })
                //    )
                //.Where(x => x.l.Deleted == false || x.wh == null )
                //.Select(x => x.l)
                .Where(x =>
                    (x.Deleted == false) &&
                    (filter.Name == null || x.Name.Contains(filter.Name)) &&
                    (filter.Utilization == -1 || x.Utilization >= minVal && x.Utilization <= maxVal) &&
                    (filter.ParentWarehouseLocationId == null || filter.ParentWarehouseLocationId <= 0 || x.ParentWarehouseLocationId == filter.ParentWarehouseLocationId) &&
                    (filter.WarehouseId <= 0 || x.WarehouseId == filter.WarehouseId) &&
                    (filter.TypeId == null || filter.TypeId <= 0 || x.TypeId == filter.TypeId)
                )
                .OrderBy(x => x.Warehouse.Name)
                .ThenBy(x => x.Name);
        }
        public IQueryable<WarehouseLocation> GetList(int warehouseId)
        {
            return db.WarehouseLocations.Where(x => 
                x.Deleted == false &&
                x.WarehouseId == warehouseId
            );
        }
        public IQueryable<WarehouseLocation> GetListByNameAndWarehouse(string locationName, int warehouseId)
        {
            return db.WarehouseLocations.Where(x => 
                x.Deleted == false &&
                locationName.StartsWith(x.Name) && 
                x.WarehouseId == warehouseId);
        }
    }
}
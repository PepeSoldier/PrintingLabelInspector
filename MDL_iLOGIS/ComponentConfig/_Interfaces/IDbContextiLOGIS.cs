using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using System.Data.Entity;

namespace MDL_iLOGIS.ComponentConfig._Interfaces
{ 
    public interface IDbContextiLOGIS : IDbContextCore
    {
        DbSet<ItemWMS> ItemWMS { get; set; }
        DbSet<Package> Packages { get; set; }
        DbSet<PackageItem> PackageItems { get; set; }
        DbSet<StockUnit> StockUnits { get; set; }
        DbSet<Warehouse> Warehouses { get; set; }
        DbSet<WarehouseLocation> WarehouseLocations { get; set; }
        DbSet<WarehouseLocationType> WarehouseLocationTypes { get; set; }
        DbSet<WarehouseLocationSort> WarehouseLocationSorts { get; set; }
        DbSet<WarehouseItem> WarehouseItems { get; set; }
        DbSet<WorkstationItem> WorkstationItems { get; set; }
        DbSet<AutomaticRule> AutomaticRules { get; set; }

        DbSet<Delivery> Deliveries { get; set; }
        DbSet<DeliveryItem> DeliveryItems { get; set; }

        DbSet<DeliveryList> DeliveryLists { get; set; }
        DbSet<DeliveryListItem> DeliveryListItems { get; set; }

        DbSet<PickingList> PickingLists { get; set; }
        DbSet<PickingListItem> PickingListItems { get; set; }

        DbSet<Transporter> Transporters { get; set; }
        DbSet<TransporterLog> TransporterLogs { get; set; }

        DbSet<Movement> Movements { get; set; }

        //WhDocuments
        DbSet<WhDocumentWZ> WhDocumentWZs { get; set; }
        DbSet<WhDocumentCMR> WhDocumentCMRs { get; set; }
        DbSet<WhDocumentItem> WhDocumentItems { get; set; }
    }
}
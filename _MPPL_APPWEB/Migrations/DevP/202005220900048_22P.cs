namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "iLOGIS.WMS_Movement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemWMSId = c.Int(nullable: false),
                        SourceLocationId = c.Int(nullable: false),
                        SourceWarehouseId = c.Int(nullable: false),
                        SourceStockUnitSerialNumber = c.String(),
                        DestinationLocationId = c.Int(nullable: false),
                        DestinationWarehouseId = c.Int(nullable: false),
                        DestinationStockUnitSerialNumber = c.String(),
                        QtyMoved = c.Decimal(nullable: false, precision: 18, scale: 5),
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.DestinationLocationId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.DestinationWarehouseId)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.SourceLocationId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.SourceWarehouseId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.SourceLocationId)
                .Index(t => t.SourceWarehouseId)
                .Index(t => t.DestinationLocationId)
                .Index(t => t.DestinationWarehouseId)
                .Index(t => t.UserId);
            
            AddColumn("iLOGIS.CONFIG_Warehouse", "Code", c => c.String());
            AddColumn("ONEPROD.ENERGY_EnergyCost", "kWhConverter", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.ENERGY_EnergyCost", "UseConverter", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "ShelfNumber", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "QtyInPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "TotalQty", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "CurrentQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "WMSQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "MaxQtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "ReservedQty", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            DropColumn("CORE.Printer", "WorkOrderNumber");
        }
        
        public override void Down()
        {
            AddColumn("CORE.Printer", "WorkOrderNumber", c => c.String(maxLength: 150));
            DropForeignKey("iLOGIS.WMS_Movement", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_Movement", "SourceWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.WMS_Movement", "SourceLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_Movement", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_Movement", "DestinationWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.WMS_Movement", "DestinationLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropIndex("iLOGIS.WMS_Movement", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "DestinationWarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "DestinationLocationId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "SourceWarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "SourceLocationId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "ItemWMSId" });
            AlterColumn("iLOGIS.WMS_StockUnit", "ReservedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_StockUnit", "MaxQtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_StockUnit", "WMSQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_StockUnit", "CurrentQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "TotalQty", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "QtyInPackage", c => c.Int(nullable: false));
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "ShelfNumber");
            DropColumn("ONEPROD.ENERGY_EnergyCost", "UseConverter");
            DropColumn("ONEPROD.ENERGY_EnergyCost", "kWhConverter");
            DropColumn("iLOGIS.CONFIG_Warehouse", "Code");
            DropTable("iLOGIS.WMS_Movement");
        }
    }
}
namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _158K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "WarehouseLocationId" });
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
                        QtyMoved = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            AddColumn("iLOGIS.CONFIG_Warehouse", "isMRP", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "isOutOfScore", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "isProduction", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "ShelfNumber", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "QtyInPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "TotalQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));

            Sql("INSERT INTO [iLOGIS].[CONFIG_Warehouse] ([WarehouseType],[IndependentSerialNumber],[Name],[Code],[Deleted],[QtyOfSubLocations]) SELECT 0, 0, 'Mag.Komp','84911', 0, 0");
            Sql("UPDATE iLOGIS.CONFIG_WarehouseLocation SET WarehouseId = (SELECT MAX(Id) FROM [iLOGIS].[CONFIG_Warehouse]) WHERE WarehouseId IS NULL");

            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_StockUnit", "CurrentQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "WMSQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "MaxQtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "WarehouseLocationId", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_StockUnit", "ReservedQty", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId");
            CreateIndex("iLOGIS.WMS_StockUnit", "WarehouseLocationId");
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
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "DestinationWarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "DestinationLocationId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "SourceWarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "SourceLocationId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "ItemWMSId" });
            AlterColumn("iLOGIS.WMS_StockUnit", "ReservedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_StockUnit", "WarehouseLocationId", c => c.Int());
            AlterColumn("iLOGIS.WMS_StockUnit", "MaxQtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_StockUnit", "WMSQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_StockUnit", "CurrentQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", c => c.Int());
            AlterColumn("iLOGIS.WMS_DeliveryItem", "TotalQty", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "QtyInPackage", c => c.Int(nullable: false));
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "ShelfNumber");
            DropColumn("iLOGIS.CONFIG_Warehouse", "isProduction");
            DropColumn("iLOGIS.CONFIG_Warehouse", "isOutOfScore");
            DropColumn("iLOGIS.CONFIG_Warehouse", "isMRP");
            DropColumn("iLOGIS.CONFIG_Warehouse", "Code");
            DropTable("iLOGIS.WMS_Movement");
            CreateIndex("iLOGIS.WMS_StockUnit", "WarehouseLocationId");
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId");
        }
    }
}

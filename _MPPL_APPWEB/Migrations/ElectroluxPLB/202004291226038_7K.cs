namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "WarehouseLocationId" });
            AddColumn("iLOGIS.CONFIG_Warehouse", "Code", c => c.String());
            AddColumn("iLOGIS.CONFIG_Warehouse", "isMRP", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "isOutOfScore", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "isProduction", c => c.Boolean(nullable: false));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_StockUnit", "WarehouseLocationId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId");
            CreateIndex("iLOGIS.WMS_StockUnit", "WarehouseLocationId");
            DropColumn("CORE.Printer", "WorkOrderNumber");
        }
        
        public override void Down()
        {
            AddColumn("CORE.Printer", "WorkOrderNumber", c => c.String(maxLength: 150));
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            AlterColumn("iLOGIS.WMS_StockUnit", "WarehouseLocationId", c => c.Int());
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", c => c.Int());
            DropColumn("iLOGIS.CONFIG_Warehouse", "isProduction");
            DropColumn("iLOGIS.CONFIG_Warehouse", "isOutOfScore");
            DropColumn("iLOGIS.CONFIG_Warehouse", "isMRP");
            DropColumn("iLOGIS.CONFIG_Warehouse", "Code");
            CreateIndex("iLOGIS.WMS_StockUnit", "WarehouseLocationId");
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId");
        }
    }
}

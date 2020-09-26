namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_Warehouse", "WarehouseType", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "WasPrinted", c => c.Boolean(nullable: false));
            AddColumn("CORE.Printer", "WorkOrderNumber", c => c.String(maxLength: 150));
            CreateIndex("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "AccountingWarehouseId" });
            DropColumn("CORE.Printer", "WorkOrderNumber");
            DropColumn("iLOGIS.WMS_DeliveryItem", "WasPrinted");
            DropColumn("iLOGIS.CONFIG_Warehouse", "WarehouseType");
            DropColumn("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _153K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "AccountingWarehouseId" });
            AlterColumn("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", c => c.Int());
            CreateIndex("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");

            Sql("UPDATE iLOGIS.CONFIG_Warehouse SET AccountingWarehouseId = NULL, WarehouseType = 0");
        }
        
        public override void Down()
        {
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "AccountingWarehouseId" });
            AlterColumn("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");
        }
    }
}

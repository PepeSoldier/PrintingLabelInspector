namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10K : DbMigration
    {
        public override void Up()
        {
            AlterColumn("iLOGIS.WMS_Movement", "QtyMoved", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "CurrentQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "WMSQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "MaxQtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_StockUnit", "ReservedQty", c => c.Decimal(nullable: false, precision: 18, scale: 5));
        }
        
        public override void Down()
        {
            AlterColumn("iLOGIS.WMS_StockUnit", "ReservedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_StockUnit", "MaxQtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_StockUnit", "WMSQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_StockUnit", "CurrentQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_Movement", "QtyMoved", c => c.Int(nullable: false));
        }
    }
}

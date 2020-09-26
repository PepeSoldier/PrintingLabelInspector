namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12K : DbMigration
    {
        public override void Up()
        {
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyRequested", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyDelivered", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyUsed", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.CONFIG_PackageItem", "QtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
        }
        
        public override void Down()
        {
            AlterColumn("iLOGIS.CONFIG_PackageItem", "QtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyUsed", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyDelivered", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyRequested", c => c.Int(nullable: false));
        }
    }
}

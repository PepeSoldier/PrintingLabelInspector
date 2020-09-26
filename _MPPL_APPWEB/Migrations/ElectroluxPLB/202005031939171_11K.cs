namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11K : DbMigration
    {
        public override void Up()
        {
            AlterColumn("iLOGIS.WMS_DeliveryItem", "QtyInPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "TotalQty", c => c.Decimal(nullable: false, precision: 18, scale: 5));
        }
        
        public override void Down()
        {
            AlterColumn("iLOGIS.WMS_DeliveryItem", "TotalQty", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "QtyInPackage", c => c.Int(nullable: false));
        }
    }
}

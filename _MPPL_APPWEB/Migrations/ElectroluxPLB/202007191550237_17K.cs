namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_DeliveryItem", "PackageItemId", c => c.Int());
            AddColumn("iLOGIS.WMS_StockUnit", "ReferenceDeliveryItemId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.WMS_DeliveryItem", "PackageItemId");
            AddForeignKey("iLOGIS.WMS_DeliveryItem", "PackageItemId", "iLOGIS.CONFIG_PackageItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "PackageItemId", "iLOGIS.CONFIG_PackageItem");
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "PackageItemId" });
            DropColumn("iLOGIS.WMS_StockUnit", "ReferenceDeliveryItemId");
            DropColumn("iLOGIS.WMS_DeliveryItem", "PackageItemId");
        }
    }
}

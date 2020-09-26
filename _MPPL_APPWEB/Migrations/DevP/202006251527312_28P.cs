namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28P : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ContractorId" });
            AddColumn("iLOGIS.WMS_Delivery", "ExternalId", c => c.String());
            AddColumn("iLOGIS.WMS_Delivery", "ExternalUserName", c => c.String());
            AddColumn("iLOGIS.WMS_DeliveryItem", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "StockStatus", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "MovementType", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "DestinationWarehouseCode", c => c.String());
            AddColumn("iLOGIS.WMS_DeliveryItem", "IsSpecialStock", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.OEE_Reason", "IsGroup", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.OEE_Reason", "GroupId", c => c.Int());
            AddColumn("iLOGIS.WMS_Movement", "ExternalId", c => c.String());
            AddColumn("iLOGIS.WMS_Movement", "ExternalUserName", c => c.String());
            AlterColumn("iLOGIS.WHDOC_WhDocument", "ContractorId", c => c.Int());
            CreateIndex("ONEPROD.OEE_Reason", "GroupId");
            CreateIndex("iLOGIS.WHDOC_WhDocument", "ContractorId");
            AddForeignKey("ONEPROD.OEE_Reason", "GroupId", "ONEPROD.OEE_Reason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.OEE_Reason", "GroupId", "ONEPROD.OEE_Reason");
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ContractorId" });
            DropIndex("ONEPROD.OEE_Reason", new[] { "GroupId" });
            AlterColumn("iLOGIS.WHDOC_WhDocument", "ContractorId", c => c.Int(nullable: false));
            DropColumn("iLOGIS.WMS_Movement", "ExternalUserName");
            DropColumn("iLOGIS.WMS_Movement", "ExternalId");
            DropColumn("ONEPROD.OEE_Reason", "GroupId");
            DropColumn("ONEPROD.OEE_Reason", "IsGroup");
            DropColumn("iLOGIS.WMS_DeliveryItem", "IsSpecialStock");
            DropColumn("iLOGIS.WMS_DeliveryItem", "DestinationWarehouseCode");
            DropColumn("iLOGIS.WMS_DeliveryItem", "MovementType");
            DropColumn("iLOGIS.WMS_DeliveryItem", "StockStatus");
            DropColumn("iLOGIS.WMS_DeliveryItem", "UnitOfMeasure");
            DropColumn("iLOGIS.WMS_Delivery", "ExternalUserName");
            DropColumn("iLOGIS.WMS_Delivery", "ExternalId");
            CreateIndex("iLOGIS.WHDOC_WhDocument", "ContractorId");
        }
    }
}

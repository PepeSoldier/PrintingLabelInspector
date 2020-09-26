namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_Delivery", "ExternalId", c => c.String());
            AddColumn("iLOGIS.WMS_Delivery", "ExternalUserName", c => c.String());
            AddColumn("iLOGIS.WMS_DeliveryItem", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "StockStatus", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "MovementType", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "DestinationWarehouseCode", c => c.String());
            AddColumn("iLOGIS.WMS_DeliveryItem", "IsSpecialStock", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_Movement", "ExternalId", c => c.String());
            AddColumn("iLOGIS.WMS_Movement", "ExternalUserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WMS_Movement", "ExternalUserName");
            DropColumn("iLOGIS.WMS_Movement", "ExternalId");
            DropColumn("iLOGIS.WMS_DeliveryItem", "IsSpecialStock");
            DropColumn("iLOGIS.WMS_DeliveryItem", "DestinationWarehouseCode");
            DropColumn("iLOGIS.WMS_DeliveryItem", "MovementType");
            DropColumn("iLOGIS.WMS_DeliveryItem", "StockStatus");
            DropColumn("iLOGIS.WMS_DeliveryItem", "UnitOfMeasure");
            DropColumn("iLOGIS.WMS_Delivery", "ExternalUserName");
            DropColumn("iLOGIS.WMS_Delivery", "ExternalId");
        }
    }
}

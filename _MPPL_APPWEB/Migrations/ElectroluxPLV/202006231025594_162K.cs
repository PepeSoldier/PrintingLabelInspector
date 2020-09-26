namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _162K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CORE.NotificationDevice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PushEndpoint = c.String(),
                        PushP256DH = c.String(),
                        PushAuth = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
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
            CreateIndex("ONEPROD.OEE_Reason", "GroupId");
            AddForeignKey("ONEPROD.OEE_Reason", "GroupId", "ONEPROD.OEE_Reason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("CORE.NotificationDevice", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.OEE_Reason", "GroupId", "ONEPROD.OEE_Reason");
            DropIndex("CORE.NotificationDevice", new[] { "UserId" });
            DropIndex("ONEPROD.OEE_Reason", new[] { "GroupId" });
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
            DropTable("CORE.NotificationDevice");
        }
    }
}

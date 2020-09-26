namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _25K : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "_MPPL.MASTERDATA_Client", newName: "MASTERDATA_Contractor");
            CreateTable(
                "iLOGIS.Delivery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        SupplierId = c.Int(nullable: false),
                        DocumentNumber = c.String(maxLength: 12),
                        DocumentDate = c.DateTime(nullable: false),
                        StampTime = c.DateTime(nullable: false),
                        EnumDeliveryStatus = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Contractor", t => t.SupplierId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "iLOGIS.DeliveryItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deleted = c.Boolean(nullable: false),
                        DeliveryId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        NumberOfPackages = c.Int(nullable: false),
                        QtyInPackage = c.Int(nullable: false),
                        TotalQty = c.Int(nullable: false),
                        AdminEntry = c.Boolean(nullable: false),
                        OperatorEntry = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.Delivery", t => t.DeliveryId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemId)
                .Index(t => t.DeliveryId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.DeliveryItem", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.DeliveryItem", "DeliveryId", "iLOGIS.Delivery");
            DropForeignKey("iLOGIS.Delivery", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.Delivery", "SupplierId", "_MPPL.MASTERDATA_Contractor");
            DropIndex("iLOGIS.DeliveryItem", new[] { "ItemId" });
            DropIndex("iLOGIS.DeliveryItem", new[] { "DeliveryId" });
            DropIndex("iLOGIS.Delivery", new[] { "SupplierId" });
            DropIndex("iLOGIS.Delivery", new[] { "UserId" });
            DropTable("iLOGIS.DeliveryItem");
            DropTable("iLOGIS.Delivery");
            RenameTable(name: "_MPPL.MASTERDATA_Contractor", newName: "MASTERDATA_Client");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "iLOGIS.Delivery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        DocumentNumber = c.String(maxLength: 12),
                        DocumentDate = c.DateTime(nullable: false),
                        DespatchTime = c.DateTime(),
                        ReferenceNumber = c.String(),
                        ReferenceDate = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Contractor", t => t.SupplierId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "iLOGIS.DeliveryItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deleted = c.Boolean(nullable: false),
                        DeliveryId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        DespatchedQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.Delivery", t => t.DeliveryId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemId)
                .Index(t => t.DeliveryId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.Delivery", "SupplierId", "_MPPL.MASTERDATA_Contractor");
            DropForeignKey("iLOGIS.DeliveryItem", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.DeliveryItem", "DeliveryId", "iLOGIS.Delivery");
            DropIndex("iLOGIS.DeliveryItem", new[] { "ItemId" });
            DropIndex("iLOGIS.DeliveryItem", new[] { "DeliveryId" });
            DropIndex("iLOGIS.Delivery", new[] { "SupplierId" });
            DropTable("iLOGIS.DeliveryItem");
            DropTable("iLOGIS.Delivery");
        }
    }
}

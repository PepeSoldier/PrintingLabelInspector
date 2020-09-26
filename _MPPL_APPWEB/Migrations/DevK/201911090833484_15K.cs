namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "iLOGIS.WMS_TransporterLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransporterId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        ItemQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WorkorderNumber = c.String(),
                        ProductItemCode = c.String(),
                        EntryType = c.Int(nullable: false),
                        RelatedObjectId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Comment = c.String(),
                        Location = c.String(),
                        TimeStamp = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemId)
                .ForeignKey("iLOGIS.CONFIG_Transporter", t => t.TransporterId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.TransporterId)
                .Index(t => t.ItemId, unique: true, name: "IX_ItemWorkstationWorkorderTransporter")
                .Index(t => t.UserId);
            
            AlterColumn("iLOGIS.WMS_PackageInstance", "CurrentQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_PackageInstance", "ReservedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_PickingListItem", "QtyRequested", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_PickingListItem", "QtyPicked", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_PickingListItem", "Bilance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.WMS_PickingListItem", "BomQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.WMS_TransporterLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "ItemId", "iLOGIS.CONFIG_Item");
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_TransporterLog", "IX_ItemWorkstationWorkorderTransporter");
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "TransporterId" });
            AlterColumn("iLOGIS.WMS_PickingListItem", "BomQty", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_PickingListItem", "Bilance", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_PickingListItem", "QtyPicked", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_PickingListItem", "QtyRequested", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_PackageInstance", "ReservedQty", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_PackageInstance", "CurrentQtyinPackage", c => c.Int(nullable: false));
            DropTable("iLOGIS.WMS_TransporterLog");
        }
    }
}

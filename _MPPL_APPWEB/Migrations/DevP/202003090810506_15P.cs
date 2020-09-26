namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.Delivery", "UserId", c => c.String(maxLength: 128));
            AddColumn("iLOGIS.DeliveryItem", "NumberOfPackages", c => c.Int(nullable: false));
            AddColumn("iLOGIS.DeliveryItem", "QtyInPackage", c => c.Int(nullable: false));
            AddColumn("iLOGIS.DeliveryItem", "TotalQty", c => c.Int(nullable: false));
            AddColumn("iLOGIS.DeliveryItem", "AdminEntry", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.DeliveryItem", "OperatorEntry", c => c.Boolean(nullable: false));
            CreateIndex("iLOGIS.Delivery", "UserId");
            AddForeignKey("iLOGIS.Delivery", "UserId", "_MPPL.IDENTITY_User", "Id");
            DropColumn("iLOGIS.DeliveryItem", "PackageQty");
            DropColumn("iLOGIS.DeliveryItem", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.DeliveryItem", "Quantity", c => c.Int(nullable: false));
            AddColumn("iLOGIS.DeliveryItem", "PackageQty", c => c.Int(nullable: false));
            DropForeignKey("iLOGIS.Delivery", "UserId", "_MPPL.IDENTITY_User");
            DropIndex("iLOGIS.Delivery", new[] { "UserId" });
            DropColumn("iLOGIS.DeliveryItem", "OperatorEntry");
            DropColumn("iLOGIS.DeliveryItem", "AdminEntry");
            DropColumn("iLOGIS.DeliveryItem", "TotalQty");
            DropColumn("iLOGIS.DeliveryItem", "QtyInPackage");
            DropColumn("iLOGIS.DeliveryItem", "NumberOfPackages");
            DropColumn("iLOGIS.Delivery", "UserId");
        }
    }
}

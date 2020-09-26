namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_PickingListItem", "PackageInstanceId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.WMS_PickingListItem", "PackageInstanceId");
            AddForeignKey("iLOGIS.WMS_PickingListItem", "PackageInstanceId", "iLOGIS.WMS_PackageInstance", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.WMS_PickingListItem", "PackageInstanceId", "iLOGIS.WMS_PackageInstance");
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PackageInstanceId" });
            DropColumn("iLOGIS.WMS_PickingListItem", "PackageInstanceId");
        }
    }
}

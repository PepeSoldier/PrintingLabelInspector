namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "PackageItemId" });
            AlterColumn("iLOGIS.WMS_PackageInstance", "PackageItemId", c => c.Int());
            CreateIndex("iLOGIS.WMS_PackageInstance", "PackageItemId");
        }
        
        public override void Down()
        {
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "PackageItemId" });
            AlterColumn("iLOGIS.WMS_PackageInstance", "PackageItemId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.WMS_PackageInstance", "PackageItemId");
        }
    }
}

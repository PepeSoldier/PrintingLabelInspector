namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.WMS_TransporterLog", "IX_ItemWorkstationWorkorderTransporter");
            AlterColumn("iLOGIS.WMS_TransporterLog", "ItemId", c => c.Int());
            CreateIndex("iLOGIS.WMS_TransporterLog", "ItemId");
        }
        
        public override void Down()
        {
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "ItemId" });
            AlterColumn("iLOGIS.WMS_TransporterLog", "ItemId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.WMS_TransporterLog", "ItemId", unique: true, name: "IX_ItemWorkstationWorkorderTransporter");
        }
    }
}

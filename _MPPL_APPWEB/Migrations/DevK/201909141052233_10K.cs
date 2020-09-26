namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_PackageInstance", "SerialNumber", c => c.String());
            AddColumn("iLOGIS.WMS_PackageInstance", "ReservedQty", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_PackageInstance", "ReservedForPickingListId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WMS_PackageInstance", "ReservedForPickingListId");
            DropColumn("iLOGIS.WMS_PackageInstance", "ReservedQty");
            DropColumn("iLOGIS.WMS_PackageInstance", "SerialNumber");
        }
    }
}

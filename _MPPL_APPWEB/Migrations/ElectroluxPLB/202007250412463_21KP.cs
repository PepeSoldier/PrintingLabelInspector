namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _21KP : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_Delivery", "Guid", c => c.String());
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "AvailableForPicker", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "AvailableForPicker");
            DropColumn("iLOGIS.WMS_Delivery", "Guid");
        }
    }
}

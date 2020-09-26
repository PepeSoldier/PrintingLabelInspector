namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _156K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.CONFIG_Warehouse", "IndependentSerialNumber", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "Qty_Scrap", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Utilization", c => c.Decimal(nullable: false, precision: 14, scale: 12));
        }
        
        public override void Down()
        {
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Utilization", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel");
            DropColumn("ONEPROD.CORE_Workorder", "Qty_Scrap");
            DropColumn("iLOGIS.CONFIG_Warehouse", "IndependentSerialNumber");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.ENERGY_EnergyConsumptionData", "ProductionQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.ENERGY_EnergyConsumptionData", "TotalStopTime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("iLOGIS.WMS_PackageInstance", "WMSQtyinPackage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("iLOGIS.WMS_PackageInstance", "WMSLastCheck", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WMS_PackageInstance", "WMSLastCheck");
            DropColumn("iLOGIS.WMS_PackageInstance", "WMSQtyinPackage");
            DropColumn("ONEPROD.ENERGY_EnergyConsumptionData", "TotalStopTime");
            DropColumn("ONEPROD.ENERGY_EnergyConsumptionData", "ProductionQty");
        }
    }
}

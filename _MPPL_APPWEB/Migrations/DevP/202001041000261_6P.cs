namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "PackageId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "PackageName", c => c.String());
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "QtyPerPackage", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "PackagesPerPallet", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "PalletW", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "PalletD", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "PalletH", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "WeightGross", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "IsPackageType", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "LastChange", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "UserName", c => c.String());
            AddColumn("ONEPROD.ENERGY_EnergyConsumptionData", "ProductionQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.ENERGY_EnergyConsumptionData", "TotalStopTime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.ENERGY_EnergyConsumptionData", "TotalStopTime");
            DropColumn("ONEPROD.ENERGY_EnergyConsumptionData", "ProductionQty");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "UserName");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "LastChange");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "IsPackageType");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "WeightGross");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "PalletH");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "PalletD");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "PalletW");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "PackagesPerPallet");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "QtyPerPackage");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "PackageName");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "PackageId");
        }
    }
}

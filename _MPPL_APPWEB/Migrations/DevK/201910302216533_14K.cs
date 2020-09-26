namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.CONFIG_PackageItem", "PalletH", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_PackageItem", "WeightGross", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("iLOGIS.CONFIG_Package", "PackagesPerPallet", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Package", "FullPalletHeight", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.CONFIG_Package", "FullPalletHeight");
            DropColumn("iLOGIS.CONFIG_Package", "PackagesPerPallet");
            DropColumn("iLOGIS.CONFIG_PackageItem", "WeightGross");
            DropColumn("iLOGIS.CONFIG_PackageItem", "PalletH");
        }
    }
}

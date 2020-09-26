namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9K : DbMigration
    {
        public override void Up()
        {
            AlterColumn("ONEPROD.OEE_OEEReportProductionData", "ProductionDate", c => c.DateTime(nullable: false));
            AlterColumn("ONEPROD.OEE_OEEReportProductionData", "ProdQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("ONEPROD.RTV_RTVOEEReportProductionData", "ProductionDate", c => c.DateTime(nullable: false));
            AlterColumn("ONEPROD.RTV_RTVOEEReportProductionData", "ProdQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("ONEPROD.RTV_RTVOEEReportProductionData", "ProdQty", c => c.Int(nullable: false));
            AlterColumn("ONEPROD.RTV_RTVOEEReportProductionData", "ProductionDate", c => c.DateTime());
            AlterColumn("ONEPROD.OEE_OEEReportProductionData", "ProdQty", c => c.Int(nullable: false));
            AlterColumn("ONEPROD.OEE_OEEReportProductionData", "ProductionDate", c => c.DateTime());
        }
    }
}

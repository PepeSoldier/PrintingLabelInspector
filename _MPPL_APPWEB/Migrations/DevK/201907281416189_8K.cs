namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.OEE_OEEReport", "TotalQtyCountedOnline", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_OEEReport", "TotalQtyDeclaredByOperator", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_OEEReport", "TotalStoppageTimeCountedOnline", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_OEEReport", "TotalStoppageTimeDeclaredByOperator", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_OEEReportProductionData", "ProdQtyCountedOnline", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.OEE_OEEReportProductionData", "ProdQtyCountedOnline");
            DropColumn("ONEPROD.OEE_OEEReport", "TotalStoppageTimeDeclaredByOperator");
            DropColumn("ONEPROD.OEE_OEEReport", "TotalStoppageTimeCountedOnline");
            DropColumn("ONEPROD.OEE_OEEReport", "TotalQtyDeclaredByOperator");
            DropColumn("ONEPROD.OEE_OEEReport", "TotalQtyCountedOnline");
        }
    }
}

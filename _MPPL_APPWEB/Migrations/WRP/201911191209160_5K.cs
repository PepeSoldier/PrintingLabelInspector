namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.RTV_RTVOEEReportProductionData", "PiecesPerPallet", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.RTV_RTVOEEReportProductionData", "PiecesPerPallet");
        }
    }
}

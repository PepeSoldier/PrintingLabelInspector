namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _155K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.MES_Workplace", "Type", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsTraceability", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsReportOnline", c => c.Boolean(nullable: false));

            Sql("UPDATE ONEPROD.MES_Workplace SET Type = 90, IsReportOnline = 1, IsTraceability = 0");

        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.MES_Workplace", "IsReportOnline");
            DropColumn("ONEPROD.MES_Workplace", "IsTraceability");
            DropColumn("ONEPROD.MES_Workplace", "Type");
        }
    }
}

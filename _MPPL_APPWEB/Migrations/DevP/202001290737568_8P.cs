namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8P : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.MES_ProductionLogTraceability", "ItemCode", c => c.String(maxLength: 50));
            AddColumn("ONEPROD.MES_ProductionLogTraceability", "SerialNumber", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.MES_ProductionLogTraceability", "SerialNumber");
            DropColumn("ONEPROD.MES_ProductionLogTraceability", "ItemCode");
        }
    }
}

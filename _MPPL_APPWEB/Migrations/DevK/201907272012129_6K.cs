namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.MES_ProductionLog", "EntryType", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_ProductionLog", "ReasonId", c => c.Int());
            CreateIndex("ONEPROD.MES_ProductionLog", "ReasonId");
            AddForeignKey("ONEPROD.MES_ProductionLog", "ReasonId", "ONEPROD.OEE_Reason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.MES_ProductionLog", "ReasonId", "ONEPROD.OEE_Reason");
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "ReasonId" });
            DropColumn("ONEPROD.MES_ProductionLog", "ReasonId");
            DropColumn("ONEPROD.MES_ProductionLog", "EntryType");
        }
    }
}

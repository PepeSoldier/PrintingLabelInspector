namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ONEPROD.MES_ProductionLogTraceability",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                        ProductionDate = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ChildId)
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.MES_ProductionLogTraceability", "ParentId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_ProductionLogTraceability", "ChildId", "ONEPROD.MES_ProductionLog");
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ParentId" });
            DropTable("ONEPROD.MES_ProductionLogTraceability");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ONEPROD.MES_ProductionLogTraceability",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(),
                        ItemCode = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 25),
                        ProductionDate = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ChildId)
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
            CreateTable(
                "ONEPROD.MES_WorkplaceBuffer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentWorkorderId = c.Int(nullable: false),
                        WorkplaceId = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                        ProductionLogId = c.Int(),
                        ProcessId = c.Int(nullable: false),
                        Barcode = c.String(),
                        SerialNumber = c.String(),
                        QtyAvailable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QtyInBom = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Code = c.String(),
                        Name = c.String(),
                        TimeLoaded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ChildId)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ParentId)
                .ForeignKey("ONEPROD.CORE_Workorder", t => t.ParentWorkorderId)
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ProductionLogId)
                .ForeignKey("ONEPROD.MES_Workplace", t => t.WorkplaceId)
                .Index(t => t.ParentWorkorderId)
                .Index(t => t.WorkplaceId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId)
                .Index(t => t.ProductionLogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "WorkplaceId", "ONEPROD.MES_Workplace");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ProductionLogId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ChildId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.MES_ProductionLogTraceability", "ParentId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_ProductionLogTraceability", "ChildId", "ONEPROD.MES_ProductionLog");
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ProductionLogId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "WorkplaceId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentWorkorderId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ParentId" });
            DropTable("ONEPROD.MES_WorkplaceBuffer");
            DropTable("ONEPROD.MES_ProductionLogTraceability");
        }
    }
}

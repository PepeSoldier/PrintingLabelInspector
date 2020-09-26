namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8P : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ChildId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ProductionLogId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "WorkplaceId", "ONEPROD.MES_Workplace");
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentWorkorderId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "WorkplaceId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ProductionLogId" });
            AddColumn("_MPPL.IDENTITY_User", "Deleted", c => c.Boolean(nullable: false));
            AlterColumn("ONEPROD.MES_ProductionLogTraceability", "ChildId", c => c.Int(nullable: false));
            CreateIndex("ONEPROD.MES_ProductionLogTraceability", "ChildId");
            DropColumn("ONEPROD.MES_ProductionLog", "UsedQty");
            DropColumn("ONEPROD.MES_ProductionLogTraceability", "ItemCode");
            DropColumn("ONEPROD.MES_ProductionLogTraceability", "SerialNumber");
            DropTable("ONEPROD.MES_WorkplaceBuffer");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("ONEPROD.MES_ProductionLogTraceability", "SerialNumber", c => c.String(maxLength: 25));
            AddColumn("ONEPROD.MES_ProductionLogTraceability", "ItemCode", c => c.String(maxLength: 50));
            AddColumn("ONEPROD.MES_ProductionLog", "UsedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            AlterColumn("ONEPROD.MES_ProductionLogTraceability", "ChildId", c => c.Int());
            DropColumn("_MPPL.IDENTITY_User", "Deleted");
            CreateIndex("ONEPROD.MES_WorkplaceBuffer", "ProductionLogId");
            CreateIndex("ONEPROD.MES_WorkplaceBuffer", "ChildId");
            CreateIndex("ONEPROD.MES_WorkplaceBuffer", "ParentId");
            CreateIndex("ONEPROD.MES_WorkplaceBuffer", "WorkplaceId");
            CreateIndex("ONEPROD.MES_WorkplaceBuffer", "ParentWorkorderId");
            CreateIndex("ONEPROD.MES_ProductionLogTraceability", "ChildId");
            AddForeignKey("ONEPROD.MES_WorkplaceBuffer", "WorkplaceId", "ONEPROD.MES_Workplace", "Id");
            AddForeignKey("ONEPROD.MES_WorkplaceBuffer", "ProductionLogId", "ONEPROD.MES_ProductionLog", "Id");
            AddForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentWorkorderId", "ONEPROD.CORE_Workorder", "Id");
            AddForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentId", "ONEPROD.CORE_Item", "Id");
            AddForeignKey("ONEPROD.MES_WorkplaceBuffer", "ChildId", "ONEPROD.CORE_Item", "Id");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10K : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("ONEPROD.MES_ProductionLogTraceability", "ChildId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_ProductionLogTraceability", "ParentId", "ONEPROD.MES_ProductionLog");
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ParentId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            CreateTable(
                "CORE.Printer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        User = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        IpAdress = c.String(nullable: false, maxLength: 50),
                        Model = c.String(maxLength: 150),
                        SerialNumber = c.String(maxLength: 150),
                        PrinterType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IpAdress, unique: true);
            
            AddColumn("_MPPL.MASTERDATA_Item", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_ProductionLog", "UsedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.MES_Workplace", "Type", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsTraceability", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsReportOnline", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "Qty_Scrap", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel", c => c.Int(nullable: false));
            DropTable("ONEPROD.MES_ProductionLogTraceability");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel");
            DropColumn("ONEPROD.CORE_Workorder", "Qty_Scrap");
            DropColumn("ONEPROD.MES_Workplace", "IsReportOnline");
            DropColumn("ONEPROD.MES_Workplace", "IsTraceability");
            DropColumn("ONEPROD.MES_Workplace", "Type");
            DropColumn("ONEPROD.MES_ProductionLog", "UsedQty");
            DropColumn("_MPPL.MASTERDATA_Item", "UnitOfMeasure");
            DropTable("CORE.Printer");
            CreateIndex("ONEPROD.MES_ProductionLogTraceability", "ChildId");
            CreateIndex("ONEPROD.MES_ProductionLogTraceability", "ParentId");
            AddForeignKey("ONEPROD.MES_ProductionLogTraceability", "ParentId", "ONEPROD.MES_ProductionLog", "Id");
            AddForeignKey("ONEPROD.MES_ProductionLogTraceability", "ChildId", "ONEPROD.MES_ProductionLog", "Id");
        }
    }
}

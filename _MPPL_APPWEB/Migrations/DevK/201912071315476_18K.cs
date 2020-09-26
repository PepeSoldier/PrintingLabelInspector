namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18K : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("ONEPROD.RTV_RTVOEEReportProductionData", "DetailId", "ONEPROD.OEE_OEEReportProductionDataDetails");
            //DropIndex("ONEPROD.RTV_RTVOEEReportProductionData", new[] { "DetailId" });
            CreateTable(
                "ONEPROD.RTV_RTVOEEParameter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourceId = c.Int(nullable: false),
                        DataType = c.Int(nullable: false),
                        Decimals = c.Int(nullable: false),
                        MinChangeValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(maxLength: 150),
                        Target = c.String(maxLength: 50),
                        Min = c.String(maxLength: 50),
                        Max = c.String(maxLength: 50),
                        MemoryAddress = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.ResourceId)
                .Index(t => t.ResourceId);
            
            CreateTable(
                "ONEPROD.RTV_RTVOEEReportProductionDataParameter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        RTVParameterId = c.Int(nullable: false),
                        Value = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.RTV_RTVOEEParameter", t => t.RTVParameterId)
                .Index(t => t.RTVParameterId);
            
            AddColumn("ONEPROD.CORE_Resource", "Breaks", c => c.String());
            AddColumn("ONEPROD.OEE_Reason", "ColorGroup", c => c.String(maxLength: 35));
            AddColumn("ONEPROD.RTV_RTVOEEReportProductionData", "PiecesPerPallet", c => c.Int(nullable: false));
            //DropColumn("ONEPROD.RTV_RTVOEEReportProductionData", "DetailId");
        }
        
        public override void Down()
        {
            AddColumn("ONEPROD.RTV_RTVOEEReportProductionData", "DetailId", c => c.Int());
            DropForeignKey("ONEPROD.RTV_RTVOEEReportProductionDataParameter", "RTVParameterId", "ONEPROD.RTV_RTVOEEParameter");
            DropForeignKey("ONEPROD.RTV_RTVOEEParameter", "ResourceId", "ONEPROD.CORE_Resource");
            DropIndex("ONEPROD.RTV_RTVOEEReportProductionDataParameter", new[] { "RTVParameterId" });
            DropIndex("ONEPROD.RTV_RTVOEEParameter", new[] { "ResourceId" });
            DropColumn("ONEPROD.RTV_RTVOEEReportProductionData", "PiecesPerPallet");
            DropColumn("ONEPROD.OEE_Reason", "ColorGroup");
            DropColumn("ONEPROD.CORE_Resource", "Breaks");
            DropTable("ONEPROD.RTV_RTVOEEReportProductionDataParameter");
            DropTable("ONEPROD.RTV_RTVOEEParameter");
            CreateIndex("ONEPROD.RTV_RTVOEEReportProductionData", "DetailId");
            AddForeignKey("ONEPROD.RTV_RTVOEEReportProductionData", "DetailId", "ONEPROD.OEE_OEEReportProductionDataDetails", "Id");
        }
    }
}

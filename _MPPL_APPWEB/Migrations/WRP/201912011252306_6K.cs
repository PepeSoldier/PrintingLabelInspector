namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6K : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.RTV_RTVOEEReportProductionDataParameter", "RTVParameterId", "ONEPROD.RTV_RTVOEEParameter");
            DropForeignKey("ONEPROD.RTV_RTVOEEParameter", "ResourceId", "ONEPROD.CORE_Resource");
            DropIndex("ONEPROD.RTV_RTVOEEReportProductionDataParameter", new[] { "RTVParameterId" });
            DropIndex("ONEPROD.RTV_RTVOEEParameter", new[] { "ResourceId" });
            DropTable("ONEPROD.RTV_RTVOEEReportProductionDataParameter");
            DropTable("ONEPROD.RTV_RTVOEEParameter");
        }
    }
}

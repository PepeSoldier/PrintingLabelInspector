namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ONEPROD.OEE_ReasonType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        EntryType = c.Int(nullable: false),
                        GenerateCharts = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        Color = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.OEE_MachineTarget",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourceId = c.Int(nullable: false),
                        ReasonTypeId = c.Int(nullable: false),
                        Target = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.ResourceId)
                .Index(t => t.ResourceId);
            
            AddColumn("ONEPROD.OEE_Reason", "ReasonTypeId", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_OEEReportProductionData", "ReasonTypeId", c => c.Int());
            AddColumn("ONEPROD.MES_ProductionLog", "ReasonTypeId", c => c.Int());
            AddColumn("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonTypeId", c => c.Int());
            CreateIndex("ONEPROD.OEE_Reason", "ReasonTypeId");
            CreateIndex("ONEPROD.OEE_OEEReportProductionData", "ReasonTypeId");
            CreateIndex("ONEPROD.MES_ProductionLog", "ReasonTypeId");
            CreateIndex("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonTypeId");

            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('TimeAvailable', 0, 0, 0, '#6c6c6d', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('Production', 10, 0, 10, '#14bd2c', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('ScrapMaterial', 11, 0, 11, '#ff0000', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('ScrapProcess', 12, 0, 12, '#ff0000', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('ScrapProcessScratch', 12, 0, 13, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('ScrapProcessDent', 12, 0, 14, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('ScrapProcessCrack', 12, 0, 15, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('ScrapProcessFold', 12, 0, 16, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('ScrapLabel', 19, 0, 19, NULL, 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('StopPlanned', 20, 1, 20, '#0070ffa1', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('StopPlannedChangeOver', 20, 0, 21, '#ae59d2', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('StopUnplanned', 30, 1, 33, '#ff0000a1', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('StopUnplannedBreakdown', 30, 0, 30, '#98012f', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('StopUnplannedPreformance', 30, 0, 32, '#95b5b5a1', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('StopUnplannedChangeOver', 30, 0, 31, '#ae59d2', 0)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");
            Sql("INSERT INTO ONEPROD.OEE_ReasonType(Name, EntryType, GenerateCharts, SortOrder, Color, Deleted) VALUES ('unused', -1, 0, 0, NULL, 1)");

            Sql("UPDATE ONEPROD.OEE_Reason SET ReasonTypeId = EntryType WHERE EntryType > 0");
            Sql("UPDATE ONEPROD.OEE_Reason SET ReasonTypeId = 1 WHERE EntryType = 0");
            Sql("UPDATE ONEPROD.OEE_Reason SET ReasonTypeId = NULL WHERE EntryType < 0");

            Sql("UPDATE ONEPROD.OEE_OEEReportProductionData SET ReasonTypeId = EntryType WHERE EntryType > 0");
            Sql("UPDATE ONEPROD.OEE_OEEReportProductionData SET ReasonTypeId = 1 WHERE EntryType = 0");
            Sql("UPDATE ONEPROD.OEE_OEEReportProductionData SET ReasonTypeId = NULL WHERE EntryType < 0");

            Sql("UPDATE ONEPROD.MES_ProductionLog SET ReasonTypeId = EntryType WHERE EntryType > 0");
            Sql("UPDATE ONEPROD.MES_ProductionLog SET ReasonTypeId = 1 WHERE EntryType = 0");
            Sql("UPDATE ONEPROD.MES_ProductionLog SET ReasonTypeId = NULL WHERE EntryType < 0");

            Sql("UPDATE ONEPROD.RTV_RTVOEEReportProductionData SET ReasonTypeId = EntryType WHERE EntryType > 0");
            Sql("UPDATE ONEPROD.RTV_RTVOEEReportProductionData SET ReasonTypeId = 1 WHERE EntryType = 0");
            Sql("UPDATE ONEPROD.RTV_RTVOEEReportProductionData SET ReasonTypeId = NULL WHERE EntryType < 0");

            Sql("INSERT INTO [ONEPROD].[OEE_MachineTarget] ([ResourceId],[ReasonTypeId],[Target],[Deleted]) SELECT Id as ResourceId, 20 as ReasonTypeId, TargetInSec_StopPlanned as [Target], 0 as [Deleted] FROM [ONEPROD].[CORE_Resource] r");
            Sql("INSERT INTO [ONEPROD].[OEE_MachineTarget] ([ResourceId],[ReasonTypeId],[Target],[Deleted]) SELECT Id as ResourceId, 33 as ReasonTypeId, TargetInSec_StopPlannedChangeOver as [Target], 0 as [Deleted] FROM [ONEPROD].[CORE_Resource] r");
            Sql("INSERT INTO [ONEPROD].[OEE_MachineTarget] ([ResourceId],[ReasonTypeId],[Target],[Deleted]) SELECT Id as ResourceId, 30 as ReasonTypeId, TargetInSec_StopUnplanned as [Target], 0 as [Deleted] FROM [ONEPROD].[CORE_Resource] r");
            Sql("INSERT INTO [ONEPROD].[OEE_MachineTarget] ([ResourceId],[ReasonTypeId],[Target],[Deleted]) SELECT Id as ResourceId, 31 as ReasonTypeId, TargetInSec_StopUnplannedBreakdown as [Target], 0 as [Deleted] FROM [ONEPROD].[CORE_Resource] r");
            Sql("INSERT INTO [ONEPROD].[OEE_MachineTarget] ([ResourceId],[ReasonTypeId],[Target],[Deleted]) SELECT Id as ResourceId, 32 as ReasonTypeId, TargetInSec_StopUnplannedPreformance as [Target], 0 as [Deleted] FROM [ONEPROD].[CORE_Resource] r");
            Sql("INSERT INTO [ONEPROD].[OEE_MachineTarget] ([ResourceId],[ReasonTypeId],[Target],[Deleted]) SELECT Id as ResourceId, 33 as ReasonTypeId, TargetInSec_StopUnplannedChangeOver as [Target], 0 as [Deleted] FROM [ONEPROD].[CORE_Resource] r");


            AddForeignKey("ONEPROD.OEE_Reason", "ReasonTypeId", "ONEPROD.OEE_ReasonType", "Id");
            AddForeignKey("ONEPROD.OEE_OEEReportProductionData", "ReasonTypeId", "ONEPROD.OEE_ReasonType", "Id");
            AddForeignKey("ONEPROD.MES_ProductionLog", "ReasonTypeId", "ONEPROD.OEE_ReasonType", "Id");
            AddForeignKey("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonTypeId", "ONEPROD.OEE_ReasonType", "Id");
            DropColumn("ONEPROD.CORE_Resource", "TargetInSec_StopPlanned");
            DropColumn("ONEPROD.CORE_Resource", "TargetInSec_StopPlannedChangeOver");
            DropColumn("ONEPROD.CORE_Resource", "TargetInSec_StopUnplanned");
            DropColumn("ONEPROD.CORE_Resource", "TargetInSec_StopUnplannedBreakdown");
            DropColumn("ONEPROD.CORE_Resource", "TargetInSec_StopUnplannedPreformance");
            DropColumn("ONEPROD.CORE_Resource", "TargetInSec_StopUnplannedChangeOver");
            DropColumn("ONEPROD.OEE_Reason", "EntryType");
            DropColumn("ONEPROD.OEE_OEEReportProductionData", "EntryType");
            DropColumn("ONEPROD.MES_ProductionLog", "EntryType");
            DropColumn("ONEPROD.RTV_RTVOEEReportProductionData", "EntryType");
        }
        
        public override void Down()
        {
            AddColumn("ONEPROD.RTV_RTVOEEReportProductionData", "EntryType", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_ProductionLog", "EntryType", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_OEEReportProductionData", "EntryType", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_Reason", "EntryType", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Resource", "TargetInSec_StopUnplannedChangeOver", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Resource", "TargetInSec_StopUnplannedPreformance", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Resource", "TargetInSec_StopUnplannedBreakdown", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Resource", "TargetInSec_StopUnplanned", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Resource", "TargetInSec_StopPlannedChangeOver", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Resource", "TargetInSec_StopPlanned", c => c.Int(nullable: false));
            DropForeignKey("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonTypeId", "ONEPROD.OEE_ReasonType");
            DropForeignKey("ONEPROD.MES_ProductionLog", "ReasonTypeId", "ONEPROD.OEE_ReasonType");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ReasonTypeId", "ONEPROD.OEE_ReasonType");
            DropForeignKey("ONEPROD.OEE_MachineTarget", "ResourceId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.OEE_Reason", "ReasonTypeId", "ONEPROD.OEE_ReasonType");
            DropIndex("ONEPROD.RTV_RTVOEEReportProductionData", new[] { "ReasonTypeId" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "ReasonTypeId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ReasonTypeId" });
            DropIndex("ONEPROD.OEE_MachineTarget", new[] { "ResourceId" });
            DropIndex("ONEPROD.OEE_Reason", new[] { "ReasonTypeId" });
            DropColumn("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonTypeId");
            DropColumn("ONEPROD.MES_ProductionLog", "ReasonTypeId");
            DropColumn("ONEPROD.OEE_OEEReportProductionData", "ReasonTypeId");
            DropColumn("ONEPROD.OEE_Reason", "ReasonTypeId");
            DropTable("ONEPROD.OEE_MachineTarget");
            DropTable("ONEPROD.OEE_ReasonType");
        }
    }
}

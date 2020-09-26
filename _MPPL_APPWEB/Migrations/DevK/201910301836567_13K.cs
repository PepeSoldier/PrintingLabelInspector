namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "ItemId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "WorkstationId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "WorkOrderId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "TransporterId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PackageInstanceId" });
            CreateTable(
                "ONEPROD.OEE_ReasonType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        NameEnglish = c.String(maxLength: 100),
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
            
            CreateTable(
                "iLOGIS.CONFIG_WarehouseLocationSort",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegalNumber = c.String(),
                        SortOrder = c.Int(nullable: false),
                        SortColumnAscending = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "RegalNumber", c => c.String());
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "ColumnNumber", c => c.Int(nullable: false));
            AddColumn("CORE.ChangeLog", "ObjectDescription", c => c.String(maxLength: 100));
            AddColumn("CORE.ChangeLog", "ParentObjectName", c => c.String(maxLength: 70));
            AddColumn("CORE.ChangeLog", "ParentObjectDescription", c => c.String(maxLength: 100));
            AddColumn("iLOGIS.WMS_DeliveryListItem", "BomQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("iLOGIS.CONFIG_Transporter", "LoopQty", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_Reason", "ReasonTypeId", c => c.Int(nullable: false));
            AddColumn("ONEPROD.OEE_OEEReportProductionData", "ReasonTypeId", c => c.Int());
            AddColumn("iLOGIS.WMS_PickingListItem", "BomQty", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_ProductionLog", "ReasonTypeId", c => c.Int());
            AddColumn("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonTypeId", c => c.Int());
            AlterColumn("iLOGIS.WMS_PickingListItem", "PackageInstanceId", c => c.Int());
            CreateIndex("iLOGIS.WMS_DeliveryListItem", new[] { "TransporterId", "WorkOrderId", "WorkstationId", "ItemId" }, unique: true, name: "IX_ItemWorkstationWorkorderTransporter");
            CreateIndex("ONEPROD.OEE_Reason", "ReasonTypeId");
            CreateIndex("ONEPROD.OEE_OEEReportProductionData", "ReasonTypeId");
            CreateIndex("iLOGIS.WMS_PickingListItem", "PackageInstanceId");
            CreateIndex("ONEPROD.MES_ProductionLog", "ReasonTypeId");
            CreateIndex("ONEPROD.MES_Workplace", "ComputerHostName", unique: true);
            CreateIndex("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonTypeId");
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
            DropIndex("ONEPROD.MES_Workplace", new[] { "ComputerHostName" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "ReasonTypeId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PackageInstanceId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ReasonTypeId" });
            DropIndex("ONEPROD.OEE_MachineTarget", new[] { "ResourceId" });
            DropIndex("ONEPROD.OEE_Reason", new[] { "ReasonTypeId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", "IX_ItemWorkstationWorkorderTransporter");
            AlterColumn("iLOGIS.WMS_PickingListItem", "PackageInstanceId", c => c.Int(nullable: false));
            DropColumn("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonTypeId");
            DropColumn("ONEPROD.MES_ProductionLog", "ReasonTypeId");
            DropColumn("iLOGIS.WMS_PickingListItem", "BomQty");
            DropColumn("ONEPROD.OEE_OEEReportProductionData", "ReasonTypeId");
            DropColumn("ONEPROD.OEE_Reason", "ReasonTypeId");
            DropColumn("iLOGIS.CONFIG_Transporter", "LoopQty");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "BomQty");
            DropColumn("CORE.ChangeLog", "ParentObjectDescription");
            DropColumn("CORE.ChangeLog", "ParentObjectName");
            DropColumn("CORE.ChangeLog", "ObjectDescription");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "ColumnNumber");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "RegalNumber");
            DropTable("iLOGIS.CONFIG_WarehouseLocationSort");
            DropTable("ONEPROD.OEE_MachineTarget");
            DropTable("ONEPROD.OEE_ReasonType");
            CreateIndex("iLOGIS.WMS_PickingListItem", "PackageInstanceId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "TransporterId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "WorkOrderId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "WorkstationId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "ItemId");
        }
    }
}

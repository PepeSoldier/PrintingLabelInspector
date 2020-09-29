namespace _MPPL_WEB_START.Migrations.PackingLabel
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3P : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ONEPROD.CORE_ClientOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Resource = c.String(),
                        ClientId = c.Int(),
                        ClientItemCode = c.String(maxLength: 50),
                        ClientItemName = c.String(maxLength: 250),
                        ItemCode = c.String(maxLength: 50),
                        ItemName = c.String(maxLength: 250),
                        OrderNo = c.String(maxLength: 50),
                        Qty_Total = c.Int(nullable: false),
                        Qty_Produced = c.Int(nullable: false),
                        UnitOfMeasure = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        RefOrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Contractor", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "ONEPROD.CORE_CycleTime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(nullable: false),
                        ItemGroupId = c.Int(nullable: false),
                        Preferred = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CycleTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProgramNumber = c.Int(nullable: false),
                        ProgramName = c.String(maxLength: 50),
                        PiecesPerPallet = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemGroupId)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.MachineId)
                .Index(t => t.MachineId)
                .Index(t => t.ItemGroupId);
            
            CreateTable(
                "ONEPROD.WMS_ItemInventory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        StockCalculated = c.Int(nullable: false),
                        Stock = c.Int(nullable: false),
                        ScrapQty = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ReportDate = c.DateTime(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        IsScrap = c.Boolean(nullable: false),
                        isScrapApplied = c.Boolean(nullable: false),
                        isStockApplied = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "ONEPROD.QUALITY_ItemMeasurement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemOPId = c.Int(nullable: false),
                        SerialNumber = c.String(),
                        Counter = c.Int(nullable: false),
                        Result0 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result3 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result4 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result5 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result6 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result7 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result8 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result9 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemOPId)
                .Index(t => t.ItemOPId);
            
            CreateTable(
                "ONEPROD.QUALITY_ItemParameter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemOPId = c.Int(nullable: false),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Depth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Length_Tolerance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width_Tolerance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Depth_Tolerance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(),
                        ProgramNumber = c.Int(nullable: false),
                        ProgramName = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemOPId)
                .Index(t => t.ItemOPId);
            
            CreateTable(
                "ONEPROD.CORE_Param",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.CORE_Workorder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueNumber = c.String(maxLength: 100),
                        ParentWorkorderId = c.Int(),
                        ClientOrderId = c.Int(),
                        ItemId = c.Int(),
                        ResourceId = c.Int(),
                        ToolId = c.Int(),
                        LV = c.Int(nullable: false),
                        ReleaseDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Qty_Total = c.Int(nullable: false),
                        Qty_Produced = c.Int(nullable: false),
                        Qty_Used = c.Int(nullable: false),
                        Qty_Scrap = c.Int(nullable: false),
                        Qty_ControlLabel = c.Int(nullable: false),
                        ProcessingTime = c.Int(nullable: false),
                        OrderSeq = c.Int(nullable: false),
                        BatchNumber = c.Int(nullable: false),
                        Index = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        Param1 = c.Int(nullable: false),
                        Param2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_ClientOrder", t => t.ClientOrderId)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemId)
                .ForeignKey("ONEPROD.CORE_Workorder", t => t.ParentWorkorderId)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.ResourceId)
                .ForeignKey("ONEPROD.APS_Tool", t => t.ToolId)
                .Index(t => t.ParentWorkorderId)
                .Index(t => t.ClientOrderId)
                .Index(t => t.ItemId)
                .Index(t => t.ResourceId)
                .Index(t => t.ToolId);
            
            CreateTable(
                "ONEPROD.APS_Tool",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        ToolGroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.APS_ToolGroup", t => t.ToolGroupId)
                .Index(t => t.ToolGroupId);
            
            CreateTable(
                "ONEPROD.APS_ToolGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.CORE_Item",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MinBatch = c.Int(nullable: false),
                        WorkOrderGenerator = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "ONEPROD.CORE_Resource",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        StageNo = c.Int(nullable: false),
                        IsBottleneck = c.Boolean(nullable: false),
                        SafetyTime = c.Int(nullable: false),
                        ShowBatches = c.Boolean(nullable: false),
                        ProdStartDay = c.Int(nullable: false),
                        IsOEE = c.Boolean(nullable: false),
                        TargetOee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetAvailability = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetPerformance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetQuality = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ToolRequired = c.Boolean(nullable: false),
                        Breaks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.Id)
                .Index(t => t.Id);
            
            AddForeignKey("_MPPL.MASTERDATA_Resource", "ResourceGroupId", "ONEPROD.CORE_Resource", "Id");
            AddForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "ONEPROD.CORE_Item", "Id");
            AddForeignKey("_MPPL.MASTERDATA_Item", "ResourceGroupId", "ONEPROD.CORE_Resource", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.CORE_Resource", "Id", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("ONEPROD.CORE_Item", "Id", "_MPPL.MASTERDATA_Item");
            DropForeignKey("ONEPROD.CORE_Workorder", "ToolId", "ONEPROD.APS_Tool");
            DropForeignKey("ONEPROD.APS_Tool", "ToolGroupId", "ONEPROD.APS_ToolGroup");
            DropForeignKey("ONEPROD.CORE_Workorder", "ResourceId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("ONEPROD.CORE_Workorder", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.CORE_Workorder", "ClientOrderId", "ONEPROD.CORE_ClientOrder");
            DropForeignKey("ONEPROD.QUALITY_ItemParameter", "ItemOPId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.QUALITY_ItemMeasurement", "ItemOPId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.WMS_ItemInventory", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.CORE_CycleTime", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.CORE_CycleTime", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.CORE_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Contractor");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ResourceGroupId", "ONEPROD.CORE_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ResourceGroupId", "ONEPROD.CORE_Resource");
            DropIndex("ONEPROD.CORE_Resource", new[] { "Id" });
            DropIndex("ONEPROD.CORE_Item", new[] { "Id" });
            DropIndex("ONEPROD.APS_Tool", new[] { "ToolGroupId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ToolId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ResourceId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ItemId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ClientOrderId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ParentWorkorderId" });
            DropIndex("ONEPROD.QUALITY_ItemParameter", new[] { "ItemOPId" });
            DropIndex("ONEPROD.QUALITY_ItemMeasurement", new[] { "ItemOPId" });
            DropIndex("ONEPROD.WMS_ItemInventory", new[] { "ItemId" });
            DropIndex("ONEPROD.CORE_CycleTime", new[] { "ItemGroupId" });
            DropIndex("ONEPROD.CORE_CycleTime", new[] { "MachineId" });
            DropIndex("ONEPROD.CORE_ClientOrder", new[] { "ClientId" });
            DropTable("ONEPROD.CORE_Resource");
            DropTable("ONEPROD.CORE_Item");
            DropTable("ONEPROD.APS_ToolGroup");
            DropTable("ONEPROD.APS_Tool");
            DropTable("ONEPROD.CORE_Workorder");
            DropTable("ONEPROD.CORE_Param");
            DropTable("ONEPROD.QUALITY_ItemParameter");
            DropTable("ONEPROD.QUALITY_ItemMeasurement");
            DropTable("ONEPROD.WMS_ItemInventory");
            DropTable("ONEPROD.CORE_CycleTime");
            DropTable("ONEPROD.CORE_ClientOrder");
        }
    }
}

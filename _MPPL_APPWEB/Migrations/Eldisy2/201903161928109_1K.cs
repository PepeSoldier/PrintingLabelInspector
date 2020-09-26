namespace _MPPL_WEB_START.Migrations.Eldisy2
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "_MPPL.MASTERDATA_Anc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 25),
                        Name = c.String(nullable: false, maxLength: 100),
                        Name2 = c.String(maxLength: 100),
                        Inactive = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        New = c.Boolean(nullable: false),
                        IsCommon = c.Boolean(nullable: false),
                        Lenght = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_Area",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.BASE_Attachment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        SubDirectory = c.String(maxLength: 100),
                        FileNamePrefix = c.String(maxLength: 50),
                        FileNameSuffix = c.String(maxLength: 50),
                        Extension = c.String(maxLength: 10),
                        ParentObjectId = c.Int(nullable: false),
                        ParentType = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.PREPROD_BOM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChildItemId = c.Int(),
                        ParentItemId = c.Int(),
                        Level = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Item", t => t.ChildItemId)
                .ForeignKey("ONEPROD.PREPROD_Item", t => t.ParentItemId)
                .Index(t => t.ChildItemId)
                .Index(t => t.ParentItemId);
            
            CreateTable(
                "ONEPROD.PREPROD_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 50),
                        OriginalName = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Comment = c.String(maxLength: 50),
                        Color1 = c.String(maxLength: 25),
                        Color2 = c.String(maxLength: 25),
                        Specific1 = c.String(maxLength: 25),
                        Specific2 = c.String(maxLength: 25),
                        Specific3 = c.String(maxLength: 25),
                        Specific4 = c.String(maxLength: 25),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        MinBatch = c.Int(nullable: false),
                        ItemGroupId = c.Int(),
                        GroupId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Item", t => t.GroupId)
                .ForeignKey("ONEPROD.PREPROD_ItemGroup", t => t.ItemGroupId)
                .Index(t => t.ItemGroupId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "ONEPROD.PREPROD_ItemGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        MinBatch = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ProcessId = c.Int(),
                        ResourceGroupId = c.Int(),
                        Color = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Process", t => t.ProcessId)
                .ForeignKey("ONEPROD.PREPROD_Resource", t => t.ResourceGroupId)
                .Index(t => t.ProcessId)
                .Index(t => t.ResourceGroupId);
            
            CreateTable(
                "ONEPROD.PREPROD_Process",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        ParentId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.PREPROD_Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        Type = c.Int(nullable: false),
                        ResourceGroupId = c.Int(),
                        StageNo = c.Int(nullable: false),
                        IsBottleneck = c.Boolean(nullable: false),
                        SafetyTime = c.Int(nullable: false),
                        ShowBatches = c.Boolean(nullable: false),
                        ProdStartDay = c.Int(nullable: false),
                        Color = c.String(maxLength: 35),
                        FlowTime = c.Int(nullable: false),
                        IsOEE = c.Boolean(nullable: false),
                        TargetOee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetAvailability = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetPerformance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetQuality = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetInSec_StopPlanned = c.Int(nullable: false),
                        TargetInSec_StopPlannedChangeOver = c.Int(nullable: false),
                        TargetInSec_StopUnplanned = c.Int(nullable: false),
                        TargetInSec_StopUnplannedBreakdown = c.Int(nullable: false),
                        TargetInSec_StopUnplannedPreformance = c.Int(nullable: false),
                        TargetInSec_StopUnplannedChangeOver = c.Int(nullable: false),
                        ToolRequired = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Resource", t => t.ResourceGroupId)
                .Index(t => t.ResourceGroupId);
            
            CreateTable(
                "_MPPL.MASTERDATA_BomWorkorder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNo = c.String(maxLength: 20),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                        BC1 = c.String(maxLength: 3),
                        LV = c.Int(nullable: false),
                        QtyUsed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BC2 = c.String(maxLength: 3),
                        DEF = c.String(maxLength: 3),
                        Prefix = c.String(maxLength: 12),
                        Suffix = c.String(maxLength: 12),
                        IDCO = c.String(maxLength: 12),
                        DirPar = c.String(maxLength: 20),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Anc", t => t.ChildId)
                .ForeignKey("_MPPL.MASTERDATA_Pnc", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Pnc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PFEP.Calculation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackingInstructionId = c.Int(nullable: false),
                        ClientProfileCode = c.String(),
                        ProfileCode = c.String(),
                        ProfileName = c.String(),
                        PackingInstructionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculatedInstructionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetInstructionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PFEP.PackingInstruction", t => t.PackingInstructionId)
                .Index(t => t.PackingInstructionId);
            
            CreateTable(
                "PFEP.PackingInstruction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstructionNumber = c.Int(nullable: false),
                        InstructionVersion = c.Int(nullable: false),
                        Description = c.String(),
                        AmountOnLayer = c.Int(nullable: false),
                        AmountOnBox = c.Int(nullable: false),
                        AmountOnPallet = c.Int(nullable: false),
                        UnitOfMeasure = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Examined = c.Boolean(nullable: false),
                        ExaminedDate = c.DateTime(nullable: false),
                        Confirmed = c.Boolean(nullable: false),
                        ConfirmedDate = c.DateTime(nullable: false),
                        NumberOfCorrections = c.Int(),
                        CreatorId = c.String(maxLength: 128),
                        ExaminerId = c.String(maxLength: 128),
                        ConfirmId = c.String(maxLength: 128),
                        AreaId = c.Int(nullable: false),
                        ExamineComment = c.String(maxLength: 200),
                        ConfirmComment = c.String(maxLength: 200),
                        ProfileCode = c.String(nullable: false, maxLength: 100),
                        ProfileName = c.String(nullable: false, maxLength: 100),
                        ClientName = c.String(),
                        ClientProfileCode = c.String(),
                        CurrentPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculationPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                        TempPhoto1 = c.String(maxLength: 64),
                        TempPhoto2 = c.String(maxLength: 64),
                        TempPhoto3 = c.String(maxLength: 64),
                        TempPhoto4 = c.String(maxLength: 64),
                        TmpConfirmName = c.String(),
                        TmpExaminerName = c.String(),
                        TmpCreatorName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.ConfirmId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.CreatorId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.ExaminerId)
                .Index(t => t.CreatorId)
                .Index(t => t.ExaminerId)
                .Index(t => t.ConfirmId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "_MPPL.IDENTITY_User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DepartmentId = c.Int(),
                        SuperVisorUserId = c.String(maxLength: 128),
                        SupervisorUserName = c.String(),
                        Title = c.String(maxLength: 50),
                        Factory = c.String(maxLength: 25),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Department", t => t.DepartmentId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.SuperVisorUserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.SuperVisorUserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "_MPPL.IDENTITY_UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.IDENTITY_UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_MPPL.IDENTITY_UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("_MPPL.IDENTITY_Role", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "_MPPL.ChangeLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ObjectName = c.String(maxLength: 70),
                        FieldName = c.String(maxLength: 70),
                        FieldDisplayName = c.String(maxLength: 140),
                        NewValue = c.String(maxLength: 255),
                        OldValue = c.String(maxLength: 255),
                        ObjectId = c.Int(nullable: false),
                        ParentObjectId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "ONEPROD.PREPROD_ClientOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Resource = c.String(),
                        ClientItemCode = c.String(maxLength: 50),
                        ClientItemName = c.String(maxLength: 250),
                        Orderno = c.String(maxLength: 50),
                        Qty_Total = c.Int(nullable: false),
                        Qty_Produced = c.Int(nullable: false),
                        UnitOfMeasure = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        RefOrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.MASTERDATA_Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.Correction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackingInstructionId = c.Int(nullable: false),
                        ApplicationStart = c.DateTime(nullable: false),
                        Applicationfinished = c.DateTime(),
                        ApplicantId = c.String(maxLength: 128),
                        CorrectionClosed = c.Boolean(nullable: false),
                        CorrectionOpen = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CorrectionText = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.ApplicantId)
                .ForeignKey("PFEP.PackingInstruction", t => t.PackingInstructionId)
                .Index(t => t.PackingInstructionId)
                .Index(t => t.ApplicantId);
            
            CreateTable(
                "ONEPROD.PREPROD_CycleTime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(nullable: false),
                        ItemGroupId = c.Int(nullable: false),
                        Preferred = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CycleTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProgramNumber = c.Int(nullable: false),
                        PiecesPerPallet = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_ItemGroup", t => t.ItemGroupId)
                .ForeignKey("ONEPROD.PREPROD_Resource", t => t.MachineId)
                .Index(t => t.MachineId)
                .Index(t => t.ItemGroupId);
            
            CreateTable(
                "ONEPROD.PREPROD_ItemInventory",
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
                .ForeignKey("ONEPROD.PREPROD_Item", t => t.ItemId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Line",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        AreaId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "ONEPROD.OEE_MachineReason",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(nullable: false),
                        ReasonId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Resource", t => t.MachineId)
                .ForeignKey("ONEPROD.OEE_Reason", t => t.ReasonId)
                .Index(t => t.MachineId)
                .Index(t => t.ReasonId);
            
            CreateTable(
                "ONEPROD.OEE_Reason",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        NameEnglish = c.String(maxLength: 100),
                        Color = c.String(maxLength: 35),
                        EntryType = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.OEE_OEEReportEmployee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        EmployeeName = c.String(),
                        SkillsCount = c.Int(nullable: false),
                        enumOperatorType = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.OEE_OEEReport", t => t.ReportId)
                .Index(t => t.ReportId);
            
            CreateTable(
                "ONEPROD.OEE_OEEReport",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deleted = c.Boolean(nullable: false),
                        ReportDate = c.DateTime(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Shift = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        MachineId = c.Int(),
                        LabourBrigadeId = c.Int(),
                        IsDraft = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Resource", t => t.MachineId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MachineId);
            
            CreateTable(
                "ONEPROD.OEE_OEEReportProductionData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ProductionDate = c.DateTime(),
                        ItemId = c.Int(),
                        ProdQty = c.Int(nullable: false),
                        CycleTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UsedTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        entryType = c.Int(nullable: false),
                        ReasonId = c.Int(),
                        UserId = c.String(maxLength: 128),
                        TimeStamp = c.DateTime(nullable: false),
                        DetailId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Item", t => t.ItemId)
                .ForeignKey("ONEPROD.OEE_Reason", t => t.ReasonId)
                .ForeignKey("ONEPROD.OEE_OEEReport", t => t.ReportId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.ReportId)
                .Index(t => t.ItemId)
                .Index(t => t.ReasonId)
                .Index(t => t.UserId);
            
            CreateTable(
                "PFEP.DEF_Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PFEP.PackingInstruction_Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        PriceForHundredPackages = c.String(),
                        PackageId = c.Int(nullable: false),
                        PackingInstructionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PFEP.DEF_Package", t => t.PackageId)
                .ForeignKey("PFEP.PackingInstruction", t => t.PackingInstructionId)
                .Index(t => t.PackageId)
                .Index(t => t.PackingInstructionId);
            
            CreateTable(
                "ONEPROD.PREPROD_Param",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.PREPROD_ProductionLogRawMaterial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductionLogId = c.Int(nullable: false),
                        UsedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PartCode = c.String(maxLength: 50),
                        BatchSerialNo = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_ProductionLog", t => t.ProductionLogId)
                .Index(t => t.ProductionLogId);
            
            CreateTable(
                "ONEPROD.PREPROD_ProductionLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        OEEReportProductionDataId = c.Int(),
                        WorkplaceId = c.Int(nullable: false),
                        UserName = c.String(maxLength: 40),
                        WorkOrderNumber = c.String(maxLength: 20),
                        PartCode = c.String(maxLength: 50),
                        InternalWorkOrderNumber = c.String(maxLength: 100),
                        BatchSerialNo = c.String(),
                        TaskTotalQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeclaredQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostCenter = c.String(maxLength: 15),
                        TransferNumber = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.OEE_OEEReportProductionData", t => t.OEEReportProductionDataId)
                .ForeignKey("ONEPROD.PREPROD_Workplace", t => t.WorkplaceId)
                .Index(t => t.OEEReportProductionDataId)
                .Index(t => t.WorkplaceId);
            
            CreateTable(
                "ONEPROD.PREPROD_Workplace",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        MachineId = c.Int(),
                        SelectedTaskId = c.Int(),
                        ComputerHostName = c.String(maxLength: 25),
                        PrinterIPv4 = c.String(maxLength: 20),
                        LoggedUserName = c.String(maxLength: 40),
                        LabelANC = c.String(maxLength: 50),
                        LabelName = c.String(maxLength: 15),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Resource", t => t.MachineId)
                .ForeignKey("ONEPROD.PREPROD_Task", t => t.SelectedTaskId)
                .Index(t => t.MachineId)
                .Index(t => t.SelectedTaskId);
            
            CreateTable(
                "ONEPROD.PREPROD_Task",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueTaskNumber = c.String(maxLength: 100),
                        OrderId = c.Int(),
                        PartId = c.Int(),
                        MachineId = c.Int(),
                        ToolId = c.Int(),
                        ReleaseDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Qty_Total = c.Int(nullable: false),
                        Qty_Produced = c.Int(nullable: false),
                        Qty_Used = c.Int(nullable: false),
                        ProcessingTime = c.Int(nullable: false),
                        OrderSeq = c.Int(nullable: false),
                        BatchNumber = c.Int(nullable: false),
                        LV = c.Int(nullable: false),
                        Index = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        Param1 = c.Int(nullable: false),
                        Param2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.PREPROD_Resource", t => t.MachineId)
                .ForeignKey("ONEPROD.PREPROD_ClientOrder", t => t.OrderId)
                .ForeignKey("ONEPROD.PREPROD_Item", t => t.PartId)
                .Index(t => t.OrderId)
                .Index(t => t.PartId)
                .Index(t => t.MachineId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        NameShort = c.String(nullable: false, maxLength: 25),
                        SortOrder1 = c.Int(nullable: false),
                        SortOrder2 = c.Int(nullable: false),
                        ParentId = c.Int(),
                        TypeId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.ParentId)
                .ForeignKey("_MPPL.MASTERDATA_Resource_Type", t => t.TypeId)
                .Index(t => t.ParentId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Resource_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Color = c.String(maxLength: 25),
                        Icon = c.String(maxLength: 100),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "_MPPL.IDENTITY_Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "_MPPL.MASTERDATA_Workstation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        SortOrder = c.Int(nullable: false),
                        LineId = c.Int(),
                        AreaId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.MASTERDATA_Line", t => t.LineId)
                .Index(t => t.LineId)
                .Index(t => t.AreaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "LineId", "_MPPL.MASTERDATA_Line");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "RoleId", "_MPPL.IDENTITY_Role");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "TypeId", "_MPPL.MASTERDATA_Resource_Type");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ParentId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("ONEPROD.PREPROD_ProductionLogRawMaterial", "ProductionLogId", "ONEPROD.PREPROD_ProductionLog");
            DropForeignKey("ONEPROD.PREPROD_ProductionLog", "WorkplaceId", "ONEPROD.PREPROD_Workplace");
            DropForeignKey("ONEPROD.PREPROD_Workplace", "SelectedTaskId", "ONEPROD.PREPROD_Task");
            DropForeignKey("ONEPROD.PREPROD_Task", "PartId", "ONEPROD.PREPROD_Item");
            DropForeignKey("ONEPROD.PREPROD_Task", "OrderId", "ONEPROD.PREPROD_ClientOrder");
            DropForeignKey("ONEPROD.PREPROD_Task", "MachineId", "ONEPROD.PREPROD_Resource");
            DropForeignKey("ONEPROD.PREPROD_Workplace", "MachineId", "ONEPROD.PREPROD_Resource");
            DropForeignKey("ONEPROD.PREPROD_ProductionLog", "OEEReportProductionDataId", "ONEPROD.OEE_OEEReportProductionData");
            DropForeignKey("PFEP.PackingInstruction_Package", "PackingInstructionId", "PFEP.PackingInstruction");
            DropForeignKey("PFEP.PackingInstruction_Package", "PackageId", "PFEP.DEF_Package");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ReportId", "ONEPROD.OEE_OEEReport");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ReasonId", "ONEPROD.OEE_Reason");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ItemId", "ONEPROD.PREPROD_Item");
            DropForeignKey("ONEPROD.OEE_OEEReportEmployee", "ReportId", "ONEPROD.OEE_OEEReport");
            DropForeignKey("ONEPROD.OEE_OEEReport", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.OEE_OEEReport", "MachineId", "ONEPROD.PREPROD_Resource");
            DropForeignKey("ONEPROD.OEE_MachineReason", "ReasonId", "ONEPROD.OEE_Reason");
            DropForeignKey("ONEPROD.OEE_MachineReason", "MachineId", "ONEPROD.PREPROD_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Line", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("ONEPROD.PREPROD_ItemInventory", "ItemId", "ONEPROD.PREPROD_Item");
            DropForeignKey("ONEPROD.PREPROD_CycleTime", "MachineId", "ONEPROD.PREPROD_Resource");
            DropForeignKey("ONEPROD.PREPROD_CycleTime", "ItemGroupId", "ONEPROD.PREPROD_ItemGroup");
            DropForeignKey("_MPPL.Correction", "PackingInstructionId", "PFEP.PackingInstruction");
            DropForeignKey("_MPPL.Correction", "ApplicantId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.ChangeLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("PFEP.Calculation", "PackingInstructionId", "PFEP.PackingInstruction");
            DropForeignKey("PFEP.PackingInstruction", "ExaminerId", "_MPPL.IDENTITY_User");
            DropForeignKey("PFEP.PackingInstruction", "CreatorId", "_MPPL.IDENTITY_User");
            DropForeignKey("PFEP.PackingInstruction", "ConfirmId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "SuperVisorUserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserLogin", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "DepartmentId", "_MPPL.MASTERDATA_Department");
            DropForeignKey("_MPPL.IDENTITY_UserClaim", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("PFEP.PackingInstruction", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("_MPPL.MASTERDATA_BomWorkorder", "ParentId", "_MPPL.MASTERDATA_Pnc");
            DropForeignKey("_MPPL.MASTERDATA_BomWorkorder", "ChildId", "_MPPL.MASTERDATA_Anc");
            DropForeignKey("ONEPROD.PREPROD_BOM", "ParentItemId", "ONEPROD.PREPROD_Item");
            DropForeignKey("ONEPROD.PREPROD_BOM", "ChildItemId", "ONEPROD.PREPROD_Item");
            DropForeignKey("ONEPROD.PREPROD_Item", "ItemGroupId", "ONEPROD.PREPROD_ItemGroup");
            DropForeignKey("ONEPROD.PREPROD_ItemGroup", "ResourceGroupId", "ONEPROD.PREPROD_Resource");
            DropForeignKey("ONEPROD.PREPROD_Resource", "ResourceGroupId", "ONEPROD.PREPROD_Resource");
            DropForeignKey("ONEPROD.PREPROD_ItemGroup", "ProcessId", "ONEPROD.PREPROD_Process");
            DropForeignKey("ONEPROD.PREPROD_Item", "GroupId", "ONEPROD.PREPROD_Item");
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "LineId" });
            DropIndex("_MPPL.IDENTITY_Role", "RoleNameIndex");
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "TypeId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ParentId" });
            DropIndex("ONEPROD.PREPROD_Task", new[] { "MachineId" });
            DropIndex("ONEPROD.PREPROD_Task", new[] { "PartId" });
            DropIndex("ONEPROD.PREPROD_Task", new[] { "OrderId" });
            DropIndex("ONEPROD.PREPROD_Workplace", new[] { "SelectedTaskId" });
            DropIndex("ONEPROD.PREPROD_Workplace", new[] { "MachineId" });
            DropIndex("ONEPROD.PREPROD_ProductionLog", new[] { "WorkplaceId" });
            DropIndex("ONEPROD.PREPROD_ProductionLog", new[] { "OEEReportProductionDataId" });
            DropIndex("ONEPROD.PREPROD_ProductionLogRawMaterial", new[] { "ProductionLogId" });
            DropIndex("PFEP.PackingInstruction_Package", new[] { "PackingInstructionId" });
            DropIndex("PFEP.PackingInstruction_Package", new[] { "PackageId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "UserId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ReasonId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ItemId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ReportId" });
            DropIndex("ONEPROD.OEE_OEEReport", new[] { "MachineId" });
            DropIndex("ONEPROD.OEE_OEEReport", new[] { "UserId" });
            DropIndex("ONEPROD.OEE_OEEReportEmployee", new[] { "ReportId" });
            DropIndex("ONEPROD.OEE_MachineReason", new[] { "ReasonId" });
            DropIndex("ONEPROD.OEE_MachineReason", new[] { "MachineId" });
            DropIndex("_MPPL.MASTERDATA_Line", new[] { "AreaId" });
            DropIndex("ONEPROD.PREPROD_ItemInventory", new[] { "ItemId" });
            DropIndex("ONEPROD.PREPROD_CycleTime", new[] { "ItemGroupId" });
            DropIndex("ONEPROD.PREPROD_CycleTime", new[] { "MachineId" });
            DropIndex("_MPPL.Correction", new[] { "ApplicantId" });
            DropIndex("_MPPL.Correction", new[] { "PackingInstructionId" });
            DropIndex("_MPPL.ChangeLog", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "RoleId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserLogin", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserClaim", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_User", "UserNameIndex");
            DropIndex("_MPPL.IDENTITY_User", new[] { "SuperVisorUserId" });
            DropIndex("_MPPL.IDENTITY_User", new[] { "DepartmentId" });
            DropIndex("PFEP.PackingInstruction", new[] { "AreaId" });
            DropIndex("PFEP.PackingInstruction", new[] { "ConfirmId" });
            DropIndex("PFEP.PackingInstruction", new[] { "ExaminerId" });
            DropIndex("PFEP.PackingInstruction", new[] { "CreatorId" });
            DropIndex("PFEP.Calculation", new[] { "PackingInstructionId" });
            DropIndex("_MPPL.MASTERDATA_BomWorkorder", new[] { "ChildId" });
            DropIndex("_MPPL.MASTERDATA_BomWorkorder", new[] { "ParentId" });
            DropIndex("ONEPROD.PREPROD_Resource", new[] { "ResourceGroupId" });
            DropIndex("ONEPROD.PREPROD_ItemGroup", new[] { "ResourceGroupId" });
            DropIndex("ONEPROD.PREPROD_ItemGroup", new[] { "ProcessId" });
            DropIndex("ONEPROD.PREPROD_Item", new[] { "GroupId" });
            DropIndex("ONEPROD.PREPROD_Item", new[] { "ItemGroupId" });
            DropIndex("ONEPROD.PREPROD_BOM", new[] { "ParentItemId" });
            DropIndex("ONEPROD.PREPROD_BOM", new[] { "ChildItemId" });
            DropTable("_MPPL.MASTERDATA_Workstation");
            DropTable("_MPPL.IDENTITY_Role");
            DropTable("_MPPL.MASTERDATA_Resource_Type");
            DropTable("_MPPL.MASTERDATA_Resource");
            DropTable("ONEPROD.PREPROD_Task");
            DropTable("ONEPROD.PREPROD_Workplace");
            DropTable("ONEPROD.PREPROD_ProductionLog");
            DropTable("ONEPROD.PREPROD_ProductionLogRawMaterial");
            DropTable("ONEPROD.PREPROD_Param");
            DropTable("PFEP.PackingInstruction_Package");
            DropTable("PFEP.DEF_Package");
            DropTable("ONEPROD.OEE_OEEReportProductionData");
            DropTable("ONEPROD.OEE_OEEReport");
            DropTable("ONEPROD.OEE_OEEReportEmployee");
            DropTable("ONEPROD.OEE_Reason");
            DropTable("ONEPROD.OEE_MachineReason");
            DropTable("_MPPL.MASTERDATA_Line");
            DropTable("ONEPROD.PREPROD_ItemInventory");
            DropTable("ONEPROD.PREPROD_CycleTime");
            DropTable("_MPPL.Correction");
            DropTable("_MPPL.MASTERDATA_Client");
            DropTable("ONEPROD.PREPROD_ClientOrder");
            DropTable("_MPPL.ChangeLog");
            DropTable("_MPPL.IDENTITY_UserRole");
            DropTable("_MPPL.IDENTITY_UserLogin");
            DropTable("_MPPL.MASTERDATA_Department");
            DropTable("_MPPL.IDENTITY_UserClaim");
            DropTable("_MPPL.IDENTITY_User");
            DropTable("PFEP.PackingInstruction");
            DropTable("PFEP.Calculation");
            DropTable("_MPPL.MASTERDATA_Pnc");
            DropTable("_MPPL.MASTERDATA_BomWorkorder");
            DropTable("ONEPROD.PREPROD_Resource");
            DropTable("ONEPROD.PREPROD_Process");
            DropTable("ONEPROD.PREPROD_ItemGroup");
            DropTable("ONEPROD.PREPROD_Item");
            DropTable("ONEPROD.PREPROD_BOM");
            DropTable("_MPPL.BASE_Attachment");
            DropTable("_MPPL.MASTERDATA_Area");
            DropTable("_MPPL.MASTERDATA_Anc");
        }
    }
}

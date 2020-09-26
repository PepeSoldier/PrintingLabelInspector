namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1K : DbMigration
    {
        public override void Up()
        {
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
                "ONEPROD.APS_Calendar",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(nullable: false),
                        DateClose = c.DateTime(nullable: false),
                        DateOpen = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.MachineId)
                .Index(t => t.MachineId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Type = c.Int(nullable: false),
                        ResourceGroupId = c.Int(),
                        AreaId = c.Int(),
                        Color = c.String(maxLength: 35),
                        FlowTime = c.Int(nullable: false),
                        OldId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.ResourceGroupId)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.ResourceGroupId)
                .Index(t => t.ResourceGroupId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "CORE.ChangeLog",
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
                "ONEPROD.APS_ChangeOver",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ToolModification = c.Int(nullable: false),
                        MachineToolChange = c.Int(nullable: false),
                        ToolChange = c.Int(nullable: false),
                        CatergoyChange = c.Int(nullable: false),
                        AncChange = c.Int(nullable: false),
                        AreaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .ForeignKey("_MPPL.MASTERDATA_Client", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Country = c.String(),
                        Language = c.String(),
                        NIP = c.String(),
                        ContactPersonName = c.String(),
                        ContactPhoneNumber = c.String(),
                        ContactEmail = c.String(),
                        ContactAdress = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        PiecesPerPallet = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemGroupId)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.MachineId)
                .Index(t => t.MachineId)
                .Index(t => t.ItemGroupId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        OriginalName = c.String(maxLength: 100),
                        Name = c.String(maxLength: 100),
                        Comment = c.String(maxLength: 50),
                        Color1 = c.String(maxLength: 25),
                        Color2 = c.String(maxLength: 25),
                        Specific1 = c.String(maxLength: 25),
                        Specific2 = c.String(maxLength: 25),
                        Specific3 = c.String(maxLength: 25),
                        Specific4 = c.String(maxLength: 25),
                        DEF = c.String(maxLength: 25),
                        BC = c.String(maxLength: 25),
                        PREFIX = c.String(maxLength: 25),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        ProcessId = c.Int(),
                        ItemGroupId = c.Int(),
                        ResourceGroupId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        Id_old = c.Int(),
                        Color = c.String(maxLength: 25),
                        New = c.Boolean(nullable: false),
                        Lenght = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        IsCommon = c.Boolean(nullable: false),
                        Old_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemGroupId)
                .ForeignKey("_MPPL.MASTERDATA_Process", t => t.ProcessId)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.ResourceGroupId)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemGroupId)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.ResourceGroupId)
                .Index(t => t.ProcessId)
                .Index(t => t.ItemGroupId)
                .Index(t => t.ResourceGroupId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Process",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        ParentId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.APS_ToolItemGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemGroupId = c.Int(nullable: false),
                        ToolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemGroupId)
                .ForeignKey("ONEPROD.APS_Tool", t => t.ToolId)
                .Index(t => t.ItemGroupId)
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
                "_MPPL.MASTERDATA_LabourBrigade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .ForeignKey("ONEPROD.CORE_Resource", t => t.MachineId)
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
                        TotalQtyCountedOnline = c.Int(nullable: false),
                        TotalQtyDeclaredByOperator = c.Int(nullable: false),
                        TotalStoppageTimeCountedOnline = c.Int(nullable: false),
                        TotalStoppageTimeDeclaredByOperator = c.Int(nullable: false),
                        IsDraft = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_LabourBrigade", t => t.LabourBrigadeId)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.MachineId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MachineId)
                .Index(t => t.LabourBrigadeId);
            
            CreateTable(
                "ONEPROD.OEE_OEEReportProductionData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        ProdQtyCountedOnline = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ProductionDate = c.DateTime(nullable: false),
                        ItemId = c.Int(),
                        ProdQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CycleTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UsedTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EntryType = c.Int(nullable: false),
                        ReasonId = c.Int(),
                        UserId = c.String(maxLength: 128),
                        TimeStamp = c.DateTime(nullable: false),
                        DetailId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.OEE_OEEReportProductionDataDetails", t => t.DetailId)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemId)
                .ForeignKey("ONEPROD.OEE_Reason", t => t.ReasonId)
                .ForeignKey("ONEPROD.OEE_OEEReport", t => t.ReportId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.ReportId)
                .Index(t => t.ItemId)
                .Index(t => t.ReasonId)
                .Index(t => t.UserId)
                .Index(t => t.DetailId);
            
            CreateTable(
                "ONEPROD.OEE_OEEReportProductionDataDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deleted = c.Boolean(nullable: false),
                        Comment = c.String(maxLength: 200),
                        ProductionCycleTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CoilId = c.Int(nullable: false),
                        FormWeightProcess = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FormWeightScrap = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaperWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TubeWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeOptInMin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeUrInMin = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "ONEPROD.MES_ProductionLogRawMaterial",
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
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ProductionLogId)
                .Index(t => t.ProductionLogId);
            
            CreateTable(
                "ONEPROD.MES_ProductionLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        OEEReportProductionDataId = c.Int(),
                        WorkplaceId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        EntryType = c.Int(nullable: false),
                        ReasonId = c.Int(),
                        ClientWorkOrderNumber = c.String(maxLength: 20),
                        InternalWorkOrderNumber = c.String(maxLength: 100),
                        WorkorderTotalQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeclaredQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SerialNo = c.String(maxLength: 25),
                        UserName = c.String(maxLength: 40),
                        CostCenter = c.String(maxLength: 15),
                        TransferNumber = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemId)
                .ForeignKey("ONEPROD.OEE_OEEReportProductionData", t => t.OEEReportProductionDataId)
                .ForeignKey("ONEPROD.OEE_Reason", t => t.ReasonId)
                .ForeignKey("ONEPROD.MES_Workplace", t => t.WorkplaceId)
                .Index(t => t.OEEReportProductionDataId)
                .Index(t => t.WorkplaceId)
                .Index(t => t.ItemId)
                .Index(t => t.ReasonId);
            
            CreateTable(
                "ONEPROD.MES_Workplace",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        MachineId = c.Int(nullable: false),
                        SelectedTaskId = c.Int(),
                        ComputerHostName = c.String(maxLength: 25),
                        PrinterIPv4 = c.String(maxLength: 20),
                        LoggedUserName = c.String(maxLength: 40),
                        LabelANC = c.String(maxLength: 50),
                        LabelName = c.String(maxLength: 15),
                        PrintLabel = c.Boolean(nullable: false),
                        SerialNumberType = c.Int(nullable: false),
                        PrinterType = c.Int(nullable: false),
                        LabelLayoutNo = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.MachineId)
                .ForeignKey("ONEPROD.CORE_Workorder", t => t.SelectedTaskId)
                .Index(t => t.MachineId)
                .Index(t => t.SelectedTaskId)
                .Index(t => t.ComputerHostName, unique: true);
            
            CreateTable(
                "ONEPROD.CORE_Workorder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueNumber = c.String(maxLength: 100),
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
                .ForeignKey("ONEPROD.CORE_Resource", t => t.ResourceId)
                .ForeignKey("ONEPROD.APS_Tool", t => t.ToolId)
                .Index(t => t.ClientOrderId)
                .Index(t => t.ItemId)
                .Index(t => t.ResourceId)
                .Index(t => t.ToolId);
            
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
                "ONEPROD.RTV_RTVOEEPLCData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deleted = c.Boolean(nullable: false),
                        PlcIP = c.String(maxLength: 20),
                        PlcStatus = c.Boolean(nullable: false),
                        MachineId = c.Int(nullable: false),
                        EntryDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        P1 = c.String(maxLength: 100),
                        P2 = c.String(maxLength: 100),
                        P3 = c.String(maxLength: 100),
                        P4 = c.String(maxLength: 100),
                        P5 = c.String(maxLength: 100),
                        P6 = c.String(maxLength: 100),
                        P7 = c.String(maxLength: 100),
                        P8 = c.String(maxLength: 100),
                        P9 = c.String(maxLength: 100),
                        P10 = c.String(maxLength: 100),
                        P11 = c.String(maxLength: 100),
                        P12 = c.String(maxLength: 100),
                        P13 = c.String(maxLength: 100),
                        P14 = c.String(maxLength: 100),
                        P15 = c.String(maxLength: 100),
                        P16 = c.String(maxLength: 100),
                        P17 = c.String(maxLength: 100),
                        P18 = c.String(maxLength: 100),
                        P19 = c.String(maxLength: 100),
                        P20 = c.String(maxLength: 100),
                        P21 = c.String(maxLength: 100),
                        P22 = c.String(maxLength: 100),
                        P23 = c.String(maxLength: 100),
                        P24 = c.String(maxLength: 100),
                        P25 = c.String(maxLength: 100),
                        P26 = c.String(maxLength: 100),
                        P27 = c.String(maxLength: 100),
                        P28 = c.String(maxLength: 100),
                        P29 = c.String(maxLength: 100),
                        P30 = c.String(maxLength: 100),
                        P31 = c.String(maxLength: 100),
                        P32 = c.String(maxLength: 100),
                        P33 = c.String(maxLength: 100),
                        P34 = c.String(maxLength: 100),
                        P35 = c.String(maxLength: 100),
                        P36 = c.String(maxLength: 100),
                        P37 = c.String(maxLength: 100),
                        P38 = c.String(maxLength: 100),
                        P39 = c.String(maxLength: 100),
                        P40 = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ONEPROD.RTV_RTVOEEReportProductionData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(nullable: false),
                        ProdQtyTotal = c.Int(nullable: false),
                        ProdQtyShift = c.Int(nullable: false),
                        ProdQtyCorrector = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ProductionDate = c.DateTime(nullable: false),
                        ItemId = c.Int(),
                        ProdQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CycleTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UsedTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EntryType = c.Int(nullable: false),
                        ReasonId = c.Int(),
                        UserId = c.String(maxLength: 128),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemId)
                .ForeignKey("ONEPROD.OEE_Reason", t => t.ReasonId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.MachineId)
                .Index(t => t.ItemId)
                .Index(t => t.ReasonId)
                .Index(t => t.UserId);
            
            CreateTable(
                "ONEPROD.RTV_RTVOEEReportProductionDataDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deleted = c.Boolean(nullable: false),
                        PartName = c.String(),
                        ProgramName = c.String(),
                        ProgramNo = c.Int(nullable: false),
                        CycleTime = c.Decimal(precision: 18, scale: 2),
                        PirometerTemp1 = c.Decimal(precision: 18, scale: 2),
                        PirometerTemp2 = c.Decimal(precision: 18, scale: 2),
                        PirometerTemp3 = c.Decimal(precision: 18, scale: 2),
                        PirometerTemp4 = c.Decimal(precision: 18, scale: 2),
                        PirometerTemp5 = c.Decimal(precision: 18, scale: 2),
                        PirometerTemp6 = c.Decimal(precision: 18, scale: 2),
                        PirometerTemp7 = c.Decimal(precision: 18, scale: 2),
                        PirometerTemp8 = c.Decimal(precision: 18, scale: 2),
                        PirometerMin1 = c.Decimal(precision: 18, scale: 2),
                        PirometerMin2 = c.Decimal(precision: 18, scale: 2),
                        PirometerMin3 = c.Decimal(precision: 18, scale: 2),
                        PirometerMin4 = c.Decimal(precision: 18, scale: 2),
                        PirometerMin5 = c.Decimal(precision: 18, scale: 2),
                        PirometerMin6 = c.Decimal(precision: 18, scale: 2),
                        PirometerMin7 = c.Decimal(precision: 18, scale: 2),
                        PirometerMin8 = c.Decimal(precision: 18, scale: 2),
                        PirometerMax1 = c.Decimal(precision: 18, scale: 2),
                        PirometerMax2 = c.Decimal(precision: 18, scale: 2),
                        PirometerMax3 = c.Decimal(precision: 18, scale: 2),
                        PirometerMax4 = c.Decimal(precision: 18, scale: 2),
                        PirometerMax5 = c.Decimal(precision: 18, scale: 2),
                        PirometerMax6 = c.Decimal(precision: 18, scale: 2),
                        PirometerMax7 = c.Decimal(precision: 18, scale: 2),
                        PirometerMax8 = c.Decimal(precision: 18, scale: 2),
                        RTVOEEProductionData_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.RTV_RTVOEEReportProductionData", t => t.RTVOEEProductionData_Id)
                .Index(t => t.RTVOEEProductionData_Id);
            
            CreateTable(
                "CORE.SystemVariables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Value = c.String(),
                        Type = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "ONEPROD.APS_ToolChangeOver",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tool1Id = c.Int(nullable: false),
                        Tool2Id = c.Int(),
                        Time = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.APS_Tool", t => t.Tool1Id)
                .ForeignKey("ONEPROD.APS_Tool", t => t.Tool2Id)
                .Index(t => t.Tool1Id)
                .Index(t => t.Tool2Id);
            
            CreateTable(
                "ONEPROD.APS_ToolMachine",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(nullable: false),
                        ToolId = c.Int(nullable: false),
                        Placed = c.Boolean(nullable: false),
                        InUse = c.Boolean(nullable: false),
                        Preffered = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Resource", t => t.MachineId)
                .ForeignKey("ONEPROD.APS_Tool", t => t.ToolId)
                .Index(t => t.MachineId)
                .Index(t => t.ToolId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Workstation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        SortOrder = c.Int(nullable: false),
                        SortOrderTrain = c.Int(nullable: false),
                        LineId = c.Int(),
                        AreaId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        ProductsFromIn = c.Int(nullable: false),
                        ProductsFromOut = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.LineId)
                .Index(t => t.LineId)
                .Index(t => t.AreaId);
            
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
                        TargetInSec_StopPlanned = c.Int(nullable: false),
                        TargetInSec_StopPlannedChangeOver = c.Int(nullable: false),
                        TargetInSec_StopUnplanned = c.Int(nullable: false),
                        TargetInSec_StopUnplannedBreakdown = c.Int(nullable: false),
                        TargetInSec_StopUnplannedPreformance = c.Int(nullable: false),
                        TargetInSec_StopUnplannedChangeOver = c.Int(nullable: false),
                        ToolRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ONEPROD.CORE_Resource", "Id", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("ONEPROD.CORE_Item", "Id", "_MPPL.MASTERDATA_Item");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "LineId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("ONEPROD.APS_ToolMachine", "ToolId", "ONEPROD.APS_Tool");
            DropForeignKey("ONEPROD.APS_ToolMachine", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.APS_ToolChangeOver", "Tool2Id", "ONEPROD.APS_Tool");
            DropForeignKey("ONEPROD.APS_ToolChangeOver", "Tool1Id", "ONEPROD.APS_Tool");
            DropForeignKey("CORE.SystemVariables", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.RTV_RTVOEEReportProductionDataDetails", "RTVOEEProductionData_Id", "ONEPROD.RTV_RTVOEEReportProductionData");
            DropForeignKey("ONEPROD.RTV_RTVOEEReportProductionData", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.RTV_RTVOEEReportProductionData", "ReasonId", "ONEPROD.OEE_Reason");
            DropForeignKey("ONEPROD.RTV_RTVOEEReportProductionData", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "RoleId", "_MPPL.IDENTITY_Role");
            DropForeignKey("ONEPROD.MES_ProductionLogRawMaterial", "ProductionLogId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_ProductionLog", "WorkplaceId", "ONEPROD.MES_Workplace");
            DropForeignKey("ONEPROD.MES_Workplace", "SelectedTaskId", "ONEPROD.CORE_Workorder");
            DropForeignKey("ONEPROD.CORE_Workorder", "ToolId", "ONEPROD.APS_Tool");
            DropForeignKey("ONEPROD.CORE_Workorder", "ResourceId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.CORE_Workorder", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.CORE_Workorder", "ClientOrderId", "ONEPROD.CORE_ClientOrder");
            DropForeignKey("ONEPROD.MES_Workplace", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.MES_ProductionLog", "ReasonId", "ONEPROD.OEE_Reason");
            DropForeignKey("ONEPROD.MES_ProductionLog", "OEEReportProductionDataId", "ONEPROD.OEE_OEEReportProductionData");
            DropForeignKey("ONEPROD.MES_ProductionLog", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ReportId", "ONEPROD.OEE_OEEReport");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ReasonId", "ONEPROD.OEE_Reason");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "DetailId", "ONEPROD.OEE_OEEReportProductionDataDetails");
            DropForeignKey("ONEPROD.OEE_OEEReportEmployee", "ReportId", "ONEPROD.OEE_OEEReport");
            DropForeignKey("ONEPROD.OEE_OEEReport", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.OEE_OEEReport", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.OEE_OEEReport", "LabourBrigadeId", "_MPPL.MASTERDATA_LabourBrigade");
            DropForeignKey("ONEPROD.OEE_MachineReason", "ReasonId", "ONEPROD.OEE_Reason");
            DropForeignKey("ONEPROD.OEE_MachineReason", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.WMS_ItemInventory", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.APS_ToolItemGroup", "ToolId", "ONEPROD.APS_Tool");
            DropForeignKey("ONEPROD.APS_Tool", "ToolGroupId", "ONEPROD.APS_ToolGroup");
            DropForeignKey("ONEPROD.APS_ToolItemGroup", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.CORE_CycleTime", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.CORE_CycleTime", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ResourceGroupId", "ONEPROD.CORE_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ResourceGroupId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ProcessId", "_MPPL.MASTERDATA_Process");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("ONEPROD.CORE_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Client");
            DropForeignKey("CORE.ChangeLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "SuperVisorUserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserLogin", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "DepartmentId", "_MPPL.MASTERDATA_Department");
            DropForeignKey("_MPPL.IDENTITY_UserClaim", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.APS_Calendar", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ResourceGroupId", "ONEPROD.CORE_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ResourceGroupId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "AreaId", "_MPPL.MASTERDATA_Area");
            DropIndex("ONEPROD.CORE_Resource", new[] { "Id" });
            DropIndex("ONEPROD.CORE_Item", new[] { "Id" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "LineId" });
            DropIndex("ONEPROD.APS_ToolMachine", new[] { "ToolId" });
            DropIndex("ONEPROD.APS_ToolMachine", new[] { "MachineId" });
            DropIndex("ONEPROD.APS_ToolChangeOver", new[] { "Tool2Id" });
            DropIndex("ONEPROD.APS_ToolChangeOver", new[] { "Tool1Id" });
            DropIndex("CORE.SystemVariables", new[] { "UserId" });
            DropIndex("ONEPROD.RTV_RTVOEEReportProductionDataDetails", new[] { "RTVOEEProductionData_Id" });
            DropIndex("ONEPROD.RTV_RTVOEEReportProductionData", new[] { "UserId" });
            DropIndex("ONEPROD.RTV_RTVOEEReportProductionData", new[] { "ReasonId" });
            DropIndex("ONEPROD.RTV_RTVOEEReportProductionData", new[] { "ItemId" });
            DropIndex("ONEPROD.RTV_RTVOEEReportProductionData", new[] { "MachineId" });
            DropIndex("_MPPL.IDENTITY_Role", "RoleNameIndex");
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ToolId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ResourceId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ItemId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ClientOrderId" });
            DropIndex("ONEPROD.MES_Workplace", new[] { "ComputerHostName" });
            DropIndex("ONEPROD.MES_Workplace", new[] { "SelectedTaskId" });
            DropIndex("ONEPROD.MES_Workplace", new[] { "MachineId" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "ReasonId" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "ItemId" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "WorkplaceId" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "OEEReportProductionDataId" });
            DropIndex("ONEPROD.MES_ProductionLogRawMaterial", new[] { "ProductionLogId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "DetailId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "UserId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ReasonId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ItemId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ReportId" });
            DropIndex("ONEPROD.OEE_OEEReport", new[] { "LabourBrigadeId" });
            DropIndex("ONEPROD.OEE_OEEReport", new[] { "MachineId" });
            DropIndex("ONEPROD.OEE_OEEReport", new[] { "UserId" });
            DropIndex("ONEPROD.OEE_OEEReportEmployee", new[] { "ReportId" });
            DropIndex("ONEPROD.OEE_MachineReason", new[] { "ReasonId" });
            DropIndex("ONEPROD.OEE_MachineReason", new[] { "MachineId" });
            DropIndex("ONEPROD.WMS_ItemInventory", new[] { "ItemId" });
            DropIndex("ONEPROD.APS_Tool", new[] { "ToolGroupId" });
            DropIndex("ONEPROD.APS_ToolItemGroup", new[] { "ToolId" });
            DropIndex("ONEPROD.APS_ToolItemGroup", new[] { "ItemGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ResourceGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ItemGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ProcessId" });
            DropIndex("ONEPROD.CORE_CycleTime", new[] { "ItemGroupId" });
            DropIndex("ONEPROD.CORE_CycleTime", new[] { "MachineId" });
            DropIndex("ONEPROD.CORE_ClientOrder", new[] { "ClientId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "RoleId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserLogin", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserClaim", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_User", "UserNameIndex");
            DropIndex("_MPPL.IDENTITY_User", new[] { "SuperVisorUserId" });
            DropIndex("_MPPL.IDENTITY_User", new[] { "DepartmentId" });
            DropIndex("CORE.ChangeLog", new[] { "UserId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ResourceGroupId" });
            DropIndex("ONEPROD.APS_Calendar", new[] { "MachineId" });
            DropTable("ONEPROD.CORE_Resource");
            DropTable("ONEPROD.CORE_Item");
            DropTable("_MPPL.MASTERDATA_Workstation");
            DropTable("ONEPROD.APS_ToolMachine");
            DropTable("ONEPROD.APS_ToolChangeOver");
            DropTable("CORE.SystemVariables");
            DropTable("ONEPROD.RTV_RTVOEEReportProductionDataDetails");
            DropTable("ONEPROD.RTV_RTVOEEReportProductionData");
            DropTable("ONEPROD.RTV_RTVOEEPLCData");
            DropTable("_MPPL.IDENTITY_Role");
            DropTable("ONEPROD.CORE_Workorder");
            DropTable("ONEPROD.MES_Workplace");
            DropTable("ONEPROD.MES_ProductionLog");
            DropTable("ONEPROD.MES_ProductionLogRawMaterial");
            DropTable("ONEPROD.CORE_Param");
            DropTable("ONEPROD.OEE_OEEReportProductionDataDetails");
            DropTable("ONEPROD.OEE_OEEReportProductionData");
            DropTable("ONEPROD.OEE_OEEReport");
            DropTable("ONEPROD.OEE_OEEReportEmployee");
            DropTable("ONEPROD.OEE_Reason");
            DropTable("ONEPROD.OEE_MachineReason");
            DropTable("_MPPL.MASTERDATA_LabourBrigade");
            DropTable("ONEPROD.WMS_ItemInventory");
            DropTable("ONEPROD.APS_ToolGroup");
            DropTable("ONEPROD.APS_Tool");
            DropTable("ONEPROD.APS_ToolItemGroup");
            DropTable("_MPPL.MASTERDATA_Process");
            DropTable("_MPPL.MASTERDATA_Item");
            DropTable("ONEPROD.CORE_CycleTime");
            DropTable("_MPPL.MASTERDATA_Client");
            DropTable("ONEPROD.CORE_ClientOrder");
            DropTable("ONEPROD.APS_ChangeOver");
            DropTable("_MPPL.IDENTITY_UserRole");
            DropTable("_MPPL.IDENTITY_UserLogin");
            DropTable("_MPPL.MASTERDATA_Department");
            DropTable("_MPPL.IDENTITY_UserClaim");
            DropTable("_MPPL.IDENTITY_User");
            DropTable("CORE.ChangeLog");
            DropTable("_MPPL.MASTERDATA_Resource");
            DropTable("ONEPROD.APS_Calendar");
            DropTable("_MPPL.MASTERDATA_Area");
        }
    }
}

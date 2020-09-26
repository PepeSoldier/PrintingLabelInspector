namespace _MPPL_WEB_START.Migrations.ElectroluxPLV_Staging
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1K : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "AP.ActionActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionId = c.Int(nullable: false),
                        ActivityDescription = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        ActivityEnum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("AP.Action", t => t.ActionId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.CreatorId)
                .Index(t => t.ActionId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "AP.Action",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentActionId = c.Int(nullable: false),
                        SubactionsCount = c.Int(nullable: false),
                        Progress = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        PlannedEndDate = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 60),
                        Problem = c.String(),
                        DepartmentId = c.Int(),
                        CreatorId = c.String(maxLength: 128),
                        AssignedId = c.String(maxLength: 128),
                        AreaId = c.Int(nullable: false),
                        LineId = c.Int(nullable: false),
                        ShiftCodeId = c.Int(nullable: false),
                        WorkstationId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Area", t => t.AreaId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.AssignedId)
                .ForeignKey("AP.DEF_Category", t => t.CategoryId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.CreatorId)
                .ForeignKey("_MPPL.MASTERDATA_Department", t => t.DepartmentId)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.LineId)
                .ForeignKey("_MPPL.MASTERDATA_LabourBrigade", t => t.ShiftCodeId)
                .ForeignKey("AP.DEF_Type", t => t.TypeId)
                .ForeignKey("_MPPL.MASTERDATA_Workstation", t => t.WorkstationId)
                .Index(t => t.DepartmentId)
                .Index(t => t.CreatorId)
                .Index(t => t.AssignedId)
                .Index(t => t.AreaId)
                .Index(t => t.LineId)
                .Index(t => t.ShiftCodeId)
                .Index(t => t.WorkstationId)
                .Index(t => t.CategoryId)
                .Index(t => t.TypeId);
            
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
                "AP.DEF_Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "_MPPL.MASTERDATA_LabourBrigade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "AP.DEF_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "AP.ActionObserver",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionId = c.Int(nullable: false),
                        UserId = c.String(),
                        ObserverId = c.Int(nullable: false),
                        ObserverType = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PFEP.AncFixedLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AncId = c.Int(nullable: false),
                        FixedLocation = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.AncId)
                .Index(t => t.AncId);
            
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
                "PFEP.AncPackage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AncId = c.Int(nullable: false),
                        PackageId = c.Int(nullable: false),
                        Returnable = c.Boolean(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfBoxesOnPallet = c.Int(nullable: false),
                        Stackable = c.Boolean(nullable: false),
                        PackagingCard = c.Boolean(nullable: false),
                        PackagingCardFile = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.AncId)
                .ForeignKey("iLOGIS.CONFIG_Package", t => t.PackageId)
                .Index(t => t.AncId)
                .Index(t => t.PackageId);
            
            CreateTable(
                "iLOGIS.CONFIG_Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Returnable = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PFEP.AncWorkstation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AncId = c.Int(nullable: false),
                        WorkstationId = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                        BomQuantity = c.Int(nullable: false),
                        MontageTypeId = c.Int(nullable: false),
                        FeederTypeId = c.Int(nullable: false),
                        BufferTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.AncId)
                .ForeignKey("PFEP.DEF_Types", t => t.BufferTypeId)
                .ForeignKey("PFEP.DEF_Types", t => t.FeederTypeId)
                .ForeignKey("PFEP.DEF_Types", t => t.MontageTypeId)
                .ForeignKey("_MPPL.MASTERDATA_Workstation", t => t.WorkstationId)
                .Index(t => t.AncId)
                .Index(t => t.WorkstationId)
                .Index(t => t.MontageTypeId)
                .Index(t => t.FeederTypeId)
                .Index(t => t.BufferTypeId);
            
            CreateTable(
                "PFEP.DEF_Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Deleted = c.Boolean(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "CORE.Attachment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        SubDirectory = c.String(maxLength: 100),
                        FileNamePrefix = c.String(maxLength: 50),
                        FileNameSuffix = c.String(maxLength: 50),
                        PackingCardUrl = c.String(maxLength: 150),
                        Extension = c.String(maxLength: 10),
                        ParentObjectId = c.Int(nullable: false),
                        ParentType = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "iLOGIS.CONFIG_AutomaticRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        PREFIX = c.String(maxLength: 20),
                        WorkstationName = c.String(maxLength: 100),
                        LineNames = c.String(maxLength: 255),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "CORE.BOM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AncId = c.Int(),
                        PncId = c.Int(),
                        LV = c.Int(nullable: false),
                        PCS = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        BC = c.String(maxLength: 3),
                        DEF = c.String(maxLength: 3),
                        Prefix = c.String(maxLength: 12),
                        Suffix = c.String(maxLength: 12),
                        IDCO = c.String(maxLength: 12),
                        Task = c.String(maxLength: 50),
                        TaskForce = c.String(maxLength: 50),
                        Formula = c.String(maxLength: 50),
                        Condition = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.AncId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.PncId)
                .Index(t => t.AncId)
                .Index(t => t.PncId);
            
            CreateTable(
                "CORE.BOM_Workorder",
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
                        UnitOfMeasure = c.Int(nullable: false),
                        InterItm = c.String(),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ChildId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
            CreateTable(
                "ONEPROD.PREPROD_BufforLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ANC = c.String(maxLength: 9),
                        ItemGroupId = c.Int(nullable: false),
                        BoxId = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        UsedBoxes = c.Int(nullable: false),
                        TotalUsedBoxes = c.Int(nullable: false),
                        TotalBoxes = c.Int(nullable: false),
                        MaxStore = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.BoxId)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemGroupId)
                .Index(t => t.ItemGroupId)
                .Index(t => t.BoxId);
            
            CreateTable(
                "iLOGIS.CONFIG_WarehouseLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentWarehouseLocationId = c.Int(),
                        QtyOfSubLocations = c.Int(nullable: false),
                        WarehouseId = c.Int(),
                        Utilization = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                        V = c.Int(nullable: false),
                        W = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.ParentWarehouseLocationId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.WarehouseId)
                .Index(t => t.ParentWarehouseLocationId)
                .Index(t => t.WarehouseId);
            
            CreateTable(
                "PRD.CALENDAR",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineName = c.String(maxLength: 50),
                        Date = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Hours = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CycleTime = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                "iLOGIS.WMS_DeliveryListItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        QtyRequested = c.Int(nullable: false),
                        QtyDelivered = c.Int(nullable: false),
                        QtyUsed = c.Int(nullable: false),
                        QtyPerPackage = c.Int(nullable: false),
                        WorkstationId = c.Int(),
                        WorkOrderId = c.Int(nullable: false),
                        TransporterId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemId)
                .ForeignKey("iLOGIS.CONFIG_Transporter", t => t.TransporterId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("PRD.ProdOrder", t => t.WorkOrderId)
                .ForeignKey("_MPPL.MASTERDATA_Workstation", t => t.WorkstationId)
                .Index(t => t.ItemId)
                .Index(t => t.WorkstationId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.TransporterId)
                .Index(t => t.UserId);
            
            CreateTable(
                "iLOGIS.CONFIG_Transporter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        Code = c.String(maxLength: 25),
                        DedicatedResources = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        ConnectedTransporters = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PRD.ProdOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(maxLength: 20),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        QtyPlanned = c.Int(nullable: false),
                        QtyRemain = c.Int(nullable: false),
                        PncId = c.Int(nullable: false),
                        LineId = c.Int(nullable: false),
                        Notice = c.String(),
                        Sequence = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        SerialNoFrom = c.String(maxLength: 25),
                        SerialNoTo = c.String(maxLength: 25),
                        CounterProductsIn = c.Int(nullable: false),
                        CounterProductsOut = c.Int(nullable: false),
                        CounterProductsFGW = c.Int(nullable: false),
                        FirstProductIn = c.DateTime(),
                        LastProductIn = c.DateTime(),
                        FirstProductOut = c.DateTime(),
                        LastProductOut = c.DateTime(),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Resource", t => t.LineId)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.PncId)
                .Index(t => t.PncId)
                .Index(t => t.LineId);
            
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
                "OTHER.VISUALCONTROL_JobItemConfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        JobNo = c.Int(nullable: false),
                        PairNo = c.Int(nullable: false),
                        CameraNo = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Location = c.Int(nullable: false),
                        Description = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemId)
                .Index(t => t.ItemId);
            
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
                "AP.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeetingName = c.String(),
                        OwnerId = c.String(maxLength: 128),
                        BeginnigDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
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
                "PRD.PSI_Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Line = c.String(maxLength: 5),
                        OrderNo = c.String(maxLength: 10),
                        PNC = c.String(maxLength: 9),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        QtyPlanned = c.Int(nullable: false),
                        QtyRemain = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PRD.PSI_OrderArchive",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Line = c.String(maxLength: 5),
                        OrderNo = c.String(maxLength: 10),
                        PNC = c.String(maxLength: 9),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        QtyPlanned = c.Int(nullable: false),
                        QtyRemain = c.Int(nullable: false),
                        FreezeDate = c.DateTime(nullable: false),
                        Type = c.String(maxLength: 3),
                        FirstScanTime = c.DateTime(nullable: false),
                        ReasonDetails = c.String(maxLength: 150),
                        CommentText = c.String(maxLength: 255),
                        CommentAnc = c.String(maxLength: 30),
                        CommentSupplier = c.String(maxLength: 30),
                        Reason_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PRD.PSI_Reason", t => t.Reason_Id)
                .Index(t => t.Reason_Id);
            
            CreateTable(
                "PRD.PSI_Reason",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "iLOGIS.WMS_PackageInstance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentQtyinPackage = c.Int(nullable: false),
                        MaxQtyPerPackage = c.Int(nullable: false),
                        WarehouseLocationId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        PackageItemId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemId)
                .ForeignKey("iLOGIS.CONFIG_PackageItem", t => t.PackageItemId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.WarehouseLocationId)
                .Index(t => t.WarehouseLocationId)
                .Index(t => t.ItemId)
                .Index(t => t.PackageItemId);
            
            CreateTable(
                "iLOGIS.CONFIG_PackageItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        QtyPerPackage = c.Int(nullable: false),
                        PackagesPerPallet = c.Int(nullable: false),
                        PalletW = c.Int(nullable: false),
                        PalletD = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemId)
                .ForeignKey("iLOGIS.CONFIG_Package", t => t.PackageId)
                .Index(t => t.PackageId)
                .Index(t => t.ItemId);
            
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
                "iLOGIS.WMS_PickingListItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QtyRequested = c.Int(nullable: false),
                        QtyPicked = c.Int(nullable: false),
                        Bilance = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Status = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        WarehouseLocationId = c.Int(nullable: false),
                        PlatformId = c.Int(),
                        PickingListId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemId)
                .ForeignKey("iLOGIS.WMS_PickingList", t => t.PickingListId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.PlatformId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.WarehouseLocationId)
                .Index(t => t.ItemId)
                .Index(t => t.WarehouseLocationId)
                .Index(t => t.PlatformId)
                .Index(t => t.PickingListId)
                .Index(t => t.UserId);
            
            CreateTable(
                "iLOGIS.WMS_PickingList",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        WorkOrderId = c.Int(nullable: false),
                        TransporterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Transporter", t => t.TransporterId)
                .ForeignKey("PRD.ProdOrder", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.TransporterId);
            
            CreateTable(
                "PFEP.PrintHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Printnumber = c.Int(nullable: false),
                        PrintDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Order20Id = c.Int(nullable: false),
                        RoutineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PRD.ProdOrder_20", t => t.Order20Id, cascadeDelete: true)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.Order20Id);
            
            CreateTable(
                "PRD.ProdOrder_20",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        PartQty = c.Int(nullable: false),
                        PartQtyRemain = c.Int(nullable: false),
                        PartNumber = c.Int(nullable: false),
                        PartStartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PRD.ProdOrder", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "PRD.ProdOrder_Sequence",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        OriginalSequence = c.Int(nullable: false),
                        OriginalStartDate = c.DateTime(nullable: false),
                        OriginalLineName = c.String(maxLength: 50),
                        OriginalLineId = c.Int(nullable: false),
                        SnapshotNo = c.Int(nullable: false),
                        SnapshotSeq = c.Int(nullable: false),
                        SnapshotLineName = c.String(maxLength: 50),
                        SnapshotStartDate = c.DateTime(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreatorUserName = c.String(),
                        Applied = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PRD.ProdOrder", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "PRD.ProdOrder_Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        StatusName = c.String(maxLength: 8),
                        StatusState = c.Int(nullable: false),
                        StatusInfo = c.String(maxLength: 8),
                        StatusInfoExtra = c.String(maxLength: 100),
                        StatusInfoExtra2 = c.String(maxLength: 100),
                        StausInfoExtraNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PRD.ProdOrder", t => t.OrderId)
                .Index(t => t.OrderId);
            
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
                .ForeignKey("ONEPROD.MES_Workplace", t => t.WorkplaceId)
                .Index(t => t.OEEReportProductionDataId)
                .Index(t => t.WorkplaceId);
            
            CreateTable(
                "ONEPROD.MES_Workplace",
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
                .Index(t => t.SelectedTaskId);
            
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
                        ProductionDate = c.DateTime(),
                        ItemId = c.Int(),
                        ProdQty = c.Int(nullable: false),
                        CycleTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UsedTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        entryType = c.Int(nullable: false),
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
                "iLOGIS.CONFIG_WarehouseItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseId = c.Int(nullable: false),
                        ItemGroupId = c.Int(nullable: false),
                        QtyPerLocation = c.Int(nullable: false),
                        HoursCoverage = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemGroupId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.WarehouseId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ItemGroupId);
            
            CreateTable(
                "iLOGIS.CONFIG_WorkstationItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkstationId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        MaxPackages = c.Int(nullable: false),
                        SafetyStock = c.Int(nullable: false),
                        MaxBomQty = c.Int(nullable: false),
                        CheckOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemId)
                .ForeignKey("_MPPL.MASTERDATA_Workstation", t => t.WorkstationId)
                .Index(t => t.WorkstationId)
                .Index(t => t.ItemId);
            
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
                "iLOGIS.CONFIG_Item",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        V = c.Int(nullable: false),
                        W = c.Int(nullable: false),
                        T = c.Int(nullable: false),
                        PickerNo = c.Int(nullable: false),
                        TrainNo = c.Int(nullable: false),
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
            
            CreateTable(
                "iLOGIS.CONFIG_Warehouse",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "Id", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("ONEPROD.CORE_Resource", "Id", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("iLOGIS.CONFIG_Item", "Id", "_MPPL.MASTERDATA_Item");
            DropForeignKey("ONEPROD.CORE_Item", "Id", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item");
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
            DropForeignKey("ONEPROD.MES_ProductionLog", "OEEReportProductionDataId", "ONEPROD.OEE_OEEReportProductionData");
            DropForeignKey("PRD.ProdOrder_Status", "OrderId", "PRD.ProdOrder");
            DropForeignKey("PRD.ProdOrder_Sequence", "OrderId", "PRD.ProdOrder");
            DropForeignKey("PFEP.PrintHistory", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("PFEP.PrintHistory", "Order20Id", "PRD.ProdOrder_20");
            DropForeignKey("PRD.ProdOrder_20", "OrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "PlatformId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "PickingListId", "iLOGIS.WMS_PickingList");
            DropForeignKey("iLOGIS.WMS_PickingList", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_PickingList", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PackageInstance", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_PackageInstance", "PackageItemId", "iLOGIS.CONFIG_PackageItem");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PackageInstance", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("PRD.PSI_OrderArchive", "Reason_Id", "PRD.PSI_Reason");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ReportId", "ONEPROD.OEE_OEEReport");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ReasonId", "ONEPROD.OEE_Reason");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.OEE_OEEReportProductionData", "DetailId", "ONEPROD.OEE_OEEReportProductionDataDetails");
            DropForeignKey("ONEPROD.OEE_OEEReportEmployee", "ReportId", "ONEPROD.OEE_OEEReport");
            DropForeignKey("ONEPROD.OEE_OEEReport", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.OEE_OEEReport", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.OEE_OEEReport", "LabourBrigadeId", "_MPPL.MASTERDATA_LabourBrigade");
            DropForeignKey("AP.Meeting", "OwnerId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.OEE_MachineReason", "ReasonId", "ONEPROD.OEE_Reason");
            DropForeignKey("ONEPROD.OEE_MachineReason", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("OTHER.VISUALCONTROL_JobItemConfig", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("ONEPROD.WMS_ItemInventory", "ItemId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.APS_ToolItemGroup", "ToolId", "ONEPROD.APS_Tool");
            DropForeignKey("ONEPROD.APS_Tool", "ToolGroupId", "ONEPROD.APS_ToolGroup");
            DropForeignKey("ONEPROD.APS_ToolItemGroup", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("PRD.ProdOrder", "PncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("PRD.ProdOrder", "LineId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("ONEPROD.CORE_CycleTime", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.CORE_CycleTime", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.CORE_ClientOrder", "ClientId", "_MPPL.MASTERDATA_Client");
            DropForeignKey("CORE.ChangeLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("ONEPROD.APS_Calendar", "MachineId", "ONEPROD.CORE_Resource");
            DropForeignKey("ONEPROD.PREPROD_BufforLog", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "ParentWarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("CORE.BOM_Workorder", "ParentId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM_Workorder", "ChildId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "PncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "AncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("PFEP.AncWorkstation", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropForeignKey("PFEP.AncWorkstation", "MontageTypeId", "PFEP.DEF_Types");
            DropForeignKey("PFEP.AncWorkstation", "FeederTypeId", "PFEP.DEF_Types");
            DropForeignKey("PFEP.AncWorkstation", "BufferTypeId", "PFEP.DEF_Types");
            DropForeignKey("PFEP.AncWorkstation", "AncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("PFEP.AncPackage", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("PFEP.AncPackage", "AncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("PFEP.AncFixedLocation", "AncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ResourceGroupId", "ONEPROD.CORE_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "ONEPROD.CORE_Item");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ResourceGroupId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ProcessId", "_MPPL.MASTERDATA_Process");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("AP.ActionActivity", "CreatorId", "_MPPL.IDENTITY_User");
            DropForeignKey("AP.ActionActivity", "ActionId", "AP.Action");
            DropForeignKey("AP.Action", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "LineId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("AP.Action", "TypeId", "AP.DEF_Type");
            DropForeignKey("AP.Action", "ShiftCodeId", "_MPPL.MASTERDATA_LabourBrigade");
            DropForeignKey("AP.Action", "LineId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ResourceGroupId", "ONEPROD.CORE_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ResourceGroupId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("AP.Action", "DepartmentId", "_MPPL.MASTERDATA_Department");
            DropForeignKey("AP.Action", "CreatorId", "_MPPL.IDENTITY_User");
            DropForeignKey("AP.Action", "CategoryId", "AP.DEF_Category");
            DropForeignKey("AP.Action", "AssignedId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "SuperVisorUserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserRole", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_UserLogin", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("_MPPL.IDENTITY_User", "DepartmentId", "_MPPL.MASTERDATA_Department");
            DropForeignKey("_MPPL.IDENTITY_UserClaim", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("AP.Action", "AreaId", "_MPPL.MASTERDATA_Area");
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "Id" });
            DropIndex("ONEPROD.CORE_Resource", new[] { "Id" });
            DropIndex("iLOGIS.CONFIG_Item", new[] { "Id" });
            DropIndex("ONEPROD.CORE_Item", new[] { "Id" });
            DropIndex("iLOGIS.CONFIG_WorkstationItem", new[] { "ItemId" });
            DropIndex("iLOGIS.CONFIG_WorkstationItem", new[] { "WorkstationId" });
            DropIndex("iLOGIS.CONFIG_WarehouseItem", new[] { "ItemGroupId" });
            DropIndex("iLOGIS.CONFIG_WarehouseItem", new[] { "WarehouseId" });
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
            DropIndex("ONEPROD.MES_Workplace", new[] { "SelectedTaskId" });
            DropIndex("ONEPROD.MES_Workplace", new[] { "MachineId" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "WorkplaceId" });
            DropIndex("ONEPROD.MES_ProductionLog", new[] { "OEEReportProductionDataId" });
            DropIndex("ONEPROD.MES_ProductionLogRawMaterial", new[] { "ProductionLogId" });
            DropIndex("PRD.ProdOrder_Status", new[] { "OrderId" });
            DropIndex("PRD.ProdOrder_Sequence", new[] { "OrderId" });
            DropIndex("PRD.ProdOrder_20", new[] { "OrderId" });
            DropIndex("PFEP.PrintHistory", new[] { "Order20Id" });
            DropIndex("PFEP.PrintHistory", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_PickingList", new[] { "TransporterId" });
            DropIndex("iLOGIS.WMS_PickingList", new[] { "WorkOrderId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PickingListId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PlatformId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "ItemId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "ItemId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "PackageId" });
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "PackageItemId" });
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "ItemId" });
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "WarehouseLocationId" });
            DropIndex("PRD.PSI_OrderArchive", new[] { "Reason_Id" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "DetailId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "UserId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ReasonId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ItemId" });
            DropIndex("ONEPROD.OEE_OEEReportProductionData", new[] { "ReportId" });
            DropIndex("ONEPROD.OEE_OEEReport", new[] { "LabourBrigadeId" });
            DropIndex("ONEPROD.OEE_OEEReport", new[] { "MachineId" });
            DropIndex("ONEPROD.OEE_OEEReport", new[] { "UserId" });
            DropIndex("ONEPROD.OEE_OEEReportEmployee", new[] { "ReportId" });
            DropIndex("AP.Meeting", new[] { "OwnerId" });
            DropIndex("ONEPROD.OEE_MachineReason", new[] { "ReasonId" });
            DropIndex("ONEPROD.OEE_MachineReason", new[] { "MachineId" });
            DropIndex("OTHER.VISUALCONTROL_JobItemConfig", new[] { "ItemId" });
            DropIndex("ONEPROD.WMS_ItemInventory", new[] { "ItemId" });
            DropIndex("ONEPROD.APS_Tool", new[] { "ToolGroupId" });
            DropIndex("ONEPROD.APS_ToolItemGroup", new[] { "ToolId" });
            DropIndex("ONEPROD.APS_ToolItemGroup", new[] { "ItemGroupId" });
            DropIndex("PRD.ProdOrder", new[] { "LineId" });
            DropIndex("PRD.ProdOrder", new[] { "PncId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "TransporterId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "WorkOrderId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "WorkstationId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "ItemId" });
            DropIndex("ONEPROD.CORE_CycleTime", new[] { "ItemGroupId" });
            DropIndex("ONEPROD.CORE_CycleTime", new[] { "MachineId" });
            DropIndex("ONEPROD.CORE_ClientOrder", new[] { "ClientId" });
            DropIndex("CORE.ChangeLog", new[] { "UserId" });
            DropIndex("ONEPROD.APS_Calendar", new[] { "MachineId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "ParentWarehouseLocationId" });
            DropIndex("ONEPROD.PREPROD_BufforLog", new[] { "BoxId" });
            DropIndex("ONEPROD.PREPROD_BufforLog", new[] { "ItemGroupId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ChildId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ParentId" });
            DropIndex("CORE.BOM", new[] { "PncId" });
            DropIndex("CORE.BOM", new[] { "AncId" });
            DropIndex("PFEP.AncWorkstation", new[] { "BufferTypeId" });
            DropIndex("PFEP.AncWorkstation", new[] { "FeederTypeId" });
            DropIndex("PFEP.AncWorkstation", new[] { "MontageTypeId" });
            DropIndex("PFEP.AncWorkstation", new[] { "WorkstationId" });
            DropIndex("PFEP.AncWorkstation", new[] { "AncId" });
            DropIndex("PFEP.AncPackage", new[] { "PackageId" });
            DropIndex("PFEP.AncPackage", new[] { "AncId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ResourceGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ItemGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ProcessId" });
            DropIndex("PFEP.AncFixedLocation", new[] { "AncId" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "LineId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ResourceGroupId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "RoleId" });
            DropIndex("_MPPL.IDENTITY_UserRole", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserLogin", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_UserClaim", new[] { "UserId" });
            DropIndex("_MPPL.IDENTITY_User", "UserNameIndex");
            DropIndex("_MPPL.IDENTITY_User", new[] { "SuperVisorUserId" });
            DropIndex("_MPPL.IDENTITY_User", new[] { "DepartmentId" });
            DropIndex("AP.Action", new[] { "TypeId" });
            DropIndex("AP.Action", new[] { "CategoryId" });
            DropIndex("AP.Action", new[] { "WorkstationId" });
            DropIndex("AP.Action", new[] { "ShiftCodeId" });
            DropIndex("AP.Action", new[] { "LineId" });
            DropIndex("AP.Action", new[] { "AreaId" });
            DropIndex("AP.Action", new[] { "AssignedId" });
            DropIndex("AP.Action", new[] { "CreatorId" });
            DropIndex("AP.Action", new[] { "DepartmentId" });
            DropIndex("AP.ActionActivity", new[] { "CreatorId" });
            DropIndex("AP.ActionActivity", new[] { "ActionId" });
            DropTable("iLOGIS.CONFIG_Warehouse");
            DropTable("ONEPROD.CORE_Resource");
            DropTable("iLOGIS.CONFIG_Item");
            DropTable("ONEPROD.CORE_Item");
            DropTable("iLOGIS.CONFIG_WorkstationItem");
            DropTable("iLOGIS.CONFIG_WarehouseItem");
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
            DropTable("PRD.ProdOrder_Status");
            DropTable("PRD.ProdOrder_Sequence");
            DropTable("PRD.ProdOrder_20");
            DropTable("PFEP.PrintHistory");
            DropTable("iLOGIS.WMS_PickingList");
            DropTable("iLOGIS.WMS_PickingListItem");
            DropTable("ONEPROD.CORE_Param");
            DropTable("iLOGIS.CONFIG_PackageItem");
            DropTable("iLOGIS.WMS_PackageInstance");
            DropTable("PRD.PSI_Reason");
            DropTable("PRD.PSI_OrderArchive");
            DropTable("PRD.PSI_Order");
            DropTable("ONEPROD.OEE_OEEReportProductionDataDetails");
            DropTable("ONEPROD.OEE_OEEReportProductionData");
            DropTable("ONEPROD.OEE_OEEReport");
            DropTable("ONEPROD.OEE_OEEReportEmployee");
            DropTable("AP.Meeting");
            DropTable("ONEPROD.OEE_Reason");
            DropTable("ONEPROD.OEE_MachineReason");
            DropTable("OTHER.VISUALCONTROL_JobItemConfig");
            DropTable("ONEPROD.WMS_ItemInventory");
            DropTable("ONEPROD.APS_ToolGroup");
            DropTable("ONEPROD.APS_Tool");
            DropTable("ONEPROD.APS_ToolItemGroup");
            DropTable("PRD.ProdOrder");
            DropTable("iLOGIS.CONFIG_Transporter");
            DropTable("iLOGIS.WMS_DeliveryListItem");
            DropTable("ONEPROD.CORE_CycleTime");
            DropTable("_MPPL.MASTERDATA_Client");
            DropTable("ONEPROD.CORE_ClientOrder");
            DropTable("ONEPROD.APS_ChangeOver");
            DropTable("CORE.ChangeLog");
            DropTable("ONEPROD.APS_Calendar");
            DropTable("PRD.CALENDAR");
            DropTable("iLOGIS.CONFIG_WarehouseLocation");
            DropTable("ONEPROD.PREPROD_BufforLog");
            DropTable("CORE.BOM_Workorder");
            DropTable("CORE.BOM");
            DropTable("iLOGIS.CONFIG_AutomaticRules");
            DropTable("CORE.Attachment");
            DropTable("PFEP.DEF_Types");
            DropTable("PFEP.AncWorkstation");
            DropTable("iLOGIS.CONFIG_Package");
            DropTable("PFEP.AncPackage");
            DropTable("_MPPL.MASTERDATA_Process");
            DropTable("_MPPL.MASTERDATA_Item");
            DropTable("PFEP.AncFixedLocation");
            DropTable("AP.ActionObserver");
            DropTable("_MPPL.MASTERDATA_Workstation");
            DropTable("AP.DEF_Type");
            DropTable("_MPPL.MASTERDATA_LabourBrigade");
            DropTable("_MPPL.MASTERDATA_Resource");
            DropTable("AP.DEF_Category");
            DropTable("_MPPL.IDENTITY_UserRole");
            DropTable("_MPPL.IDENTITY_UserLogin");
            DropTable("_MPPL.MASTERDATA_Department");
            DropTable("_MPPL.IDENTITY_UserClaim");
            DropTable("_MPPL.IDENTITY_User");
            DropTable("_MPPL.MASTERDATA_Area");
            DropTable("AP.Action");
            DropTable("AP.ActionActivity");
        }
    }
}

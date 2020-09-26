namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6K : DbMigration
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
                        MaxPackages = c.Int(nullable: false),
                        SafetyStock = c.Int(nullable: false),
                        MaxBomQty = c.Int(nullable: false),
                        CheckOnly = c.Boolean(nullable: false),
                        PackageId = c.Int(),
                        PackageName = c.String(),
                        QtyPerPackage = c.Int(nullable: false),
                        PackagesPerPallet = c.Int(nullable: false),
                        PalletW = c.Int(nullable: false),
                        PalletD = c.Int(nullable: false),
                        PalletH = c.Int(nullable: false),
                        WeightGross = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPackageType = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        LastChange = c.DateTime(nullable: false),
                        UserName = c.String(),
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
                        UnitOfMeasure = c.Int(nullable: false),
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
                .Index(t => t.ResourceGroupId)
                .Index(t => t.AreaId);
            
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
                "CORE.ChangeLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ObjectName = c.String(maxLength: 70),
                        ObjectDescription = c.String(maxLength: 100),
                        FieldName = c.String(maxLength: 70),
                        FieldDisplayName = c.String(maxLength: 140),
                        NewValue = c.String(maxLength: 255),
                        OldValue = c.String(maxLength: 255),
                        ObjectId = c.Int(nullable: false),
                        ParentObjectId = c.Int(nullable: false),
                        ParentObjectName = c.String(maxLength: 70),
                        ParentObjectDescription = c.String(maxLength: 100),
                        UserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "_MPPL.MASTERDATA_Contractor",
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
                "iLOGIS.WMS_Delivery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        SupplierId = c.Int(nullable: false),
                        DocumentNumber = c.String(maxLength: 12),
                        DocumentDate = c.DateTime(nullable: false),
                        StampTime = c.DateTime(nullable: false),
                        EnumDeliveryStatus = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Contractor", t => t.SupplierId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "iLOGIS.WMS_DeliveryItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DeliveryId = c.Int(nullable: false),
                        ItemWMSId = c.Int(nullable: false),
                        NumberOfPackages = c.Int(nullable: false),
                        QtyInPackage = c.Int(nullable: false),
                        TotalQty = c.Int(nullable: false),
                        AdminEntry = c.Boolean(nullable: false),
                        OperatorEntry = c.Boolean(nullable: false),
                        WasPrinted = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.WMS_Delivery", t => t.DeliveryId)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.DeliveryId)
                .Index(t => t.ItemWMSId);
            
            CreateTable(
                "iLOGIS.CONFIG_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        H = c.Int(nullable: false),
                        PickerNo = c.Int(nullable: false),
                        TrainNo = c.Int(nullable: false),
                        ABC = c.Int(nullable: false),
                        XYZ = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "iLOGIS.WMS_DeliveryListItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemWMSId = c.Int(nullable: false),
                        QtyRequested = c.Int(nullable: false),
                        QtyDelivered = c.Int(nullable: false),
                        QtyUsed = c.Int(nullable: false),
                        QtyPerPackage = c.Int(nullable: false),
                        BomQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WorkstationId = c.Int(),
                        WorkOrderId = c.Int(nullable: false),
                        TransporterId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_Transporter", t => t.TransporterId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("PRD.ProdOrder", t => t.WorkOrderId)
                .ForeignKey("_MPPL.MASTERDATA_Workstation", t => t.WorkstationId)
                .Index(t => new { t.TransporterId, t.WorkOrderId, t.WorkstationId, t.ItemWMSId }, unique: true, name: "IX_ItemWorkstationWorkorderTransporter")
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
                        LoopQty = c.Int(nullable: false),
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
                        QtyProducedInPast = c.Int(nullable: false),
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
                "_MPPL.MASTERDATA_LabourBrigade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "iLOGIS.CONFIG_PackageItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageId = c.Int(nullable: false),
                        ItemWMSId = c.Int(nullable: false),
                        WarehouseId = c.Int(),
                        WarehouseLocationTypeId = c.Int(nullable: false),
                        PickingStrategy = c.Int(nullable: false),
                        QtyPerPackage = c.Int(nullable: false),
                        PackagesPerPallet = c.Int(nullable: false),
                        PalletW = c.Int(nullable: false),
                        PalletD = c.Int(nullable: false),
                        PalletH = c.Int(nullable: false),
                        WeightGross = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_Package", t => t.PackageId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.WarehouseId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocationType", t => t.WarehouseLocationTypeId)
                .Index(t => t.PackageId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.WarehouseId)
                .Index(t => t.WarehouseLocationTypeId);
            
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
                        PackagesPerPallet = c.Int(nullable: false),
                        FullPalletHeight = c.Int(nullable: false),
                        Returnable = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "iLOGIS.CONFIG_Warehouse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        QtyOfSubLocations = c.Int(nullable: false),
                        ParentWarehouseId = c.Int(),
                        AccountingWarehouseId = c.Int(),
                        WarehouseType = c.Int(nullable: false),
                        IndependentSerialNumber = c.Boolean(nullable: false),
                        LabelLayoutFileName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.AccountingWarehouseId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.ParentWarehouseId)
                .Index(t => t.ParentWarehouseId)
                .Index(t => t.AccountingWarehouseId);
            
            CreateTable(
                "iLOGIS.CONFIG_WarehouseLocationType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        DisplayFormat = c.String(maxLength: 50),
                        Description = c.String(maxLength: 50),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        MaxWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TypeEnum = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "iLOGIS.WMS_PickingListItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QtyRequested = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QtyPicked = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bilance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BomQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(maxLength: 100),
                        Status = c.Int(nullable: false),
                        ItemWMSId = c.Int(nullable: false),
                        WarehouseLocationId = c.Int(nullable: false),
                        StockUnitId = c.Int(),
                        PlatformId = c.Int(),
                        PickingListId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.WMS_PickingList", t => t.PickingListId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.PlatformId)
                .ForeignKey("iLOGIS.WMS_StockUnit", t => t.StockUnitId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.WarehouseLocationId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.WarehouseLocationId)
                .Index(t => t.StockUnitId)
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
                "iLOGIS.CONFIG_WarehouseLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseId = c.Int(),
                        ParentWarehouseLocationId = c.Int(),
                        TypeId = c.Int(),
                        RegalNumber = c.String(maxLength: 6),
                        ColumnNumber = c.Int(nullable: false),
                        Name = c.String(maxLength: 25),
                        Utilization = c.Decimal(nullable: false, precision: 14, scale: 12),
                        InsertCounter = c.Int(nullable: false),
                        RemoveCounter = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        ABC = c.Int(nullable: false),
                        XYZ = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        QtyOfSubLocations = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.ParentWarehouseLocationId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocationType", t => t.TypeId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.WarehouseId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ParentWarehouseLocationId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "iLOGIS.WMS_StockUnit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentQtyinPackage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WMSQtyinPackage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxQtyPerPackage = c.Int(nullable: false),
                        WarehouseLocationId = c.Int(),
                        SerialNumber = c.String(),
                        ItemWMSId = c.Int(nullable: false),
                        PackageItemId = c.Int(),
                        ReservedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReservedForPickingListId = c.Int(nullable: false),
                        WMSLastCheck = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        BestBeforeDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        IsLocated = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_PackageItem", t => t.PackageItemId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.WarehouseLocationId)
                .Index(t => t.WarehouseLocationId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.PackageItemId);
            
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
                        WorkOrderNumber = c.String(maxLength: 150),
                        PrinterType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IpAdress, unique: true);
            
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
                "iLOGIS.WMS_TransporterLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransporterId = c.Int(nullable: false),
                        ItemWMSId = c.Int(),
                        ItemQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WorkorderNumber = c.String(),
                        ProductItemCode = c.String(),
                        EntryType = c.Int(nullable: false),
                        RelatedObjectId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Comment = c.String(),
                        Location = c.String(),
                        TimeStamp = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_Transporter", t => t.TransporterId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.TransporterId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.UserId);
            
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
            
            CreateTable(
                "iLOGIS.CONFIG_WorkstationItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkstationId = c.Int(nullable: false),
                        ItemWMSId = c.Int(nullable: false),
                        MaxPackages = c.Int(nullable: false),
                        SafetyStock = c.Int(nullable: false),
                        MaxBomQty = c.Int(nullable: false),
                        CheckOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("_MPPL.MASTERDATA_Workstation", t => t.WorkstationId)
                .Index(t => t.WorkstationId)
                .Index(t => t.ItemWMSId);
            
            AddColumn("_MPPL.IDENTITY_User", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("PRD.ProdOrder_Status", "OrderId", "PRD.ProdOrder");
            DropForeignKey("PRD.ProdOrder_Sequence", "OrderId", "PRD.ProdOrder");
            DropForeignKey("PRD.ProdOrder_20", "OrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "StockUnitId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("iLOGIS.WMS_StockUnit", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_StockUnit", "PackageItemId", "iLOGIS.CONFIG_PackageItem");
            DropForeignKey("iLOGIS.WMS_StockUnit", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "PlatformId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "TypeId", "iLOGIS.CONFIG_WarehouseLocationType");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "ParentWarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "PickingListId", "iLOGIS.WMS_PickingList");
            DropForeignKey("iLOGIS.WMS_PickingList", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_PickingList", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId", "iLOGIS.CONFIG_WarehouseLocationType");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "LineId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Workstation", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("PRD.ProdOrder", "PncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("PRD.ProdOrder", "LineId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_Delivery", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_Delivery", "SupplierId", "_MPPL.MASTERDATA_Contractor");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_Item", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "DeliveryId", "iLOGIS.WMS_Delivery");
            DropForeignKey("CORE.ChangeLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("CORE.BOM_Workorder", "ParentId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM_Workorder", "ChildId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "PncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM", "AncId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ResourceGroupId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "ResourceGroupId", "_MPPL.MASTERDATA_Resource");
            DropForeignKey("_MPPL.MASTERDATA_Resource", "AreaId", "_MPPL.MASTERDATA_Area");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ProcessId", "_MPPL.MASTERDATA_Process");
            DropForeignKey("_MPPL.MASTERDATA_Item", "ItemGroupId", "_MPPL.MASTERDATA_Item");
            DropIndex("iLOGIS.CONFIG_WorkstationItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.CONFIG_WorkstationItem", new[] { "WorkstationId" });
            DropIndex("iLOGIS.CONFIG_WarehouseItem", new[] { "ItemGroupId" });
            DropIndex("iLOGIS.CONFIG_WarehouseItem", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "TransporterId" });
            DropIndex("PRD.ProdOrder_Status", new[] { "OrderId" });
            DropIndex("PRD.ProdOrder_Sequence", new[] { "OrderId" });
            DropIndex("PRD.ProdOrder_20", new[] { "OrderId" });
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "PackageItemId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "TypeId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "ParentWarehouseLocationId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_PickingList", new[] { "TransporterId" });
            DropIndex("iLOGIS.WMS_PickingList", new[] { "WorkOrderId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PickingListId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PlatformId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "StockUnitId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "AccountingWarehouseId" });
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "ParentWarehouseId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "WarehouseLocationTypeId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "WarehouseId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "PackageId" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Workstation", new[] { "LineId" });
            DropIndex("PRD.ProdOrder", new[] { "LineId" });
            DropIndex("PRD.ProdOrder", new[] { "PncId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", "IX_ItemWorkstationWorkorderTransporter");
            DropIndex("iLOGIS.CONFIG_Item", new[] { "ItemId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "DeliveryId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_Delivery", new[] { "SupplierId" });
            DropIndex("iLOGIS.WMS_Delivery", new[] { "UserId" });
            DropIndex("CORE.ChangeLog", new[] { "UserId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ChildId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ParentId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "AreaId" });
            DropIndex("_MPPL.MASTERDATA_Resource", new[] { "ResourceGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ResourceGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ItemGroupId" });
            DropIndex("_MPPL.MASTERDATA_Item", new[] { "ProcessId" });
            DropIndex("CORE.BOM", new[] { "PncId" });
            DropIndex("CORE.BOM", new[] { "AncId" });
            DropColumn("_MPPL.IDENTITY_User", "Deleted");
            DropTable("iLOGIS.CONFIG_WorkstationItem");
            DropTable("iLOGIS.CONFIG_WarehouseLocationSort");
            DropTable("iLOGIS.CONFIG_WarehouseItem");
            DropTable("iLOGIS.WMS_TransporterLog");
            DropTable("PRD.ProdOrder_Status");
            DropTable("PRD.ProdOrder_Sequence");
            DropTable("PRD.ProdOrder_20");
            DropTable("CORE.Printer");
            DropTable("iLOGIS.WMS_StockUnit");
            DropTable("iLOGIS.CONFIG_WarehouseLocation");
            DropTable("iLOGIS.WMS_PickingList");
            DropTable("iLOGIS.WMS_PickingListItem");
            DropTable("iLOGIS.CONFIG_WarehouseLocationType");
            DropTable("iLOGIS.CONFIG_Warehouse");
            DropTable("iLOGIS.CONFIG_Package");
            DropTable("iLOGIS.CONFIG_PackageItem");
            DropTable("_MPPL.MASTERDATA_LabourBrigade");
            DropTable("_MPPL.MASTERDATA_Workstation");
            DropTable("PRD.ProdOrder");
            DropTable("iLOGIS.CONFIG_Transporter");
            DropTable("iLOGIS.WMS_DeliveryListItem");
            DropTable("iLOGIS.CONFIG_Item");
            DropTable("iLOGIS.WMS_DeliveryItem");
            DropTable("iLOGIS.WMS_Delivery");
            DropTable("_MPPL.MASTERDATA_Contractor");
            DropTable("CORE.ChangeLog");
            DropTable("PRD.CALENDAR");
            DropTable("CORE.BOM_Workorder");
            DropTable("_MPPL.MASTERDATA_Resource");
            DropTable("_MPPL.MASTERDATA_Process");
            DropTable("_MPPL.MASTERDATA_Item");
            DropTable("CORE.BOM");
            DropTable("iLOGIS.CONFIG_AutomaticRules");
            DropTable("CORE.Attachment");
            DropTable("_MPPL.MASTERDATA_Area");
        }
    }
}

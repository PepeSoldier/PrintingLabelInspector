namespace _MPPL_WEB_START.Migrations.Grandhome
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3K : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "_MPPL.MASTERDATA_Client", newName: "MASTERDATA_Contractor");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "Id", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Item", "Id", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "Id" });
            DropIndex("iLOGIS.CONFIG_Item", new[] { "Id" });
            DropPrimaryKey("iLOGIS.CONFIG_Item");
            DropPrimaryKey("iLOGIS.CONFIG_Warehouse");
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
                "_MPPL.MASTERDATA_ItemUoM",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        DefaultUnitOfMeasure = c.Int(nullable: false),
                        QtyForDefaultUnitOfMeasure = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AlternativeUnitOfMeasure = c.Int(nullable: false),
                        QtyForAlternativeUnitOfMeasure = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Item", t => t.ItemId)
                .Index(t => t.ItemId);
            
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
                "iLOGIS.WMS_Delivery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        SupplierId = c.Int(nullable: false),
                        DocumentNumber = c.String(maxLength: 25),
                        DocumentDate = c.DateTime(nullable: false),
                        StampTime = c.DateTime(nullable: false),
                        EnumDeliveryStatus = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Guid = c.String(),
                        ExternalId = c.String(),
                        ExternalUserName = c.String(),
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
                        PackageItemId = c.Int(),
                        NumberOfPackages = c.Int(nullable: false),
                        QtyInPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        TotalQty = c.Decimal(nullable: false, precision: 18, scale: 5),
                        UnitOfMeasure = c.Int(nullable: false),
                        StockStatus = c.Int(nullable: false),
                        MovementType = c.Int(nullable: false),
                        DestinationWarehouseCode = c.String(),
                        IsSpecialStock = c.Boolean(nullable: false),
                        AdminEntry = c.Boolean(nullable: false),
                        OperatorEntry = c.Boolean(nullable: false),
                        WasPrinted = c.Boolean(nullable: false),
                        IsLocationAssigned = c.Boolean(nullable: false),
                        IsLocated = c.Boolean(nullable: false),
                        TotalLocatedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.WMS_Delivery", t => t.DeliveryId)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_PackageItem", t => t.PackageItemId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.DeliveryId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.PackageItemId);
            
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
                        QtyPerPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        PackagesPerPallet = c.Int(nullable: false),
                        PalletW = c.Int(nullable: false),
                        PalletD = c.Int(nullable: false),
                        PalletH = c.Int(nullable: false),
                        WeightGross = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
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
                "iLOGIS.WMS_DeliveryListItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemWMSId = c.Int(nullable: false),
                        QtyRequested = c.Decimal(nullable: false, precision: 18, scale: 5),
                        QtyDelivered = c.Decimal(nullable: false, precision: 18, scale: 5),
                        QtyUsed = c.Decimal(nullable: false, precision: 18, scale: 5),
                        QtyPerPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        BomQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        WorkstationId = c.Int(),
                        WorkOrderId = c.Int(nullable: false),
                        TransporterId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        IsSubstituteData = c.Boolean(nullable: false),
                        PickingListItemId = c.Int(),
                        DeliveryListId = c.Int(),
                        StockUnitId = c.Int(),
                        WarehouseLocationId = c.Int(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.WMS_DeliveryList", t => t.DeliveryListId)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.WMS_PickingListItem", t => t.PickingListItemId)
                .ForeignKey("iLOGIS.WMS_StockUnit", t => t.StockUnitId)
                .ForeignKey("iLOGIS.CONFIG_Transporter", t => t.TransporterId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.WarehouseLocationId)
                .ForeignKey("PRD.ProdOrder", t => t.WorkOrderId)
                .ForeignKey("_MPPL.MASTERDATA_Workstation", t => t.WorkstationId)
                .Index(t => new { t.TransporterId, t.WorkOrderId, t.WorkstationId, t.ItemWMSId, t.PickingListItemId }, unique: true, name: "IX_ItemWorkstationWorkorderTransporterPickignListItem")
                .Index(t => t.UserId)
                .Index(t => t.DeliveryListId)
                .Index(t => t.StockUnitId)
                .Index(t => t.WarehouseLocationId);
            
            CreateTable(
                "iLOGIS.WMS_DeliveryList",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        TransporterId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Transporter", t => t.TransporterId)
                .ForeignKey("PRD.ProdOrder", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.TransporterId);
            
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
                        ConnectedStatus = c.String(),
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
                        UnitOfMeasure = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Status = c.Int(nullable: false),
                        StatusLFI = c.Int(nullable: false),
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
                        Guid = c.String(),
                        GuidCreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Transporter", t => t.TransporterId)
                .ForeignKey("PRD.ProdOrder", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.TransporterId);
            
            CreateTable(
                "iLOGIS.WMS_StockUnit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentQtyinPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        ReservedQty = c.Decimal(nullable: false, precision: 18, scale: 5),
                        WMSQtyinPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        MaxQtyPerPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        InitialQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        WarehouseLocationId = c.Int(nullable: false),
                        SerialNumber = c.String(),
                        ItemWMSId = c.Int(nullable: false),
                        PackageItemId = c.Int(),
                        WMSLastCheck = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        BestBeforeDate = c.DateTime(nullable: false),
                        IncomeDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        IsLocated = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ReferenceDeliveryItemId = c.Int(nullable: false),
                        IsGroup = c.Boolean(nullable: false),
                        GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.WMS_StockUnit", t => t.GroupId)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_PackageItem", t => t.PackageItemId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.WarehouseLocationId)
                .Index(t => t.WarehouseLocationId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.PackageItemId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "iLOGIS.WMS_Movement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemWMSId = c.Int(nullable: false),
                        SourceLocationId = c.Int(nullable: false),
                        SourceWarehouseId = c.Int(nullable: false),
                        SourceStockUnitSerialNumber = c.String(),
                        DestinationLocationId = c.Int(nullable: false),
                        DestinationWarehouseId = c.Int(nullable: false),
                        DestinationStockUnitSerialNumber = c.String(),
                        QtyMoved = c.Decimal(nullable: false, precision: 18, scale: 5),
                        UnitOfMeasure = c.Int(nullable: false),
                        FreeText1 = c.String(maxLength: 200),
                        FreeText2 = c.String(maxLength: 200),
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ExternalId = c.String(maxLength: 150),
                        ExternalUserName = c.String(maxLength: 100),
                        ExportDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.DestinationLocationId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.DestinationWarehouseId)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_WarehouseLocation", t => t.SourceLocationId)
                .ForeignKey("iLOGIS.CONFIG_Warehouse", t => t.SourceWarehouseId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.SourceLocationId)
                .Index(t => t.SourceWarehouseId)
                .Index(t => t.DestinationLocationId)
                .Index(t => t.DestinationWarehouseId)
                .Index(t => t.UserId);
            
            CreateTable(
                "CORE.NotificationDevice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PushEndpoint = c.String(),
                        PushP256DH = c.String(),
                        PushAuth = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .Index(t => t.UserId);
            
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
            
            CreateTable(
                "ONEPROD.MES_ProductionLogTraceability",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(),
                        ItemCode = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 25),
                        ProductionDate = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ChildId)
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId);
            
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
                "ONEPROD.MES_WorkplaceBuffer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentWorkorderId = c.Int(nullable: false),
                        WorkplaceId = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                        ProductionLogId = c.Int(),
                        ProcessId = c.Int(nullable: false),
                        Barcode = c.String(),
                        SerialNumber = c.String(),
                        QtyAvailable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QtyInBom = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Code = c.String(),
                        Name = c.String(),
                        TimeLoaded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ChildId)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ParentId)
                .ForeignKey("ONEPROD.CORE_Workorder", t => t.ParentWorkorderId)
                .ForeignKey("ONEPROD.MES_ProductionLog", t => t.ProductionLogId)
                .ForeignKey("ONEPROD.MES_Workplace", t => t.WorkplaceId)
                .Index(t => t.ParentWorkorderId)
                .Index(t => t.WorkplaceId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildId)
                .Index(t => t.ProductionLogId);
            
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
                        PutTo = c.String(),
                        CheckOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("_MPPL.MASTERDATA_Workstation", t => t.WorkstationId)
                .Index(t => t.WorkstationId)
                .Index(t => t.ItemWMSId);
            
            AddColumn("_MPPL.MASTERDATA_Item", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "ItemId", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "H", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "ABC", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "XYZ", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Resource", "Breaks", c => c.String());
            AddColumn("ONEPROD.APS_Calendar", "Date", c => c.DateTime(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Hours", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxQty", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxCycleTime", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Efficiency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.CORE_CycleTime", "ProgramName", c => c.String(maxLength: 50));
            AddColumn("ONEPROD.MES_ProductionLog", "UsedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.OEE_Reason", "ColorGroup", c => c.String(maxLength: 35));
            AddColumn("ONEPROD.OEE_Reason", "IsGroup", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.OEE_Reason", "GroupId", c => c.Int());
            AddColumn("_MPPL.IDENTITY_User", "LastPasswordChangedDate", c => c.DateTime(nullable: false));
            AddColumn("_MPPL.IDENTITY_User", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("_MPPL.IDENTITY_UserRole", "Id", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "Type", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsTraceability", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsReportOnline", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId", c => c.Int());
            AddColumn("ONEPROD.CORE_Workorder", "Qty_Scrap", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "Code", c => c.String());
            AddColumn("iLOGIS.CONFIG_Warehouse", "Name", c => c.String());
            AddColumn("iLOGIS.CONFIG_Warehouse", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "QtyOfSubLocations", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_Warehouse", "WarehouseType", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "IndependentSerialNumber", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "isMRP", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "isOutOfScore", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "isProduction", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "LabelLayoutFileName", c => c.String());
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "TypeId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "ShelfNumber", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "AvailableForPicker", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "InsertCounter", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "RemoveCounter", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "UpdateDate", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "ABC", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "XYZ", c => c.Int(nullable: false));
            AddColumn("_MPPL.MASTERDATA_Workstation", "FlowRackLOverride", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_Item", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("iLOGIS.CONFIG_Item", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.CONFIG_Warehouse", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Name", c => c.String(maxLength: 25));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Utilization", c => c.Decimal(nullable: false, precision: 14, scale: 12));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "RegalNumber", c => c.String(maxLength: 6));
            AddPrimaryKey("iLOGIS.CONFIG_Item", "Id");
            AddPrimaryKey("iLOGIS.CONFIG_Warehouse", "Id");
            CreateIndex("iLOGIS.CONFIG_Item", "ItemId");
            CreateIndex("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId");
            CreateIndex("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId");
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "TypeId");
            CreateIndex("ONEPROD.OEE_Reason", "GroupId");
            CreateIndex("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "TypeId", "iLOGIS.CONFIG_WarehouseLocationType", "Id");
            AddForeignKey("ONEPROD.OEE_Reason", "GroupId", "ONEPROD.OEE_Reason", "Id");
            AddForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item", "Id", cascadeDelete: true);
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id", cascadeDelete: true);
            AddForeignKey("iLOGIS.CONFIG_Item", "ItemId", "_MPPL.MASTERDATA_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            DropColumn("iLOGIS.CONFIG_Item", "V");
            DropColumn("iLOGIS.CONFIG_Item", "W");
            DropColumn("iLOGIS.CONFIG_Item", "T");
            DropColumn("_MPPL.IDENTITY_UserRole", "Discriminator");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "V");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "W");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "Type");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "Type", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "W", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "V", c => c.Int(nullable: false));
            AddColumn("_MPPL.IDENTITY_UserRole", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("iLOGIS.CONFIG_Item", "T", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "W", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "V", c => c.Int(nullable: false));
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Item", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "WorkplaceId", "ONEPROD.MES_Workplace");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ProductionLogId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ChildId", "ONEPROD.CORE_Item");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("ONEPROD.MES_ProductionLogTraceability", "ParentId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_ProductionLogTraceability", "ChildId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("ONEPROD.OEE_Reason", "GroupId", "ONEPROD.OEE_Reason");
            DropForeignKey("CORE.NotificationDevice", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_Movement", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_Movement", "SourceWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.WMS_Movement", "SourceLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_Movement", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_Movement", "DestinationWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.WMS_Movement", "DestinationLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WorkstationId", "_MPPL.MASTERDATA_Workstation");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "StockUnitId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "PickingListItemId", "iLOGIS.WMS_PickingListItem");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "StockUnitId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("iLOGIS.WMS_StockUnit", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_StockUnit", "PackageItemId", "iLOGIS.CONFIG_PackageItem");
            DropForeignKey("iLOGIS.WMS_StockUnit", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_StockUnit", "GroupId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "PlatformId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "TypeId", "iLOGIS.CONFIG_WarehouseLocationType");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "PickingListId", "iLOGIS.WMS_PickingList");
            DropForeignKey("iLOGIS.WMS_PickingList", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_PickingList", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "DeliveryListId", "iLOGIS.WMS_DeliveryList");
            DropForeignKey("iLOGIS.WMS_DeliveryList", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_DeliveryList", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_Delivery", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_Delivery", "SupplierId", "_MPPL.MASTERDATA_Contractor");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "PackageItemId", "iLOGIS.CONFIG_PackageItem");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId", "iLOGIS.CONFIG_WarehouseLocationType");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "DeliveryId", "iLOGIS.WMS_Delivery");
            DropForeignKey("CORE.BOM_Workorder", "ParentId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("CORE.BOM_Workorder", "ChildId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("_MPPL.MASTERDATA_ItemUoM", "ItemId", "_MPPL.MASTERDATA_Item");
            DropIndex("iLOGIS.CONFIG_WorkstationItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.CONFIG_WorkstationItem", new[] { "WorkstationId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ProductionLogId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "WorkplaceId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentWorkorderId" });
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_TransporterLog", new[] { "TransporterId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ParentId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ParentWorkorderId" });
            DropIndex("ONEPROD.OEE_Reason", new[] { "GroupId" });
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropIndex("CORE.NotificationDevice", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "DestinationWarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "DestinationLocationId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "SourceWarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "SourceLocationId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "GroupId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "PackageItemId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "TypeId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_PickingList", new[] { "TransporterId" });
            DropIndex("iLOGIS.WMS_PickingList", new[] { "WorkOrderId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PickingListId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PlatformId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "StockUnitId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_DeliveryList", new[] { "TransporterId" });
            DropIndex("iLOGIS.WMS_DeliveryList", new[] { "WorkOrderId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "StockUnitId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "DeliveryListId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", "IX_ItemWorkstationWorkorderTransporterPickignListItem");
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "AccountingWarehouseId" });
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "ParentWarehouseId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "WarehouseLocationTypeId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "WarehouseId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "PackageId" });
            DropIndex("iLOGIS.CONFIG_Item", new[] { "ItemId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "PackageItemId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "DeliveryId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_Delivery", new[] { "SupplierId" });
            DropIndex("iLOGIS.WMS_Delivery", new[] { "UserId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ChildId" });
            DropIndex("CORE.BOM_Workorder", new[] { "ParentId" });
            DropIndex("_MPPL.MASTERDATA_ItemUoM", new[] { "ItemId" });
            DropPrimaryKey("iLOGIS.CONFIG_Warehouse");
            DropPrimaryKey("iLOGIS.CONFIG_Item");
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "RegalNumber", c => c.String());
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Utilization", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", c => c.Int());
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Name", c => c.String());
            AlterColumn("iLOGIS.CONFIG_Warehouse", "Id", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_Item", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.CONFIG_Item", "Id", c => c.Int(nullable: false));
            DropColumn("_MPPL.MASTERDATA_Workstation", "FlowRackLOverride");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "XYZ");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "ABC");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "UpdateDate");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "RemoveCounter");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "InsertCounter");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "AvailableForPicker");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "ShelfNumber");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "TypeId");
            DropColumn("iLOGIS.CONFIG_Warehouse", "LabelLayoutFileName");
            DropColumn("iLOGIS.CONFIG_Warehouse", "isProduction");
            DropColumn("iLOGIS.CONFIG_Warehouse", "isOutOfScore");
            DropColumn("iLOGIS.CONFIG_Warehouse", "isMRP");
            DropColumn("iLOGIS.CONFIG_Warehouse", "IndependentSerialNumber");
            DropColumn("iLOGIS.CONFIG_Warehouse", "WarehouseType");
            DropColumn("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");
            DropColumn("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId");
            DropColumn("iLOGIS.CONFIG_Warehouse", "QtyOfSubLocations");
            DropColumn("iLOGIS.CONFIG_Warehouse", "Deleted");
            DropColumn("iLOGIS.CONFIG_Warehouse", "Name");
            DropColumn("iLOGIS.CONFIG_Warehouse", "Code");
            DropColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel");
            DropColumn("ONEPROD.CORE_Workorder", "Qty_Scrap");
            DropColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            DropColumn("ONEPROD.MES_Workplace", "IsReportOnline");
            DropColumn("ONEPROD.MES_Workplace", "IsTraceability");
            DropColumn("ONEPROD.MES_Workplace", "Type");
            DropColumn("_MPPL.IDENTITY_UserRole", "Id");
            DropColumn("_MPPL.IDENTITY_User", "Deleted");
            DropColumn("_MPPL.IDENTITY_User", "LastPasswordChangedDate");
            DropColumn("ONEPROD.OEE_Reason", "GroupId");
            DropColumn("ONEPROD.OEE_Reason", "IsGroup");
            DropColumn("ONEPROD.OEE_Reason", "ColorGroup");
            DropColumn("ONEPROD.MES_ProductionLog", "UsedQty");
            DropColumn("ONEPROD.CORE_CycleTime", "ProgramName");
            DropColumn("ONEPROD.APS_Calendar", "Efficiency");
            DropColumn("ONEPROD.APS_Calendar", "MaxCycleTime");
            DropColumn("ONEPROD.APS_Calendar", "MaxQty");
            DropColumn("ONEPROD.APS_Calendar", "Hours");
            DropColumn("ONEPROD.APS_Calendar", "Date");
            DropColumn("ONEPROD.CORE_Resource", "Breaks");
            DropColumn("iLOGIS.CONFIG_Item", "XYZ");
            DropColumn("iLOGIS.CONFIG_Item", "ABC");
            DropColumn("iLOGIS.CONFIG_Item", "H");
            DropColumn("iLOGIS.CONFIG_Item", "ItemId");
            DropColumn("_MPPL.MASTERDATA_Item", "UnitOfMeasure");
            DropTable("iLOGIS.CONFIG_WorkstationItem");
            DropTable("ONEPROD.MES_WorkplaceBuffer");
            DropTable("iLOGIS.CONFIG_WarehouseLocationSort");
            DropTable("iLOGIS.WMS_TransporterLog");
            DropTable("ONEPROD.MES_ProductionLogTraceability");
            DropTable("CORE.Printer");
            DropTable("CORE.NotificationDevice");
            DropTable("iLOGIS.WMS_Movement");
            DropTable("iLOGIS.WMS_StockUnit");
            DropTable("iLOGIS.WMS_PickingList");
            DropTable("iLOGIS.WMS_PickingListItem");
            DropTable("iLOGIS.CONFIG_Transporter");
            DropTable("iLOGIS.WMS_DeliveryList");
            DropTable("iLOGIS.WMS_DeliveryListItem");
            DropTable("iLOGIS.CONFIG_WarehouseLocationType");
            DropTable("iLOGIS.CONFIG_Package");
            DropTable("iLOGIS.CONFIG_PackageItem");
            DropTable("iLOGIS.WMS_DeliveryItem");
            DropTable("iLOGIS.WMS_Delivery");
            DropTable("CORE.BOM_Workorder");
            DropTable("_MPPL.MASTERDATA_ItemUoM");
            DropTable("iLOGIS.CONFIG_AutomaticRules");
            AddPrimaryKey("iLOGIS.CONFIG_Warehouse", "Id");
            AddPrimaryKey("iLOGIS.CONFIG_Item", "Id");
            CreateIndex("iLOGIS.CONFIG_Item", "Id");
            CreateIndex("iLOGIS.CONFIG_Warehouse", "Id");
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_Item", "Id", "_MPPL.MASTERDATA_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "Id", "iLOGIS.CONFIG_WarehouseLocation", "Id");
            RenameTable(name: "_MPPL.MASTERDATA_Contractor", newName: "MASTERDATA_Client");
        }
    }
}

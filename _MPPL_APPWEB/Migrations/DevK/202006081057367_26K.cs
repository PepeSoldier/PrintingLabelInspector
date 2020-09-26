namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _26K : DbMigration
    {
        public override void Up()
        {
            
            DropForeignKey("iLOGIS.DeliveryItem", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.WMS_PackageInstance", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PackageInstance", "PackageItemId", "iLOGIS.CONFIG_PackageItem");
            DropForeignKey("iLOGIS.WMS_PackageInstance", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "PackageInstanceId", "iLOGIS.WMS_PackageInstance");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "Id", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Item", "Id", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse");

            RenameTable(name: "iLOGIS.Delivery", newName: "WMS_Delivery");
            RenameTable(name: "iLOGIS.DeliveryItem", newName: "WMS_DeliveryItem");

            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "ItemId" });
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "ItemId" });
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "PackageItemId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "PackageInstanceId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            DropIndex("iLOGIS.CONFIG_Item", new[] { "Id" });
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "Id" });
            RenameColumn(table: "iLOGIS.WMS_DeliveryListItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.CONFIG_PackageItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.WMS_TransporterLog", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.CONFIG_WorkstationItem", name: "ItemId", newName: "ItemWMSId");
            RenameIndex(table: "iLOGIS.CONFIG_PackageItem", name: "IX_ItemId", newName: "IX_ItemWMSId");
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_ItemId", newName: "IX_ItemWMSId");
            RenameIndex(table: "iLOGIS.WMS_TransporterLog", name: "IX_ItemId", newName: "IX_ItemWMSId");
            RenameIndex(table: "iLOGIS.CONFIG_WorkstationItem", name: "IX_ItemId", newName: "IX_ItemWMSId");
            DropPrimaryKey("iLOGIS.CONFIG_Item");
            DropPrimaryKey("iLOGIS.CONFIG_Warehouse");
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
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
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
                "iLOGIS.WMS_StockUnit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentQtyinPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        WMSQtyinPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        MaxQtyPerPackage = c.Decimal(nullable: false, precision: 18, scale: 5),
                        WarehouseLocationId = c.Int(nullable: false),
                        SerialNumber = c.String(),
                        ItemWMSId = c.Int(nullable: false),
                        PackageItemId = c.Int(),
                        ReservedQty = c.Decimal(nullable: false, precision: 18, scale: 5),
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
                        PrinterType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IpAdress, unique: true);
            
            CreateTable(
                "iLOGIS.WHDOC_WhDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContractorId = c.Int(nullable: false),
                        DocumentNumber = c.String(maxLength: 12),
                        ReferrenceDocument = c.String(maxLength: 40),
                        CostPayer = c.String(maxLength: 128),
                        DocumentDate = c.DateTime(nullable: false),
                        StampTime = c.DateTime(nullable: false),
                        CostCenter = c.String(maxLength: 32),
                        Reason = c.String(maxLength: 128),
                        MeansOfTransport = c.String(maxLength: 128),
                        CreatorId = c.String(maxLength: 128),
                        IssuerId = c.String(maxLength: 128),
                        IssueDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        isSigned = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ForwadrerComments = c.String(),
                        ApproverId = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("_MPPL.MASTERDATA_Contractor", t => t.ContractorId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.CreatorId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.IssuerId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.ApproverId)
                .Index(t => t.ContractorId)
                .Index(t => t.CreatorId)
                .Index(t => t.IssuerId)
                .Index(t => t.ApproverId);
            
            CreateTable(
                "iLOGIS.WHDOC_WhDocumentItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        WhDocumentId = c.Int(nullable: false),
                        ItemWMSId = c.Int(),
                        PackageId = c.Int(),
                        ItemCode = c.String(maxLength: 100),
                        ItemName = c.String(maxLength: 100),
                        DisposedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssuedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.Int(nullable: false),
                        AdminEntry = c.Boolean(nullable: false),
                        OperatorEntry = c.Boolean(nullable: false),
                        WasPrinted = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("iLOGIS.CONFIG_Item", t => t.ItemWMSId)
                .ForeignKey("iLOGIS.CONFIG_Package", t => t.PackageId)
                .ForeignKey("_MPPL.IDENTITY_User", t => t.UserId)
                .ForeignKey("iLOGIS.WHDOC_WhDocument", t => t.WhDocumentId)
                .Index(t => t.UserId)
                .Index(t => t.WhDocumentId)
                .Index(t => t.ItemWMSId)
                .Index(t => t.PackageId);
            
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

            AddColumn("_MPPL.MASTERDATA_Item", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "ItemId", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "ABC", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "XYZ", c => c.Int(nullable: false));
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
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "InsertCounter", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "RemoveCounter", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "UpdateDate", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "ABC", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "XYZ", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Date", c => c.DateTime(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Hours", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxQty", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "MaxCycleTime", c => c.Int(nullable: false));
            AddColumn("ONEPROD.APS_Calendar", "Efficiency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("iLOGIS.WMS_DeliveryItem", "UserId", c => c.String(maxLength: 128));
            AddColumn("iLOGIS.WMS_DeliveryItem", "ItemWMSId", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "WasPrinted", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.ENERGY_EnergyCost", "kWhConverter", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.ENERGY_EnergyCost", "UseConverter", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_PackageItem", "WarehouseId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_PackageItem", "PickingStrategy", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_PickingListItem", "StockUnitId", c => c.Int());
            AddColumn("ONEPROD.MES_ProductionLog", "UsedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("ONEPROD.MES_Workplace", "Type", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsTraceability", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsReportOnline", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId", c => c.Int());
            AddColumn("ONEPROD.CORE_Workorder", "Qty_Scrap", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_ProductionLogTraceability", "ItemCode", c => c.String(maxLength: 50));
            AddColumn("ONEPROD.MES_ProductionLogTraceability", "SerialNumber", c => c.String(maxLength: 25));
            AlterColumn("iLOGIS.CONFIG_Item", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("iLOGIS.CONFIG_Warehouse", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Name", c => c.String(maxLength: 25));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Utilization", c => c.Decimal(nullable: false, precision: 14, scale: 12));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "RegalNumber", c => c.String(maxLength: 6));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "QtyInPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "TotalQty", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyRequested", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyDelivered", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyUsed", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.CONFIG_PackageItem", "QtyPerPackage", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("ONEPROD.MES_ProductionLogTraceability", "ChildId", c => c.Int());
            AddPrimaryKey("iLOGIS.CONFIG_Item", "Id");
            AddPrimaryKey("iLOGIS.CONFIG_Warehouse", "Id");
            CreateIndex("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId");
            CreateIndex("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId");
            CreateIndex("iLOGIS.WMS_DeliveryItem", "UserId");
            CreateIndex("iLOGIS.WMS_DeliveryItem", "ItemWMSId");
            CreateIndex("iLOGIS.CONFIG_Item", "ItemId");
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId");
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "TypeId");
            CreateIndex("iLOGIS.CONFIG_PackageItem", "WarehouseId");
            CreateIndex("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId");
            CreateIndex("iLOGIS.WMS_PickingListItem", "StockUnitId");
            CreateIndex("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            CreateIndex("ONEPROD.MES_ProductionLogTraceability", "ChildId");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryItem", "ItemWMSId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryItem", "UserId", "_MPPL.IDENTITY_User", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "TypeId", "iLOGIS.CONFIG_WarehouseLocationType", "Id");
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId", "iLOGIS.CONFIG_WarehouseLocationType", "Id");
            AddForeignKey("iLOGIS.WMS_PickingListItem", "StockUnitId", "iLOGIS.WMS_StockUnit", "Id");
            AddForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item", "Id", cascadeDelete: true);
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id", cascadeDelete: true);
            AddForeignKey("iLOGIS.CONFIG_Item", "ItemId", "_MPPL.MASTERDATA_Item", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemWMSId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "ItemWMSId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_PickingListItem", "ItemWMSId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_TransporterLog", "ItemWMSId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemWMSId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            DropColumn("iLOGIS.CONFIG_Item", "V");
            DropColumn("iLOGIS.CONFIG_Item", "W");
            DropColumn("iLOGIS.CONFIG_Item", "T");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "V");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "W");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "Type");
            DropColumn("iLOGIS.WMS_DeliveryItem", "ItemId");
            DropColumn("iLOGIS.WMS_PickingListItem", "PackageInstanceId");
            DropTable("iLOGIS.WMS_PackageInstance");

        
        }
        
        public override void Down()
        {
            CreateTable(
                "iLOGIS.WMS_PackageInstance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentQtyinPackage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WMSQtyinPackage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxQtyPerPackage = c.Int(nullable: false),
                        WarehouseLocationId = c.Int(nullable: false),
                        SerialNumber = c.String(),
                        ItemId = c.Int(nullable: false),
                        PackageItemId = c.Int(),
                        ReservedQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReservedForPickingListId = c.Int(nullable: false),
                        WMSLastCheck = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("iLOGIS.WMS_PickingListItem", "PackageInstanceId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryItem", "ItemId", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "Type", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "W", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "V", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "T", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "W", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "V", c => c.Int(nullable: false));
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_Item", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "WorkplaceId", "ONEPROD.MES_Workplace");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ProductionLogId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ChildId", "ONEPROD.CORE_Item");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "ApproverId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "WhDocumentId", "iLOGIS.WHDOC_WhDocument");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "IssuerId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "CreatorId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocument", "ContractorId", "_MPPL.MASTERDATA_Contractor");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "PackageId", "iLOGIS.CONFIG_Package");
            DropForeignKey("iLOGIS.WHDOC_WhDocumentItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("ONEPROD.CORE_Workorder", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "StockUnitId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("iLOGIS.WMS_StockUnit", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_StockUnit", "PackageItemId", "iLOGIS.CONFIG_PackageItem");
            DropForeignKey("iLOGIS.WMS_StockUnit", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId", "iLOGIS.CONFIG_WarehouseLocationType");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.WMS_Movement", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_Movement", "SourceWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.WMS_Movement", "SourceLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_Movement", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_Movement", "DestinationWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.WMS_Movement", "DestinationLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "TypeId", "iLOGIS.CONFIG_WarehouseLocationType");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ProductionLogId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "WorkplaceId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentWorkorderId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "PackageId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "WhDocumentId" });
            DropIndex("iLOGIS.WHDOC_WhDocumentItem", new[] { "UserId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ApproverId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "IssuerId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "CreatorId" });
            DropIndex("iLOGIS.WHDOC_WhDocument", new[] { "ContractorId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            DropIndex("ONEPROD.CORE_Workorder", new[] { "ParentWorkorderId" });
            DropIndex("CORE.Printer", new[] { "IpAdress" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "PackageItemId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.WMS_PickingListItem", new[] { "StockUnitId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "WarehouseLocationTypeId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "WarehouseId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "TypeId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "UserId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "DestinationWarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "DestinationLocationId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "SourceWarehouseId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "SourceLocationId" });
            DropIndex("iLOGIS.WMS_Movement", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.CONFIG_Item", new[] { "ItemId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "ItemWMSId" });
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "UserId" });
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "AccountingWarehouseId" });
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "ParentWarehouseId" });
            DropPrimaryKey("iLOGIS.CONFIG_Warehouse");
            DropPrimaryKey("iLOGIS.CONFIG_Item");
            AlterColumn("ONEPROD.MES_ProductionLogTraceability", "ChildId", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_PackageItem", "QtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyPerPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyUsed", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyDelivered", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryListItem", "QtyRequested", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "TotalQty", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_DeliveryItem", "QtyInPackage", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "RegalNumber", c => c.String());
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Utilization", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", c => c.Int());
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Name", c => c.String());
            AlterColumn("iLOGIS.CONFIG_Warehouse", "Id", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_Item", "Id", c => c.Int(nullable: false));
            DropColumn("ONEPROD.MES_ProductionLogTraceability", "SerialNumber");
            DropColumn("ONEPROD.MES_ProductionLogTraceability", "ItemCode");
            DropColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel");
            DropColumn("ONEPROD.CORE_Workorder", "Qty_Scrap");
            DropColumn("ONEPROD.CORE_Workorder", "ParentWorkorderId");
            DropColumn("ONEPROD.MES_Workplace", "IsReportOnline");
            DropColumn("ONEPROD.MES_Workplace", "IsTraceability");
            DropColumn("ONEPROD.MES_Workplace", "Type");
            DropColumn("ONEPROD.MES_ProductionLog", "UsedQty");
            DropColumn("iLOGIS.WMS_PickingListItem", "StockUnitId");
            DropColumn("iLOGIS.CONFIG_PackageItem", "PickingStrategy");
            DropColumn("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId");
            DropColumn("iLOGIS.CONFIG_PackageItem", "WarehouseId");
            DropColumn("ONEPROD.ENERGY_EnergyCost", "UseConverter");
            DropColumn("ONEPROD.ENERGY_EnergyCost", "kWhConverter");
            DropColumn("iLOGIS.WMS_DeliveryItem", "WasPrinted");
            DropColumn("iLOGIS.WMS_DeliveryItem", "ItemWMSId");
            DropColumn("iLOGIS.WMS_DeliveryItem", "UserId");
            DropColumn("ONEPROD.APS_Calendar", "Efficiency");
            DropColumn("ONEPROD.APS_Calendar", "MaxCycleTime");
            DropColumn("ONEPROD.APS_Calendar", "MaxQty");
            DropColumn("ONEPROD.APS_Calendar", "Hours");
            DropColumn("ONEPROD.APS_Calendar", "Date");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "XYZ");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "ABC");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "UpdateDate");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "RemoveCounter");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "InsertCounter");
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
            DropColumn("iLOGIS.CONFIG_Item", "XYZ");
            DropColumn("iLOGIS.CONFIG_Item", "ABC");
            DropColumn("iLOGIS.CONFIG_Item", "ItemId");
            DropColumn("_MPPL.MASTERDATA_Item", "UnitOfMeasure");
            DropTable("ONEPROD.MES_WorkplaceBuffer");
            DropTable("iLOGIS.WHDOC_WhDocumentItem");
            DropTable("iLOGIS.WHDOC_WhDocument");
            DropTable("CORE.Printer");
            DropTable("iLOGIS.WMS_StockUnit");
            DropTable("iLOGIS.CONFIG_WarehouseLocationType");
            DropTable("iLOGIS.WMS_Movement");
            AddPrimaryKey("iLOGIS.CONFIG_Warehouse", "Id");
            AddPrimaryKey("iLOGIS.CONFIG_Item", "Id");
            RenameIndex(table: "iLOGIS.CONFIG_WorkstationItem", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameIndex(table: "iLOGIS.WMS_TransporterLog", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameIndex(table: "iLOGIS.CONFIG_PackageItem", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameColumn(table: "iLOGIS.CONFIG_WorkstationItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_TransporterLog", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.CONFIG_PackageItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_DeliveryListItem", name: "ItemWMSId", newName: "ItemId");
            CreateIndex("iLOGIS.CONFIG_Warehouse", "Id");
            CreateIndex("iLOGIS.CONFIG_Item", "Id");
            CreateIndex("ONEPROD.MES_ProductionLogTraceability", "ChildId");
            CreateIndex("iLOGIS.WMS_PickingListItem", "PackageInstanceId");
            CreateIndex("iLOGIS.WMS_PackageInstance", "PackageItemId");
            CreateIndex("iLOGIS.WMS_PackageInstance", "ItemId");
            CreateIndex("iLOGIS.WMS_PackageInstance", "WarehouseLocationId");
            CreateIndex("iLOGIS.WMS_DeliveryItem", "ItemId");
            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId");
            AddForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_TransporterLog", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_PickingListItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_Item", "Id", "_MPPL.MASTERDATA_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "Id", "iLOGIS.CONFIG_WarehouseLocation", "Id");
            AddForeignKey("iLOGIS.WMS_PickingListItem", "PackageInstanceId", "iLOGIS.WMS_PackageInstance", "Id");
            AddForeignKey("iLOGIS.WMS_PackageInstance", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation", "Id");
            AddForeignKey("iLOGIS.WMS_PackageInstance", "PackageItemId", "iLOGIS.CONFIG_PackageItem", "Id");
            AddForeignKey("iLOGIS.WMS_PackageInstance", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.DeliveryItem", "ItemId", "_MPPL.MASTERDATA_Item", "Id");
            RenameTable(name: "iLOGIS.WMS_DeliveryItem", newName: "DeliveryItem");
            RenameTable(name: "iLOGIS.WMS_Delivery", newName: "Delivery");
        }
    }
}

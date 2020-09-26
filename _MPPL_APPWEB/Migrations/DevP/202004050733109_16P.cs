namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16P : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE [iLOGIS].[WMS_DeliveryListItem] DROP CONSTRAINT [FK_iLOGIS.WMS_DeliveryListItem_iLOGIS.CONFIG_Item_ItemId]");
            Sql("ALTER TABLE [iLOGIS].[CONFIG_PackageItem] DROP CONSTRAINT [FK_iLOGIS.COFIG_PackageItem_iLOGIS.CONFIG_Item_ItemWMSId]");
            Sql("ALTER TABLE [iLOGIS].[CONFIG_WorkstationItem] DROP CONSTRAINT [FK_iLOGIS.CONFIG_WorkstationItem__MPPL.MASTERDATA_Item_ItemId]");
            Sql("ALTER TABLE [iLOGIS].[WMS_PackageInstance] DROP CONSTRAINT [FK_iLOGIS.WMS_PackageInstance__MPPL.MASTERDATA_Item_ItemId]");


            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_Item", "Id", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PackageInstance", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "WarehouseLocationId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            
            DropIndex("iLOGIS.CONFIG_Item", new[] { "Id" });
            DropPrimaryKey("iLOGIS.CONFIG_Item");
            CreateTable(
                "iLOGIS.CONFIG_WarehouseLocationTpe",
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

            Sql("INSERT INTO [iLOGIS].[CONFIG_WarehouseLocationTpe] " +
               "([Name],[Width],[Height],[Depth],[MaxWeight],[TypeEnum],[Deleted]) " +
               "VALUES ('STD',80,100,120,0,0,0)");

           // Sql("UPDATE [iLOGIS].[CONFIG_WarehouseLocation] SET TypeId = (SELECT TOP 1 ID FROM [iLOGIS].[CONFIG_WarehouseLocationTpe])");
          //  Sql("UPDATE [iLOGIS].[CONFIG_PackageItem] SET WarehouseLocationTypeId = (SELECT TOP 1 ID FROM [iLOGIS].[CONFIG_WarehouseLocationTpe])");


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

            Sql("UPDATE [iLOGIS].[CONFIG_Item] SET ItemId = Id");
            DropColumn("iLOGIS.CONFIG_Item", "Id");
            AddColumn("iLOGIS.CONFIG_Item", "Id", c => c.Int(nullable: false, identity: true));

            Sql("UPDATE t SET t.ItemId = i.Id FROM [iLOGIS].[WMS_DeliveryListItem]    t LEFT JOIN [iLOGIS].[CONFIG_Item] i ON t.ItemId = i.ItemId");
            Sql("UPDATE t SET t.ItemId = i.Id FROM [iLOGIS].[WMS_PackageInstance]     t LEFT JOIN [iLOGIS].[CONFIG_Item] i ON t.ItemId = i.ItemId");
            Sql("UPDATE t SET t.ItemId = i.Id FROM [iLOGIS].[CONFIG_PackageItem]      t LEFT JOIN [iLOGIS].[CONFIG_Item] i ON t.ItemId = i.ItemId");
            Sql("UPDATE t SET t.ItemId = i.Id FROM [iLOGIS].[WMS_PickingListItem]     t LEFT JOIN [iLOGIS].[CONFIG_Item] i ON t.ItemId = i.ItemId");
            Sql("UPDATE t SET t.ItemId = i.Id FROM [iLOGIS].[WMS_TransporterLog]      t LEFT JOIN [iLOGIS].[CONFIG_Item] i ON t.ItemId = i.ItemId");
            Sql("UPDATE t SET t.ItemId = i.Id FROM [iLOGIS].[CONFIG_WorkstationItem]  t LEFT JOIN [iLOGIS].[CONFIG_Item] i ON t.ItemId = i.ItemId");
            Sql("UPDATE t SET t.ItemGroupId=i.Id FROM [iLOGIS].[CONFIG_WarehouseItem] t LEFT JOIN [iLOGIS].[CONFIG_Item] i ON t.ItemGroupId = i.ItemId");



            AddColumn("iLOGIS.CONFIG_Item", "ABC", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "XYZ", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "TypeId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "InsertCounter", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "RemoveCounter", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "UpdateDate", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "ABC", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "XYZ", c => c.Int(nullable: false));
           // AddColumn("iLOGIS.Delivery", "EnumDeliveryStatus", c => c.Int(nullable: false));
            AddColumn("iLOGIS.DeliveryItem", "UserId", c => c.String(maxLength: 128));
            AddColumn("iLOGIS.WMS_PackageInstance", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.WMS_PackageInstance", "BestBeforeDate", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.WMS_PackageInstance", "Status", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_PackageInstance", "IsLocated", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_PackageItem", "WarehouseId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_PackageItem", "PickingStrategy", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_ProductionLog", "UsedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AddColumn("ONEPROD.MES_ProductionLogTraceability", "ItemCode", c => c.String(maxLength: 50));
            //AddColumn("ONEPROD.MES_ProductionLogTraceability", "SerialNumber", c => c.String(maxLength: 25));
            AlterColumn("iLOGIS.CONFIG_Item", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Name", c => c.String(maxLength: 25));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "RegalNumber", c => c.String(maxLength: 6));
            AlterColumn("iLOGIS.WMS_PackageInstance", "WarehouseLocationId", c => c.Int());
            AlterColumn("ONEPROD.MES_ProductionLogTraceability", "ChildId", c => c.Int());
            
            
            AddPrimaryKey("iLOGIS.CONFIG_Item", "Id");
            

            CreateIndex("iLOGIS.CONFIG_WarehouseLocation", "TypeId");
            CreateIndex("iLOGIS.DeliveryItem", "UserId");
            CreateIndex("iLOGIS.CONFIG_Item", "ItemId");
            CreateIndex("iLOGIS.WMS_PackageInstance", "WarehouseLocationId");
            CreateIndex("iLOGIS.CONFIG_PackageItem", "WarehouseId");
            CreateIndex("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId");
            CreateIndex("ONEPROD.MES_ProductionLogTraceability", "ChildId");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "TypeId", "iLOGIS.CONFIG_WarehouseLocationTpe", "Id");
            AddForeignKey("iLOGIS.DeliveryItem", "UserId", "_MPPL.IDENTITY_User", "Id");
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            //AddForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId", "iLOGIS.CONFIG_WarehouseLocationTpe", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item", "Id", cascadeDelete: true);
            AddForeignKey("iLOGIS.CONFIG_Item", "ItemId", "_MPPL.MASTERDATA_Item", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
           
            AddForeignKey("iLOGIS.WMS_PackageInstance", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_PickingListItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_TransporterLog", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            DropColumn("iLOGIS.CONFIG_Item", "V");
            DropColumn("iLOGIS.CONFIG_Item", "W");
            DropColumn("iLOGIS.CONFIG_Item", "T");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "V");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "W");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "Type");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "Type", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "W", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "V", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "T", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "W", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Item", "V", c => c.Int(nullable: false));
            DropForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_TransporterLog", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PickingListItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_PackageInstance", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemId", "iLOGIS.CONFIG_Item");
            DropForeignKey("iLOGIS.CONFIG_Item", "ItemId", "_MPPL.MASTERDATA_Item");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "WorkplaceId", "ONEPROD.MES_Workplace");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ProductionLogId", "ONEPROD.MES_ProductionLog");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentWorkorderId", "ONEPROD.CORE_Workorder");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ParentId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.MES_WorkplaceBuffer", "ChildId", "ONEPROD.CORE_Item");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId", "iLOGIS.CONFIG_WarehouseLocationTpe");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.DeliveryItem", "UserId", "_MPPL.IDENTITY_User");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "TypeId", "iLOGIS.CONFIG_WarehouseLocationTpe");
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ProductionLogId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ChildId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "WorkplaceId" });
            DropIndex("ONEPROD.MES_WorkplaceBuffer", new[] { "ParentWorkorderId" });
            DropIndex("ONEPROD.MES_ProductionLogTraceability", new[] { "ChildId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "WarehouseLocationTypeId" });
            DropIndex("iLOGIS.CONFIG_PackageItem", new[] { "WarehouseId" });
            DropIndex("iLOGIS.WMS_PackageInstance", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.CONFIG_Item", new[] { "ItemId" });
            DropIndex("iLOGIS.DeliveryItem", new[] { "UserId" });
            DropIndex("iLOGIS.CONFIG_WarehouseLocation", new[] { "TypeId" });
            DropPrimaryKey("iLOGIS.CONFIG_Item");
            AlterColumn("ONEPROD.MES_ProductionLogTraceability", "ChildId", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.WMS_PackageInstance", "WarehouseLocationId", c => c.Int(nullable: false));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "RegalNumber", c => c.String());
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Name", c => c.String());
            AlterColumn("iLOGIS.CONFIG_Item", "Id", c => c.Int(nullable: false));
            //DropColumn("ONEPROD.MES_ProductionLogTraceability", "SerialNumber");
            //DropColumn("ONEPROD.MES_ProductionLogTraceability", "ItemCode");
            DropColumn("ONEPROD.MES_ProductionLog", "UsedQty");
            DropColumn("iLOGIS.CONFIG_PackageItem", "PickingStrategy");
            DropColumn("iLOGIS.CONFIG_PackageItem", "WarehouseLocationTypeId");
            DropColumn("iLOGIS.CONFIG_PackageItem", "WarehouseId");
            DropColumn("iLOGIS.WMS_PackageInstance", "IsLocated");
            DropColumn("iLOGIS.WMS_PackageInstance", "Status");
            DropColumn("iLOGIS.WMS_PackageInstance", "BestBeforeDate");
            DropColumn("iLOGIS.WMS_PackageInstance", "CreatedDate");
            DropColumn("iLOGIS.DeliveryItem", "UserId");
            //DropColumn("iLOGIS.Delivery", "EnumDeliveryStatus");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "XYZ");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "ABC");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "UpdateDate");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "RemoveCounter");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "InsertCounter");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "TypeId");
            DropColumn("iLOGIS.CONFIG_Item", "XYZ");
            DropColumn("iLOGIS.CONFIG_Item", "ABC");
            DropColumn("iLOGIS.CONFIG_Item", "ItemId");
            DropColumn("_MPPL.MASTERDATA_Item", "UnitOfMeasure");
            DropTable("ONEPROD.MES_WorkplaceBuffer");
            DropTable("iLOGIS.CONFIG_WarehouseLocationTpe");
            AddPrimaryKey("iLOGIS.CONFIG_Item", "Id");
            CreateIndex("iLOGIS.CONFIG_Item", "Id");
            CreateIndex("ONEPROD.MES_ProductionLogTraceability", "ChildId");
            CreateIndex("iLOGIS.WMS_PackageInstance", "WarehouseLocationId");
            AddForeignKey("iLOGIS.CONFIG_WorkstationItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_TransporterLog", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_PickingListItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_PackageInstance", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "ItemId", "iLOGIS.CONFIG_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_Item", "Id", "_MPPL.MASTERDATA_Item", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "ItemGroupId", "iLOGIS.CONFIG_Item", "Id");
        }
    }
}

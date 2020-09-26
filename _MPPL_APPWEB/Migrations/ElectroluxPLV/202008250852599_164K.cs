namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _164K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.WMS_DeliveryListItem", "IX_ItemWorkstationWorkorderTransporter");
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
            
            AddColumn("_MPPL.IDENTITY_User", "LastPasswordChangedDate", c => c.DateTime(nullable: false));
            AddColumn("_MPPL.MASTERDATA_Workstation", "FlowRackLOverride", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_Delivery", "Guid", c => c.String());
            AddColumn("iLOGIS.WMS_DeliveryItem", "PackageItemId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryListItem", "PickingListItemId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryListItem", "DeliveryListId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryListItem", "StockUnitId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryListItem", "Status", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_Movement", "FreeText1", c => c.String(maxLength: 200));
            AddColumn("iLOGIS.WMS_Movement", "FreeText2", c => c.String(maxLength: 200));
            AddColumn("iLOGIS.WMS_Movement", "ExportDateTime", c => c.DateTime());
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "AvailableForPicker", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_PickingListItem", "StatusLFI", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_PickingList", "Guid", c => c.String());
            AddColumn("iLOGIS.WMS_PickingList", "GuidCreationDate", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "ReferenceDeliveryItemId", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WorkstationItem", "PutTo", c => c.String());
            AlterColumn("iLOGIS.CONFIG_Item", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("iLOGIS.WMS_Movement", "ExternalId", c => c.String(maxLength: 150));
            AlterColumn("iLOGIS.WMS_Movement", "ExternalUserName", c => c.String(maxLength: 100));
            CreateIndex("iLOGIS.WMS_DeliveryItem", "PackageItemId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", new[] { "TransporterId", "WorkOrderId", "WorkstationId", "ItemWMSId", "PickingListItemId" }, unique: true, name: "IX_ItemWorkstationWorkorderTransporterPickignListItem");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "DeliveryListId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "StockUnitId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId");
            AddForeignKey("iLOGIS.WMS_DeliveryItem", "PackageItemId", "iLOGIS.CONFIG_PackageItem", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "DeliveryListId", "iLOGIS.WMS_DeliveryList", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "PickingListItemId", "iLOGIS.WMS_PickingListItem", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "StockUnitId", "iLOGIS.WMS_StockUnit", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "StockUnitId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "PickingListItemId", "iLOGIS.WMS_PickingListItem");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "DeliveryListId", "iLOGIS.WMS_DeliveryList");
            DropForeignKey("iLOGIS.WMS_DeliveryList", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_DeliveryList", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "PackageItemId", "iLOGIS.CONFIG_PackageItem");
            DropIndex("iLOGIS.WMS_DeliveryList", new[] { "TransporterId" });
            DropIndex("iLOGIS.WMS_DeliveryList", new[] { "WorkOrderId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "StockUnitId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "DeliveryListId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", "IX_ItemWorkstationWorkorderTransporterPickignListItem");
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "PackageItemId" });
            AlterColumn("iLOGIS.WMS_Movement", "ExternalUserName", c => c.String());
            AlterColumn("iLOGIS.WMS_Movement", "ExternalId", c => c.String());
            AlterColumn("iLOGIS.CONFIG_Item", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("iLOGIS.CONFIG_WorkstationItem", "PutTo");
            DropColumn("iLOGIS.WMS_StockUnit", "ReferenceDeliveryItemId");
            DropColumn("iLOGIS.WMS_PickingList", "GuidCreationDate");
            DropColumn("iLOGIS.WMS_PickingList", "Guid");
            DropColumn("iLOGIS.WMS_PickingListItem", "StatusLFI");
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "AvailableForPicker");
            DropColumn("iLOGIS.WMS_Movement", "ExportDateTime");
            DropColumn("iLOGIS.WMS_Movement", "FreeText2");
            DropColumn("iLOGIS.WMS_Movement", "FreeText1");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "Status");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "StockUnitId");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "DeliveryListId");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "PickingListItemId");
            DropColumn("iLOGIS.WMS_DeliveryItem", "PackageItemId");
            DropColumn("iLOGIS.WMS_Delivery", "Guid");
            DropColumn("_MPPL.MASTERDATA_Workstation", "FlowRackLOverride");
            DropColumn("_MPPL.IDENTITY_User", "LastPasswordChangedDate");
            DropTable("iLOGIS.WMS_DeliveryList");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", new[] { "TransporterId", "WorkOrderId", "WorkstationId", "ItemWMSId" }, unique: true, name: "IX_ItemWorkstationWorkorderTransporter");
        }
    }
}

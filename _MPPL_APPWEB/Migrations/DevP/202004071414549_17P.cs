namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17P : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "iLOGIS.Delivery", newName: "WMS_Delivery");
            RenameTable(name: "iLOGIS.DeliveryItem", newName: "WMS_DeliveryItem");
            RenameTable(name: "iLOGIS.WMS_PackageInstance", newName: "WMS_StockUnit");
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "ItemId", "_MPPL.MASTERDATA_Item");
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "ItemId" });
            RenameColumn(table: "iLOGIS.WMS_DeliveryListItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.WMS_StockUnit", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.CONFIG_PackageItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "PackageInstanceId", newName: "StockUnitId");
            RenameColumn(table: "iLOGIS.WMS_TransporterLog", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.CONFIG_WorkstationItem", name: "ItemId", newName: "ItemWMSId");
            RenameIndex(table: "iLOGIS.CONFIG_PackageItem", name: "IX_ItemId", newName: "IX_ItemWMSId");
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_ItemId", newName: "IX_ItemWMSId");
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_PackageInstanceId", newName: "IX_StockUnitId");
            RenameIndex(table: "iLOGIS.WMS_StockUnit", name: "IX_ItemId", newName: "IX_ItemWMSId");
            RenameIndex(table: "iLOGIS.WMS_TransporterLog", name: "IX_ItemId", newName: "IX_ItemWMSId");
            RenameIndex(table: "iLOGIS.CONFIG_WorkstationItem", name: "IX_ItemId", newName: "IX_ItemWMSId");
            AddColumn("iLOGIS.WMS_DeliveryItem", "ItemWMSId", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.WMS_DeliveryItem", "ItemWMSId");
            AddForeignKey("iLOGIS.WMS_DeliveryItem", "ItemWMSId", "iLOGIS.CONFIG_Item", "Id");
            DropColumn("iLOGIS.WMS_DeliveryItem", "ItemId");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.WMS_DeliveryItem", "ItemId", c => c.Int(nullable: false));
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "ItemWMSId", "iLOGIS.CONFIG_Item");
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "ItemWMSId" });
            DropColumn("iLOGIS.WMS_DeliveryItem", "ItemWMSId");
            RenameIndex(table: "iLOGIS.CONFIG_WorkstationItem", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameIndex(table: "iLOGIS.WMS_TransporterLog", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameIndex(table: "iLOGIS.WMS_StockUnit", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_StockUnitId", newName: "IX_PackageInstanceId");
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameIndex(table: "iLOGIS.CONFIG_PackageItem", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameColumn(table: "iLOGIS.CONFIG_WorkstationItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_TransporterLog", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "StockUnitId", newName: "PackageInstanceId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.CONFIG_PackageItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_StockUnit", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_DeliveryListItem", name: "ItemWMSId", newName: "ItemId");
            CreateIndex("iLOGIS.WMS_DeliveryItem", "ItemId");
            AddForeignKey("iLOGIS.DeliveryItem", "ItemId", "_MPPL.MASTERDATA_Item", "Id");
            RenameTable(name: "iLOGIS.WMS_StockUnit", newName: "WMS_PackageInstance");
            RenameTable(name: "iLOGIS.WMS_DeliveryItem", newName: "DeliveryItem");
            RenameTable(name: "iLOGIS.WMS_Delivery", newName: "Delivery");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _151K : DbMigration
    {
        public override void Up()
        {
            //Sql("EXEC sp_rename @objname = N'[iLOGIS].[WMS_Delivery].[PK_iLOGIS.Delivery]', @newname = N'PK_iLOGIS.WMS_Delivery'");
            //Sql("EXEC sp_rename @objname = N'[iLOGIS].[FK_iLOGIS.Delivery__MPPL.IDENTITY_User_UserId]', @newname = N'FK_iLOGIS.WMS_Delivery__MPPL.IDENTITY_User_UserId'");
            Sql("EXEC sp_rename @objname = N'[iLOGIS].[FK_iLOGIS.Delivery__MPPL.MASTERDATA_Contractor_SupplierId]', @newname = N'FK_iLOGIS.WMS_Delivery__MPPL.MASTERDATA_Contractor_SupplierId'");

            Sql("EXEC sp_rename @objname = N'[iLOGIS].[WMS_DeliveryItem].[PK_iLOGIS.DeliveryItem]', @newname = N'PK_iLOGIS.WMS_DeliveryItem'");
            Sql("EXEC sp_rename @objname = N'[iLOGIS].[FK_iLOGIS.DeliveryItem__MPPL.MASTERDATA_Item_ItemId]', @newname = N'FK_iLOGIS.WMS_DeliveryItem__MPPL.MASTERDATA_Item_ItemId'");
            Sql("EXEC sp_rename @objname = N'[iLOGIS].[FK_iLOGIS.DeliveryItem_iLOGIS.Delivery_DeliveryId]', @newname = N'FK_iLOGIS.DeliveryItem_iLOGIS.WMS_Delivery_DeliveryId'");
            
            DropForeignKey("iLOGIS.WMS_DeliveryItem", "ItemId", "_MPPL.MASTERDATA_Item");
            DropIndex("iLOGIS.WMS_DeliveryItem", new[] { "ItemId" });
            RenameColumn(table: "iLOGIS.WMS_DeliveryListItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.CONFIG_PackageItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.WMS_StockUnit", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.WMS_TransporterLog", name: "ItemId", newName: "ItemWMSId");
            RenameColumn(table: "iLOGIS.CONFIG_WorkstationItem", name: "ItemId", newName: "ItemWMSId");
            RenameIndex(table: "iLOGIS.CONFIG_PackageItem", name: "IX_ItemId", newName: "IX_ItemWMSId");
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_ItemId", newName: "IX_ItemWMSId");
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
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameIndex(table: "iLOGIS.CONFIG_PackageItem", name: "IX_ItemWMSId", newName: "IX_ItemId");
            RenameColumn(table: "iLOGIS.CONFIG_WorkstationItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_TransporterLog", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_StockUnit", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.CONFIG_PackageItem", name: "ItemWMSId", newName: "ItemId");
            RenameColumn(table: "iLOGIS.WMS_DeliveryListItem", name: "ItemWMSId", newName: "ItemId");
            CreateIndex("iLOGIS.WMS_DeliveryItem", "ItemId");
            AddForeignKey("iLOGIS.WMS_DeliveryItem", "ItemId", "_MPPL.MASTERDATA_Item", "Id");
        }
    }
}

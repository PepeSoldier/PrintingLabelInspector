namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20K : DbMigration
    {
        public override void Up()
        {
            DropIndex("iLOGIS.WMS_DeliveryListItem", "IX_ItemWorkstationWorkorderTransporter");
            AddColumn("iLOGIS.WMS_DeliveryListItem", "PickingListItemId", c => c.Int());
            CreateIndex("iLOGIS.WMS_DeliveryListItem", new[] { "TransporterId", "WorkOrderId", "WorkstationId", "ItemWMSId", "PickingListItemId" }, unique: true, name: "IX_ItemWorkstationWorkorderTransporterPickignListItem");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "PickingListItemId", "iLOGIS.WMS_PickingListItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "PickingListItemId", "iLOGIS.WMS_PickingListItem");
            DropIndex("iLOGIS.WMS_DeliveryListItem", "IX_ItemWorkstationWorkorderTransporterPickignListItem");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "PickingListItemId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", new[] { "TransporterId", "WorkOrderId", "WorkstationId", "ItemWMSId" }, unique: true, name: "IX_ItemWorkstationWorkorderTransporter");
        }
    }
}

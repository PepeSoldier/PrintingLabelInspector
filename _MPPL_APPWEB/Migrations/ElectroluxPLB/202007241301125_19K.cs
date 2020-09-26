namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19K : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("iLOGIS.WMS_DeliveryListItem", "DeliveryListId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryListItem", "StockUnitId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId", c => c.Int());
            AddColumn("iLOGIS.WMS_DeliveryListItem", "Status", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_Movement", "ExportDateTime", c => c.DateTime());
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "DeliveryListId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "StockUnitId");
            CreateIndex("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "DeliveryListId", "iLOGIS.WMS_DeliveryList", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "StockUnitId", "iLOGIS.WMS_StockUnit", "Id");
            AddForeignKey("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation", "Id");
            DropColumn("iLOGIS.WMS_PickingList", "StatusLF");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.WMS_PickingList", "StatusLF", c => c.Int(nullable: false));
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "StockUnitId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("iLOGIS.WMS_DeliveryListItem", "DeliveryListId", "iLOGIS.WMS_DeliveryList");
            DropForeignKey("iLOGIS.WMS_DeliveryList", "WorkOrderId", "PRD.ProdOrder");
            DropForeignKey("iLOGIS.WMS_DeliveryList", "TransporterId", "iLOGIS.CONFIG_Transporter");
            DropIndex("iLOGIS.WMS_DeliveryList", new[] { "TransporterId" });
            DropIndex("iLOGIS.WMS_DeliveryList", new[] { "WorkOrderId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "WarehouseLocationId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "StockUnitId" });
            DropIndex("iLOGIS.WMS_DeliveryListItem", new[] { "DeliveryListId" });
            DropColumn("iLOGIS.WMS_Movement", "ExportDateTime");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "Status");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "WarehouseLocationId");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "StockUnitId");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "DeliveryListId");
            DropTable("iLOGIS.WMS_DeliveryList");
        }
    }
}

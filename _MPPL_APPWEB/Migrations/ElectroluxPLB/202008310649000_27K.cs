namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _27K : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("iLOGIS.WMS_DeliveryItem", "isLocationAssigned", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "isLocated", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_PackageItem", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryListItem", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryListItem", "IsSubstituteData", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Transporter", "ConnectedStatus", c => c.String());
            AddColumn("iLOGIS.WMS_PickingListItem", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "IncomeDate", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "IsGroup", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "GroupId", c => c.Int());
            AddColumn("iLOGIS.WMS_Movement", "UnitOfMeasure", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.WMS_StockUnit", "GroupId");
            AddForeignKey("iLOGIS.WMS_StockUnit", "GroupId", "iLOGIS.WMS_StockUnit", "Id");
            DropColumn("iLOGIS.WMS_StockUnit", "ReservedForPickingListId");

            Sql("UPDATE t " +
            "SET t.UnitOfMeasure = mi.UnitOfMeasure " +
            "FROM [iLOGIS].[WMS_Movement] t " +
            "LEFT JOIN [iLOGIS].[CONFIG_Item] ii ON t.ItemWMSId = ii.Id " +
            "LEFT JOIN [_MPPL].[MASTERDATA_Item] mi ON ii.ItemId = mi.id ");

            Sql("UPDATE t " +
            "SET t.UnitOfMeasure = mi.UnitOfMeasure " +
            "FROM [iLOGIS].[WMS_PickingListItem] t " +
            "LEFT JOIN [iLOGIS].[CONFIG_Item] ii ON t.ItemWMSId = ii.Id " +
            "LEFT JOIN [_MPPL].[MASTERDATA_Item] mi ON ii.ItemId = mi.id ");

            Sql("UPDATE t " +
            "SET t.UnitOfMeasure = mi.UnitOfMeasure " +
            "FROM [iLOGIS].[WMS_DeliveryListItem] t " +
            "LEFT JOIN [iLOGIS].[CONFIG_Item] ii ON t.ItemWMSId = ii.Id " +
            "LEFT JOIN [_MPPL].[MASTERDATA_Item] mi ON ii.ItemId = mi.id ");

            Sql("UPDATE t  " +
            "SET t.UnitOfMeasure = mi.UnitOfMeasure  " +
            "FROM [iLOGIS].[WMS_StockUnit] t  " +
            "LEFT JOIN [iLOGIS].[CONFIG_Item] ii ON t.ItemWMSId = ii.Id " +
            "LEFT JOIN [_MPPL].[MASTERDATA_Item] mi ON ii.ItemId = mi.id ");

            Sql("UPDATE [iLOGIS].[WMS_StockUnit] SET IncomeDate = CreatedDate");
            Sql("UPDATE [iLOGIS].[WMS_StockUnit] SET [IncomeDate] = [CreatedDate]");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.WMS_StockUnit", "ReservedForPickingListId", c => c.Int(nullable: false));
            DropForeignKey("iLOGIS.WMS_StockUnit", "GroupId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("_MPPL.MASTERDATA_ItemUoM", "ItemId", "_MPPL.MASTERDATA_Item");
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "GroupId" });
            DropIndex("_MPPL.MASTERDATA_ItemUoM", new[] { "ItemId" });
            DropColumn("iLOGIS.WMS_Movement", "UnitOfMeasure");
            DropColumn("iLOGIS.WMS_StockUnit", "GroupId");
            DropColumn("iLOGIS.WMS_StockUnit", "IsGroup");
            DropColumn("iLOGIS.WMS_StockUnit", "IncomeDate");
            DropColumn("iLOGIS.WMS_StockUnit", "UnitOfMeasure");
            DropColumn("iLOGIS.WMS_PickingListItem", "UnitOfMeasure");
            DropColumn("iLOGIS.CONFIG_Transporter", "ConnectedStatus");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "IsSubstituteData");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "UnitOfMeasure");
            DropColumn("iLOGIS.CONFIG_PackageItem", "Deleted");
            DropColumn("iLOGIS.WMS_DeliveryItem", "isLocated");
            DropColumn("iLOGIS.WMS_DeliveryItem", "isLocationAssigned");
            DropTable("_MPPL.MASTERDATA_ItemUoM");
        }
    }
}

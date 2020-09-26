namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _165K : DbMigration
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
            
            CreateTable(
                "ONEPROD.QUALITY_ItemMeasurement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemOPId = c.Int(nullable: false),
                        SerialNumber = c.String(),
                        Counter = c.Int(nullable: false),
                        Result0 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result3 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result4 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result5 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result6 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result7 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result8 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Result9 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemOPId)
                .Index(t => t.ItemOPId);
            
            CreateTable(
                "ONEPROD.QUALITY_ItemParameter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemOPId = c.Int(nullable: false),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Depth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Length_Tolerance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width_Tolerance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Depth_Tolerance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(),
                        ProgramNumber = c.Int(nullable: false),
                        ProgramName = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ONEPROD.CORE_Item", t => t.ItemOPId)
                .Index(t => t.ItemOPId);
            
            AddColumn("iLOGIS.WMS_DeliveryItem", "IsLocationAssigned", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "IsLocated", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryItem", "TotalLocatedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("iLOGIS.CONFIG_PackageItem", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryListItem", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_DeliveryListItem", "IsSubstituteData", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Transporter", "ConnectedStatus", c => c.String());
            AddColumn("iLOGIS.WMS_PickingListItem", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "InitialQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("iLOGIS.WMS_StockUnit", "UnitOfMeasure", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "IncomeDate", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "IsGroup", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.WMS_StockUnit", "GroupId", c => c.Int());
            AddColumn("iLOGIS.WMS_Movement", "UnitOfMeasure", c => c.Int(nullable: false));
            CreateIndex("iLOGIS.WMS_StockUnit", "GroupId");
            AddForeignKey("iLOGIS.WMS_StockUnit", "GroupId", "iLOGIS.WMS_StockUnit", "Id");
            DropColumn("iLOGIS.WMS_StockUnit", "ReservedForPickingListId");
            DropTable("PFEP.DEF_Types");
        }
        
        public override void Down()
        {
            CreateTable(
                "PFEP.DEF_Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Deleted = c.Boolean(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("iLOGIS.WMS_StockUnit", "ReservedForPickingListId", c => c.Int(nullable: false));
            DropForeignKey("ONEPROD.QUALITY_ItemParameter", "ItemOPId", "ONEPROD.CORE_Item");
            DropForeignKey("ONEPROD.QUALITY_ItemMeasurement", "ItemOPId", "ONEPROD.CORE_Item");
            DropForeignKey("iLOGIS.WMS_StockUnit", "GroupId", "iLOGIS.WMS_StockUnit");
            DropForeignKey("_MPPL.MASTERDATA_ItemUoM", "ItemId", "_MPPL.MASTERDATA_Item");
            DropIndex("ONEPROD.QUALITY_ItemParameter", new[] { "ItemOPId" });
            DropIndex("ONEPROD.QUALITY_ItemMeasurement", new[] { "ItemOPId" });
            DropIndex("iLOGIS.WMS_StockUnit", new[] { "GroupId" });
            DropIndex("_MPPL.MASTERDATA_ItemUoM", new[] { "ItemId" });
            DropColumn("iLOGIS.WMS_Movement", "UnitOfMeasure");
            DropColumn("iLOGIS.WMS_StockUnit", "GroupId");
            DropColumn("iLOGIS.WMS_StockUnit", "IsGroup");
            DropColumn("iLOGIS.WMS_StockUnit", "IncomeDate");
            DropColumn("iLOGIS.WMS_StockUnit", "UnitOfMeasure");
            DropColumn("iLOGIS.WMS_StockUnit", "InitialQty");
            DropColumn("iLOGIS.WMS_PickingListItem", "UnitOfMeasure");
            DropColumn("iLOGIS.CONFIG_Transporter", "ConnectedStatus");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "IsSubstituteData");
            DropColumn("iLOGIS.WMS_DeliveryListItem", "UnitOfMeasure");
            DropColumn("iLOGIS.CONFIG_PackageItem", "Deleted");
            DropColumn("iLOGIS.WMS_DeliveryItem", "TotalLocatedQty");
            DropColumn("iLOGIS.WMS_DeliveryItem", "IsLocated");
            DropColumn("iLOGIS.WMS_DeliveryItem", "IsLocationAssigned");
            DropTable("ONEPROD.QUALITY_ItemParameter");
            DropTable("ONEPROD.QUALITY_ItemMeasurement");
            DropTable("_MPPL.MASTERDATA_ItemUoM");
        }
    }
}

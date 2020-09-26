namespace _MPPL_WEB_START.Migrations.DevP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20P : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "iLOGIS.CONFIG_WarehouseLocationTpe", newName: "CONFIG_WarehouseLocationType");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "Id", "iLOGIS.CONFIG_WarehouseLocation");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "Id" });
            DropPrimaryKey("iLOGIS.CONFIG_Warehouse");
            AddColumn("iLOGIS.CONFIG_Warehouse", "Name", c => c.String());
            AddColumn("iLOGIS.CONFIG_Warehouse", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "QtyOfSubLocations", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", c => c.Int());
            AddColumn("iLOGIS.CONFIG_Warehouse", "IndependentSerialNumber", c => c.Boolean(nullable: false));

            //copy parameters of warehouse
            AddColumn("iLOGIS.CONFIG_Warehouse", "OldId", c => c.Int());
            Sql("UPDATE w SET w.[Name] = wl.[Name], w.Deleted = wl.Deleted, w.QtyOfSubLocations = wl.QtyOfSubLocations, w.ParentWarehouseId = wl.WarehouseId, w.OldId = wl.Id " +
                "FROM[iLOGIS].[CONFIG_Warehouse] w LEFT JOIN[iLOGIS].[CONFIG_WarehouseLocation] wl ON w.Id = wl.id");

            //ALTERCOLUMN zamieniony na drop i add.
            DropColumn("iLOGIS.CONFIG_Warehouse", "Id");
            AddColumn("iLOGIS.CONFIG_Warehouse", "Id", c => c.Int(nullable: false, identity: true));

            //Przypisywanie nowonadanych ID'ków
            Sql("UPDATE wl SET wl.WarehouseId = w.Id FROM [iLOGIS].[CONFIG_WarehouseLocation] wl LEFT JOIN [iLOGIS].[CONFIG_Warehouse] w  ON w.OldId = wl.Id");
            Sql("UPDATe w SET w.ParentWarehouseId = (SELECT w2.Id FROM [iLOGIS].[CONFIG_Warehouse] w2 WHERE w2.OldId = w.ParentWarehouseId) FROM [iLOGIS].[CONFIG_Warehouse] w");
            Sql("UPDATE pi1 SET pi1.WarehouseId = w.Id FROM [iLOGIS].[CONFIG_PackageItem] pi1 LEFT JOIN [iLOGIS].[CONFIG_Warehouse] w  ON w.OldId = pi1.WarehouseId");
            Sql("UPDATE wi1 SET wi1.WarehouseId = w.Id FROM [iLOGIS].CONFIG_WarehouseItem wi1 LEFT JOIN [iLOGIS].[CONFIG_Warehouse] w  ON w.OldId = wi1.WarehouseId");
            Sql("UPDATE bl SET bl.BoxId = w.Id FROM [ONEPROD].PREPROD_BufforLog bl LEFT JOIN [iLOGIS].[CONFIG_Warehouse] w  ON w.OldId = bl.BoxId");

            DropColumn("iLOGIS.CONFIG_Warehouse", "OldId");


            AddColumn("ONEPROD.MES_Workplace", "Type", c => c.Int(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsTraceability", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.MES_Workplace", "IsReportOnline", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "Qty_Scrap", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel", c => c.Int(nullable: false));
            //AlterColumn("iLOGIS.CONFIG_Warehouse", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Utilization", c => c.Decimal(nullable: false, precision: 14, scale: 12));
            AddPrimaryKey("iLOGIS.CONFIG_Warehouse", "Id");
            CreateIndex("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id", cascadeDelete: true);
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropForeignKey("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId", "iLOGIS.CONFIG_Warehouse");
            DropIndex("iLOGIS.CONFIG_Warehouse", new[] { "ParentWarehouseId" });
            DropPrimaryKey("iLOGIS.CONFIG_Warehouse");
            AlterColumn("iLOGIS.CONFIG_WarehouseLocation", "Utilization", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("iLOGIS.CONFIG_Warehouse", "Id", c => c.Int(nullable: false));
            DropColumn("ONEPROD.CORE_Workorder", "Qty_ControlLabel");
            DropColumn("ONEPROD.CORE_Workorder", "Qty_Scrap");
            DropColumn("ONEPROD.MES_Workplace", "IsReportOnline");
            DropColumn("ONEPROD.MES_Workplace", "IsTraceability");
            DropColumn("ONEPROD.MES_Workplace", "Type");
            DropColumn("iLOGIS.CONFIG_Warehouse", "IndependentSerialNumber");
            DropColumn("iLOGIS.CONFIG_Warehouse", "ParentWarehouseId");
            DropColumn("iLOGIS.CONFIG_Warehouse", "QtyOfSubLocations");
            DropColumn("iLOGIS.CONFIG_Warehouse", "Deleted");
            DropColumn("iLOGIS.CONFIG_Warehouse", "Name");
            AddPrimaryKey("iLOGIS.CONFIG_Warehouse", "Id");
            CreateIndex("iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "AccountingWarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_PackageItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("ONEPROD.PREPROD_BufforLog", "BoxId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseLocation", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_WarehouseItem", "WarehouseId", "iLOGIS.CONFIG_Warehouse", "Id");
            AddForeignKey("iLOGIS.CONFIG_Warehouse", "Id", "iLOGIS.CONFIG_WarehouseLocation", "Id");
            RenameTable(name: "iLOGIS.CONFIG_WarehouseLocationType", newName: "CONFIG_WarehouseLocationTpe");
        }
    }
}

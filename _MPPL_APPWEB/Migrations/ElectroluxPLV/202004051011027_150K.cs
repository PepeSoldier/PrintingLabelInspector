namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _150K : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "iLOGIS.WMS_PackageInstance", newName: "WMS_StockUnit");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "PackageInstanceId", newName: "StockUnitId");
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_PackageInstanceId", newName: "IX_StockUnitId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "iLOGIS.WMS_PickingListItem", name: "IX_StockUnitId", newName: "IX_PackageInstanceId");
            RenameColumn(table: "iLOGIS.WMS_PickingListItem", name: "StockUnitId", newName: "PackageInstanceId");
            RenameTable(name: "iLOGIS.WMS_StockUnit", newName: "WMS_PackageInstance");
        }
    }
}

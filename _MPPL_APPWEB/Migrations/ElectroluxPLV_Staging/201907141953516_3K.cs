namespace _MPPL_WEB_START.Migrations.ElectroluxPLV_Staging
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3K : DbMigration
    {
        public override void Up()
        {
            AddColumn("PRD.ProdOrder", "QtyProducedInPast", c => c.Int(nullable: false));
            DropColumn("iLOGIS.WMS_DeliveryListItem", "QtyProducedInPast");
        }
        
        public override void Down()
        {
            AddColumn("iLOGIS.WMS_DeliveryListItem", "QtyProducedInPast", c => c.Int(nullable: false));
            DropColumn("PRD.ProdOrder", "QtyProducedInPast");
        }
    }
}

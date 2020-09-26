namespace _MPPL_WEB_START.Migrations.ElectroluxPLV_Staging
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_DeliveryListItem", "QtyProducedInPast", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WMS_DeliveryListItem", "QtyProducedInPast");
        }
    }
}

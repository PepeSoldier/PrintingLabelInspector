namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _154K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_DeliveryItem", "WasPrinted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WMS_DeliveryItem", "WasPrinted");
        }
    }
}

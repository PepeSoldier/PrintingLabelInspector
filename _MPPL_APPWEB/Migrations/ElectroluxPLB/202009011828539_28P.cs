namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_StockUnit", "InitialQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WMS_StockUnit", "InitialQty");
        }
    }
}

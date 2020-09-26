namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.CONFIG_WarehouseLocation", "ShelfNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.CONFIG_WarehouseLocation", "ShelfNumber");
        }
    }
}

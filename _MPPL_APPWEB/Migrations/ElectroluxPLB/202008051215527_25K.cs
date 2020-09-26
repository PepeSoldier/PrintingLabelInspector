namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _25K : DbMigration
    {
        public override void Up()
        {
            AlterColumn("iLOGIS.CONFIG_Item", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 5));
        }
        
        public override void Down()
        {
            AlterColumn("iLOGIS.CONFIG_Item", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

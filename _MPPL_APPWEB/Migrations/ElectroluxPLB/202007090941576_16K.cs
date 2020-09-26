namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16K : DbMigration
    {
        public override void Up()
        {
            AlterColumn("iLOGIS.WMS_Delivery", "DocumentNumber", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("iLOGIS.WMS_Delivery", "DocumentNumber", c => c.String(maxLength: 12));
        }
    }
}

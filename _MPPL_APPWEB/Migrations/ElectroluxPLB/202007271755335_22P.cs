namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22P : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_PickingList", "Guid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.WMS_PickingList", "Guid");
        }
    }
}

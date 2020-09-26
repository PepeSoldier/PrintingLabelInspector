namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _24KP : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_PickingList", "GuidCreationDate", c => c.DateTime(nullable: false));
            AddColumn("_MPPL.MASTERDATA_Workstation", "FlowRackLOverride", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.MASTERDATA_Workstation", "FlowRackLOverride");
            DropColumn("iLOGIS.WMS_PickingList", "GuidCreationDate");
        }
    }
}

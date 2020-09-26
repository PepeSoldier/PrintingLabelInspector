namespace _MPPL_WEB_START.Migrations.ElectroluxPLB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.WMS_PickingListItem", "StatusLFI", c => c.Int(nullable: false));
            AddColumn("iLOGIS.WMS_PickingList", "StatusLF", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_WorkstationItem", "PutTo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.CONFIG_WorkstationItem", "PutTo");
            DropColumn("iLOGIS.WMS_PickingList", "StatusLF");
            DropColumn("iLOGIS.WMS_PickingListItem", "StatusLFI");
        }
    }
}

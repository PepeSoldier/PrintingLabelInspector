namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18P : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.IDENTITY_User", "LastPasswordChangedDate", c => c.DateTime(nullable: false));
            AddColumn("_MPPL.MASTERDATA_Workstation", "FlowRackLOverride", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.MASTERDATA_Workstation", "FlowRackLOverride");
            DropColumn("_MPPL.IDENTITY_User", "LastPasswordChangedDate");
        }
    }
}

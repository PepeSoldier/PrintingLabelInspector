namespace _MPPL_WEB_START.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _163K : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.IDENTITY_UserRole", "Id", c => c.Int(nullable: false));
            AddColumn("ONEPROD.CORE_CycleTime", "ProgramName", c => c.String(maxLength: 50));
            AlterColumn("iLOGIS.WMS_Delivery", "DocumentNumber", c => c.String(maxLength: 25));
            DropColumn("_MPPL.IDENTITY_UserRole", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("_MPPL.IDENTITY_UserRole", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("iLOGIS.WMS_Delivery", "DocumentNumber", c => c.String(maxLength: 12));
            DropColumn("ONEPROD.CORE_CycleTime", "ProgramName");
            DropColumn("_MPPL.IDENTITY_UserRole", "Id");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.OEE_Reason", "IsGroup", c => c.Boolean(nullable: false));
            AddColumn("ONEPROD.OEE_Reason", "GroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.OEE_Reason", "GroupId");
            DropColumn("ONEPROD.OEE_Reason", "IsGroup");
        }
    }
}

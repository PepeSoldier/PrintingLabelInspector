namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _17K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.CONFIG_Item", "H", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.CONFIG_Item", "H");
        }
    }
}

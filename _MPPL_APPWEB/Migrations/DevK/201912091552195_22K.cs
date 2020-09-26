namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "PackageName", c => c.String());
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "LastChange", c => c.DateTime(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "UserName");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "LastChange");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "PackageName");
        }
    }
}

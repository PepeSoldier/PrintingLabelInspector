namespace _MPPL_WEB_START.Migrations.DevK
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5K : DbMigration
    {
        public override void Up()
        {
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "MaxPackages", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "SafetyStock", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "MaxBomQty", c => c.Int(nullable: false));
            AddColumn("iLOGIS.CONFIG_AutomaticRules", "CheckOnly", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "CheckOnly");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "MaxBomQty");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "SafetyStock");
            DropColumn("iLOGIS.CONFIG_AutomaticRules", "MaxPackages");
        }
    }
}

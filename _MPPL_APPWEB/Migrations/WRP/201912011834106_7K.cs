namespace _MPPL_WEB_START.Migrations.WRP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.CORE_Resource", "Breaks", c => c.String());
            AddColumn("ONEPROD.OEE_Reason", "ColorGroup", c => c.String(maxLength: 35));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.OEE_Reason", "ColorGroup");
            DropColumn("ONEPROD.CORE_Resource", "Breaks");
        }
    }
}

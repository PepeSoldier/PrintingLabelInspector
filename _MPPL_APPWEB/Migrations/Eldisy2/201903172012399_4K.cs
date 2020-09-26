namespace _MPPL_WEB_START.Migrations.Eldisy2
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4K : DbMigration
    {
        public override void Up()
        {
            AddColumn("ONEPROD.PREPROD_ClientOrder", "ItemCode", c => c.String(maxLength: 50));
            AddColumn("ONEPROD.PREPROD_ClientOrder", "ItemName", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("ONEPROD.PREPROD_ClientOrder", "ItemName");
            DropColumn("ONEPROD.PREPROD_ClientOrder", "ItemCode");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18P : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.Correction", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.Correction", "Deleted");
        }
    }
}

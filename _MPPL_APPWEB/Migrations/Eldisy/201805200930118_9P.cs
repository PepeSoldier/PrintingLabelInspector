namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9P : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.Correction", "CorrectionOpen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.Correction", "CorrectionOpen");
        }
    }
}

namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10P : DbMigration
    {
        public override void Up()
        {
            AddColumn("_MPPL.Correction", "CorrectionClosed", c => c.Boolean(nullable: false));
            DropColumn("_MPPL.Correction", "Finished");
        }
        
        public override void Down()
        {
            AddColumn("_MPPL.Correction", "Finished", c => c.Boolean(nullable: false));
            DropColumn("_MPPL.Correction", "CorrectionClosed");
        }
    }
}

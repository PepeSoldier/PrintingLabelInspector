namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12P : DbMigration
    {
        public override void Up()
        {
            AlterColumn("_MPPL.Correction", "Applicationfinished", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("_MPPL.Correction", "Applicationfinished", c => c.DateTime(nullable: false));
        }
    }
}

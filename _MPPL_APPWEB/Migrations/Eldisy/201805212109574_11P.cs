namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11P : DbMigration
    {
        public override void Up()
        {
            AddColumn("PFEP.PackingInstruction", "IsCorrectionOpen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("PFEP.PackingInstruction", "IsCorrectionOpen");
        }
    }
}

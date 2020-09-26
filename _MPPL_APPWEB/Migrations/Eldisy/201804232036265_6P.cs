namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6P : DbMigration
    {
        public override void Up()
        {
            DropColumn("PFEP.PackingInstruction", "AmountOnBoxId");
        }
        
        public override void Down()
        {
            AddColumn("PFEP.PackingInstruction", "AmountOnBoxId", c => c.Int(nullable: false));
        }
    }
}

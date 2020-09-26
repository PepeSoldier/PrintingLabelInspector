namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19P : DbMigration
    {
        public override void Up()
        {
            AddColumn("PFEP.PackingInstruction", "NumberOfCorrections", c => c.Int());
            DropColumn("PFEP.PackingInstruction", "IsCorrectionOpen");
        }
        
        public override void Down()
        {
            AddColumn("PFEP.PackingInstruction", "IsCorrectionOpen", c => c.Boolean(nullable: false));
            DropColumn("PFEP.PackingInstruction", "NumberOfCorrections");
        }
    }
}

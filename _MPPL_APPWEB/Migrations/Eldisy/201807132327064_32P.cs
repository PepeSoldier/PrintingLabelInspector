namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _32P : DbMigration
    {
        public override void Up()
        {
            AddColumn("PFEP.PackingInstruction", "ExamineComment", c => c.String(maxLength: 200));
            AddColumn("PFEP.PackingInstruction", "ConfirmComment", c => c.String(maxLength: 200));
            AddColumn("PFEP.PackingInstruction", "CurrentPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("_MPPL.ChangeLog", "FieldDisplayName", c => c.String(maxLength: 140));
        }
        
        public override void Down()
        {
            DropColumn("_MPPL.ChangeLog", "FieldDisplayName");
            DropColumn("PFEP.PackingInstruction", "CurrentPrice");
            DropColumn("PFEP.PackingInstruction", "ConfirmComment");
            DropColumn("PFEP.PackingInstruction", "ExamineComment");
        }
    }
}

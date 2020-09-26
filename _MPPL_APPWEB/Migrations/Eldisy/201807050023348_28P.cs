namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _28P : DbMigration
    {
        public override void Up()
        {
            AlterColumn("PFEP.PackingInstruction", "ExaminedDate", c => c.DateTime(nullable: false));
            AlterColumn("PFEP.PackingInstruction", "ConfirmedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("PFEP.PackingInstruction", "ConfirmedDate", c => c.DateTime());
            AlterColumn("PFEP.PackingInstruction", "ExaminedDate", c => c.DateTime());
        }
    }
}
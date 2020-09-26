namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7P : DbMigration
    {
        public override void Up()
        {
            DropColumn("PFEP.PackingInstruction", "SpecialCharacteristic");
        }
        
        public override void Down()
        {
            AddColumn("PFEP.PackingInstruction", "SpecialCharacteristic", c => c.String());
        }
    }
}

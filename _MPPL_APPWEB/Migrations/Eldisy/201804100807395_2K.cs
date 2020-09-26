namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "PFEP.PackagingIntruction_Item", newName: "PackingInstruction_Item");
        }
        
        public override void Down()
        {
            RenameTable(name: "PFEP.PackingInstruction_Item", newName: "PackagingIntruction_Item");
        }
    }
}

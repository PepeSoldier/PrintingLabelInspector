namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _31P : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "PFEP.PackagingInstruction", newName: "PackingInstruction");
            RenameTable(name: "PFEP.PackagingInstruction_Item", newName: "PackingInstruction_Package");
            RenameColumn(table: "PFEP.PackingInstruction_Package", name: "PackagingInstructionId", newName: "PackingInstructionId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "PFEP.PackingInstruction_Package", name: "PackingInstructionId", newName: "PackagingInstructionId");
            RenameTable(name: "PFEP.PackingInstruction_Package", newName: "PackingInstruction_Item");
            RenameTable(name: "PFEP.PackingInstruction", newName: "PackagingInstruction");
        }
    }
}

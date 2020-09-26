namespace _MPPL_WEB_START.Migrations.Eldisy
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _29P : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "PFEP.PackingInstruction", name: "Examiner_Id", newName: "ExaminerId");
            RenameIndex(table: "PFEP.PackingInstruction", name: "IX_Examiner_Id", newName: "IX_ExaminerId");
            DropColumn("PFEP.PackingInstruction", "ExamineId");
        }
        
        public override void Down()
        {
            AddColumn("PFEP.PackingInstruction", "ExamineId", c => c.String());
            RenameIndex(table: "PFEP.PackingInstruction", name: "IX_ExaminerId", newName: "IX_Examiner_Id");
            RenameColumn(table: "PFEP.PackingInstruction", name: "ExaminerId", newName: "Examiner_Id");
        }
    }
}

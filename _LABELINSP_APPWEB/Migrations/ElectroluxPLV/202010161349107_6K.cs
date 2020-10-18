namespace _LABELINSP_APPWEB.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6K : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "LABELINSP.WorkorderLabelInspection", name: "PackingLabelId", newName: "WorkorderLabelId");
            //RenameIndex(table: "LABELINSP.WorkorderLabelInspection", name: "IX_PackingLabelId", newName: "IX_WorkorderLabelId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "LABELINSP.WorkorderLabelInspection", name: "IX_WorkorderLabelId", newName: "IX_PackingLabelId");
            RenameColumn(table: "LABELINSP.WorkorderLabelInspection", name: "WorkorderLabelId", newName: "PackingLabelId");
        }
    }
}

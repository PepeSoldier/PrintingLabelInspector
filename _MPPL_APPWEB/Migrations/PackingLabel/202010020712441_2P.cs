namespace _MPPL_WEB_START.Migrations.PackingLabel
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2P : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "_LABELINSP.MASTERDATA_PackingLabel", newName: "LABELINSP_PackingLabel");
            RenameTable(name: "_LABELINSP.MASTERDATA_PackingLabelTest", newName: "LABELINSP_PackingLabelTest");
        }
        
        public override void Down()
        {
            RenameTable(name: "_LABELINSP.LABELINSP_PackingLabelTest", newName: "MASTERDATA_PackingLabelTest");
            RenameTable(name: "_LABELINSP.LABELINSP_PackingLabel", newName: "MASTERDATA_PackingLabel");
        }
    }
}

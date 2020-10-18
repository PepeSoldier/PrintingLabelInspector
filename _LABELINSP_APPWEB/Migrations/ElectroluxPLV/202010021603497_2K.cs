namespace _LABELINSP_APPWEB.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2K : DbMigration
    {
        public override void Up()
        {
            AddColumn("LABELINSP.WorkorderLabelInspection", "ExpectedValueText", c => c.String());
            AddColumn("LABELINSP.WorkorderLabelInspection", "ActualValueText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("LABELINSP.WorkorderLabelInspection", "ActualValueText");
            DropColumn("LABELINSP.WorkorderLabelInspection", "ExpectedValueText");
        }
    }
}

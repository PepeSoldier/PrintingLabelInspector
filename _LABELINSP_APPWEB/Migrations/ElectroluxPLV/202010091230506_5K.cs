namespace _LABELINSP_APPWEB.Migrations.ElectroluxPLV
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5K : DbMigration
    {
        public override void Up()
        {
            AddColumn("LABELINSP.Workorder", "SerialNumberFromInt", c => c.Int(nullable: false));
            AddColumn("LABELINSP.Workorder", "SerialNumberToInt", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("LABELINSP.Workorder", "SerialNumberToInt");
            DropColumn("LABELINSP.Workorder", "SerialNumberFromInt");
        }
    }
}
